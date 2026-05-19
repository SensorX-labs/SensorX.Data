using Ardalis.Specification;
using SensorX.Data.Domain.StrongIDs;

namespace SensorX.Data.Domain.Contexts.UserContext.StaffAggregate.Specs;

public sealed class AccountIdSpec : Specification<Staff>
{
    public AccountIdSpec(AccountId accountId)
    {
        Query.Where(x => x.AccountId == accountId);
    }
}