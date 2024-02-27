﻿namespace GameStore.Shared.DTOs.Payment.Terminal;

public class TerminalTransactionRequestDto
{
    public decimal TransactionAmount { get; set; }

    public string AccountNumber { get; set; }

    public Guid InvoiceNumber { get; set; }
}