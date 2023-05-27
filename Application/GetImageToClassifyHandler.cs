using Ardalis.Specification;
using Domain;
using Infrastructure;
using MediatR;

namespace Application;

public class GetImageHandler:IRequestHandler<SelectImageCommand,SelectImageResult>
{
    IReadRepository<Image> imageRepository;
    IReadRepository<ImageClassification> imageClassificationRepository;
    IReadRepository<UserInfo> userInfoRepository;

    public async Task<SelectImageResult> Handle(SelectImageCommand request, CancellationToken cancellationToken)
    {

        var user=await userInfoRepository.FirstOrDefaultAsync(new UserByChatSpecification(request.ChatId));
    }
}

public sealed class UserByChatSpecification : Specification<UserInfo>
{
    public UserByChatSpecification(long chatId)
    {
        Query.Where(c => c.ChatId == chatId);
    }
}

public sealed class ClassificationsByUserSpecification : Specification<ImageClassification>
{
    public ClassificationsByUserSpecification(UserInfo user)
    {
        Query.Where(c => c.User == user);
    }
}

public record SelectImageCommand(long ChatId) : IRequest<SelectImageResult>;
public record SelectImageResult(Image Image, ImageClassification ImageClassification);