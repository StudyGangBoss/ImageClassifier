using Application.Specifications;
using Ardalis.Specification;
using Domain;
using MediatR;

namespace Application;

public class AddUserHandler : IRequestHandler<AddUserCommand, UserInfo>
{
    //generate Handler that will search user in repository and if it is not found, create new user
    private readonly IRepositoryBase<UserInfo> repository;

    public AddUserHandler(IRepositoryBase<UserInfo> repository)
    {
        this.repository = repository;
    }

    public async Task<UserInfo> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.FirstOrDefaultAsync(new UserByChatSpecification(request.chatId), cancellationToken);
        if (user is null)
        {
            user = new UserInfo(request.chatId);
            await repository.AddAsync(user, cancellationToken);
        }

        return user;
    }
}

public record AddUserCommand(long chatId) : IRequest<UserInfo>;
