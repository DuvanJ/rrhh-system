using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Proyecto1.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class AguinaldoPdf : IDocument
{
    private List<Aguinaldo> _lista;
    private int _anio;
    private readonly CultureInfo culturaCR = new CultureInfo("es-CR");

    public AguinaldoPdf(List<Aguinaldo> lista, int anio)
    {
        _lista = lista;
        _anio = anio;
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
                col.Item().Text("REPORTE DE AGUINALDO").FontSize(18).Bold().FontColor(Colors.Blue.Darken3);
                col.Item().Text($"Temporada: {_anio}").FontSize(11);
                col.Item().Text($"Generado: {DateTime.Now:dd/MM/yyyy}").FontSize(11);
                col.Item().PaddingTop(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
            });

            // 🔷 TABLA
            page.Content().PaddingVertical(15).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                });

                table.Header(header =>
                {
                    var headerStyle = TextStyle.Default.Bold().FontColor(Colors.White);

                    header.Cell().Background(Colors.Blue.Darken3).Text("Colaborador").Style(headerStyle);
                    header.Cell().Background(Colors.Blue.Darken3).AlignRight().Text("Total Salarios").Style(headerStyle);
                    header.Cell().Background(Colors.Blue.Darken3).AlignRight().Text("Aguinaldo").Style(headerStyle);
                });

                foreach (var item in _lista)
                {
                    table.Cell().Text(item.NombreColaborador ?? "—");
                    table.Cell().AlignRight()
                        .Text(item.TotalSalarios.ToString("C", culturaCR));
                    table.Cell().AlignRight()
                        .Text(item.MontoAguinaldo.ToString("C", culturaCR));
                }

                var totalStyle = TextStyle.Default.Bold().FontColor(Colors.White);
                var totalSalarios = _lista.Sum(x => x.TotalSalarios);
                var totalAguinaldo = _lista.Sum(x => x.MontoAguinaldo);

                table.Cell().Background(Colors.Green.Medium).AlignRight().Text("TOTAL GENERAL:").Style(totalStyle);
                table.Cell().Background(Colors.Green.Medium).AlignRight().Text(totalSalarios.ToString("C", culturaCR)).Style(totalStyle);
                table.Cell().Background(Colors.Green.Medium).AlignRight()
                    .Text(totalAguinaldo.ToString("C", culturaCR)).Style(totalStyle);
            });

            // 🔷 FOOTER
            page.Footer()
                .BorderTop(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingTop(10)
                .AlignCenter()
                .Text("Documento generado automáticamente por el sistema RRHH").FontSize(9).FontColor(Colors.Grey.Darken2);
        });
    }
}