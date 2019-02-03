using Microsoft.EntityFrameworkCore;

namespace TrackingPedidos.Models
{
    public partial class TrackingContext : DbContext
    {
        public TrackingContext()
        {
        }

        public TrackingContext(DbContextOptions<TrackingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Entregas> Entregas { get; set; }
        public virtual DbSet<Pedidos> Pedidos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Database=Tracking;Username=postgres;Password=12345678");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entregas>(entity =>
            {
                entity.HasKey(e => e.EntId);

                entity.HasIndex(e => e.PedId)
                    .HasName("ak_k_ped_ent_entregas")
                    .IsUnique();

                entity.Property(e => e.EntId)
                    .HasColumnName("ent_id")
                    .HasDefaultValueSql("nextval('entregas_ent_id_seq'::regclass)");

                entity.Property(e => e.EntCelular)
                    .IsRequired()
                    .HasColumnName("ent_celular")
                    .HasMaxLength(10);

                entity.Property(e => e.EntPerApellidos)
                    .IsRequired()
                    .HasColumnName("ent_per_apellidos")
                    .HasMaxLength(30);

                entity.Property(e => e.EntPerIdentificacion)
                    .IsRequired()
                    .HasColumnName("ent_per_identificacion")
                    .HasMaxLength(10);

                entity.Property(e => e.EntPerNombres)
                    .IsRequired()
                    .HasColumnName("ent_per_nombres")
                    .HasMaxLength(30);

                entity.Property(e => e.PedId).HasColumnName("ped_id");

                entity.HasOne(d => d.Ped)
                    .WithOne(p => p.Entregas)
                    .HasForeignKey<Entregas>(d => d.PedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_entregas_pedidos");
            });

            modelBuilder.Entity<Pedidos>(entity =>
            {
                entity.HasKey(e => e.PedId);

                entity.Property(e => e.PedId)
                    .HasColumnName("ped_id")
                    .HasDefaultValueSql("nextval('pedidos_ped_id_seq'::regclass)");

                entity.Property(e => e.ClienteEmail)
                    .IsRequired()
                    .HasColumnName("cliente_email")
                    .HasMaxLength(50);

                entity.Property(e => e.InvoiceNumber)
                    .IsRequired()
                    .HasColumnName("invoice_number")
                    .HasMaxLength(20);

                entity.Property(e => e.PedCostoExtra)
                    .HasColumnName("ped_costo_extra")
                    .HasColumnType("numeric(4,2)");

                entity.Property(e => e.PedDescripcion).HasColumnName("ped_descripcion");

                entity.Property(e => e.PedDireccionEntrega)
                    .IsRequired()
                    .HasColumnName("ped_direccion_entrega")
                    .HasMaxLength(30);

                entity.Property(e => e.PedEnvioEstandar).HasColumnName("ped_envio_estandar");

                entity.Property(e => e.PedFase)
                    .HasColumnName("ped_fase")
                    .HasDefaultValueSql("'P'::bpchar");

                entity.Property(e => e.PedFechaCamino).HasColumnName("ped_fecha_camino");

                entity.Property(e => e.PedFechaDespachado).HasColumnName("ped_fecha_despachado");

                entity.Property(e => e.PedFechaEntrega).HasColumnName("ped_fecha_entrega");

                entity.Property(e => e.PedFechaEnvio).HasColumnName("ped_fecha_envio");

                entity.Property(e => e.PedFechaFin).HasColumnName("ped_fecha_fin");

                entity.Property(e => e.PedLugarDestino)
                    .IsRequired()
                    .HasColumnName("ped_lugar_destino")
                    .HasMaxLength(30);

                entity.Property(e => e.PedLugarOrigen)
                    .IsRequired()
                    .HasColumnName("ped_lugar_origen")
                    .HasMaxLength(30);

                entity.Property(e => e.PedRegalo).HasColumnName("ped_regalo");

                entity.Property(e => e.PedTarjeta).HasColumnName("ped_tarjeta");

                entity.Property(e => e.PedTotal)
                    .HasColumnName("ped_total")
                    .HasColumnType("numeric(8,2)");
            });

            modelBuilder.HasSequence<int>("entregas_ent_id_seq");

            modelBuilder.HasSequence<int>("pedidos_ped_id_seq");
        }
    }
}