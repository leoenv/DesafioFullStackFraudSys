namespace FraudSys.Domain.Entities;

public class AccountLimit
{
    public string Document { get; set; } = string.Empty;
    public string Agency { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public decimal PixLimit { get; set; }

    public bool HasSufficientLimit(decimal amount) => amount <= PixLimit;

    public void ConsumeLimit(decimal amount)
    {
        if (!HasSufficientLimit(amount))
            throw new InvalidOperationException("Insufficient PIX limit.");

        PixLimit -= amount;
    }
}