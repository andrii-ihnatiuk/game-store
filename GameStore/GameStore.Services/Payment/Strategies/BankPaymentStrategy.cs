using System.Globalization;
using System.Net.Mime;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Services.Interfaces.Payment;
using GameStore.Services.Models;
using GameStore.Shared.Constants;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace GameStore.Services.Payment.Strategies;

public class BankPaymentStrategy : PaymentStrategyBase
{
    public BankPaymentStrategy(IUnitOfWork unitOfWork, ICoreOrderService orderService)
        : base(unitOfWork, orderService)
    {
    }

    public override PaymentStrategyName Name => PaymentStrategyName.Bank;

    protected override Task<IPaymentResult> DoPaymentAsync(PaymentRequest request)
    {
        byte[] fileBytes = GeneratePdfInvoice(Order);
        var fileName = $"{DateTime.UtcNow:u}.pdf";

        return Task.FromResult<IPaymentResult>(new BankPaymentResult
        {
            InvoiceFileBytes = fileBytes,
            FileDownloadName = fileName,
            ContentType = MediaTypeNames.Application.Pdf,
        });
    }

    protected override async Task CompletePaymentAsync()
    {
        Order.Status = OrderStatus.Checkout;
        UpdateQuantitiesForProducts(Order.OrderDetails);
        await UnitOfWork.SaveAsync();
    }

    private static byte[] GeneratePdfInvoice(Order order)
    {
        const int validity = 3;
        var validThru = DateTime.UtcNow.AddDays(validity).ToString("dddd, dd MMMM yyyy HH:mm", CultureInfo.InvariantCulture);

        using var stream = new MemoryStream();
        var writer = new PdfWriter(stream);
        var pdf = new PdfDocument(writer);
        var document = new Document(pdf);

        document.Add(new Paragraph($"Customer ID: {order.CustomerId}"));
        document.Add(new Paragraph($"Order ID: {order.Id}"));
        document.Add(new Paragraph($"Valid thru: {validThru}"));
        document.Add(new Paragraph($"Sum: {order.Sum}"));
        document.Close();

        return stream.ToArray();
    }
}