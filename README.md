# 🏢 RRHH System — Sistema de Gestión de Recursos Humanos

Sistema integral de gestión administrativa de recursos humanos desarrollado en ASP.NET MVC. Automatiza procesos clave del departamento de RRHH siguiendo la normativa del Código de Trabajo de Costa Rica.

## ✨ Funcionalidades

- **Gestión de Colaboradores** — Registro y administración de empleados con validación de cédula única
- **Nómina / Planillas** — Generación automática de salarios con cálculos de deducciones (CCSS, IVM, Renta, INS), horas extras, feriados, incapacidades y aguinaldo
- **Asistencia** — Registro de entrada/salida con generación automática por período (excluye domingos y feriados)
- **Evaluaciones de Desempeño** — Evaluaciones con escala 0–10, criterios customizables y cálculo automático de promedios
- **Solicitudes de Empleados** — Gestión de vacaciones, permisos, horas extras e incapacidades con flujo de aprobación
- **Incapacidades** — Registro con carga de documentos y cálculo automático de subsidios (CCSS, INS)
- **Control de Acceso** — Sistema de usuarios con roles (Admin, RRHH, Colaborador) y recuperación de contraseña
- **Reportes** — Exportación de planillas y colillas a PDF

## 💻 Stack Tecnológico

| Capa | Tecnología |
|------|-----------|
| Backend | ASP.NET MVC 6.0 |
| Base de Datos | SQL Server 2019+ |
| ORM | Entity Framework Core |
| Frontend | Razor Views, Bootstrap 5 |
| UI Components | Bootstrap Icons, jQuery DataTables |
| Reportes | QuestPDF |
| Autenticación | Session-based + Stored Procedures |

## 📸 Screenshots

| Login | Dashboard |
|-------|-----------|
| ![Login](screenshots/01-login.png) | ![Dashboard](screenshots/02-dashboard.png) |

| Gestión de Planillas | Incapacidades |
|----------------------|---------------|
| ![Planillas](screenshots/03-planillas.png) | ![Incapacidades](screenshots/04-incapacidades.png) |

## 🚀 Correr Localmente

### Requisitos

- .NET 6.0+
- SQL Server 2019+
- Visual Studio 2022

### Pasos

**1. Clonar el repositorio**
```bash
git clone https://github.com/DuvanJ/rrhh-system.git
cd rrhh-system
```

**2. Restaurar la base de datos**

Abrir SQL Server Management Studio y ejecutar los scripts en orden:
```
Scripts/00_CrearBD.sql
Scripts/01_Tablas.sql
Scripts/02_Catalogos.sql
Scripts/03_DatosIniciales.sql
Scripts/04_Feriados.sql
```

**3. Configurar conexión**

Editar `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=NOMBRE_SERVIDOR;Database=BD_RRHH;Trusted_Connection=true;"
  }
}
```

**4. Ejecutar**
```bash
dotnet restore
dotnet run
```

La aplicación estará disponible en: `https://localhost:5001`

### Credenciales por defecto
```
Email:      admin@rrhh.com
Contraseña: Admin123!
```

## 📝 Notas

- Zona horaria: Costa Rica (UTC -6)
- Moneda: Colones Costarricenses (₡)
- Deducciones: CCSS 9.25%, IVM 2.75%, Renta variable, INS 0.75%

## 👤 Autor

**Duvan** — [GitHub](https://github.com/DuvanJ)
