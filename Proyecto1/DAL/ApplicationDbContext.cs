using Microsoft.EntityFrameworkCore;
using Proyecto1.Models;

namespace Proyecto1.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Colaborador> Colaboradores { get; set; }

        public DbSet<LoginUsuarioResult> LoginUsuarioResult { get; set; }

        public DbSet<UsuarioListado> UsuariosListado { get; set; }

        public DbSet<Asistencia> Asistencia { get; set; }

        public DbSet<AsistenciaAuditoria> AsistenciaAuditoria { get; set; }

        public DbSet<HoraExtra> HorasExtras { get; set; }

        public DbSet<HoraExtraAdminVM> HorasExtrasAdmin { get; set; }

        public DbSet<Permiso> Permisos { get; set; }

        public DbSet<PermisoDetalleVM> PermisosDetalle { get; set; }

        public DbSet<VacacionDetalleVM> VacacionesDetalle { get; set; }
        public DbSet<VacacionAdminVM> VacacionesAdmin { get; set; }
        public DbSet<VacacionColabVM> VacacionesColab { get; set; }

        public DbSet<Incapacidad> Incapacidades { get; set; }

        public DbSet<IncapacidadAdminVM> IncapacidadAdminVM { get; set; }
        public DbSet<IncapacidadColabVM> IncapacidadColabVM { get; set; }
        public DbSet<IncapacidadDetalleVM> IncapacidadDetalleVM { get; set; }

        public DbSet<PermisoColaboradorVM> PermisosColaborador { get; set; }

        public DbSet<Planilla> Planillas { get; set; }
        public DbSet<PlanillaDetalle> PlanillaDetalle { get; set; }

        public DbSet<PlanillaDetalleVM> PlanillaDetalleVM { get; set; }

        public DbSet<Aguinaldo> Aguinaldos { get; set; }

        public DbSet<Liquidacion> Liquidaciones { get; set; }
        public DbSet<LiquidacionDetalle> LiquidacionDetalle { get; set; }

        public DbSet<EntregaUniforme> EntregasUniforme { get; set; }
        public DbSet<TipoUniforme> TiposUniforme { get; set; }
        public DbSet<UniformeVM> UniformesVM { get; set; }
        public DbSet<UniformeHistorialVM> UniformeHistorialVM { get; set; }

        public DbSet<UniformeVM> Uniformes { get; set; }

        public DbSet<CriterioEvaluacion> CriteriosEvaluacion { get; set; }
        public DbSet<Evaluacion> Evaluaciones { get; set; }
        public DbSet<EvaluacionDetalle> EvaluacionesDetalle { get; set; }
        public DbSet<EvaluacionVM> EvaluacionVM { get; set; }

        public DbSet<ColaboradorVM> ColaboradoresEvaluacion { get; set; }






        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>().HasNoKey(); // Usuarios vía SP

            modelBuilder.Entity<LoginUsuarioResult>().HasNoKey();

            modelBuilder.Entity<Colaborador>()
                .HasKey(c => c.IdColaborador); // Colaboradores tabla real

            modelBuilder.Entity<UsuarioListado>().HasNoKey();

            modelBuilder.Entity<HoraExtraAdminVM>().HasNoKey(); //Este modelo viene de un SP o vista, no tiene PK, no intentes hacer tracking

            modelBuilder.Entity<HoraExtraDetalleVM>().HasNoKey();

            modelBuilder.Entity<PermisoDetalleVM>().HasNoKey();

            modelBuilder.Entity<PermisoAdminVM>().HasNoKey();

            modelBuilder.Entity<VacacionDetalleVM>().HasNoKey();

            modelBuilder.Entity<VacacionAdminVM>().HasNoKey();

            modelBuilder.Entity<VacacionColabVM>().HasNoKey();

            modelBuilder.Entity<DiasDisponiblesVM>().HasNoKey();

            modelBuilder.Entity<IncapacidadAdminVM>().HasNoKey();
            modelBuilder.Entity<IncapacidadColabVM>().HasNoKey();
            modelBuilder.Entity<IncapacidadDetalleVM>().HasNoKey();

            modelBuilder.Entity<PermisoColaboradorVM>().HasNoKey();

            modelBuilder.Entity<PlanillaDetalle>().HasNoKey();

            modelBuilder.Entity<PlanillaDetalleVM>().HasNoKey();

            modelBuilder.Entity<Aguinaldo>().HasNoKey();

            modelBuilder.Entity<LiquidacionVM>().HasNoKey();

            modelBuilder.Entity<LiquidacionDetalleVM>().HasNoKey();

            modelBuilder.Entity<ColaboradorVM>().HasNoKey();

            modelBuilder.Entity<TipoSalidaVM>().HasNoKey();


            modelBuilder.Entity<UniformeVM>().HasNoKey();
            modelBuilder.Entity<UniformeHistorialVM>().HasNoKey();

            modelBuilder.Entity<EvaluacionVM>().HasNoKey();

            modelBuilder.Entity<CriterioEvaluacion>()
            .ToTable("Cat_CriteriosEvaluacion")
            .HasKey(c => c.IdCriterio);


        }


    }
}
