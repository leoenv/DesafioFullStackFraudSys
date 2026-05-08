using FraudSys.Application.DTOs;
using FraudSys.Domain.Interfaces;

namespace FraudSys.Application.Services;

public class PixTransactionService
{
    private readonly ILimitRepository _repository;

    public PixTransactionService(ILimitRepository repository)
    {
        _repository = repository;
    }

    public async Task<PixTransactionResponse> ValidateAndConsumeAsync(PixTransactionRequest request)
    {
        var account = await _repository.GetByAccountAsync(request.Agency, request.AccountNumber);

        if (account is null)
            return new PixTransactionResponse
            {
                Approved = false,
                Message = "Conta não encontrada. Verifique a agência e o número da conta.",
                CurrentLimit = 0
            };

        if (!account.HasSufficientLimit(request.Amount ?? 0))
            return new PixTransactionResponse
            {
                Approved = false,
                Message = "Transação negada por limite PIX insuficiente.",
                CurrentLimit = account.PixLimit
            };

        account.ConsumeLimit(request.Amount ?? 0);
        await _repository.UpdateAsync(account);

        return new PixTransactionResponse
        {
            Approved = true,
            Message = "Transação PIX aprovada com sucesso.",
            CurrentLimit = account.PixLimit
        };
    }
}