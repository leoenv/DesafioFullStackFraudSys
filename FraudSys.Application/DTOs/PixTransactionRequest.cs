using System.ComponentModel.DataAnnotations;

namespace FraudSys.Application.DTOs;

public class PixTransactionRequest
{
    [Required(ErrorMessage = "Agência é obrigatória.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Agência deve conter apenas números.")]
    public string Agency { get; set; } = string.Empty;

    [Required(ErrorMessage = "Número da conta é obrigatório.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Conta deve conter apenas números.")]
    public string AccountNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "O valor do PIX é obrigatório.")]
    [Range(0.01, 999999.99, ErrorMessage = "O valor deve ser entre R$ 0,01 e R$ 999.999,99.")]
    public decimal? Amount { get; set; }
}