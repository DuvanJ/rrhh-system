USE [master]
GO

GO

/****** Object:  Database [BD_RRHH]    Script Date: 12/10/2026 19:37:12 ******/
CREATE DATABASE [BD_RRHH]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BD_RRHH', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\BD_RRHH.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BD_RRHH_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\BD_RRHH_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [BD_RRHH] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BD_RRHH].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BD_RRHH] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BD_RRHH] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BD_RRHH] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BD_RRHH] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BD_RRHH] SET ARITHABORT OFF 
GO
ALTER DATABASE [BD_RRHH] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [BD_RRHH] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BD_RRHH] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BD_RRHH] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BD_RRHH] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BD_RRHH] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BD_RRHH] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BD_RRHH] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BD_RRHH] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BD_RRHH] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BD_RRHH] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BD_RRHH] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BD_RRHH] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BD_RRHH] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BD_RRHH] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BD_RRHH] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BD_RRHH] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BD_RRHH] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BD_RRHH] SET  MULTI_USER 
GO
ALTER DATABASE [BD_RRHH] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BD_RRHH] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BD_RRHH] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BD_RRHH] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BD_RRHH] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BD_RRHH] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [BD_RRHH] SET QUERY_STORE = OFF
GO

GO

USE [BD_RRHH]
GO

GO

/****** Object:  Table [dbo].[Aguinaldo]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Aguinaldo](
	[IdAguinaldo] [int] IDENTITY(1,1) NOT NULL,
	[IdColaborador] [int] NOT NULL,
	[Año] [int] NOT NULL,
	[TotalSalarios] [decimal](12, 2) NOT NULL,
	[MontoAguinaldo] [decimal](12, 2) NOT NULL,
	[FechaCalculo] [date] NOT NULL,
	[IdEstadoAguinaldo] [int] NOT NULL,
	[FechaPago] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdAguinaldo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Aguinaldo] UNIQUE NONCLUSTERED 
(
	[IdColaborador] ASC,
	[Año] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Asistencia]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Asistencia](
	[IdAsistencia] [int] IDENTITY(1,1) NOT NULL,
	[IdColaborador] [int] NOT NULL,
	[Fecha] [date] NOT NULL,
	[HoraEntrada] [time](7) NULL,
	[HoraSalida] [time](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdAsistencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[AsistenciaAuditoria]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AsistenciaAuditoria](
	[IdAuditoria] [int] IDENTITY(1,1) NOT NULL,
	[IdAsistencia] [int] NOT NULL,
	[IdColaborador] [int] NOT NULL,
	[Fecha] [date] NOT NULL,
	[HoraEntradaAnterior] [time](7) NULL,
	[HoraSalidaAnterior] [time](7) NULL,
	[HoraEntradaNueva] [time](7) NULL,
	[HoraSalidaNueva] [time](7) NULL,
	[UsuarioModifica] [varchar](100) NOT NULL,
	[FechaModificacion] [datetime] NOT NULL,
	[Motivo] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdAuditoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Cat_CriteriosEvaluacion]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cat_CriteriosEvaluacion](
	[IdCriterio] [int] IDENTITY(1,1) NOT NULL,
	[NombreCriterio] [varchar](100) NOT NULL,
	[Descripcion] [varchar](250) NULL,
	[Estado] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCriterio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Cat_DeduccionesObligatorias]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cat_DeduccionesObligatorias](
	[IdDeduccion] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](150) NOT NULL,
	[TipoAporte] [varchar](20) NOT NULL,
	[PorcentajePatrono] [decimal](5, 2) NOT NULL,
	[PorcentajeTrabajador] [decimal](5, 2) NOT NULL,
	[AplicaSobre] [varchar](50) NOT NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdDeduccion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Cat_EstadoPlanilla]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cat_EstadoPlanilla](
	[IdEstadoPlanilla] [int] NOT NULL,
	[Nombre] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEstadoPlanilla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Cat_TipoSalida]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cat_TipoSalida](
	[IdTipoSalida] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[AplicaCesantia] [bit] NOT NULL,
	[AplicaPreaviso] [bit] NOT NULL,
	[Activo] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdTipoSalida] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Cat_TiposIncapacidad]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cat_TiposIncapacidad](
	[IdTipoIncapacidad] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](20) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Entidad] [varchar](100) NOT NULL,
	[EsRemunerada] [bit] NOT NULL,
	[DescripcionPago] [varchar](200) NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdTipoIncapacidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Cat_TiposUniforme]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cat_TiposUniforme](
	[IdTipoUniforme] [int] IDENTITY(1,1) NOT NULL,
	[NombreTipo] [varchar](100) NOT NULL,
	[Descripcion] [varchar](255) NULL,
	[EntregaAutomatica] [bit] NULL,
	[MesesParaEntrega] [int] NULL,
	[CantidadPorEntrega] [int] NULL,
	[FrecuenciaMeses] [int] NULL,
	[EsOpcional] [bit] NULL,
	[PermiteCobro] [bit] NULL,
	[Activo] [bit] NULL,
	[FechaCreacion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdTipoUniforme] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Cat_TramosRenta]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cat_TramosRenta](
	[IdTramoRenta] [int] IDENTITY(1,1) NOT NULL,
	[LimiteInferior] [decimal](18, 2) NOT NULL,
	[LimiteSuperior] [decimal](18, 2) NULL,
	[Porcentaje] [decimal](5, 2) NOT NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdTramoRenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Colaboradores]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Colaboradores](
	[IdColaborador] [int] IDENTITY(1,1) NOT NULL,
	[Cedula] [varchar](20) NOT NULL,
	[Nombre] [varchar](80) NOT NULL,
	[Apellidos] [varchar](120) NOT NULL,
	[Puesto] [varchar](80) NULL,
	[Departamento] [varchar](80) NULL,
	[FechaIngreso] [date] NOT NULL,
	[SalarioBase] [decimal](10, 2) NOT NULL,
	[Correo] [varchar](120) NULL,
	[Estado] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdColaborador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Cedula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[DescansosColaborador]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DescansosColaborador](
	[IdDescanso] [int] IDENTITY(1,1) NOT NULL,
	[IdColaborador] [int] NOT NULL,
	[FechaDescanso] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdDescanso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[EntregasUniforme]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntregasUniforme](
	[IdEntrega] [int] IDENTITY(1,1) NOT NULL,
	[IdColaborador] [int] NOT NULL,
	[IdTipoUniforme] [int] NOT NULL,
	[Talla] [varchar](10) NOT NULL,
	[Cantidad] [int] NOT NULL,
	[FechaEntrega] [date] NOT NULL,
	[Costo] [decimal](10, 2) NULL,
	[CobrarEnPlanilla] [bit] NULL,
	[Observaciones] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEntrega] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[EstadoAguinaldo]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoAguinaldo](
	[IdEstadoAguinaldo] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEstadoAguinaldo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Evaluacion]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Evaluacion](
	[IdEvaluacion] [int] IDENTITY(1,1) NOT NULL,
	[IdColaborador] [int] NOT NULL,
	[FechaEvaluacion] [date] NOT NULL,
	[ComentariosGenerales] [varchar](300) NULL,
	[PuntajeTotal] [decimal](5, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEvaluacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[EvaluacionDetalle]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EvaluacionDetalle](
	[IdEvaluacionDetalle] [int] IDENTITY(1,1) NOT NULL,
	[IdEvaluacion] [int] NOT NULL,
	[IdCriterio] [int] NOT NULL,
	[Puntaje] [decimal](5, 2) NOT NULL,
	[Observacion] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEvaluacionDetalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Feriados]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feriados](
	[IdFeriado] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [date] NULL,
	[Descripcion] [varchar](100) NULL,
	[EsObligatorio] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdFeriado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[HorasExtras]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HorasExtras](
	[IdHoraExtra] [int] IDENTITY(1,1) NOT NULL,
	[IdColaborador] [int] NOT NULL,
	[Fecha] [date] NOT NULL,
	[CantidadHoras] [decimal](5, 2) NOT NULL,
	[Motivo] [varchar](200) NULL,
	[Estado] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdHoraExtra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Incapacidades]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Incapacidades](
	[IdIncapacidad] [int] IDENTITY(1,1) NOT NULL,
	[IdColaborador] [int] NOT NULL,
	[FechaInicio] [date] NOT NULL,
	[FechaFin] [date] NOT NULL,
	[DiasIncapacidad] [int] NOT NULL,
	[EsRemunerada] [bit] NOT NULL,
	[Motivo] [varchar](200) NULL,
	[DocumentoAdjunto] [varchar](250) NULL,
	[FechaRegistro] [datetime] NOT NULL,
	[Estado] [varchar](20) NOT NULL,
	[IdTipoIncapacidad] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdIncapacidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[LiquidacionDetalle]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LiquidacionDetalle](
	[IdDetalle] [int] IDENTITY(1,1) NOT NULL,
	[IdLiquidacion] [int] NULL,
	[Rubro] [varchar](100) NULL,
	[Monto] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdDetalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Liquidaciones]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Liquidaciones](
	[IdLiquidacion] [int] IDENTITY(1,1) NOT NULL,
	[IdColaborador] [int] NOT NULL,
	[IdTipoSalida] [int] NOT NULL,
	[FechaSalida] [date] NOT NULL,
	[MontoTotal] [decimal](18, 2) NOT NULL,
	[Estado] [varchar](20) NULL,
	[UsuarioRegistro] [varchar](50) NULL,
	[FechaRegistro] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdLiquidacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Perfiles]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Perfiles](
	[IdPerfil] [int] IDENTITY(1,1) NOT NULL,
	[NombrePerfil] [varchar](50) NOT NULL,
	[Descripcion] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPerfil] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Permisos]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permisos](
	[IdPermiso] [int] IDENTITY(1,1) NOT NULL,
	[IdColaborador] [int] NOT NULL,
	[TipoPermiso] [varchar](50) NOT NULL,
	[FechaSolicitud] [date] NOT NULL,
	[FechaInicio] [date] NOT NULL,
	[FechaFin] [date] NOT NULL,
	[Estado] [varchar](20) NOT NULL,
	[Observaciones] [varchar](250) NULL,
	[EsConGoce] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPermiso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[PlanillaDetalle]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlanillaDetalle](
	[IdPlanillaDetalle] [int] IDENTITY(1,1) NOT NULL,
	[IdPlanilla] [int] NOT NULL,
	[IdColaborador] [int] NOT NULL,
	[SalarioBase] [decimal](10, 2) NOT NULL,
	[DiasLaborados] [int] NOT NULL,
	[HorasOrdinarias] [decimal](10, 2) NOT NULL,
	[HorasExtras] [decimal](10, 2) NULL,
	[MontoHorasExtras] [decimal](10, 2) NULL,
	[Deducciones] [decimal](10, 2) NOT NULL,
	[SalarioBruto] [decimal](10, 2) NOT NULL,
	[SalarioNeto] [decimal](10, 2) NOT NULL,
	[AlertaDiasIncompletos] [bit] NOT NULL,
	[DiasSinGoce] [int] NULL,
	[MontoRebajoSinGoce] [decimal](18, 2) NOT NULL,
	[DiasConGoce] [int] NULL,
	[DiasVacaciones] [int] NOT NULL,
	[DiasIncapacidad] [int] NOT NULL,
	[MontoIncapacidadPatrono] [decimal](18, 2) NOT NULL,
	[MontoIncapacidadCCSS] [decimal](18, 2) NOT NULL,
	[MontoIncapacidadINS] [decimal](18, 2) NOT NULL,
	[MontoRebajoIncapacidad] [decimal](18, 2) NOT NULL,
	[MontoUniformes] [decimal](10, 2) NOT NULL,
	[MontoRebajoAsistencia] [decimal](18, 2) NOT NULL,
	[SubsidioIncapacidad] [decimal](18, 2) NOT NULL,
	[AfectacionRealIncapacidad] [decimal](18, 2) NOT NULL,
	[HorasFeriado] [decimal](10, 2) NULL,
	[MontoFeriado] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPlanillaDetalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[PlanillaDetalleDeducciones]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlanillaDetalleDeducciones](
	[IdDetalleDeduccion] [int] IDENTITY(1,1) NOT NULL,
	[IdPlanillaDetalle] [int] NOT NULL,
	[IdDeduccion] [int] NOT NULL,
	[PorcentajeAplicadoPatrono] [decimal](5, 2) NOT NULL,
	[PorcentajeAplicadoTrabajador] [decimal](5, 2) NOT NULL,
	[MontoPatrono] [decimal](18, 2) NOT NULL,
	[MontoTrabajador] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdDetalleDeduccion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Planillas]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Planillas](
	[IdPlanilla] [int] IDENTITY(1,1) NOT NULL,
	[Mes] [int] NOT NULL,
	[Año] [int] NOT NULL,
	[FechaGeneracion] [date] NOT NULL,
	[Estado] [int] NOT NULL,
	[Observacion] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPlanilla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Usuarios]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[NombreUsuario] [varchar](50) NOT NULL,
	[Correo] [varchar](100) NOT NULL,
	[ContrasenaHash] [varchar](200) NOT NULL,
	[IdPerfil] [int] NOT NULL,
	[Estado] [bit] NULL,
	[IdColaborador] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[NombreUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Table [dbo].[Vacaciones]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vacaciones](
	[IdVacacion] [int] IDENTITY(1,1) NOT NULL,
	[IdColaborador] [int] NOT NULL,
	[FechaSolicitud] [date] NOT NULL,
	[FechaInicio] [date] NOT NULL,
	[FechaFin] [date] NOT NULL,
	[DiasSolicitados] [int] NOT NULL,
	[Estado] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdVacacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

/****** Object:  Index [UX_Descanso_Unico]    Script Date: 12/10/2026 19:37:13 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Descanso_Unico] ON [dbo].[DescansosColaborador]
(
	[IdColaborador] ASC,
	[FechaDescanso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

GO

ALTER TABLE [dbo].[Aguinaldo] ADD  DEFAULT ((1)) FOR [IdEstadoAguinaldo]
GO

GO

ALTER TABLE [dbo].[AsistenciaAuditoria] ADD  DEFAULT (getdate()) FOR [FechaModificacion]
GO

GO

ALTER TABLE [dbo].[Cat_CriteriosEvaluacion] ADD  DEFAULT ((1)) FOR [Estado]
GO

GO

ALTER TABLE [dbo].[Cat_DeduccionesObligatorias] ADD  DEFAULT ((0)) FOR [PorcentajePatrono]
GO

GO

ALTER TABLE [dbo].[Cat_DeduccionesObligatorias] ADD  DEFAULT ((0)) FOR [PorcentajeTrabajador]
GO

GO

ALTER TABLE [dbo].[Cat_DeduccionesObligatorias] ADD  DEFAULT ('SalarioBruto') FOR [AplicaSobre]
GO

GO

ALTER TABLE [dbo].[Cat_DeduccionesObligatorias] ADD  DEFAULT ((1)) FOR [Estado]
GO

GO

ALTER TABLE [dbo].[Cat_TipoSalida] ADD  DEFAULT ((1)) FOR [Activo]
GO

GO

ALTER TABLE [dbo].[Cat_TiposIncapacidad] ADD  DEFAULT ((1)) FOR [Estado]
GO

GO

ALTER TABLE [dbo].[Cat_TiposUniforme] ADD  DEFAULT ((0)) FOR [EntregaAutomatica]
GO

GO

ALTER TABLE [dbo].[Cat_TiposUniforme] ADD  DEFAULT ((0)) FOR [EsOpcional]
GO

GO

ALTER TABLE [dbo].[Cat_TiposUniforme] ADD  DEFAULT ((0)) FOR [PermiteCobro]
GO

GO

ALTER TABLE [dbo].[Cat_TiposUniforme] ADD  DEFAULT ((1)) FOR [Activo]
GO

GO

ALTER TABLE [dbo].[Cat_TiposUniforme] ADD  DEFAULT (getdate()) FOR [FechaCreacion]
GO

GO

ALTER TABLE [dbo].[Cat_TramosRenta] ADD  DEFAULT ((1)) FOR [Estado]
GO

GO

ALTER TABLE [dbo].[Colaboradores] ADD  DEFAULT ((1)) FOR [Estado]
GO

GO

ALTER TABLE [dbo].[EntregasUniforme] ADD  DEFAULT ((0)) FOR [CobrarEnPlanilla]
GO

GO

ALTER TABLE [dbo].[Incapacidades] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO

GO

ALTER TABLE [dbo].[Incapacidades] ADD  DEFAULT ('Activa') FOR [Estado]
GO

GO

ALTER TABLE [dbo].[Liquidaciones] ADD  DEFAULT ('Calculado') FOR [Estado]
GO

GO

ALTER TABLE [dbo].[Liquidaciones] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO

GO

ALTER TABLE [dbo].[Permisos] ADD  DEFAULT (getdate()) FOR [FechaSolicitud]
GO

GO

ALTER TABLE [dbo].[Permisos] ADD  DEFAULT ('Pendiente') FOR [Estado]
GO

GO

ALTER TABLE [dbo].[Permisos] ADD  CONSTRAINT [DF_Permisos_EsConGoce]  DEFAULT ((1)) FOR [EsConGoce]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle] ADD  DEFAULT ((0)) FOR [HorasExtras]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle] ADD  DEFAULT ((0)) FOR [MontoHorasExtras]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle] ADD  CONSTRAINT [DF_PlanillaDetalle_AlertaDiasIncompletos]  DEFAULT ((0)) FOR [AlertaDiasIncompletos]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle] ADD  DEFAULT ((0)) FOR [DiasSinGoce]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle] ADD  DEFAULT ((0)) FOR [MontoRebajoSinGoce]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle] ADD  DEFAULT ((0)) FOR [DiasConGoce]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle] ADD  DEFAULT ((0)) FOR [DiasVacaciones]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle] ADD  DEFAULT ((0)) FOR [DiasIncapacidad]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle] ADD  DEFAULT ((0)) FOR [MontoIncapacidadPatrono]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle] ADD  DEFAULT ((0)) FOR [MontoIncapacidadCCSS]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle] ADD  DEFAULT ((0)) FOR [MontoIncapacidadINS]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle] ADD  DEFAULT ((0)) FOR [MontoRebajoIncapacidad]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle] ADD  DEFAULT ((0)) FOR [MontoUniformes]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle] ADD  DEFAULT ((0)) FOR [MontoRebajoAsistencia]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle] ADD  DEFAULT ((0)) FOR [SubsidioIncapacidad]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle] ADD  DEFAULT ((0)) FOR [AfectacionRealIncapacidad]
GO

GO

ALTER TABLE [dbo].[Planillas] ADD  CONSTRAINT [DF_Planillas_Estado]  DEFAULT ((1)) FOR [Estado]
GO

GO

ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT ((1)) FOR [Estado]
GO

GO

ALTER TABLE [dbo].[Aguinaldo]  WITH CHECK ADD  CONSTRAINT [FK_Aguinaldo_Colaborador] FOREIGN KEY([IdColaborador])
REFERENCES [dbo].[Colaboradores] ([IdColaborador])
GO
ALTER TABLE [dbo].[Aguinaldo] CHECK CONSTRAINT [FK_Aguinaldo_Colaborador]
GO

GO

ALTER TABLE [dbo].[Aguinaldo]  WITH CHECK ADD  CONSTRAINT [FK_Aguinaldo_Estado] FOREIGN KEY([IdEstadoAguinaldo])
REFERENCES [dbo].[EstadoAguinaldo] ([IdEstadoAguinaldo])
GO
ALTER TABLE [dbo].[Aguinaldo] CHECK CONSTRAINT [FK_Aguinaldo_Estado]
GO

GO

ALTER TABLE [dbo].[Asistencia]  WITH CHECK ADD FOREIGN KEY([IdColaborador])
REFERENCES [dbo].[Colaboradores] ([IdColaborador])
GO

GO

ALTER TABLE [dbo].[DescansosColaborador]  WITH CHECK ADD  CONSTRAINT [FK_Descanso_Colaborador] FOREIGN KEY([IdColaborador])
REFERENCES [dbo].[Colaboradores] ([IdColaborador])
GO
ALTER TABLE [dbo].[DescansosColaborador] CHECK CONSTRAINT [FK_Descanso_Colaborador]
GO

GO

ALTER TABLE [dbo].[EntregasUniforme]  WITH CHECK ADD FOREIGN KEY([IdColaborador])
REFERENCES [dbo].[Colaboradores] ([IdColaborador])
GO

GO

ALTER TABLE [dbo].[EntregasUniforme]  WITH CHECK ADD FOREIGN KEY([IdTipoUniforme])
REFERENCES [dbo].[Cat_TiposUniforme] ([IdTipoUniforme])
GO

GO

ALTER TABLE [dbo].[Evaluacion]  WITH CHECK ADD FOREIGN KEY([IdColaborador])
REFERENCES [dbo].[Colaboradores] ([IdColaborador])
GO

GO

ALTER TABLE [dbo].[EvaluacionDetalle]  WITH CHECK ADD FOREIGN KEY([IdCriterio])
REFERENCES [dbo].[Cat_CriteriosEvaluacion] ([IdCriterio])
GO

GO

ALTER TABLE [dbo].[EvaluacionDetalle]  WITH CHECK ADD FOREIGN KEY([IdEvaluacion])
REFERENCES [dbo].[Evaluacion] ([IdEvaluacion])
GO

GO

ALTER TABLE [dbo].[HorasExtras]  WITH CHECK ADD FOREIGN KEY([IdColaborador])
REFERENCES [dbo].[Colaboradores] ([IdColaborador])
GO

GO

ALTER TABLE [dbo].[Incapacidades]  WITH CHECK ADD FOREIGN KEY([IdColaborador])
REFERENCES [dbo].[Colaboradores] ([IdColaborador])
GO

GO

ALTER TABLE [dbo].[Incapacidades]  WITH CHECK ADD  CONSTRAINT [FK_Incapacidades_Cat_Tipos] FOREIGN KEY([IdTipoIncapacidad])
REFERENCES [dbo].[Cat_TiposIncapacidad] ([IdTipoIncapacidad])
GO
ALTER TABLE [dbo].[Incapacidades] CHECK CONSTRAINT [FK_Incapacidades_Cat_Tipos]
GO

GO

ALTER TABLE [dbo].[LiquidacionDetalle]  WITH CHECK ADD FOREIGN KEY([IdLiquidacion])
REFERENCES [dbo].[Liquidaciones] ([IdLiquidacion])
GO

GO

ALTER TABLE [dbo].[Liquidaciones]  WITH CHECK ADD FOREIGN KEY([IdColaborador])
REFERENCES [dbo].[Colaboradores] ([IdColaborador])
GO

GO

ALTER TABLE [dbo].[Liquidaciones]  WITH CHECK ADD FOREIGN KEY([IdTipoSalida])
REFERENCES [dbo].[Cat_TipoSalida] ([IdTipoSalida])
GO

GO

ALTER TABLE [dbo].[Permisos]  WITH CHECK ADD  CONSTRAINT [FK_Permisos_Colaboradores] FOREIGN KEY([IdColaborador])
REFERENCES [dbo].[Colaboradores] ([IdColaborador])
GO
ALTER TABLE [dbo].[Permisos] CHECK CONSTRAINT [FK_Permisos_Colaboradores]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle]  WITH CHECK ADD FOREIGN KEY([IdColaborador])
REFERENCES [dbo].[Colaboradores] ([IdColaborador])
GO

GO

ALTER TABLE [dbo].[PlanillaDetalle]  WITH CHECK ADD FOREIGN KEY([IdPlanilla])
REFERENCES [dbo].[Planillas] ([IdPlanilla])
GO

GO

ALTER TABLE [dbo].[PlanillaDetalleDeducciones]  WITH CHECK ADD  CONSTRAINT [FK_PD_Deduccion] FOREIGN KEY([IdDeduccion])
REFERENCES [dbo].[Cat_DeduccionesObligatorias] ([IdDeduccion])
GO
ALTER TABLE [dbo].[PlanillaDetalleDeducciones] CHECK CONSTRAINT [FK_PD_Deduccion]
GO

GO

ALTER TABLE [dbo].[PlanillaDetalleDeducciones]  WITH CHECK ADD  CONSTRAINT [FK_PD_PlanillaDetalle] FOREIGN KEY([IdPlanillaDetalle])
REFERENCES [dbo].[PlanillaDetalle] ([IdPlanillaDetalle])
GO
ALTER TABLE [dbo].[PlanillaDetalleDeducciones] CHECK CONSTRAINT [FK_PD_PlanillaDetalle]
GO

GO

ALTER TABLE [dbo].[Planillas]  WITH CHECK ADD  CONSTRAINT [FK_Planillas_Estado] FOREIGN KEY([Estado])
REFERENCES [dbo].[Cat_EstadoPlanilla] ([IdEstadoPlanilla])
GO
ALTER TABLE [dbo].[Planillas] CHECK CONSTRAINT [FK_Planillas_Estado]
GO

GO

ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD FOREIGN KEY([IdPerfil])
REFERENCES [dbo].[Perfiles] ([IdPerfil])
GO

GO

ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_Colaboradores] FOREIGN KEY([IdColaborador])
REFERENCES [dbo].[Colaboradores] ([IdColaborador])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_Colaboradores]
GO

GO

ALTER TABLE [dbo].[Vacaciones]  WITH CHECK ADD FOREIGN KEY([IdColaborador])
REFERENCES [dbo].[Colaboradores] ([IdColaborador])
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_ActualizarColaborador]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_ActualizarColaborador]
    @IdColaborador INT,
    @Cedula VARCHAR(20),
    @Nombre VARCHAR(80),
    @Apellidos VARCHAR(120),
    @Puesto VARCHAR(80),
    @Departamento VARCHAR(80),
    @FechaIngreso DATE,
    @SalarioBase DECIMAL(10,2),
    @Correo VARCHAR(120),
    @Estado BIT
AS
BEGIN

    -- Actualizar colaborador
    UPDATE Colaboradores
    SET Cedula = @Cedula,
        Nombre = @Nombre,
        Apellidos = @Apellidos,
        Puesto = @Puesto,
        Departamento = @Departamento,
        FechaIngreso = @FechaIngreso,
        SalarioBase = @SalarioBase,
        Correo = @Correo,
        Estado = @Estado
    WHERE IdColaborador = @IdColaborador

    -- Sincronizar correo en Usuarios
    UPDATE Usuarios
    SET Correo = @Correo
    WHERE IdColaborador = @IdColaborador

END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Aguinaldo_Aprobar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Aguinaldo_Aprobar]
    @IdAguinaldo INT
AS
BEGIN
    UPDATE Aguinaldo
    SET IdEstadoAguinaldo = 2 -- Aprobado
    WHERE IdAguinaldo = @IdAguinaldo;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Aguinaldo_Calcular]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Aguinaldo_Calcular]
    @Año INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AñoInicio INT = @Año - 1;

    INSERT INTO Aguinaldo (IdColaborador, Año, TotalSalarios, MontoAguinaldo, FechaCalculo, IdEstadoAguinaldo)
    SELECT 
        pd.IdColaborador,
        @Año AS Año,
        SUM(pd.SalarioBruto) AS TotalSalarios,
        SUM(pd.SalarioBruto) / 12.0 AS MontoAguinaldo,
        GETDATE() AS FechaCalculo,
        1 AS IdEstadoAguinaldo -- Calculado
    FROM Planillas p
    INNER JOIN PlanillaDetalle pd ON p.IdPlanilla = pd.IdPlanilla
    WHERE 
        (
            (p.Año = @AñoInicio AND p.Mes = 12)
            OR
            (p.Año = @Año AND p.Mes BETWEEN 1 AND 11)
        )
        AND NOT EXISTS (
            SELECT 1 
            FROM Aguinaldo a 
            WHERE a.IdColaborador = pd.IdColaborador 
              AND a.Año = @Año
        )
    GROUP BY pd.IdColaborador;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Aguinaldo_Existe]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_Aguinaldo_Existe]
    @Año INT
AS
BEGIN
    SELECT COUNT(1)
    FROM Aguinaldo
    WHERE Año = @Año;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Aguinaldo_HayDatos]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_Aguinaldo_HayDatos]
    @Año INT
AS
BEGIN
    SELECT COUNT(1)
    FROM Planillas p
    JOIN PlanillaDetalle d ON p.IdPlanilla = d.IdPlanilla
    WHERE p.Año = @Año AND p.Estado = 1;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Aguinaldo_Listar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Aguinaldo_Listar]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        a.IdAguinaldo,
        a.IdColaborador,
        c.Nombre + ' ' + c.Apellidos AS NombreColaborador, -- ✅ CAMBIO AQUÍ
        a.Año,
        a.TotalSalarios,
        a.MontoAguinaldo,
        a.IdEstadoAguinaldo,
        ea.Nombre AS Estado,
        a.FechaCalculo,
		a.FechaPago
    FROM Aguinaldo a
    INNER JOIN Colaboradores c ON a.IdColaborador = c.IdColaborador
    INNER JOIN EstadoAguinaldo ea ON a.IdEstadoAguinaldo = ea.IdEstadoAguinaldo;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Aguinaldo_ListarPorAnio]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Aguinaldo_ListarPorAnio]
    @Año INT
AS
BEGIN
    SELECT 
        a.IdAguinaldo,
        a.IdColaborador,
        c.Nombre + ' ' + c.Apellidos AS NombreColaborador,
        a.Año,
        a.TotalSalarios,
        a.MontoAguinaldo,
        a.FechaCalculo,
		a.FechaPago,
        a.IdEstadoAguinaldo,
        e.Nombre AS Estado
    FROM Aguinaldo a
    INNER JOIN Colaboradores c ON a.IdColaborador = c.IdColaborador
    INNER JOIN EstadoAguinaldo e ON a.IdEstadoAguinaldo = e.IdEstadoAguinaldo
    WHERE a.Año = @Año;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Aguinaldo_Pagar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Aguinaldo_Pagar]
    @IdAguinaldo INT
AS
BEGIN

UPDATE Aguinaldo
SET 
    IdEstadoAguinaldo = 3, -- Pagado
    FechaPago = GETDATE()
WHERE IdAguinaldo = @IdAguinaldo;

END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Aguinaldo_PagarMasivo]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Aguinaldo_PagarMasivo]
    @Año INT
AS
BEGIN

UPDATE Aguinaldo
SET 
    IdEstadoAguinaldo = 3,
    FechaPago = GETDATE()
WHERE Año = @Año
AND IdEstadoAguinaldo = 2; -- Aprobado

END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Asistencia_Actualizar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Asistencia_Actualizar]
    @IdAsistencia INT,
    @HoraEntrada TIME,
    @HoraSalida TIME
AS
BEGIN
    UPDATE Asistencia
    SET
        HoraEntrada = @HoraEntrada,
        HoraSalida = @HoraSalida
    WHERE IdAsistencia = @IdAsistencia
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Asistencia_AuditarAjuste]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Asistencia_AuditarAjuste]
    @IdAsistencia INT,
    @UsuarioModifica VARCHAR(100),
    @HoraEntradaNueva TIME,
    @HoraSalidaNueva TIME,
    @Motivo VARCHAR(250)
AS
BEGIN
    DECLARE 
        @HoraEntradaAnterior TIME,
        @HoraSalidaAnterior TIME,
        @IdColaborador INT,
        @Fecha DATE;

    SELECT
        @HoraEntradaAnterior = HoraEntrada,
        @HoraSalidaAnterior = HoraSalida,
        @IdColaborador = IdColaborador,
        @Fecha = Fecha
    FROM Asistencia
    WHERE IdAsistencia = @IdAsistencia;

    INSERT INTO AsistenciaAuditoria (
        IdAsistencia,
        IdColaborador,
        Fecha,
        HoraEntradaAnterior,
        HoraSalidaAnterior,
        HoraEntradaNueva,
        HoraSalidaNueva,
        UsuarioModifica,
        Motivo
    )
    VALUES (
        @IdAsistencia,
        @IdColaborador,
        @Fecha,
        @HoraEntradaAnterior,
        @HoraSalidaAnterior,
        @HoraEntradaNueva,
        @HoraSalidaNueva,
        @UsuarioModifica,
        @Motivo
    );
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Asistencia_Listar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Asistencia_Listar]
AS
BEGIN
    SELECT 
        a.IdAsistencia,
        a.IdColaborador,
        a.Fecha,
        a.HoraEntrada,
        a.HoraSalida,
        c.Nombre + ' ' + c.Apellidos AS NombreColaborador
    FROM Asistencia a
    INNER JOIN Colaboradores c ON c.IdColaborador = a.IdColaborador
    ORDER BY a.Fecha DESC
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Asistencia_ObtenerPorColaborador]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Asistencia_ObtenerPorColaborador]
    @IdColaborador INT,
    @FechaInicio DATE = NULL,
    @FechaFin DATE = NULL
AS
BEGIN
    SELECT
        a.IdAsistencia,
        a.IdColaborador,
        a.Fecha,
        a.HoraEntrada,
        a.HoraSalida,
        c.Nombre + ' ' + c.Apellidos AS NombreColaborador
    FROM Asistencia a
    INNER JOIN Colaboradores c ON c.IdColaborador = a.IdColaborador
    WHERE a.IdColaborador = @IdColaborador
      AND (@FechaInicio IS NULL OR a.Fecha >= @FechaInicio)
      AND (@FechaFin IS NULL OR a.Fecha <= @FechaFin)
    ORDER BY a.Fecha DESC
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Asistencia_ObtenerPorId]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Asistencia_ObtenerPorId]
    @IdAsistencia INT
AS
BEGIN
    SELECT
        a.IdAsistencia,
        a.IdColaborador,
        a.Fecha,
        a.HoraEntrada,
        a.HoraSalida,
        c.Nombre + ' ' + c.Apellidos AS NombreColaborador
    FROM Asistencia a
    INNER JOIN Colaboradores c ON c.IdColaborador = a.IdColaborador
    WHERE a.IdAsistencia = @IdAsistencia
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Asistencia_Registrar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Asistencia_Registrar]
    @IdColaborador INT,
    @Fecha DATE,
    @HoraEntrada TIME = NULL,
    @HoraSalida TIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ExisteEntrada BIT = 0;
    DECLARE @ExisteSalida BIT = 0;

    SELECT
        @ExisteEntrada = CASE WHEN HoraEntrada IS NOT NULL THEN 1 ELSE 0 END,
        @ExisteSalida = CASE WHEN HoraSalida IS NOT NULL THEN 1 ELSE 0 END
    FROM Asistencia
    WHERE IdColaborador = @IdColaborador
      AND Fecha = @Fecha;

    -- ❌ No permitir doble entrada
    IF (@HoraEntrada IS NOT NULL AND @ExisteEntrada = 1)
    BEGIN
        RAISERROR('Ya existe una entrada registrada para hoy.', 16, 1);
        RETURN;
    END

    -- ❌ No permitir salida sin entrada
    IF (@HoraSalida IS NOT NULL AND @ExisteEntrada = 0)
    BEGIN
        RAISERROR('No puede registrar salida sin una entrada previa.', 16, 1);
        RETURN;
    END

    -- ❌ No permitir doble salida
    IF (@HoraSalida IS NOT NULL AND @ExisteSalida = 1)
    BEGIN
        RAISERROR('Ya existe una salida registrada para hoy.', 16, 1);
        RETURN;
    END

    -- 👉 Si no existe registro, insertar
    IF NOT EXISTS (
        SELECT 1 FROM Asistencia
        WHERE IdColaborador = @IdColaborador
          AND Fecha = @Fecha
    )
    BEGIN
        INSERT INTO Asistencia (IdColaborador, Fecha, HoraEntrada, HoraSalida)
        VALUES (@IdColaborador, @Fecha, @HoraEntrada, @HoraSalida);
    END
    ELSE
    BEGIN
        UPDATE Asistencia
        SET
            HoraEntrada = ISNULL(@HoraEntrada, HoraEntrada),
            HoraSalida  = ISNULL(@HoraSalida, HoraSalida)
        WHERE IdColaborador = @IdColaborador
          AND Fecha = @Fecha;
    END
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_AsistenciaAuditoria_Listar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_AsistenciaAuditoria_Listar]
AS
BEGIN
    SELECT
        aa.IdAuditoria,
        aa.IdAsistencia,
        aa.IdColaborador,
        c.Nombre + ' ' + c.Apellidos AS NombreColaborador,
        aa.Fecha,

        aa.HoraEntradaAnterior,
        aa.HoraSalidaAnterior,
        aa.HoraEntradaNueva,
        aa.HoraSalidaNueva,

        aa.UsuarioModifica,
        aa.FechaModificacion,
        aa.Motivo
    FROM AsistenciaAuditoria aa
    INNER JOIN Colaboradores c ON c.IdColaborador = aa.IdColaborador
    ORDER BY aa.FechaModificacion DESC
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_EliminarColaborador]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_EliminarColaborador]
    @IdColaborador INT
AS
BEGIN
    UPDATE Colaboradores
    SET Estado = 0
    WHERE IdColaborador = @IdColaborador
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Evaluacion_Actualizar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Evaluacion_Actualizar]
    @IdEvaluacion INT,
    @ComentariosGenerales VARCHAR(300),
    @PuntajeTotal DECIMAL(5,2)
AS
BEGIN
    UPDATE Evaluacion
    SET 
        ComentariosGenerales = @ComentariosGenerales,
        PuntajeTotal = @PuntajeTotal
    WHERE IdEvaluacion = @IdEvaluacion;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Evaluacion_ActualizarDetalle]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Evaluacion_ActualizarDetalle]
    @IdEvaluacion INT,
    @IdCriterio INT,
    @Puntaje DECIMAL(5,2),
    @Observacion VARCHAR(250)
AS
BEGIN
    UPDATE EvaluacionDetalle
    SET 
        Puntaje = @Puntaje,
        Observacion = @Observacion
    WHERE IdEvaluacion = @IdEvaluacion
      AND IdCriterio = @IdCriterio;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Evaluacion_Eliminar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Evaluacion_Eliminar]
    @IdEvaluacion INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Eliminar detalles primero
    DELETE FROM EvaluacionDetalle
    WHERE IdEvaluacion = @IdEvaluacion;

    -- Luego eliminar la evaluación
    DELETE FROM Evaluacion
    WHERE IdEvaluacion = @IdEvaluacion;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Evaluacion_Insertar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Evaluacion_Insertar]
    @IdColaborador INT,
    @FechaEvaluacion DATE,
    @ComentariosGenerales VARCHAR(300),
    @PuntajeTotal DECIMAL(5,2),
    @IdEvaluacion INT OUTPUT
AS
BEGIN
    INSERT INTO Evaluacion (IdColaborador, FechaEvaluacion, ComentariosGenerales, PuntajeTotal)
    VALUES (@IdColaborador, @FechaEvaluacion, @ComentariosGenerales, @PuntajeTotal);

    SET @IdEvaluacion = SCOPE_IDENTITY();
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Evaluacion_InsertarDetalle]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_Evaluacion_InsertarDetalle]
    @IdEvaluacion INT,
    @IdCriterio INT,
    @Puntaje DECIMAL(5,2),
    @Observacion VARCHAR(250)
AS
BEGIN
    INSERT INTO EvaluacionDetalle (IdEvaluacion, IdCriterio, Puntaje, Observacion)
    VALUES (@IdEvaluacion, @IdCriterio, @Puntaje, @Observacion);
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Evaluacion_Listar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Evaluacion_Listar]
AS
BEGIN
    SELECT 
        e.IdEvaluacion,
        e.IdColaborador,
        c.Nombre AS NombreColaborador,
        e.FechaEvaluacion,
        e.ComentariosGenerales,
        e.PuntajeTotal
    FROM Evaluacion e
    INNER JOIN Colaboradores c 
        ON e.IdColaborador = c.IdColaborador
    ORDER BY e.FechaEvaluacion DESC;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Evaluacion_ListarColaboradores]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Evaluacion_ListarColaboradores]
AS
BEGIN
    SELECT IdColaborador, Nombre AS NombreCompleto
    FROM Colaboradores
    WHERE Estado = 1
    ORDER BY Nombre;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Evaluacion_ListarCriterios]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Evaluacion_ListarCriterios]
AS
BEGIN
    SELECT IdCriterio, NombreCriterio, Descripcion, Estado
    FROM Cat_CriteriosEvaluacion
    WHERE Estado = 1
    ORDER BY IdCriterio;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Evaluacion_ListarPorColaborador]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Evaluacion_ListarPorColaborador]
    @IdColaborador INT
AS
BEGIN
    SELECT 
        e.IdEvaluacion,
        e.IdColaborador,
        c.Nombre AS NombreColaborador,   -- ← agregar esto
        e.FechaEvaluacion,
        e.ComentariosGenerales,
        e.PuntajeTotal
    FROM Evaluacion e
    INNER JOIN Colaboradores c 
        ON c.IdColaborador = e.IdColaborador
    WHERE e.IdColaborador = @IdColaborador
    ORDER BY e.FechaEvaluacion DESC;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Evaluacion_ObtenerDetalles]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_Evaluacion_ObtenerDetalles]
    @IdEvaluacion INT
AS
BEGIN
    SELECT 
        d.IdCriterio,
        c.NombreCriterio,
        d.Puntaje,
        d.Observacion
    FROM EvaluacionDetalle d
    INNER JOIN Cat_CriteriosEvaluacion c 
        ON c.IdCriterio = d.IdCriterio
    WHERE d.IdEvaluacion = @IdEvaluacion;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Evaluacion_ObtenerPorId]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Evaluacion_ObtenerPorId]
    @IdEvaluacion INT
AS
BEGIN
    SELECT 
        e.IdEvaluacion,
        e.IdColaborador,
        c.Nombre AS NombreColaborador,
        e.FechaEvaluacion,
        e.ComentariosGenerales,
        e.PuntajeTotal
    FROM Evaluacion e
    INNER JOIN Colaboradores c ON c.IdColaborador = e.IdColaborador
    WHERE e.IdEvaluacion = @IdEvaluacion;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_HorasExtras_CambiarEstado]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_HorasExtras_CambiarEstado]
    @IdHoraExtra INT,
    @Estado VARCHAR(20)
AS
BEGIN
    UPDATE HorasExtras
    SET Estado = @Estado
    WHERE IdHoraExtra = @IdHoraExtra;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_HorasExtras_Listar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_HorasExtras_Listar]
AS
BEGIN
    SELECT
        h.IdHoraExtra,
        h.IdColaborador,
        c.Nombre + ' ' + c.Apellidos AS NombreColaborador,
        h.Fecha,
        h.CantidadHoras,
        h.Motivo,
        h.Estado
    FROM HorasExtras h
    INNER JOIN Colaboradores c ON c.IdColaborador = h.IdColaborador
    ORDER BY h.Fecha DESC;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_HorasExtras_ListarPorColaborador]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_HorasExtras_ListarPorColaborador]  
    @IdColaborador INT  
AS  
BEGIN  
    SELECT  
        h.IdHoraExtra,  
        h.IdColaborador, -- ✅ OBLIGATORIO
        h.Fecha,  
        h.CantidadHoras,  
        h.Motivo,  
        h.Estado  
    FROM HorasExtras h  
    WHERE h.IdColaborador = @IdColaborador  
    ORDER BY h.Fecha DESC;  
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_HorasExtras_ObtenerDetalle]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_HorasExtras_ObtenerDetalle]
    @IdHoraExtra INT
AS
BEGIN
    SELECT
        h.IdHoraExtra,
        h.IdColaborador,
        c.Nombre + ' ' + c.Apellidos AS NombreColaborador,
        c.Correo AS CorreoColaborador,
        h.Fecha,
        h.CantidadHoras
    FROM HorasExtras h
    INNER JOIN Colaboradores c ON h.IdColaborador = c.IdColaborador
    WHERE h.IdHoraExtra = @IdHoraExtra;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_HorasExtras_Registrar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_HorasExtras_Registrar]
    @IdColaborador INT,
    @Fecha DATE,
    @CantidadHoras DECIMAL(5,2),
    @Motivo VARCHAR(200)
AS
BEGIN
    INSERT INTO HorasExtras (
        IdColaborador,
        Fecha,
        CantidadHoras,
        Motivo,
        Estado
    )
    VALUES (
        @IdColaborador,
        @Fecha,
        @CantidadHoras,
        @Motivo,
        'Pendiente'
    );
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Incapacidad_Actualizar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Incapacidad_Actualizar]
    @IdIncapacidad INT,
    @IdTipoIncapacidad INT,
    @FechaInicio DATE,
    @FechaFin DATE,
    @Motivo VARCHAR(200) = NULL,
    @DocumentoAdjunto VARCHAR(250) = NULL,
    @Estado VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1 FROM Incapacidades WHERE IdIncapacidad = @IdIncapacidad
    )
    BEGIN
        RAISERROR('La incapacidad no existe.',16,1);
        RETURN;
    END

    IF (@FechaFin < @FechaInicio)
    BEGIN
        RAISERROR('La fecha de fin no puede ser menor a la fecha de inicio.',16,1);
        RETURN;
    END

    DECLARE @DiasIncapacidad INT =
        DATEDIFF(DAY,@FechaInicio,@FechaFin) + 1;

    DECLARE @EsRemunerada BIT;

    SELECT @EsRemunerada = EsRemunerada
    FROM Cat_TiposIncapacidad
    WHERE IdTipoIncapacidad = @IdTipoIncapacidad;

    UPDATE Incapacidades
    SET IdTipoIncapacidad = @IdTipoIncapacidad,
        FechaInicio = @FechaInicio,
        FechaFin = @FechaFin,
        DiasIncapacidad = @DiasIncapacidad,
        EsRemunerada = @EsRemunerada,
        Motivo = @Motivo,
        DocumentoAdjunto = @DocumentoAdjunto,
        Estado = @Estado
    WHERE IdIncapacidad = @IdIncapacidad;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Incapacidad_CambiarEstado]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Incapacidad_CambiarEstado]
    @IdIncapacidad INT,
    @Estado VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Incapacidades
    SET Estado = @Estado
    WHERE IdIncapacidad = @IdIncapacidad;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Incapacidad_ConsultarPorColaborador]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Incapacidad_ConsultarPorColaborador]
    @IdColaborador INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        i.IdIncapacidad,
        t.Nombre AS TipoIncapacidad,
        t.Entidad,
        i.FechaInicio,
        i.FechaFin,
        i.DiasIncapacidad,
        i.EsRemunerada,
        i.Motivo,
        i.DocumentoAdjunto,
        i.Estado,
        i.FechaRegistro
    FROM Incapacidades i
    INNER JOIN Cat_TiposIncapacidad t
        ON t.IdTipoIncapacidad = i.IdTipoIncapacidad
    WHERE i.IdColaborador = @IdColaborador
    ORDER BY i.FechaInicio DESC;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Incapacidad_ConsultarPorPeriodo]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Incapacidad_ConsultarPorPeriodo]
    @FechaInicio DATE,
    @FechaFin DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.Nombre,
        c.Apellidos,
        t.Nombre AS TipoIncapacidad,
        t.Entidad,
        i.FechaInicio,
        i.FechaFin,
        i.DiasIncapacidad,
        i.EsRemunerada
    FROM Incapacidades i
    INNER JOIN Colaboradores c 
        ON c.IdColaborador = i.IdColaborador
    INNER JOIN Cat_TiposIncapacidad t
        ON t.IdTipoIncapacidad = i.IdTipoIncapacidad
    WHERE i.FechaInicio >= @FechaInicio
      AND i.FechaFin <= @FechaFin
    ORDER BY c.Nombre;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Incapacidad_Insertar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Incapacidad_Insertar]
    @IdColaborador INT,
    @IdTipoIncapacidad INT,
    @FechaInicio DATE,
    @FechaFin DATE,
    @Motivo VARCHAR(200) = NULL,
    @DocumentoAdjunto VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF (@FechaFin < @FechaInicio)
    BEGIN
        RAISERROR('La fecha de fin no puede ser menor a la fecha de inicio.',16,1);
        RETURN;
    END

    IF NOT EXISTS (
        SELECT 1 FROM Colaboradores
        WHERE IdColaborador = @IdColaborador AND Estado = 1
    )
    BEGIN
        RAISERROR('El colaborador no existe o no está activo.',16,1);
        RETURN;
    END

    IF NOT EXISTS (
        SELECT 1 FROM Cat_TiposIncapacidad
        WHERE IdTipoIncapacidad = @IdTipoIncapacidad
        AND Estado = 1
    )
    BEGIN
        RAISERROR('El tipo de incapacidad no es válido.',16,1);
        RETURN;
    END

    IF EXISTS (
        SELECT 1 FROM Incapacidades
        WHERE IdColaborador = @IdColaborador
        AND Estado = 'Activa'
        AND (
              @FechaInicio BETWEEN FechaInicio AND FechaFin
           OR @FechaFin BETWEEN FechaInicio AND FechaFin
        )
    )
    BEGIN
        RAISERROR('Ya existe una incapacidad activa en ese período.',16,1);
        RETURN;
    END

    DECLARE @DiasIncapacidad INT =
        DATEDIFF(DAY,@FechaInicio,@FechaFin) + 1;

    DECLARE @EsRemunerada BIT;

    SELECT @EsRemunerada = EsRemunerada
    FROM Cat_TiposIncapacidad
    WHERE IdTipoIncapacidad = @IdTipoIncapacidad;

    INSERT INTO Incapacidades(
        IdColaborador,
        FechaInicio,
        FechaFin,
        DiasIncapacidad,
        EsRemunerada,
        Motivo,
        DocumentoAdjunto,
        IdTipoIncapacidad
    )
    VALUES(
        @IdColaborador,
        @FechaInicio,
        @FechaFin,
        @DiasIncapacidad,
        @EsRemunerada,
        @Motivo,
        @DocumentoAdjunto,
        @IdTipoIncapacidad
    );
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Incapacidad_Listar_Admin]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Incapacidad_Listar_Admin]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        i.IdIncapacidad,
        i.IdColaborador,
        c.Nombre + ' ' + c.Apellidos AS NombreColaborador,
        t.Codigo,
        t.Nombre AS TipoIncapacidad,
        t.Entidad,
        i.FechaInicio,
        i.FechaFin,
        i.DiasIncapacidad,
        i.EsRemunerada,
        i.Estado
    FROM Incapacidades i
    INNER JOIN Colaboradores c 
        ON c.IdColaborador = i.IdColaborador
    INNER JOIN Cat_TiposIncapacidad t
        ON t.IdTipoIncapacidad = i.IdTipoIncapacidad
    ORDER BY i.FechaRegistro DESC;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Incapacidad_ObtenerDetalle]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Incapacidad_ObtenerDetalle]
    @IdIncapacidad INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        i.IdIncapacidad,
        c.Nombre + ' ' + c.Apellidos AS NombreColaborador,
        t.Codigo,
        t.Nombre AS TipoIncapacidad,
        t.Entidad,
        t.DescripcionPago,
        i.FechaInicio,
        i.FechaFin,
        i.DiasIncapacidad,
        i.EsRemunerada,
        i.Motivo,
        i.DocumentoAdjunto,
        i.Estado,
        i.FechaRegistro
    FROM Incapacidades i
    INNER JOIN Colaboradores c 
        ON c.IdColaborador = i.IdColaborador
    INNER JOIN Cat_TiposIncapacidad t
        ON t.IdTipoIncapacidad = i.IdTipoIncapacidad
    WHERE i.IdIncapacidad = @IdIncapacidad;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_InsertarColaborador]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertarColaborador]
    @Cedula VARCHAR(20),
    @Nombre VARCHAR(80),
    @Apellidos VARCHAR(120),
    @Puesto VARCHAR(80),
    @Departamento VARCHAR(80),
    @FechaIngreso DATE,
    @SalarioBase DECIMAL(10,2),
    @Correo VARCHAR(120)
AS
BEGIN
    INSERT INTO Colaboradores (Cedula, Nombre, Apellidos, Puesto, Departamento, FechaIngreso, SalarioBase, Correo)
    VALUES (@Cedula, @Nombre, @Apellidos, @Puesto, @Departamento, @FechaIngreso, @SalarioBase, @Correo);
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_InsertarUsuario]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_InsertarUsuario]
    @NombreUsuario VARCHAR(50),
    @Correo VARCHAR(100),
    @ContrasenaHash VARCHAR(255),
    @IdPerfil INT,
    @IdColaborador INT
AS
BEGIN
    INSERT INTO Usuarios
    (
        NombreUsuario,
        Correo,
        ContrasenaHash,
        IdPerfil,
        IdColaborador,
        Estado
    )
    VALUES
    (
        @NombreUsuario,
        @Correo,
        @ContrasenaHash,
        @IdPerfil,
        @IdColaborador,
        1
    )
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Liquidacion_Aprobar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_Liquidacion_Aprobar]
    @IdLiquidacion INT
AS
BEGIN
    UPDATE Liquidaciones
    SET Estado = 'Aprobado'
    WHERE IdLiquidacion = @IdLiquidacion;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Liquidacion_Calcular]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[sp_Liquidacion_Calcular]
    @IdColaborador INT,
    @IdTipoSalida INT,
    @FechaSalida DATE,
    @UsuarioRegistro NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- ================= VALIDAR COLABORADOR =================
    IF NOT EXISTS (SELECT 1 FROM Colaboradores WHERE IdColaborador = @IdColaborador AND Estado = 1)
    BEGIN
        THROW 50001, N'El colaborador no está activo o no existe.', 1;
    END

    -- ================= VALIDAR LIQUIDACIÓN EXISTENTE =================
    IF EXISTS (SELECT 1 FROM Liquidaciones 
               WHERE IdColaborador = @IdColaborador 
                 AND Estado = 'Pendiente')
    BEGIN
        THROW 50002, N'El colaborador ya tiene una liquidación pendiente. No se puede generar otra.', 1;
    END

    DECLARE 
        @FechaIngreso DATE,
        @SalarioPromedio DECIMAL(18,2),
        @MesesTrabajados INT,
        @AniosTrabajados INT,
        @VacacionesPendientes DECIMAL(18,2),
        @AguinaldoProporcional DECIMAL(18,2),
        @Cesantia DECIMAL(18,2) = 0,
        @Preaviso DECIMAL(18,2) = 0,
        @MontoTotal DECIMAL(18,2),
        @AplicaCesantia BIT,
        @AplicaPreaviso BIT;

    -- ================= DATOS DEL COLABORADOR =================
    SELECT @FechaIngreso = FechaIngreso
    FROM Colaboradores
    WHERE IdColaborador = @IdColaborador;

    -- ================= SALARIO PROMEDIO =================
    SELECT @SalarioPromedio = AVG(SalarioBruto)
    FROM PlanillaDetalle pd
    JOIN Planillas p ON p.IdPlanilla = pd.IdPlanilla
    WHERE pd.IdColaborador = @IdColaborador
      AND p.FechaGeneracion >= DATEADD(MONTH, -6, @FechaSalida);

    IF @SalarioPromedio IS NULL
    BEGIN
        SELECT @SalarioPromedio = SalarioBase
        FROM Colaboradores
        WHERE IdColaborador = @IdColaborador;
    END

    -- ================= TIEMPO LABORADO =================
    SET @MesesTrabajados = DATEDIFF(MONTH, @FechaIngreso, @FechaSalida);
    SET @AniosTrabajados = @MesesTrabajados / 12;

    -- ================= TIPO DE SALIDA =================
    SELECT @AplicaCesantia = AplicaCesantia,
           @AplicaPreaviso = AplicaPreaviso
    FROM Cat_TipoSalida
    WHERE IdTipoSalida = @IdTipoSalida;

    -- ================= VACACIONES =================
    DECLARE @DiasVacacionesTomadas INT;
    SELECT @DiasVacacionesTomadas = ISNULL(SUM(DiasSolicitados),0)
    FROM Vacaciones
    WHERE IdColaborador = @IdColaborador AND Estado = 'Aprobado';

    DECLARE @DiasGenerados INT = @MesesTrabajados;
    DECLARE @DiasPendientes INT = @DiasGenerados - @DiasVacacionesTomadas;

    IF @DiasPendientes < 0 SET @DiasPendientes = 0;
    SET @VacacionesPendientes = (@SalarioPromedio / 30) * @DiasPendientes;

    -- ================= AGUINALDO PROPORCIONAL =================
    SET @AguinaldoProporcional = (@SalarioPromedio / 12) * (@MesesTrabajados % 12);

    -- ================= CESANTÍA =================
    IF @AplicaCesantia = 1
        SET @Cesantia = @SalarioPromedio * @AniosTrabajados;

    -- ================= PREAVISO =================
    IF @AplicaPreaviso = 1
        SET @Preaviso = @SalarioPromedio;

    -- ================= TOTAL =================
    SET @MontoTotal = @VacacionesPendientes + @AguinaldoProporcional + @Cesantia + @Preaviso;

    -- ================= INSERT LIQUIDACIÓN =================
    INSERT INTO Liquidaciones
        (IdColaborador, IdTipoSalida, FechaSalida, MontoTotal, Estado, UsuarioRegistro)
    VALUES
        (@IdColaborador, @IdTipoSalida, @FechaSalida, @MontoTotal, 'Pendiente', @UsuarioRegistro);

    DECLARE @IdLiquidacion INT = SCOPE_IDENTITY();

    -- ================= INSERT DETALLE =================
    INSERT INTO LiquidacionDetalle VALUES (@IdLiquidacion, N'Vacaciones Pendientes', @VacacionesPendientes);
    INSERT INTO LiquidacionDetalle VALUES (@IdLiquidacion, N'Aguinaldo Proporcional', @AguinaldoProporcional);
    INSERT INTO LiquidacionDetalle VALUES (@IdLiquidacion, N'Cesantía', @Cesantia);
    INSERT INTO LiquidacionDetalle VALUES (@IdLiquidacion, N'Preaviso', @Preaviso);

END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Liquidacion_Detalle]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[sp_Liquidacion_Detalle]
    @IdLiquidacion INT
AS
BEGIN
    SELECT 
        c.Nombre + ' ' + c.Apellidos AS NombreColaborador,
        ld.Rubro, 
        ld.Monto
    FROM LiquidacionDetalle ld
    INNER JOIN Liquidaciones l ON l.IdLiquidacion = ld.IdLiquidacion
    INNER JOIN Colaboradores c ON c.IdColaborador = l.IdColaborador
    WHERE ld.IdLiquidacion = @IdLiquidacion;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Liquidacion_Generar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Liquidacion_Generar]
    @IdColaborador INT,
    @IdTipoSalida INT,
    @FechaSalida DATE,
    @UsuarioRegistro NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    -- VALIDAR: no permitir si ya hay liquidación pendiente
    IF EXISTS (
        SELECT 1
        FROM Liquidaciones
        WHERE IdColaborador = @IdColaborador
          AND Estado = 'Pendiente'
    )
    BEGIN
        RAISERROR('El colaborador ya tiene una liquidación pendiente. No se puede generar otra.', 16, 1);
        RETURN;
    END

    -- Si no hay pendiente, generar la liquidación (incluso si ya hay aprobadas)
    EXEC sp_Liquidacion_Calcular
        @IdColaborador = @IdColaborador,
        @IdTipoSalida = @IdTipoSalida,
        @FechaSalida = @FechaSalida,
        @UsuarioRegistro = @UsuarioRegistro;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Liquidacion_Listar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_Liquidacion_Listar]
AS
BEGIN
    SELECT 
        l.IdLiquidacion,
        l.IdColaborador, -- ✅ OBLIGATORIO
        c.Nombre + ' ' + c.Apellidos AS Colaborador,
        t.Nombre AS TipoSalida,
        l.FechaSalida,
        l.MontoTotal AS Total,  
        l.Estado
    FROM Liquidaciones l
    JOIN Colaboradores c ON c.IdColaborador = l.IdColaborador
    JOIN Cat_TipoSalida t ON t.IdTipoSalida = l.IdTipoSalida
    ORDER BY l.FechaRegistro DESC;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Liquidacion_Pagar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_Liquidacion_Pagar]
    @IdLiquidacion INT
AS
BEGIN
    UPDATE Liquidaciones
    SET Estado = 'Pagado'
    WHERE IdLiquidacion = @IdLiquidacion;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Login_Validar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_Login_Validar]
    @Usuario VARCHAR(50),
    @ContrasenaHash VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        u.IdUsuario,
        u.NombreUsuario,
        p.NombrePerfil,
        u.IdColaborador,
        u.Estado
    FROM Usuarios u
    INNER JOIN Perfiles p ON p.IdPerfil = u.IdPerfil
    WHERE u.NombreUsuario = @Usuario
      AND u.ContrasenaHash = @ContrasenaHash;
      -- ❌ ELIMINAMOS: AND u.Estado = 'Activo'
      -- Ahora devuelve el usuario aunque esté inactivo
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_LoginUsuario]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_LoginUsuario]
    @NombreUsuario VARCHAR(50),
    @ContrasenaHash VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        u.IdUsuario,
        u.NombreUsuario,
        u.IdPerfil,
        p.NombrePerfil,
        u.IdColaborador,
        u.Estado
    FROM Usuarios u
    INNER JOIN Perfiles p ON p.IdPerfil = u.IdPerfil
    WHERE u.NombreUsuario = @NombreUsuario
      AND u.ContrasenaHash = @ContrasenaHash;
      -- ❌ ELIMINAMOS: AND u.Estado = 1
      -- Ahora devuelve el usuario aunque esté inactivo
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_ObtenerColaboradores]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_ObtenerColaboradores]
AS
BEGIN
    SELECT 
        IdColaborador,
        Cedula,
        Nombre,
        Apellidos,
        Puesto,
        Departamento,
        FechaIngreso,
        SalarioBase,
        Correo,
        Estado
    FROM Colaboradores
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_ObtenerColaboradoresActivos]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_ObtenerColaboradoresActivos]
AS
BEGIN
    SELECT 
        IdColaborador,
        Cedula,
        Nombre,
        Apellidos,
        Puesto,
        Departamento,
        FechaIngreso,
        SalarioBase,
        Correo,
        Estado
    FROM Colaboradores
    WHERE Estado = 1
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_ObtenerColaboradorPorID]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_ObtenerColaboradorPorID]
    @IdColaborador INT
AS
BEGIN
    SELECT
        IdColaborador,
        Cedula,
        Nombre,
        Apellidos,        -- 🔴 OBLIGATORIO
        Puesto,
        Departamento,
        FechaIngreso,
        SalarioBase,
        Correo,
        Estado
    FROM Colaboradores
    WHERE IdColaborador = @IdColaborador
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_ObtenerUsuarios]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_ObtenerUsuarios]
AS
BEGIN
    SELECT
        u.IdUsuario,
        u.IdColaborador,
        u.NombreUsuario,
        u.Correo,
        u.ContrasenaHash,       -- 🔴 OBLIGATORIO PARA EF
        u.IdPerfil,
        u.Estado,
        p.NombrePerfil          -- 🔴 DEBE LLAMARSE EXACTO
    FROM Usuarios u
    INNER JOIN Perfiles p ON u.IdPerfil = p.IdPerfil
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Permisos_CambiarEstado]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Permisos_CambiarEstado]
    @IdPermiso INT,
    @Estado VARCHAR(20)
AS
BEGIN
    UPDATE Permisos
    SET Estado = @Estado
    WHERE IdPermiso = @IdPermiso;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Permisos_Listar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Permisos_Listar]
AS
BEGIN
    SELECT
        p.IdPermiso,
        p.IdColaborador,
        c.Nombre + ' ' + c.Apellidos AS NombreColaborador,
        p.TipoPermiso,
        p.FechaSolicitud,
        p.FechaInicio,
        p.FechaFin,
        p.Estado,
        p.Observaciones,
        p.EsConGoce
    FROM Permisos p
    INNER JOIN Colaboradores c 
        ON c.IdColaborador = p.IdColaborador
    ORDER BY p.FechaSolicitud DESC;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Permisos_Listar_SoloPermiso]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Permisos_Listar_SoloPermiso]
AS
BEGIN
    SELECT IdPermiso, IdColaborador, TipoPermiso, FechaSolicitud, FechaInicio, FechaFin, Estado, Observaciones, EsConGoce
    FROM Permisos
    ORDER BY FechaSolicitud DESC;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Permisos_ListarPorColaborador]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Permisos_ListarPorColaborador]
    @IdColaborador INT
AS
BEGIN
    SELECT
        IdPermiso,
        IdColaborador,
        TipoPermiso,
        FechaSolicitud,
        FechaInicio,
        FechaFin,
        Estado,
        Observaciones,
        EsConGoce
    FROM Permisos
    WHERE IdColaborador = @IdColaborador
    ORDER BY FechaSolicitud DESC;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Permisos_ObtenerDetalle]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_Permisos_ObtenerDetalle]
    @IdPermiso INT
AS
BEGIN
    SELECT
        p.IdPermiso,
        p.IdColaborador,
        c.Nombre + ' ' + c.Apellidos AS NombreColaborador,
        c.Correo AS CorreoColaborador,
        p.TipoPermiso,
        p.FechaInicio,
        p.FechaFin,
        p.Estado
    FROM Permisos p
    INNER JOIN Colaboradores c ON c.IdColaborador = p.IdColaborador
    WHERE p.IdPermiso = @IdPermiso;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Permisos_Registrar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_Permisos_Registrar]
    @IdColaborador INT,
    @TipoPermiso VARCHAR(50),
    @FechaInicio DATE,
    @FechaFin DATE,
    @Observaciones VARCHAR(250),
    @EsConGoce BIT
AS
BEGIN
    INSERT INTO Permisos (
        IdColaborador,
        TipoPermiso,
        FechaSolicitud,
        FechaInicio,
        FechaFin,
        Estado,
        Observaciones,
        EsConGoce
    )
    VALUES (
        @IdColaborador,
        @TipoPermiso,
        GETDATE(),
        @FechaInicio,
        @FechaFin,
        'Pendiente',
        @Observaciones,
        @EsConGoce
    );
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Planilla_Anular]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Planilla_Anular]
    @IdPlanilla INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Planillas
    SET Estado = 2 -- Anulada
    WHERE IdPlanilla = @IdPlanilla;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Planilla_CambiarEstado]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Planilla_CambiarEstado]
    @IdPlanilla INT,
    @Estado INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Planillas
    SET Estado = @Estado
    WHERE IdPlanilla = @IdPlanilla;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Planilla_Detalle_Colaborador]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Planilla_Detalle_Colaborador]
    @IdPlanilla INT,
    @IdColaborador INT
AS
BEGIN
    DECLARE @Mes INT, @Año INT;
    SELECT @Mes = Mes, @Año = Año FROM Planillas WHERE IdPlanilla = @IdPlanilla;

    DECLARE @Inicio DATE = DATEFROMPARTS(@Año,@Mes,1);
    DECLARE @Fin DATE = EOMONTH(@Inicio);

    /* RESUMEN */
    SELECT * FROM PlanillaDetalle
    WHERE IdPlanilla = @IdPlanilla
      AND IdColaborador = @IdColaborador;

    /* ASISTENCIA */
    SELECT * FROM Asistencia
    WHERE IdColaborador = @IdColaborador
      AND Fecha BETWEEN @Inicio AND @Fin;

    /* PERMISOS */
    SELECT * FROM Permisos
    WHERE IdColaborador = @IdColaborador
      AND Estado = 'Aprobado'
      AND FechaFin >= @Inicio
      AND FechaInicio <= @Fin;

    /* HORAS EXTRAS */
    SELECT * FROM HorasExtras
    WHERE IdColaborador = @IdColaborador
      AND Estado='Aprobada'
      AND Fecha BETWEEN @Inicio AND @Fin;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Planilla_Detalle_PorPlanilla]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_Planilla_Detalle_PorPlanilla]
    @IdPlanilla INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        pd.IdPlanillaDetalle,
        pd.IdPlanilla,
        pd.IdColaborador,
        c.Nombre + ' ' + c.Apellidos AS NombreColaborador,
        pd.SalarioBase,
        pd.DiasLaborados,
        pd.HorasOrdinarias,
        pd.HorasExtras,
        pd.MontoHorasExtras,
		    pd.HorasFeriado,
    pd.MontoFeriado,
        pd.Deducciones,
        pd.SalarioBruto,
        pd.SalarioNeto,
        pd.MontoUniformes  -- 🔥 importante
    FROM PlanillaDetalle pd
    INNER JOIN Colaboradores c
        ON c.IdColaborador = pd.IdColaborador
    WHERE pd.IdPlanilla = @IdPlanilla;

END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Planilla_Generar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_Planilla_Generar]
    @Mes INT,
    @Anio INT
AS
BEGIN
    SET NOCOUNT ON;

------------------------------------------------------------
-- 🔒 VALIDAR PLANILLA EXISTENTE
------------------------------------------------------------

IF EXISTS (
    SELECT 1
    FROM Planillas
    WHERE Mes = @Mes
      AND Año = @Anio
      AND Estado <> 2
)
BEGIN
    RAISERROR('Ya existe una planilla para ese periodo y no está anulada.', 16, 1);
    RETURN;
END

IF EXISTS (
    SELECT 1
    FROM Planillas
    WHERE Mes = @Mes
      AND Año = @Anio
      AND Estado = 2
)
BEGIN
    DECLARE @IdPlanillaAnulada INT;

    SELECT TOP 1 @IdPlanillaAnulada = IdPlanilla
    FROM Planillas
    WHERE Mes = @Mes
      AND Año = @Anio
      AND Estado = 2;

    DELETE FROM PlanillaDetalleDeducciones
    WHERE IdPlanillaDetalle IN (
        SELECT IdPlanillaDetalle
        FROM PlanillaDetalle
        WHERE IdPlanilla = @IdPlanillaAnulada
    );

    DELETE FROM PlanillaDetalle
    WHERE IdPlanilla = @IdPlanillaAnulada;

    DELETE FROM Planillas
    WHERE IdPlanilla = @IdPlanillaAnulada;
END

    DECLARE @FechaInicio DATE = DATEFROMPARTS(@Anio,@Mes,1);
    DECLARE @FechaFin DATE = EOMONTH(@FechaInicio);
    DECLARE @DiasMes INT = DAY(@FechaFin);
    DECLARE @IdPlanilla INT;

    INSERT INTO Planillas (Mes,Año,FechaGeneracion,Estado)
    VALUES (@Mes,@Anio,GETDATE(),1);

    SET @IdPlanilla = SCOPE_IDENTITY();

    INSERT INTO PlanillaDetalle
    (
        IdPlanilla,
        IdColaborador,
        SalarioBase,
        DiasLaborados,
        HorasOrdinarias,
        HorasExtras,
        MontoHorasExtras,
        Deducciones,
        SalarioBruto,
        SalarioNeto,
        DiasVacaciones,
        DiasIncapacidad,
        MontoIncapacidadPatrono,
        MontoIncapacidadCCSS,
        MontoIncapacidadINS
    )
    SELECT
        @IdPlanilla,
        c.IdColaborador,
        c.SalarioBase,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    FROM Colaboradores c
    WHERE c.Estado = 1;

------------------------------------------------------------
-- 🟧 PASO 1: HORAS ORDINARIAS
------------------------------------------------------------
UPDATE pd
SET 
    DiasLaborados    = ISNULL(h.DiasLaborados, 0),
    HorasOrdinarias  = ISNULL(h.HorasOrdinarias, 0),
    SalarioBruto     =
        CASE 
            WHEN ISNULL(h.HorasOrdinarias,0) = 0
                THEN 0
            WHEN ISNULL(h.HorasOrdinarias,0) >= (48 * 4.3333)
                THEN pd.SalarioBase
            ELSE ISNULL(h.HorasOrdinarias,0) 
                 * (pd.SalarioBase / (48 * 4.3333))
        END
FROM PlanillaDetalle pd
OUTER APPLY
(
    SELECT 
        COUNT(*) AS DiasLaborados,
        SUM(
            CASE 
                WHEN HorasDia > 8 THEN 8 
                ELSE HorasDia 
            END
        ) AS HorasOrdinarias
    FROM
    (
        SELECT 
            a.Fecha,
            CASE 
                WHEN DATEDIFF(MINUTE, HoraEntrada, HoraSalida) >= 360 
                    THEN (DATEDIFF(MINUTE, HoraEntrada, HoraSalida) - 60) / 60.0
                ELSE DATEDIFF(MINUTE, HoraEntrada, HoraSalida) / 60.0
            END AS HorasDia
        FROM Asistencia a
        WHERE a.IdColaborador = pd.IdColaborador
          AND a.Fecha BETWEEN @FechaInicio AND @FechaFin
          AND HoraEntrada IS NOT NULL
          AND HoraSalida IS NOT NULL
    ) t
) h
WHERE pd.IdPlanilla = @IdPlanilla;

------------------------------------------------------------
-- 🟥 PASO 2: FERIADOS
------------------------------------------------------------
UPDATE pd
SET 
    HorasFeriado = ISNULL(f.HorasFeriado, 0),
    MontoFeriado = ISNULL(f.HorasFeriado, 0) 
                   * (pd.SalarioBase / (48 * 4.3333)),
    SalarioBruto = pd.SalarioBruto 
                   + (ISNULL(f.HorasFeriado, 0) 
                   * (pd.SalarioBase / (48 * 4.3333)))
FROM PlanillaDetalle pd
OUTER APPLY (
    SELECT 
        SUM(
            CASE 
                WHEN HorasDia > 8 THEN 8
                ELSE HorasDia
            END
        ) AS HorasFeriado
    FROM
    (
        SELECT 
            a.Fecha,
            CASE 
                WHEN DATEDIFF(MINUTE, a.HoraEntrada, a.HoraSalida) >= 360 
                    THEN (DATEDIFF(MINUTE, a.HoraEntrada, a.HoraSalida) - 60) / 60.0
                ELSE DATEDIFF(MINUTE, a.HoraEntrada, a.HoraSalida) / 60.0
            END AS HorasDia
        FROM Asistencia a
        INNER JOIN Feriados f
            ON f.Fecha = a.Fecha
            AND f.EsObligatorio = 1
        WHERE a.IdColaborador = pd.IdColaborador
          AND a.Fecha BETWEEN @FechaInicio AND @FechaFin
          AND a.HoraEntrada IS NOT NULL
          AND a.HoraSalida IS NOT NULL
    ) t
) f
WHERE pd.IdPlanilla = @IdPlanilla;

    ;WITH Calendario AS (
        SELECT 
            pd.IdColaborador,
            DATEADD(DAY, v.number, @FechaInicio) AS Fecha
        FROM PlanillaDetalle pd
        JOIN master..spt_values v ON v.type = 'P'
        WHERE DATEADD(DAY, v.number, @FechaInicio) <= @FechaFin
        AND pd.IdPlanilla = @IdPlanilla
    ),
    Ausencias AS (
        SELECT c.IdColaborador, c.Fecha
        FROM Calendario c
        LEFT JOIN Asistencia a
            ON a.IdColaborador = c.IdColaborador
            AND a.Fecha = c.Fecha
        LEFT JOIN Vacaciones v 
            ON v.IdColaborador = c.IdColaborador
            AND c.Fecha BETWEEN v.FechaInicio AND v.FechaFin
            AND v.Estado = 'Aprobado'
        LEFT JOIN Permisos p 
            ON p.IdColaborador = c.IdColaborador
            AND c.Fecha BETWEEN p.FechaInicio AND p.FechaFin
            AND p.Estado = 'Aprobado'
        LEFT JOIN Incapacidades i 
            ON i.IdColaborador = c.IdColaborador
            AND c.Fecha BETWEEN i.FechaInicio AND i.FechaFin
            AND i.Estado = 'Activa'
        WHERE a.IdAsistencia IS NULL
        AND v.IdVacacion IS NULL
        AND p.IdPermiso IS NULL
        AND i.IdIncapacidad IS NULL
    )
    SELECT IdColaborador, Fecha
    INTO #TmpAusencias
    FROM Ausencias;

    UPDATE pd
    SET AlertaDiasIncompletos = 1
    FROM PlanillaDetalle pd
    WHERE pd.IdPlanilla = @IdPlanilla
    AND pd.IdColaborador IN (
        SELECT IdColaborador
        FROM #TmpAusencias
        GROUP BY IdColaborador
        HAVING COUNT(*) >= 2
    );

    UPDATE pd
    SET AlertaDiasIncompletos = 1
    FROM PlanillaDetalle pd
    WHERE pd.IdPlanilla = @IdPlanilla
    AND EXISTS (
        SELECT 1
        FROM #TmpAusencias a1
        JOIN #TmpAusencias a2
            ON a1.IdColaborador = a2.IdColaborador
            AND a2.Fecha = DATEADD(DAY, 1, a1.Fecha)
        WHERE a1.IdColaborador = pd.IdColaborador
    );

    DROP TABLE #TmpAusencias;

    UPDATE pd
    SET 
        HorasExtras = ISNULL(he.TotalHoras,0),
        MontoHorasExtras = ISNULL(he.TotalHoras,0) 
                           * (pd.SalarioBase/@DiasMes/8) * 1.5,
        SalarioBruto = SalarioBruto 
                       + (ISNULL(he.TotalHoras,0) 
                       * (pd.SalarioBase/@DiasMes/8) * 1.5)
    FROM PlanillaDetalle pd
    OUTER APPLY(
        SELECT SUM(CantidadHoras) TotalHoras
        FROM HorasExtras h
        WHERE h.IdColaborador = pd.IdColaborador
        AND h.Estado = 'Aprobada'
        AND h.Fecha BETWEEN @FechaInicio AND @FechaFin
    ) he
    WHERE pd.IdPlanilla = @IdPlanilla;

    UPDATE pd
    SET 
        DiasSinGoce = ISNULL(p.Dias,0),
        MontoRebajoSinGoce = ISNULL(p.Dias,0) * (pd.SalarioBase/@DiasMes),
        SalarioBruto = SalarioBruto 
                       - (ISNULL(p.Dias,0) * (pd.SalarioBase/@DiasMes))
    FROM PlanillaDetalle pd
    OUTER APPLY(
        SELECT SUM(DATEDIFF(DAY,FechaInicio,FechaFin)+1) Dias
        FROM Permisos
        WHERE IdColaborador = pd.IdColaborador
        AND EsConGoce = 0
        AND Estado = 'Aprobado'
        AND FechaInicio BETWEEN @FechaInicio AND @FechaFin
    ) p
    WHERE pd.IdPlanilla = @IdPlanilla;

    UPDATE pd
    SET DiasVacaciones = ISNULL(v.Dias,0)
    FROM PlanillaDetalle pd
    OUTER APPLY(
        SELECT SUM(DATEDIFF(DAY,FechaInicio,FechaFin)+1) Dias
        FROM Vacaciones
        WHERE IdColaborador = pd.IdColaborador
        AND Estado = 'Aprobado'
        AND FechaInicio BETWEEN @FechaInicio AND @FechaFin
    ) v
    WHERE pd.IdPlanilla = @IdPlanilla;

    UPDATE pd
    SET 
        DiasIncapacidad = ISNULL(i.TotalDias,0),
        MontoRebajoIncapacidad = ISNULL(i.TotalRebajo,0),
        SubsidioIncapacidad = ISNULL(i.TotalSubsidio,0),
        AfectacionRealIncapacidad = 
            ISNULL(i.TotalRebajo,0) - ISNULL(i.TotalSubsidio,0),
        SalarioBruto = pd.SalarioBruto
                   - (ISNULL(i.TotalRebajo,0) 
                      - ISNULL(i.TotalSubsidio,0))
    FROM PlanillaDetalle pd
    OUTER APPLY(
        SELECT 
            SUM(DiasCalc) AS TotalDias,
            SUM(RebajoCalc) AS TotalRebajo,
            SUM(SubsidioCalc) AS TotalSubsidio
        FROM (
            SELECT 
                DATEDIFF(DAY,
                    CASE WHEN i.FechaInicio < @FechaInicio 
                         THEN @FechaInicio 
                         ELSE i.FechaInicio END,
                    CASE WHEN i.FechaFin > @FechaFin 
                         THEN @FechaFin 
                         ELSE i.FechaFin END
                ) + 1 AS DiasCalc,

                (DATEDIFF(DAY,
                    CASE WHEN i.FechaInicio < @FechaInicio 
                         THEN @FechaInicio 
                         ELSE i.FechaInicio END,
                    CASE WHEN i.FechaFin > @FechaFin 
                         THEN @FechaFin 
                         ELSE i.FechaFin END
                ) + 1) * (pd.SalarioBase / @DiasMes) AS RebajoCalc,

                CASE 
                    WHEN ti.Codigo = 'CCSS-EC' THEN
                        CASE 
                            WHEN 
                              (DATEDIFF(DAY,i.FechaInicio,i.FechaFin)+1) <= 3
                            THEN
                              (DATEDIFF(DAY,i.FechaInicio,i.FechaFin)+1)
                              * (pd.SalarioBase / @DiasMes) * 0.5
                            ELSE
                              (3 * (pd.SalarioBase / @DiasMes) * 0.5)
                              +
                              ((DATEDIFF(DAY,i.FechaInicio,i.FechaFin)+1 - 3)
                               * (pd.SalarioBase / @DiasMes) * 0.6)
                        END

                    WHEN ti.Codigo IN ('INS-RT','INS-SOA','CCSS-MAT')
                        THEN (DATEDIFF(DAY,i.FechaInicio,i.FechaFin)+1)
                             * (pd.SalarioBase / @DiasMes)

                    ELSE 0
                END AS SubsidioCalc

            FROM Incapacidades i
            INNER JOIN Cat_TiposIncapacidad ti
                ON ti.IdTipoIncapacidad = i.IdTipoIncapacidad
            WHERE i.IdColaborador = pd.IdColaborador
              AND i.Estado = 'Activa'
              AND i.FechaInicio <= @FechaFin
              AND i.FechaFin >= @FechaInicio
        ) x
    ) i
    WHERE pd.IdPlanilla = @IdPlanilla;

    UPDATE pd
    SET 
        MontoUniformes = ISNULL(u.Total,0),
        SalarioBruto = SalarioBruto - ISNULL(u.Total,0)
    FROM PlanillaDetalle pd
    OUTER APPLY(
        SELECT SUM(Costo) Total
        FROM EntregasUniforme
        WHERE IdColaborador = pd.IdColaborador
        AND CobrarEnPlanilla = 1
        AND FechaEntrega BETWEEN @FechaInicio AND @FechaFin
    ) u
    WHERE pd.IdPlanilla = @IdPlanilla;

    INSERT INTO PlanillaDetalleDeducciones
    (
        IdPlanillaDetalle,
        IdDeduccion,
        PorcentajeAplicadoPatrono,
        PorcentajeAplicadoTrabajador,
        MontoPatrono,
        MontoTrabajador
    )
    SELECT
        pd.IdPlanillaDetalle,
        d.IdDeduccion,
        0,
        d.PorcentajeTrabajador,
        0,
        ROUND(pd.SalarioBruto * (d.PorcentajeTrabajador/100.0),2)
    FROM PlanillaDetalle pd
    JOIN Cat_DeduccionesObligatorias d
        ON d.Estado = 1
    WHERE pd.IdPlanilla = @IdPlanilla
    AND d.PorcentajeTrabajador > 0;

    -- 9️⃣ RENTA (CORREGIDO)
    CREATE TABLE #TmpRenta (
        IdPlanillaDetalle INT,
        MontoRenta DECIMAL(18,2)
    );

    INSERT INTO #TmpRenta
    SELECT 
        pd.IdPlanillaDetalle,
        ISNULL(SUM(
            CASE
                WHEN pd.SalarioBruto > tr.LimiteInferior
                THEN (CASE 
                        WHEN pd.SalarioBruto > ISNULL(tr.LimiteSuperior, pd.SalarioBruto)
                        THEN ISNULL(tr.LimiteSuperior, pd.SalarioBruto)
                        ELSE pd.SalarioBruto
                    END - tr.LimiteInferior) * (tr.Porcentaje/100.0)
                ELSE 0
            END
        ), 0)
    FROM PlanillaDetalle pd
    CROSS JOIN Cat_TramosRenta tr
    WHERE pd.IdPlanilla = @IdPlanilla
    AND tr.Estado = 1
    GROUP BY pd.IdPlanillaDetalle;

    -- Insertar renta en deducciones
    INSERT INTO PlanillaDetalleDeducciones
    (
        IdPlanillaDetalle,
        IdDeduccion,
        PorcentajeAplicadoPatrono,
        PorcentajeAplicadoTrabajador,
        MontoPatrono,
        MontoTrabajador
    )
    SELECT
        tr.IdPlanillaDetalle,
        11,
        0,
        0,
        0,
        tr.MontoRenta
    FROM #TmpRenta tr
    WHERE tr.MontoRenta > 0;

    DROP TABLE #TmpRenta;

------------------------------------------------------------
-- 🔟 TOTAL DEDUCCIONES Y NETO
------------------------------------------------------------
UPDATE pd
SET 
    Deducciones = 
        ISNULL((
            SELECT SUM(MontoTrabajador)
            FROM PlanillaDetalleDeducciones d
            WHERE d.IdPlanillaDetalle = pd.IdPlanillaDetalle
        ),0)
        + ISNULL(pd.MontoRebajoSinGoce,0)
        + ISNULL(pd.MontoRebajoIncapacidad,0)
        + ISNULL(pd.MontoUniformes,0)
        + ISNULL(pd.MontoRebajoAsistencia,0),

    SalarioNeto =
        CASE 
            WHEN 
                SalarioBruto - (
                    ISNULL((
                        SELECT SUM(MontoTrabajador)
                        FROM PlanillaDetalleDeducciones d
                        WHERE d.IdPlanillaDetalle = pd.IdPlanillaDetalle
                    ),0)
                    + ISNULL(pd.MontoRebajoSinGoce,0)
                    + ISNULL(pd.MontoRebajoIncapacidad,0)
                    + ISNULL(pd.MontoUniformes,0)
                    + ISNULL(pd.MontoRebajoAsistencia,0)
                ) < 0
            THEN 0
            ELSE
                SalarioBruto - (
                    ISNULL((
                        SELECT SUM(MontoTrabajador)
                        FROM PlanillaDetalleDeducciones d
                        WHERE d.IdPlanillaDetalle = pd.IdPlanillaDetalle
                    ),0)
                    + ISNULL(pd.MontoRebajoSinGoce,0)
                    + ISNULL(pd.MontoRebajoIncapacidad,0)
                    + ISNULL(pd.MontoUniformes,0)
                    + ISNULL(pd.MontoRebajoAsistencia,0)
                )
        END
FROM PlanillaDetalle pd
WHERE pd.IdPlanilla = @IdPlanilla;

END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Planilla_Listar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Planilla_Listar]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdPlanilla,
        Mes,
        Año,
        FechaGeneracion,
        Estado
    FROM Planillas
    ORDER BY Año DESC, Mes DESC;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Planilla_Regenerar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Planilla_Regenerar]
    @IdPlanilla INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Mes INT, @Año INT;

    -- Obtener mes y año de la planilla a regenerar
    SELECT @Mes = Mes, @Año = Año
    FROM Planillas
    WHERE IdPlanilla = @IdPlanilla;

    -- Marcar la planilla como anulada
    UPDATE Planillas
    SET Estado = 2 -- Anulada
    WHERE IdPlanilla = @IdPlanilla;

    -- Generar nueva planilla
    EXEC sp_Planilla_Generar @Mes, @Año;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Uniformes_Actualizar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Uniformes_Actualizar]
    @IdEntrega INT,
    @IdColaborador INT,
    @IdTipoUniforme INT,
    @Talla VARCHAR(10),
    @Cantidad INT,
    @FechaEntrega DATE,
    @Costo DECIMAL(10,2),
    @CobrarEnPlanilla BIT,
    @Observaciones VARCHAR(255)
AS
BEGIN
    UPDATE EntregasUniforme
    SET
        IdColaborador = @IdColaborador,
        IdTipoUniforme = @IdTipoUniforme,
        Talla = @Talla,
        Cantidad = @Cantidad,
        FechaEntrega = @FechaEntrega,
        Costo = @Costo,
        CobrarEnPlanilla = @CobrarEnPlanilla,
        Observaciones = @Observaciones
    WHERE IdEntrega = @IdEntrega;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Uniformes_Eliminar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Uniformes_Eliminar]
    @IdEntrega INT
AS
BEGIN
    DELETE FROM EntregasUniforme
    WHERE IdEntrega = @IdEntrega;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Uniformes_HistorialPorColaborador]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_Uniformes_HistorialPorColaborador]
    @IdColaborador INT
AS
BEGIN
    SELECT 
        e.IdEntrega,
        t.NombreTipo,
        e.Talla,
        e.Cantidad,
        e.FechaEntrega,
        e.Costo,
        e.CobrarEnPlanilla,
        e.Observaciones
    FROM EntregasUniforme e
    INNER JOIN Cat_TiposUniforme t 
        ON e.IdTipoUniforme = t.IdTipoUniforme
    WHERE e.IdColaborador = @IdColaborador
    ORDER BY e.FechaEntrega DESC;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Uniformes_Insertar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Uniformes_Insertar]
    @IdColaborador INT,
    @IdTipoUniforme INT,
    @Talla VARCHAR(10),
    @Cantidad INT,
    @FechaEntrega DATE,
    @Costo DECIMAL(10,2),
    @CobrarEnPlanilla BIT,
    @Observaciones VARCHAR(255)
AS
BEGIN
    INSERT INTO EntregasUniforme
    (IdColaborador, IdTipoUniforme, Talla, Cantidad,
     FechaEntrega, Costo, CobrarEnPlanilla, Observaciones)
    VALUES
    (@IdColaborador, @IdTipoUniforme, @Talla, @Cantidad,
     @FechaEntrega, @Costo, @CobrarEnPlanilla, @Observaciones);
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Uniformes_InsertarTipo]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_Uniformes_InsertarTipo]
    @NombreTipo VARCHAR(100),
    @Descripcion VARCHAR(255),
    @EntregaAutomatica BIT,
    @MesesParaEntrega INT,
    @CantidadPorEntrega INT,
    @FrecuenciaMeses INT,
    @EsOpcional BIT,
    @PermiteCobro BIT
AS
BEGIN
    INSERT INTO Cat_TiposUniforme
    (NombreTipo, Descripcion, EntregaAutomatica, MesesParaEntrega,
     CantidadPorEntrega, FrecuenciaMeses, EsOpcional, PermiteCobro)
    VALUES
    (@NombreTipo, @Descripcion, @EntregaAutomatica, @MesesParaEntrega,
     @CantidadPorEntrega, @FrecuenciaMeses, @EsOpcional, @PermiteCobro);
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Uniformes_Listar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Uniformes_Listar]
AS
BEGIN
    SELECT 
        e.IdEntrega,
        e.IdColaborador, 
        c.Nombre,
        t.NombreTipo AS TipoUniforme,  -- coincide con la propiedad en el VM
        e.Talla,
        e.Cantidad,
        e.FechaEntrega,
        e.Costo,
        e.CobrarEnPlanilla,
        e.Observaciones
    FROM EntregasUniforme e
    INNER JOIN Colaboradores c ON e.IdColaborador = c.IdColaborador
    INNER JOIN Cat_TiposUniforme t ON e.IdTipoUniforme = t.IdTipoUniforme
    ORDER BY e.FechaEntrega DESC;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Uniformes_ListarTipos]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Uniformes_ListarTipos]
AS
BEGIN
    SELECT 
        IdTipoUniforme,
        NombreTipo,
        Descripcion,
        EntregaAutomatica,
        MesesParaEntrega,
        CantidadPorEntrega,
        FrecuenciaMeses,
        EsOpcional,
        PermiteCobro,
        Activo,
        FechaCreacion   -- <--- agregar esta columna
    FROM Cat_TiposUniforme
    WHERE Activo = 1
    ORDER BY NombreTipo;
END;
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Usuarios_Actualizar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Usuarios_Actualizar]
    @IdUsuario INT,
    @NombreUsuario VARCHAR(50),
    @Correo VARCHAR(100),
    @IdPerfil INT,
    @Estado BIT
AS
BEGIN

    DECLARE @IdColaborador INT

    SELECT @IdColaborador = IdColaborador
    FROM Usuarios
    WHERE IdUsuario = @IdUsuario

    -- Actualizar usuario
    UPDATE Usuarios
    SET NombreUsuario = @NombreUsuario,
        Correo = @Correo,
        IdPerfil = @IdPerfil,
        Estado = @Estado
    WHERE IdUsuario = @IdUsuario

    -- Actualizar correo del colaborador
    UPDATE Colaboradores
    SET Correo = @Correo
    WHERE IdColaborador = @IdColaborador

END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Usuarios_ActualizarContrasena]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Usuarios_ActualizarContrasena]
    @Correo NVARCHAR(100),
    @NuevaContrasena NVARCHAR(200)
AS
BEGIN
    UPDATE Usuarios
    SET ContrasenaHash = @NuevaContrasena
    WHERE Correo = @Correo
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Usuarios_CambiarEstado]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Usuarios_CambiarEstado]
    @IdUsuario INT
AS
BEGIN
    UPDATE Usuarios
    SET Estado = CASE WHEN Estado = 1 THEN 0 ELSE 1 END
    WHERE IdUsuario = @IdUsuario
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Usuarios_DesactivarPorColaborador]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Usuarios_DesactivarPorColaborador]
    @IdColaborador INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Usuarios
    SET Estado = 0
    WHERE IdColaborador = @IdColaborador;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Usuarios_ObtenerPorCorreo]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Usuarios_ObtenerPorCorreo]
    @Correo NVARCHAR(100)
AS
BEGIN

    SELECT 
        IdUsuario,
        IdColaborador,
        NombreUsuario,
        Correo,
        ContrasenaHash,
        IdPerfil,
        Estado
    FROM Usuarios
    WHERE Correo = @Correo

END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Usuarios_ObtenerPorId]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Usuarios_ObtenerPorId]
    @IdUsuario INT
AS
BEGIN
    SELECT
        u.IdUsuario,
        u.NombreUsuario,
        u.Correo,
        u.ContrasenaHash,
        u.IdPerfil,
        p.NombrePerfil,
        u.IdColaborador,
        u.Estado
    FROM Usuarios u
    INNER JOIN Perfiles p ON p.IdPerfil = u.IdPerfil
    WHERE u.IdUsuario = @IdUsuario
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Vacaciones_CalcularDiasDisponibles]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Vacaciones_CalcularDiasDisponibles]
    @IdColaborador INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @FechaIngreso DATE;
    DECLARE @MesesTrabajados INT = 0;
    DECLARE @DiasGenerados INT = 0;
    DECLARE @DiasUsados INT = 0;

    -- Obtener fecha de ingreso
    SELECT @FechaIngreso = FechaIngreso
    FROM Colaboradores
    WHERE IdColaborador = @IdColaborador;

    -- Validar existencia
    IF @FechaIngreso IS NULL
    BEGIN
        SELECT 0 AS DiasDisponibles;
        RETURN;
    END

    -- Meses trabajados
    SET @MesesTrabajados = DATEDIFF(MONTH, @FechaIngreso, GETDATE());

    IF @MesesTrabajados < 0
        SET @MesesTrabajados = 0;

    -- 1 día por mes
    SET @DiasGenerados = @MesesTrabajados;

    -- Días usados (solo aprobados)
    SELECT @DiasUsados = ISNULL(SUM(DiasSolicitados), 0)
    FROM Vacaciones
    WHERE IdColaborador = @IdColaborador
      AND Estado = 'Aprobada';

    -- Evitar negativos
    IF (@DiasGenerados - @DiasUsados) < 0
        SELECT 0 AS DiasDisponibles;
    ELSE
        SELECT (@DiasGenerados - @DiasUsados) AS DiasDisponibles;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Vacaciones_CambiarEstado]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Vacaciones_CambiarEstado]
    @IdVacacion INT,
    @Estado VARCHAR(20)
AS
BEGIN
    UPDATE Vacaciones
    SET Estado = @Estado
    WHERE IdVacacion = @IdVacacion
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Vacaciones_HistoricoColaborador]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Vacaciones_HistoricoColaborador]
    @IdColaborador INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        v.IdVacacion,
        v.IdColaborador,
        c.Nombre + ' ' + c.Apellidos AS NombreColaborador,
        v.FechaSolicitud,
        v.FechaInicio,
        v.FechaFin,
        v.DiasSolicitados,
        v.Estado
    FROM Vacaciones v
    INNER JOIN Colaboradores c 
        ON v.IdColaborador = c.IdColaborador
    WHERE v.IdColaborador = @IdColaborador
    ORDER BY v.FechaSolicitud DESC;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Vacaciones_Listar_Admin]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Vacaciones_Listar_Admin]
AS
BEGIN
    SELECT
        v.IdVacacion,
        v.IdColaborador,
        c.Nombre + ' ' + c.Apellidos AS NombreColaborador,
        v.FechaSolicitud,
        v.FechaInicio,
        v.FechaFin,
        v.DiasSolicitados,
        v.Estado
    FROM Vacaciones v
    INNER JOIN Colaboradores c 
        ON v.IdColaborador = c.IdColaborador
    ORDER BY v.FechaSolicitud DESC
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Vacaciones_ListarPorColaborador]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Vacaciones_ListarPorColaborador]
    @IdColaborador INT
AS
BEGIN
    SELECT
        FechaInicio,
        FechaFin,
        DiasSolicitados,
        Estado
    FROM Vacaciones
    WHERE IdColaborador = @IdColaborador
    ORDER BY FechaSolicitud DESC
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Vacaciones_ObtenerDetalle]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Vacaciones_ObtenerDetalle]
    @IdVacacion INT
AS
BEGIN
    SELECT
        v.IdVacacion,
        v.IdColaborador, -- 🔴 CLAVE
        c.Nombre + ' ' + c.Apellidos AS NombreColaborador,
        c.Correo AS CorreoColaborador,
        v.FechaInicio,
        v.FechaFin,
        v.DiasSolicitados,
        v.Estado
    FROM Vacaciones v
    INNER JOIN Colaboradores c 
        ON v.IdColaborador = c.IdColaborador
    WHERE v.IdVacacion = @IdVacacion;
END
GO

GO

/****** Object:  StoredProcedure [dbo].[sp_Vacaciones_Registrar]    Script Date: 12/10/2026 19:37:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Vacaciones_Registrar]
    @IdColaborador INT,
    @FechaInicio DATE,
    @FechaFin DATE,
    @DiasSolicitados INT
AS
BEGIN
    INSERT INTO Vacaciones
    (
        IdColaborador,
        FechaSolicitud,
        FechaInicio,
        FechaFin,
        DiasSolicitados,
        Estado
    )
    VALUES
    (
        @IdColaborador,
        GETDATE(),
        @FechaInicio,
        @FechaFin,
        @DiasSolicitados,
        'Pendiente'
    )
END
GO

GO

USE [master]
GO

GO

ALTER DATABASE [BD_RRHH] SET  READ_WRITE 
GO