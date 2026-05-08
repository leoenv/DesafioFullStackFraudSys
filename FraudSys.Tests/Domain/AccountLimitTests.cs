using FluentAssertions;
using FraudSys.Domain.Entities;

namespace FraudSys.Tests.Domain;

public class AccountLimitTests
{
    [Fact]
    public void HasSufficientLimit_DeveRetornarTrue_QuandoValorMenorOuIgualAoLimite()
    {
        var account = new AccountLimit { PixLimit = 1000m };
        account.HasSufficientLimit(1000m).Should().BeTrue();
        account.HasSufficientLimit(999m).Should().BeTrue();
    }

    [Fact]
    public void HasSufficientLimit_DeveRetornarFalse_QuandoValorMaiorQueOLimite()
    {
        var account = new AccountLimit { PixLimit = 100m };
        account.HasSufficientLimit(101m).Should().BeFalse();
    }

    [Fact]
    public void ConsumeLimit_DeveDebitarValor_QuandoLimiteSuficiente()
    {
        var account = new AccountLimit { PixLimit = 1000m };
        account.ConsumeLimit(300m);
        account.PixLimit.Should().Be(700m);
    }

    [Fact]
    public void ConsumeLimit_DeveLancarExcecao_QuandoLimiteInsuficiente()
    {
        var account = new AccountLimit { PixLimit = 100m };
        account.Invoking(a => a.ConsumeLimit(500m))
            .Should().Throw<InvalidOperationException>();
    }
}