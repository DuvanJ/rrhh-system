using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Proyecto1.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class LiquidacionPdf : IDocument
{
    private List<LiquidacionDetalleVM> _detalle;
    private int _idLiquidacion;
    private readonly CultureInfo culturaCR = new CultureInfo("es-CR");

    public LiquidacionPdf(List<LiquidacionDetalleVM> detalle, int idLiquidacion)
    {
        _detalle = detalle;
        _idLiquidacion = idLiquidacion;
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
                col.Item().Text("LIQUIDACIÓN LABORAL").FontSize(18).Bold().FontColor(Colors.Blue.Darken3);
                col.Item().Text($"Colaborador: {(_detalle.FirstOrDefault()?.NombreColaborador ?? "—")}").FontSize(11);
                col.Item().Text($"Liquidación #: {_idLiquidacion}").FontSize(11);
                col.Item().Text($"Fecha generación: {DateTime.Now:dd/MM/yyyy}").FontSize(11);
                col.Item().PaddingTop(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
            });

            // 🔷 TABLA RUBROS
            page.Content().PaddingVertical(15).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3);
                    columns.RelativeColumn(1.5f);
                });

                table.Header(header =>
                {
                    var headerStyle = TextStyle.Default.Bold().FontColor(Colors.White);

                    header.Cell().Background(Colors.Blue.Darken3).Text("Concepto").Style(headerStyle);
                    header.Cell().Background(Colors.Blue.Darken3).AlignRight().Text("Monto").Style(headerStyle);
                });

                foreach (var item in _detalle)
                {
                    table.Cell().Text(item.Rubro);
                    table.Cell().AlignRight()
                        .Text(item.Monto.ToString("C", culturaCR));
                }

                var totalStyle = TextStyle.Default.Bold().FontColor(Colors.White);
                var total = _detalle.Sum(x => x.Monto);

                table.Cell().Background(Colors.Green.Medium).AlignRight().Text("TOTAL A PAGAR:").Style(totalStyle);
                table.Cell().Background(Colors.Green.Medium).AlignRight()
                    .Text(total.ToString("C", culturaCR)).Style(totalStyle);
            });

            // 🔷 FIRMA
            page.Footer()
                .BorderTop(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingTop(30)
                .Column(col =>
                {
                    col.Item().AlignCenter().Text("____________________________").FontSize(10);
                    col.Item().AlignCenter().Text("Firma del Colaborador").FontSize(10).FontColor(Colors.Grey.Darken2);
                });
        });
    }
}