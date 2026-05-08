using System.ComponentModel.DataAnnotations;

namespace FraudSys.Application.DTOs;

public class UpdateLimitRequest
{
    [Range(0.01, 999999.99, ErrorMessage = "O limite deve ser entre R$ 0,01 e R$ 999.999,99.")]
    public decimal PixLimit { get; set; }
}