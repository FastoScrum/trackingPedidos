using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TrackingPedidos.Data;
using TrackingPedidos.Utilities;
using TrackingPedidos.ViewModels;
using Vereyon.Web;

namespace TrackingPedidos.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        public IFlashMessage _flashMessage { get; private set; }

        public UsuariosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IEmailSender emailSender, IFlashMessage flashMessage)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _flashMessage = flashMessage;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.ApplicationUser
                .Include(i => i.UserRoles)
                    .ThenInclude(i => i.Role)
                .AsNoTracking()
                .OrderBy(i => i.UserName)
                .ToListAsync();

            foreach (var usu in usuarios)
            {
                if (usu.UserRoles.FirstOrDefault(i => i.Role.Name == "Administrador") != null)
                {
                    usuarios.Remove(usu);
                    break;
                }
            }

            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var user = await _context.ApplicationUser
                    .Include(i => i.UserRoles)
                        .ThenInclude(i => i.Role)
                    .FirstOrDefaultAsync(m => m.UserName == id);
            if (user == null)
            {
                return NotFound();
            }

            if (User.Identity.Name == user.UserName)
            {
                return Forbid();
            }

            return View(user);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var user = new ApplicationUser
                        {
                            UserName = userVM.Email,
                            Email = userVM.Email,
                            PhoneNumber = userVM.PhoneNumber,
                            FirstName = userVM.FirstName,
                            LastName = userVM.LastName
                        };

                        var password = RandomPassword.GenerateRandomPassword(8);

                        var resultUser = await _userManager.CreateAsync(user, password);
                        if (resultUser.Succeeded)
                        {
                            var resultRol = await _userManager.AddToRoleAsync(user, userVM.Rol);
                            if (resultRol.Succeeded)
                            {
                                transaction.Commit();

                                await this.SendEmail(user, password);
                                this._flashMessage.Queue(FlashMessageType.Confirmation, $"Usuario con email <b>{userVM.Email}</b> registrado.", string.Empty, true);

                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                this._flashMessage.Danger("No se pudo asignar el rol.");
                                transaction.Rollback();
                            }
                        }
                        else
                        {
                            this._flashMessage.Queue(FlashMessageType.Danger, $"Verifique que exista un usuario con el email <b>{userVM.Email}</b>.", string.Empty, true);
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        this._flashMessage.Queue(FlashMessageType.Danger, $"Error al crear el usuario <b>{userVM.Email}</b>.", string.Empty, true);
                    }
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendConfirmation(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                var password = RandomPassword.GenerateRandomPassword(8);

                var hash = _userManager.PasswordHasher.HashPassword(user, password);
                user.PasswordHash = hash;

                await _context.SaveChangesAsync();

                await this.SendEmail(user, password);

                this._flashMessage.Queue(FlashMessageType.Confirmation, $"Confirmación reenviada al email <b>{user.Email}</b>.", string.Empty, true);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByNameAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.ApplicationUser.Remove(user);
            await _context.SaveChangesAsync();
            this._flashMessage.Queue(FlashMessageType.Confirmation, $"Usuario <b>{user.Email}</b> eliminado.", string.Empty, true);
            return RedirectToAction(nameof(Index));
        }

        private async Task SendEmail(ApplicationUser user, string password)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code },
                protocol: Request.Scheme);
            if (callbackUrl == null)
            {
                return;
            }

            await _emailSender.SendEmailAsync(user.Email, "Confirmar cuenta",
                    $"Para confirmar tu cuenta, accede al siguiente <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>enlace</a>." +
                    "<p>Su clave es <b>" + password + "</b> .</p>");
        }
    }
}