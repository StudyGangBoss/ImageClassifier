using Application.Specifications;
using Domain;
using Infrastructure;
using JetBrains.Annotations;
using MediatR;

namespace Application;

[UsedImplicitly]
public class AddClassHandler : IRequestHandler<AddClassCommand, AddClassResult>
{
    private readonly IReadRepository<UserInfo> userInfoRepository;
    private readonly Repository<ClassificationType> classTypeRepository;

    public AddClassHandler(IReadRepository<UserInfo> userInfoRepository, Repository<ClassificationType> classTypeRepository)
    {
        this.userInfoRepository = userInfoRepository;
        this.classTypeRepository = classTypeRepository;
    }

    public async Task<AddClassResult> Handle(AddClassCommand request, CancellationToken cancellationToken)
    {
        var user = await userInfoRepository.FirstOrDefaultAsync(new UserByChatSpecification(request.ChatId), cancellationToken);
        if (user!.Role != Role.Admin)
            return new AddClassResult(null);
        var classType = new ClassificationType(request.Question, request.ClassType);
        await classTypeRepository.AddAsync(classType, cancellationToken);
        return new AddClassResult(classType.Id);
    }
}

public record AddClassCommand(long ChatId, string Question, string ClassType) : IRequest<AddClassResult>;

public record AddClassResult(Guid? id);
