using System.ComponentModel.DataAnnotations;

namespace FraudSys.Application.DTOs;

public class CreateLimitRequest
{
    [Required(ErrorMessage = "CPF é obrigatório.")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter exatamente 11 dígitos.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter apenas números.")]
    public string Document { get; set; } = string.Empty;

    [Required(ErrorMessage = "Agência é obrigatória.")]
    [StringLength(10, MinimumLength = 1, ErrorMessage = "Agência deve ter entre 1 e 10 dígitos.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Agência deve conter apenas números.")]
    public string Agency { get; set; } = string.Empty;

    [Required(ErrorMessage = "Conta é obrigatória.")]
    [StringLength(15, MinimumLength = 1, ErrorMessage = "Conta deve ter entre 1 e 15 dígitos.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Conta deve conter apenas números.")]
    public string AccountNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Limite PIX é obrigatório.")]
    [Range(0.01, 999999.99, ErrorMessage = "O limite deve ser entre R$ 0,01 e R$ 999.999,99.")]
    public decimal? PixLimit { get; set; }
}