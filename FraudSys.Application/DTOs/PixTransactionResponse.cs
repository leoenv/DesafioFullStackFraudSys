namespace FraudSys.Application.DTOs;

public class PixTransactionResponse
{
    public bool Approved { get; set; }
    public string Message { get; set; } = string.Empty;
    public decimal CurrentLimit { get; set; }
}