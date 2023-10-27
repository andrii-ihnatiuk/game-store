using System.Globalization;
using GameStore.Data.Entities;
using GameStore.Data.Repositories;
using GameStore.Services.Constants;
using GameStore.Services.Interfaces;
using GameStore.Services.Models;
using GameStore.Shared.DTOs.Order;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services.PaymentStrategies;

public class BankPaymentStrategy : IPaymentStrategy
{
    private readonly IUnitOfWork _unitOfWork;

    public BankPaymentStrategy(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public string Name => PaymentStrategyName.Bank;

    public async Task<IPaymentResult> ProcessPayment(PaymentDto payment, Guid customerId)
    {
        var order = await GetOrderForPaymentAsync(customerId);
        byte[] fileBytes = GeneratePdfInvoice(order);
        return ConvertToBankPaymentResult(fileBytes);
    }

    private async Task<Order> GetOrderForPaymentAsync(Guid customerId)
    {
        return await _unitOfWork.Orders.GetOneAsync(
            predicate: o => o.CustomerId == customerId && o.PaidDate == null,
            include: q => q.Include(o => o.OrderDetails),
            noTracking: false);
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

    private static BankPaymentResult ConvertToBankPaymentResult(byte[] fileBytes)
    {
        const string fileType = "application/pdf";
        var fileName = $"{DateTime.UtcNow:u}.pdf";

        return new BankPaymentResult()
        {
            InvoiceFileBytes = fileBytes,
            FileDownloadName = fileName,
            ContentType = fileType,
        };
    }
}