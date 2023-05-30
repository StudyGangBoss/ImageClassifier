using Application.Specifications;
using Ardalis.Specification;
using Domain;
using Infrastructure;
using MediatR;

namespace Application;

public class AddUserHandler : IRequestHandler<AddUserCommand, UserInfo>
{
    //generate Handler that will search user in repository and if it is not found, create new user
    private readonly Repository<UserInfo> repository;

    public AddUserHandler(Repository<UserInfo> repository)
    {
        this.repository = repository;
    }

    public async Task<UserInfo> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.FirstOrDefaultAsync(new UserByChatSpecification(request.ChatId), cancellationToken);
        if (user is null)
        {
            user = new UserInfo(request.ChatId, request.Role);
            await repository.AddAsync(user, cancellationToken);
        }

        return user;
    }
}

public record AddUserCommand(long ChatId, Role Role = Role.User) : IRequest<UserInfo>;
