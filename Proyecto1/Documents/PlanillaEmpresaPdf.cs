using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Proyecto1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

public class PlanillaEmpresaPdf : IDocument
{
    private List<PlanillaDetalleVM> _detalle;
    private string _empresa;
    private string _periodo;
    private readonly CultureInfo culturaCR = new CultureInfo("es-CR");

    public PlanillaEmpresaPdf(List<PlanillaDetalleVM> detalle, string empresa, string periodo)
    {
        _detalle = detalle;
        _empresa = empresa;
        _periodo = periodo;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(25);

            // 🔷 ENCABEZADO
            page.Header().Column(col =>
            {
                col.Item().Text(_empresa).FontSize(18).Bold().FontColor(Colors.Blue.Darken3);
                col.Item().Text("REPORTE GENERAL DE PLANILLA").FontSize(14).Bold();
                col.Item().Text($"Periodo: {_periodo}").FontSize(11);
                col.Item().PaddingTop(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
            });

            // 🔷 CONTENIDO
            page.Content().Column(col =>
            {
                // 📋 TABLA PRINCIPAL
                col.Item().PaddingVertical(15).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(1.5f);
                        columns.RelativeColumn(1.5f);
                        columns.RelativeColumn(1.5f);
                    });

                    table.Header(header =>
                    {
                        var headerStyle = TextStyle.Default.Bold().FontColor(Colors.White);

                        header.Cell().Background(Colors.Blue.Darken3).Text("Colaborador").Style(headerStyle);
                        header.Cell().Background(Colors.Blue.Darken3).AlignRight().Text("Bruto").Style(headerStyle);
                        header.Cell().Background(Colors.Blue.Darken3).AlignRight().Text("Deducciones").Style(headerStyle);
                        header.Cell().Background(Colors.Blue.Darken3).AlignRight().Text("Neto").Style(headerStyle);
                    });

                    foreach (var item in _detalle)
                    {
                        table.Cell().Text(item.NombreColaborador);
                        table.Cell().AlignRight().Text(item.SalarioBruto.ToString("C", culturaCR));
                        table.Cell().AlignRight().Text(item.Deducciones.ToString("C", culturaCR));
                        table.Cell().AlignRight().Text(item.SalarioNeto.ToString("C", culturaCR));
                    }

                    var totalBruto = _detalle.Sum(x => x.SalarioBruto);
                    var totalDeduc = _detalle.Sum(x => x.Deducciones);
                    var totalNeto = _detalle.Sum(x => x.SalarioNeto);

                    var totalStyle = TextStyle.Default.Bold().FontColor(Colors.White);

                    table.Cell().Background(Colors.Green.Medium).AlignRight().Text("TOTALES:").Style(totalStyle);
                    table.Cell().Background(Colors.Green.Medium).AlignRight().Text(totalBruto.ToString("C", culturaCR)).Style(totalStyle);
                    table.Cell().Background(Colors.Green.Medium).AlignRight().Text(totalDeduc.ToString("C", culturaCR)).Style(totalStyle);
                    table.Cell().Background(Colors.Green.Medium).AlignRight().Text(totalNeto.ToString("C", culturaCR)).Style(totalStyle);
                });

                // 📊 RESUMEN RÁPIDO
                col.Item().PaddingTop(20).Column(resumen =>
                {
                    resumen.Item().Text("RESUMEN").Bold().FontSize(11).FontColor(Colors.Blue.Darken3);
                    resumen.Item().PaddingTop(5).Text($"Total Colaboradores: {_detalle.Count}").FontSize(10);
                    resumen.Item().Text($"Total Salario Bruto: {_detalle.Sum(x => x.SalarioBruto).ToString("C", culturaCR)}").FontSize(10);
                    resumen.Item().Text($"Total Deducciones: {_detalle.Sum(x => x.Deducciones).ToString("C", culturaCR)}").FontSize(10);
                    resumen.Item().Text($"Total Salario Neto: {_detalle.Sum(x => x.SalarioNeto).ToString("C", culturaCR)}").FontSize(10).Bold();
                });
            });

            // 🔷 FOOTER
            page.Footer()
                .BorderTop(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingTop(10)
                .AlignCenter()
                .Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm:ss}").FontSize(9).FontColor(Colors.Grey.Darken2);
        });
    }
}