using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using TrainBooking.Application.Servises.PDF;
using TrainBooking.Domain.Entities;

namespace TrainBooking.Application.Servises.Imp.PDF
{
    public class PdfGeneratorService : IPdfGeneratorService
    {
        private readonly IQrCodeGeneratorService _qrCodeGeneratorService;

        public PdfGeneratorService(IQrCodeGeneratorService qrCodeGeneratorService)
        {
            _qrCodeGeneratorService = qrCodeGeneratorService;
        }

        public Task<byte[]> GenerateTicketPdfAsync(TicketEntity ticket)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = new TicketDocument(ticket, _qrCodeGeneratorService);

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            return Task.FromResult(stream.ToArray());
        }
    }

    public class TicketDocument : IDocument
    {
        private readonly TicketEntity _ticket;
        private readonly IQrCodeGeneratorService _qrCodeGeneratorService;

        public TicketDocument(TicketEntity ticket, IQrCodeGeneratorService qrCodeGeneratorService)
        {
            _ticket = ticket;
            _qrCodeGeneratorService = qrCodeGeneratorService;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            var qrCodeBytes = _qrCodeGeneratorService.GenerateQrCode(_ticket);

            container.Page(page =>
            {
                page.Size(PageSizes.A5);
                page.Margin(20);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("Залізничний квиток")
                    .SemiBold()
                    .FontSize(24);

                page.Content()
                    .PaddingVertical(10)
                    .Column(column =>
                    {
                        column.Spacing(10);

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"Квиток №: {_ticket.TicketId}").SemiBold();
                            row.RelativeItem().AlignRight().Text($"Дата оформлення: {_ticket.PurchaseDate:yyyy-MM-dd}");
                        });

                        column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        column.Item().Text($"Пасажир: {_ticket.FullName}").FontSize(14);

                        column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("Відправлення").Bold();
                                col.Item().Text($"Станція: {_ticket.DepartureCity} ({_ticket.DepartureStation})");
                                col.Item().Text($"Дата: {_ticket.DepartureTime:yyyy-MM-dd}");
                                col.Item().Text($"Час: {_ticket.DepartureTime:HH:mm}");
                            });

                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("Прибуття").Bold();
                                col.Item().Text($"Станція: {_ticket.ArrivalCity} ({_ticket.ArrivalStation})");
                                col.Item().Text($"Дата: {_ticket.ArrivalTime:yyyy-MM-dd}");
                                col.Item().Text($"Час: {_ticket.ArrivalTime:HH:mm}");
                            });
                        });

                        column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"Поїзд №: {_ticket.TrainNumber}");
                            row.RelativeItem().Text($"Вагон: {_ticket.CarriageNumber} ({_ticket.CarriageType})");
                            row.RelativeItem().Text($"Місце: {_ticket.SeatNumber} ({_ticket.SeatType})");
                        });

                        column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        column.Item().AlignRight().Text($"Ціна: {_ticket.TicketPrice} грн")
                            .FontSize(16)
                            .Bold();

                        column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        // --- Додавання QR-коду та тексту поряд з ним ---
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Image(qrCodeBytes).FitWidth();  // Зменшений розмір QR-коду
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("Цей QR-код містить інформацію про ваш квиток, включаючи деталі поїзда та час відправлення.");
                                col.Item().Text("Скануйте його для швидкого доступу до квитка.");
                            });
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Дякуємо, що обрали наш сервіс!").FontSize(10).Italic();
                    });
            });
        }

    }
}
