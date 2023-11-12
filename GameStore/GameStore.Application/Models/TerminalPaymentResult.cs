using GameStore.Application.Interfaces;

namespace GameStore.Application.Models;

public class TerminalPaymentResult : IPaymentResult
{
    public short Method { get; set; }

    public Guid UserId { get; set; }

    public Guid OrderId { get; set; }

    public decimal Sum { get; set; }
}