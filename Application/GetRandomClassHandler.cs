using Application.Specifications;
using Domain;
using Infrastructure;
using JetBrains.Annotations;
using MediatR;

namespace Application;
[UsedImplicitly]
public class GetRandomClassHandler : IRequestHandler<GetClassificationCommand, ImageClassification>
{
    private readonly IReadRepository<Image> imageRepository;
    private readonly IReadRepository<UserInfo> userInfoRepository;
    private readonly IReadRepository<ClassificationType> classificationTypeRepository;
    private readonly Repository<ImageClassification> imageClassificationRepository;

    public GetRandomClassHandler(IReadRepository<UserInfo> userInfoRepository, IReadRepository<Image> imageRepository, IReadRepository<ClassificationType> classificationTypeRepository, Repository<ImageClassification> imageClassificationRepository)
    {
        this.userInfoRepository = userInfoRepository;
        this.imageRepository = imageRepository;
        this.classificationTypeRepository = classificationTypeRepository;
        this.imageClassificationRepository = imageClassificationRepository;
    }

    public async Task<ImageClassification> Handle(GetClassificationCommand request, CancellationToken cancellationToken)
    {
        var random = new Random();
        var user = await userInfoRepository.FirstOrDefaultAsync(new UserByChatSpecification(request.ChatId), cancellationToken);
        var userClassifications =  await imageRepository.FirstOrDefaultAsync(new ClassificationByImageAndUserSpecification(request.Image, user!), cancellationToken);
        if(userClassifications is null)
            return null;
        var availableClasses=await classificationTypeRepository.ListAsync(new ClassificationTypeSpecification(userClassifications!), cancellationToken);
        //get class from available classes using Random and store it to variable
        var classificationType = availableClasses.OrderBy(_=>random.Next()).First();
        var classification = new ImageClassification(user!, request.Image, classificationType);
        await imageClassificationRepository.AddAsync(classification, cancellationToken);
        return classification;
    }
}

public record GetClassificationCommand(long ChatId, Image Image) : IRequest<ImageClassification>;

