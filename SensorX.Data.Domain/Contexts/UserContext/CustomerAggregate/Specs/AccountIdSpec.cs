using Ardalis.Specification;
using SensorX.Data.Domain.StrongIDs;

namespace SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate.Specs;

public sealed class AccountIdSpec : Specification<Customer>
{
    public AccountIdSpec(AccountId accountId)
    {
        Query.Where(x => x.AccountId == accountId);
    }
}