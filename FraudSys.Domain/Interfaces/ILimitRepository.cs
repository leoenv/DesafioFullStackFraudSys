using FraudSys.Domain.Entities;

namespace FraudSys.Domain.Interfaces;

public interface ILimitRepository
{
    Task CreateAsync(AccountLimit accountLimit);
    Task<AccountLimit?> GetByAccountAsync(string agency, string accountNumber);
    Task UpdateAsync(AccountLimit accountLimit);
    Task DeleteAsync(string agency, string accountNumber);
}