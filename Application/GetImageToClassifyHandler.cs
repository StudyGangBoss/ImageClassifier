using Application.Specifications;
using Domain;
using Infrastructure;
using JetBrains.Annotations;
using MediatR;

namespace Application;
[UsedImplicitly]
public class GetImageToClassifyHandler:IRequestHandler<GetImageToClassifyCommand,GetImageToClassifyResult>
{
    private readonly IReadRepository<Image> imageRepository;
    private readonly IReadRepository<UserInfo> userInfoRepository;

    public GetImageToClassifyHandler(IReadRepository<Image> imageRepository, IReadRepository<UserInfo> userInfoRepository)
    {
        this.imageRepository = imageRepository;
        this.userInfoRepository = userInfoRepository;
    }

    public async Task<GetImageToClassifyResult> Handle(GetImageToClassifyCommand request, CancellationToken cancellationToken)
    {

        var user=await userInfoRepository.FirstOrDefaultAsync(new UserByChatSpecification(request.ChatId), cancellationToken);
        var imageWithoutMarks=await imageRepository.FirstOrDefaultAsync(new ImageWithoutMarksSpecification(user!), cancellationToken);
        if(imageWithoutMarks is not null)
            return new GetImageToClassifyResult(imageWithoutMarks);
        var imageWithMinimumMarks=await imageRepository.FirstOrDefaultAsync(new ImageWithMinimumMarksSpecification(), cancellationToken);
        return new GetImageToClassifyResult(imageWithMinimumMarks!);
        
    }
}


public record GetImageToClassifyCommand(long ChatId) : IRequest<GetImageToClassifyResult>;
public record GetImageToClassifyResult(Image Image);