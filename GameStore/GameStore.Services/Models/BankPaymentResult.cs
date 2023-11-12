using System.Diagnostics.CodeAnalysis;
using GameStore.Services.Interfaces.Payment;

namespace GameStore.Services.Models;

[ExcludeFromCodeCoverage]
public class BankPaymentResult : IPaymentResult
{
    public byte[] InvoiceFileBytes { get; init; }

    public string ContentType { get; init; }

    public string FileDownloadName { get; init; }
}