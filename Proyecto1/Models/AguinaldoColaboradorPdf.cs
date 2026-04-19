using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Proyecto1.Models;
using System;
using System.Globalization;

public class AguinaldoColaboradorPdf : IDocument
{
    private Aguinaldo _detalle;
    private readonly CultureInfo culturaCR = new CultureInfo("es-CR");

    public AguinaldoColaboradorPdf(Aguinaldo detalle)
    {
        _detalle = detalle;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(30);

            // 🔷 ENCABEZADO
            page.Header().Column(col =>
            {
                col.Item().Text("COMPROBANTE DE AGUINALDO").FontSize(16).Bold().FontColor(Colors.Blue.Darken3);
                col.Item().Text($"Colaborador: {_detalle.NombreColaborador}").FontSize(12);
                col.Item().Text($"Año: {_detalle.Año}").FontSize(11);
                col.Item().PaddingTop(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
            });

            // 🔷 CONTENIDO
            page.Content().PaddingVertical(20).Column(col =>
            {
                col.Spacing(8);

                // 📊 DATOS PRINCIPALES
                col.Item().Text("DETALLES").Bold().FontSize(12).FontColor(Colors.Blue.Darken3);

                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("ID Colaborador:");
                    r.ConstantItem(150).AlignRight()
                        .Text(_detalle.IdColaborador.ToString());
                });

                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Total Salarios Acumulados:");
                    r.ConstantItem(150).AlignRight()
                        .Text(_detalle.TotalSalarios.ToString("C", culturaCR));
                });

                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Monto de Aguinaldo:");
                    r.ConstantItem(150).AlignRight()
                        .Text(_detalle.MontoAguinaldo.ToString("C", culturaCR)).Bold().FontSize(12);
                });

                if (_detalle.FechaCalculo.HasValue)
                {
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Fecha de Cálculo:");
                        r.ConstantItem(150).AlignRight()
                            .Text(_detalle.FechaCalculo?.ToString("dd/MM/yyyy"));
                    });
                }

                if (_detalle.FechaPago.HasValue)
                {
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Fecha de Pago:");
                        r.ConstantItem(150).AlignRight()
                            .Text(_detalle.FechaPago?.ToString("dd/MM/yyyy"));
                    });
                }

                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Estado:");
                    r.ConstantItem(150).AlignRight()
                        .Text(_detalle.Estado ?? "—");
                });

                // 📋 TOTAL
                col.Item().PaddingTop(15).LineHorizontal(1);

                col.Item().PaddingTop(10).Row(r =>
                {
                    r.RelativeItem().Text("TOTAL A PAGAR").Bold().FontSize(13).FontColor(Colors.Green.Darken3);
                    r.ConstantItem(150).AlignRight()
                        .Text(_detalle.MontoAguinaldo.ToString("C", culturaCR))
                        .Bold().FontSize(13).FontColor(Colors.Green.Darken3);
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