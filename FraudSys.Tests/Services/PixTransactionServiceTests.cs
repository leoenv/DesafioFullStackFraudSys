using FluentAssertions;
using FraudSys.Application.DTOs;
using FraudSys.Application.Services;
using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces;
using Moq;

namespace FraudSys.Tests.Services;

public class PixTransactionServiceTests
{
    private readonly Mock<ILimitRepository> _repositoryMock = new();
    private readonly PixTransactionService _service;

    public PixTransactionServiceTests()
    {
        _service = new PixTransactionService(_repositoryMock.Object);
    }

    [Fact]
    public async Task ValidateAndConsumeAsync_DeveAprovar_QuandoLimiteSuficiente()
    {
        var account = new AccountLimit { Agency = "0001", AccountNumber = "123456", PixLimit = 1000m };
        _repositoryMock.Setup(r => r.GetByAccountAsync("0001", "123456")).ReturnsAsync(account);

        var request = new PixTransactionRequest { Agency = "0001", AccountNumber = "123456", Amount = 300m };

        var result = await _service.ValidateAndConsumeAsync(request);

        result.Approved.Should().BeTrue();
        result.CurrentLimit.Should().Be(700m);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<AccountLimit>()), Times.Once);
    }

    [Fact]
    public async Task ValidateAndConsumeAsync_DeveNegar_QuandoLimiteInsuficiente()
    {
        var account = new AccountLimit { Agency = "0001", AccountNumber = "123456", PixLimit = 100m };
        _repositoryMock.Setup(r => r.GetByAccountAsync("0001", "123456")).ReturnsAsync(account);

        var request = new PixTransactionRequest { Agency = "0001", AccountNumber = "123456", Amount = 500m };

        var result = await _service.ValidateAndConsumeAsync(request);

        result.Approved.Should().BeFalse();
        result.CurrentLimit.Should().Be(100m);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<AccountLimit>()), Times.Never);
    }

    [Fact]
    public async Task ValidateAndConsumeAsync_NaoDeveConsumirLimite_QuandoNegado()
    {
        var account = new AccountLimit { Agency = "0001", AccountNumber = "123456", PixLimit = 200m };
        _repositoryMock.Setup(r => r.GetByAccountAsync("0001", "123456")).ReturnsAsync(account);

        var request = new PixTransactionRequest { Agency = "0001", AccountNumber = "123456", Amount = 500m };

        await _service.ValidateAndConsumeAsync(request);

        account.PixLimit.Should().Be(200m);
    }

    [Fact]
    public async Task ValidateAndConsumeAsync_DeveAprovar_QuandoValorIgualAoLimite()
    {
        var account = new AccountLimit { Agency = "0001", AccountNumber = "123456", PixLimit = 300m };
        _repositoryMock.Setup(r => r.GetByAccountAsync("0001", "123456")).ReturnsAsync(account);

        var request = new PixTransactionRequest { Agency = "0001", AccountNumber = "123456", Amount = 300m };

        var result = await _service.ValidateAndConsumeAsync(request);

        result.Approved.Should().BeTrue();
        result.CurrentLimit.Should().Be(0m);
    }

    [Fact]
    public async Task ValidateAndConsumeAsync_DeveNegar_QuandoContaNaoEncontrada()
    {
        _repositoryMock.Setup(r => r.GetByAccountAsync("0001", "999")).ReturnsAsync((AccountLimit?)null);

        var request = new PixTransactionRequest { Agency = "0001", AccountNumber = "999", Amount = 100m };

        var result = await _service.ValidateAndConsumeAsync(request);

        result.Approved.Should().BeFalse();
        result.Message.Should().Contain("não encontrada");
    }
}