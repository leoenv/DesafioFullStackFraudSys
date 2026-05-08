using FraudSys.Application.DTOs;
using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces;

namespace FraudSys.Application.Services;

public class LimitService
{
    private readonly ILimitRepository _repository;

    public LimitService(ILimitRepository repository)
    {
        _repository = repository;
    }

    public async Task CreateAsync(CreateLimitRequest request)
    {
        var existing = await _repository.GetByAccountAsync(request.Agency, request.AccountNumber);
        if (existing is not null)
            throw new InvalidOperationException("Já existe uma conta cadastrada com essa agência e número de conta.");

        var accountLimit = new AccountLimit
        {
            Document = request.Document,
            Agency = request.Agency,
            AccountNumber = request.AccountNumber,
            PixLimit = request.PixLimit ?? 0
        };

        await _repository.CreateAsync(accountLimit);
    }

    public async Task<AccountLimit> GetAsync(string agency, string accountNumber)
    {
        var account = await _repository.GetByAccountAsync(agency, accountNumber);
        if (account is null)
            throw new KeyNotFoundException("Conta não encontrada. Verifique a agência e o número da conta.");

        return account;
    }

    public async Task UpdateAsync(string agency, string accountNumber, UpdateLimitRequest request)
    {
        var account = await _repository.GetByAccountAsync(agency, accountNumber);
        if (account is null)
            throw new KeyNotFoundException("Conta não encontrada. Verifique a agência e o número da conta.");

        account.PixLimit = request.PixLimit;
        await _repository.UpdateAsync(account);
    }

    public async Task DeleteAsync(string agency, string accountNumber)
    {
        var account = await _repository.GetByAccountAsync(agency, accountNumber);
        if (account is null)
            throw new KeyNotFoundException("Conta não encontrada. Verifique a agência e o número da conta.");

        await _repository.DeleteAsync(agency, accountNumber);
    }
}