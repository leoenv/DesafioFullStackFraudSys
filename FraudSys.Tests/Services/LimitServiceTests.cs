using FluentAssertions;
using FraudSys.Application.DTOs;
using FraudSys.Application.Services;
using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces;
using Moq;

namespace FraudSys.Tests.Services;

public class LimitServiceTests
{
    private readonly Mock<ILimitRepository> _repositoryMock = new();
    private readonly LimitService _service;

    public LimitServiceTests()
    {
        _service = new LimitService(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_DeveCriarLimite_QuandoContaNaoExiste()
    {
        _repositoryMock.Setup(r => r.GetByAccountAsync("0001", "123456")).ReturnsAsync((AccountLimit?)null);

        var request = new CreateLimitRequest
        {
            Document = "12345678901",
            Agency = "0001",
            AccountNumber = "123456",
            PixLimit = 1000m
        };

        await _service.Invoking(s => s.CreateAsync(request)).Should().NotThrowAsync();
        _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<AccountLimit>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_DeveLancarExcecao_QuandoContaJaExiste()
    {
        var existing = new AccountLimit { Agency = "0001", AccountNumber = "123456", PixLimit = 500m };
        _repositoryMock.Setup(r => r.GetByAccountAsync("0001", "123456")).ReturnsAsync(existing);

        var request = new CreateLimitRequest { Agency = "0001", AccountNumber = "123456", PixLimit = 1000m };

        await _service.Invoking(s => s.CreateAsync(request))
            .Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task GetAsync_DeveRetornarConta_QuandoExiste()
    {
        var account = new AccountLimit { Agency = "0001", AccountNumber = "123456", PixLimit = 1000m };
        _repositoryMock.Setup(r => r.GetByAccountAsync("0001", "123456")).ReturnsAsync(account);

        var result = await _service.GetAsync("0001", "123456");

        result.Should().NotBeNull();
        result.PixLimit.Should().Be(1000m);
    }

    [Fact]
    public async Task GetAsync_DeveLancarExcecao_QuandoContaNaoEncontrada()
    {
        _repositoryMock.Setup(r => r.GetByAccountAsync("0001", "999999")).ReturnsAsync((AccountLimit?)null);

        await _service.Invoking(s => s.GetAsync("0001", "999999"))
            .Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task UpdateAsync_DeveAtualizarLimite_QuandoContaExiste()
    {
        var account = new AccountLimit { Agency = "0001", AccountNumber = "123456", PixLimit = 500m };
        _repositoryMock.Setup(r => r.GetByAccountAsync("0001", "123456")).ReturnsAsync(account);

        await _service.UpdateAsync("0001", "123456", new UpdateLimitRequest { PixLimit = 2000m });

        _repositoryMock.Verify(r => r.UpdateAsync(It.Is<AccountLimit>(a => a.PixLimit == 2000m)), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveRemoverConta_QuandoExiste()
    {
        var account = new AccountLimit { Agency = "0001", AccountNumber = "123456" };
        _repositoryMock.Setup(r => r.GetByAccountAsync("0001", "123456")).ReturnsAsync(account);

        await _service.DeleteAsync("0001", "123456");

        _repositoryMock.Verify(r => r.DeleteAsync("0001", "123456"), Times.Once);
    }
}