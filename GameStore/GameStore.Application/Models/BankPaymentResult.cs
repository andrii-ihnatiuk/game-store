using System.Diagnostics.CodeAnalysis;
using GameStore.Application.Interfaces;

namespace GameStore.Application.Models;

[ExcludeFromCodeCoverage]
public class BankPaymentResult : IPaymentResult
{
    public byte[] InvoiceFileBytes { get; init; }

    public string ContentType { get; init; }

    public string FileDownloadName { get; init; }
}