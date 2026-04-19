using ClosedXML.Excel;
using Proyecto1.Models;

namespace Proyecto1.BLL
{
    public class ExcelService
    {
        public byte[] ExportarPlanillas(List<Planilla> lista)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Planillas");

            ws.Cell(1, 1).Value = "ID";
            ws.Cell(1, 2).Value = "Mes";
            ws.Cell(1, 3).Value = "Año";
            ws.Cell(1, 4).Value = "Fecha Generación";
            ws.Cell(1, 5).Value = "Estado";
            ws.Cell(1, 6).Value = "Total";

            int fila = 2;

            foreach (var p in lista)
            {
                decimal total = p.Detalles.Sum(d => d.SalarioNeto);

                ws.Cell(fila, 1).Value = p.IdPlanilla;
                ws.Cell(fila, 2).Value = p.Mes;
                ws.Cell(fila, 3).Value = p.Año;
                ws.Cell(fila, 4).Value = p.FechaGeneracion;
                ws.Cell(fila, 5).Value = p.EstadoTexto;
                ws.Cell(fila, 6).Value = total;

                fila++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }



        public byte[] ExportarPlanillaResumen(List<PlanillaDetalleVM> lista)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Resumen Planilla");

            ws.Cell(1, 1).Value = "Colaborador";
            ws.Cell(1, 2).Value = "Salario Base";
            ws.Cell(1, 3).Value = "Horas Extras";
            ws.Cell(1, 4).Value = "Deducciones";
            ws.Cell(1, 5).Value = "Salario Neto";

            int fila = 2;

            foreach (var item in lista)
            {
                ws.Cell(fila, 1).Value = item.NombreColaborador;
                ws.Cell(fila, 2).Value = item.SalarioBase;
                ws.Cell(fila, 3).Value = item.MontoHorasExtras;
                ws.Cell(fila, 4).Value = item.Deducciones;
                ws.Cell(fila, 5).Value = item.SalarioNeto;

                fila++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }

        public byte[] ExportarPlanillaDetalle(List<PlanillaDetalleVM> lista)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Detalle Planilla");

            ws.Cell(1, 1).Value = "Colaborador";
            ws.Cell(1, 2).Value = "Salario Base";
            ws.Cell(1, 3).Value = "Días Laborados";
            ws.Cell(1, 4).Value = "Horas Ordinarias";
            ws.Cell(1, 5).Value = "Horas Extras";
            ws.Cell(1, 6).Value = "Monto Horas Extras";
            ws.Cell(1, 7).Value = "Deducciones";
            ws.Cell(1, 8).Value = "Uniformes";
            ws.Cell(1, 9).Value = "Salario Bruto";
            ws.Cell(1, 10).Value = "Salario Neto";

            int fila = 2;

            foreach (var item in lista)
            {
                ws.Cell(fila, 1).Value = item.NombreColaborador;
                ws.Cell(fila, 2).Value = item.SalarioBase;
                ws.Cell(fila, 3).Value = item.DiasLaborados;
                ws.Cell(fila, 4).Value = item.HorasOrdinarias;
                ws.Cell(fila, 5).Value = item.HorasExtras;
                ws.Cell(fila, 6).Value = item.MontoHorasExtras;
                ws.Cell(fila, 7).Value = item.Deducciones;
                ws.Cell(fila, 8).Value = item.MontoUniformes;
                ws.Cell(fila, 9).Value = item.SalarioBruto;
                ws.Cell(fila, 10).Value = item.SalarioNeto;

                fila++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }




        public byte[] ExportarAguinaldos(List<Aguinaldo> lista)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Aguinaldos");

            ws.Cell(1, 1).Value = "Colaborador";
            ws.Cell(1, 2).Value = "Año";
            ws.Cell(1, 3).Value = "Total Salarios";
            ws.Cell(1, 4).Value = "Aguinaldo";
            ws.Cell(1, 5).Value = "Estado";

            int fila = 2;

            foreach (var item in lista)
            {
                ws.Cell(fila, 1).Value = item.NombreColaborador;
                ws.Cell(fila, 2).Value = item.Año;
                ws.Cell(fila, 3).Value = item.TotalSalarios;
                ws.Cell(fila, 4).Value = item.MontoAguinaldo;
                ws.Cell(fila, 5).Value = item.Estado;

                fila++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }



        public byte[] ExportarAsistencia(List<AsistenciaReporteVM> lista)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Asistencia");

            // TITULOS
            ws.Cell(1, 1).Value = "Colaborador";
            ws.Cell(1, 2).Value = "Fecha";
            ws.Cell(1, 3).Value = "Hora Entrada";
            ws.Cell(1, 4).Value = "Hora Salida";
            ws.Cell(1, 5).Value = "Estado";

            int fila = 2;

            foreach (var item in lista)
            {
                ws.Cell(fila, 1).Value = item.NombreColaborador;
                ws.Cell(fila, 2).Value = item.Fecha.ToString("dd/MM/yyyy");
                ws.Cell(fila, 3).Value = item.HoraEntrada?.ToString(@"hh\:mm") ?? "-";
                ws.Cell(fila, 4).Value = item.HoraSalida?.ToString(@"hh\:mm") ?? "-";
                ws.Cell(fila, 5).Value = item.Estado;

                fila++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }


        public byte[] ExportarColaboradores(List<Colaborador> lista)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Colaboradores");

            ws.Cell(1, 1).Value = "Cédula";
            ws.Cell(1, 2).Value = "Nombre";
            ws.Cell(1, 3).Value = "Puesto";
            ws.Cell(1, 4).Value = "Departamento";
            ws.Cell(1, 5).Value = "Fecha Ingreso";
            ws.Cell(1, 6).Value = "Salario";
            ws.Cell(1, 7).Value = "Estado";

            int fila = 2;

            foreach (var item in lista)
            {
                ws.Cell(fila, 1).Value = item.Cedula;
                ws.Cell(fila, 2).Value = item.Nombre + " " + item.Apellidos;
                ws.Cell(fila, 3).Value = item.Puesto;
                ws.Cell(fila, 4).Value = item.Departamento;
                ws.Cell(fila, 5).Value = item.FechaIngreso.ToString("dd/MM/yyyy");
                ws.Cell(fila, 6).Value = item.SalarioBase;
                ws.Cell(fila, 7).Value = item.Estado ? "Activo" : "Inactivo";

                fila++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }



        public byte[] ExportarEvaluaciones(List<EvaluacionVM> lista)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Evaluaciones");

            ws.Cell(1, 1).Value = "Colaborador";
            ws.Cell(1, 2).Value = "Fecha";
            ws.Cell(1, 3).Value = "Puntaje Total";
            ws.Cell(1, 4).Value = "Comentarios";

            int fila = 2;

            foreach (var item in lista)
            {
                ws.Cell(fila, 1).Value = item.NombreColaborador;
                ws.Cell(fila, 2).Value = item.FechaEvaluacion.ToString("dd/MM/yyyy");
                ws.Cell(fila, 3).Value = item.PuntajeTotal;
                ws.Cell(fila, 4).Value = item.ComentariosGenerales;

                fila++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }




        public byte[] ExportarHorasExtras(List<HoraExtraAdminVM> lista)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Horas Extras");

            ws.Cell(1, 1).Value = "Colaborador";
            ws.Cell(1, 2).Value = "Fecha";
            ws.Cell(1, 3).Value = "Cantidad Horas";
            ws.Cell(1, 4).Value = "Motivo";
            ws.Cell(1, 5).Value = "Estado";

            int fila = 2;

            foreach (var item in lista)
            {
                ws.Cell(fila, 1).Value = item.NombreColaborador;
                ws.Cell(fila, 2).Value = item.Fecha.ToString("dd/MM/yyyy");
                ws.Cell(fila, 3).Value = item.CantidadHoras;
                ws.Cell(fila, 4).Value = item.Motivo;
                ws.Cell(fila, 5).Value = item.Estado;

                fila++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }




        public byte[] ExportarIncapacidades(List<IncapacidadAdminVM> lista)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Incapacidades");

            ws.Cell(1, 1).Value = "Colaborador";
            ws.Cell(1, 2).Value = "Código";
            ws.Cell(1, 3).Value = "Tipo Incapacidad";
            ws.Cell(1, 4).Value = "Entidad";
            ws.Cell(1, 5).Value = "Fecha Inicio";
            ws.Cell(1, 6).Value = "Fecha Fin";
            ws.Cell(1, 7).Value = "Días";
            ws.Cell(1, 8).Value = "Remunerada";
            ws.Cell(1, 9).Value = "Estado";

            int fila = 2;

            foreach (var item in lista)
            {
                ws.Cell(fila, 1).Value = item.NombreColaborador;
                ws.Cell(fila, 2).Value = item.Codigo;
                ws.Cell(fila, 3).Value = item.TipoIncapacidad;
                ws.Cell(fila, 4).Value = item.Entidad;
                ws.Cell(fila, 5).Value = item.FechaInicio.ToString("dd/MM/yyyy");
                ws.Cell(fila, 6).Value = item.FechaFin.ToString("dd/MM/yyyy");
                ws.Cell(fila, 7).Value = item.DiasIncapacidad;
                ws.Cell(fila, 8).Value = item.EsRemunerada ? "Sí" : "No";
                ws.Cell(fila, 9).Value = item.Estado;

                fila++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }



        public byte[] ExportarLiquidaciones(List<LiquidacionVM> lista)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Liquidaciones");

            ws.Cell(1, 1).Value = "Colaborador";
            ws.Cell(1, 2).Value = "Tipo Salida";
            ws.Cell(1, 3).Value = "Fecha Salida";
            ws.Cell(1, 4).Value = "Total";
            ws.Cell(1, 5).Value = "Estado";

            int fila = 2;

            foreach (var item in lista)
            {
                ws.Cell(fila, 1).Value = item.Colaborador;
                ws.Cell(fila, 2).Value = item.TipoSalida;
                ws.Cell(fila, 3).Value = item.FechaSalida.ToString("dd/MM/yyyy");
                ws.Cell(fila, 4).Value = item.Total;
                ws.Cell(fila, 5).Value = item.Estado;
                fila++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }


        public byte[] ExportarPermisos(List<PermisoAdminVM> lista)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Permisos");

            ws.Cell(1, 1).Value = "Colaborador";
            ws.Cell(1, 2).Value = "Tipo Permiso";
            ws.Cell(1, 3).Value = "Fecha Inicio";
            ws.Cell(1, 4).Value = "Fecha Fin";
            ws.Cell(1, 5).Value = "Estado";

            int fila = 2;

            foreach (var item in lista)
            {
                ws.Cell(fila, 1).Value = item.NombreColaborador;
                ws.Cell(fila, 2).Value = item.TipoPermiso;
                ws.Cell(fila, 3).Value = item.FechaInicio.ToString("dd/MM/yyyy");
                ws.Cell(fila, 4).Value = item.FechaFin.ToString("dd/MM/yyyy");
                ws.Cell(fila, 5).Value = item.Estado;

                fila++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }


        public byte[] ExportarUniformes(List<UniformeHistorialVM> lista)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Uniformes");

            ws.Cell(1, 1).Value = "Colaborador";
            ws.Cell(1, 2).Value = "Tipo Uniforme";
            ws.Cell(1, 3).Value = "Talla";
            ws.Cell(1, 4).Value = "Cantidad";
            ws.Cell(1, 5).Value = "Fecha Entrega";
            ws.Cell(1, 6).Value = "Costo";
            ws.Cell(1, 7).Value = "Cobrar Planilla";

            int fila = 2;

            foreach (var item in lista)
            {
                ws.Cell(fila, 1).Value = item.NombreColaborador;
                ws.Cell(fila, 2).Value = item.NombreTipo;
                ws.Cell(fila, 3).Value = item.Talla;
                ws.Cell(fila, 4).Value = item.Cantidad;
                ws.Cell(fila, 5).Value = item.FechaEntrega.ToString("dd/MM/yyyy");
                ws.Cell(fila, 6).Value = item.Costo;
                ws.Cell(fila, 7).Value = item.CobrarEnPlanilla ? "Sí" : "No";

                fila++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }


        public byte[] ExportarUsuarios(List<UsuarioListado> lista)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Usuarios");

            ws.Cell(1, 1).Value = "Usuario";
            ws.Cell(1, 2).Value = "Correo";
            ws.Cell(1, 3).Value = "Perfil";
            ws.Cell(1, 4).Value = "Estado";

            int fila = 2;

            foreach (var item in lista)
            {
                ws.Cell(fila, 1).Value = item.NombreUsuario;
                ws.Cell(fila, 2).Value = item.Correo;
                ws.Cell(fila, 3).Value = item.NombrePerfil;
                ws.Cell(fila, 4).Value = item.Estado ? "Activo" : "Inactivo";

                fila++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }


        public byte[] ExportarVacaciones(List<VacacionAdminVM> lista)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Vacaciones");

            ws.Cell(1, 1).Value = "Colaborador";
            ws.Cell(1, 2).Value = "Fecha Solicitud";
            ws.Cell(1, 3).Value = "Fecha Inicio";
            ws.Cell(1, 4).Value = "Fecha Fin";
            ws.Cell(1, 5).Value = "Días";
            ws.Cell(1, 6).Value = "Estado";

            int fila = 2;

            foreach (var item in lista)
            {
                ws.Cell(fila, 1).Value = item.NombreColaborador;
                ws.Cell(fila, 2).Value = item.FechaSolicitud;
                ws.Cell(fila, 3).Value = item.FechaInicio;
                ws.Cell(fila, 4).Value = item.FechaFin;
                ws.Cell(fila, 5).Value = item.DiasSolicitados;
                ws.Cell(fila, 6).Value = item.Estado;

                fila++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }


    }



}