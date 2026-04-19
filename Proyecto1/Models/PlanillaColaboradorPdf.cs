using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Proyecto1.Models;
using System;
using System.Globalization;

public class PlanillaColaboradorPdf : IDocument
{
    private PlanillaDetalleColaboradorVM _detalle;
    private readonly CultureInfo culturaCR = new CultureInfo("es-CR");

    public PlanillaColaboradorPdf(PlanillaDetalleColaboradorVM detalle)
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
                col.Item().Text("COLILLA SALARIAL").FontSize(16).Bold().FontColor(Colors.Blue.Darken3);
                col.Item().Text($"Colaborador: {_detalle.NombreColaborador}").FontSize(12);
                col.Item().Text($"Planilla #: {_detalle.IdPlanilla}").FontSize(11);
                col.Item().PaddingTop(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
            });

            // 🔷 CONTENIDO
            page.Content().PaddingVertical(20).Column(col =>
            {
                col.Spacing(8);

                // 💰 INGRESOS
                col.Item().Text("INGRESOS").Bold().FontSize(12).FontColor(Colors.Blue.Darken3);

                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Salario Base:");
                    r.ConstantItem(150).AlignRight()
                        .Text(_detalle.SalarioBase.ToString("C", culturaCR));
                });

                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Salario Bruto:");
                    r.ConstantItem(150).AlignRight()
                        .Text(_detalle.SalarioBruto.ToString("C", culturaCR)).Bold();
                });

                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Horas Extras:");
                    r.ConstantItem(150).AlignRight()
                        .Text(_detalle.HorasExtras.ToString("N2") + " hrs");
                });

                if (_detalle.MontoHorasExtras > 0)
                {
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Monto Horas Extras:");
                        r.ConstantItem(150).AlignRight()
                            .Text(_detalle.MontoHorasExtras.ToString("C", culturaCR));
                    });
                }

                if (_detalle.MontoFeriado > 0)
                {
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Pago Feriado:");
                        r.ConstantItem(150).AlignRight()
                            .Text(_detalle.MontoFeriado.ToString("C", culturaCR));
                    });
                }

                // 📉 DEDUCCIONES
                col.Item().PaddingTop(15).Text("DEDUCCIONES").Bold().FontSize(12).FontColor(Colors.Red.Darken3);

                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("CCSS (Salud + IVM):");
                    r.ConstantItem(150).AlignRight()
                        .Text(_detalle.TotalDeduccionCCSS.ToString("C", culturaCR));
                });

                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Impuesto sobre la Renta:");
                    r.ConstantItem(150).AlignRight()
                        .Text(_detalle.DeduccionRenta.ToString("C", culturaCR));
                });

                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Banco Popular:");
                    r.ConstantItem(150).AlignRight()
                        .Text(_detalle.DeduccionBancoPopular.ToString("C", culturaCR));
                });

                if (_detalle.MontoRebajoIncapacidad > 0)
                {
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Rebajo Incapacidad:");
                        r.ConstantItem(150).AlignRight()
                            .Text(_detalle.MontoRebajoIncapacidad.ToString("C", culturaCR));
                    });
                }

                if (_detalle.MontoUniformes > 0)
                {
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Uniformes:");
                        r.ConstantItem(150).AlignRight()
                            .Text(_detalle.MontoUniformes.ToString("C", culturaCR));
                    });
                }

                if (_detalle.MontoRebajoSinGoce > 0)
                {
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Permisos sin Goce:");
                        r.ConstantItem(150).AlignRight()
                            .Text(_detalle.MontoRebajoSinGoce.ToString("C", culturaCR));
                    });
                }

                // 📊 TOTAL
                col.Item().PaddingTop(15).LineHorizontal(1);

                col.Item().Row(r =>
                {
                    r.RelativeItem().Text("Total Deducciones:").FontSize(11);
                    r.ConstantItem(150).AlignRight()
                        .Text(_detalle.Deducciones.ToString("C", culturaCR)).FontSize(11);
                });

                col.Item().PaddingTop(10).Row(r =>
                {
                    r.RelativeItem().Text("SALARIO NETO").Bold().FontSize(13).FontColor(Colors.Green.Darken3);
                    r.ConstantItem(150).AlignRight()
                        .Text(_detalle.SalarioNeto.ToString("C", culturaCR))
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