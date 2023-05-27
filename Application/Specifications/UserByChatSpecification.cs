using Ardalis.Specification;
using Domain;

namespace Application.Specifications;

public sealed class UserByChatSpecification : Specification<UserInfo>
{
    public UserByChatSpecification(long chatId)
    {
        Query.Where(c => c.ChatId == chatId);
    }
}