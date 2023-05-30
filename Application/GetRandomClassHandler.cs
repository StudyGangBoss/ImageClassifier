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

    public GetRandomClassHandler(
        IReadRepository<UserInfo> userInfoRepository,
        IReadRepository<Image> imageRepository,
        IReadRepository<ClassificationType> classificationTypeRepository,
        Repository<ImageClassification> imageClassificationRepository)
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
        var userClassifications = await imageRepository
            .FirstOrDefaultAsync(new ClassificationByImageAndUserSpecification(request.Image, user!), cancellationToken);
        var imageClassifications = await imageRepository
            .FirstOrDefaultAsync(new ClassificationByImageSpecification(request.Image), cancellationToken);

        var userClassIds = userClassifications!.Select(c => c.ClassificationType.Id).ToHashSet();
        var imageClassIds = imageClassifications!.Select(c => c.ClassificationType.Id).ToHashSet();

        var allClasses = await classificationTypeRepository
            .ListAsync(new ClassificationTypeSpecification(), cancellationToken);
        ClassificationType classificationType;
        var classesUndetected = allClasses
            .Where(c => !imageClassIds.Contains(c.Id))
            .ToArray();
        if (classesUndetected.Length > 0)
            classificationType = classesUndetected.OrderBy(_ => random.Next()).First();
        else
        {
            var classesUndetectedByUser = allClasses
                .Where(c => !userClassIds.Contains(c.Id))
                .ToArray();
            classificationType = classesUndetectedByUser.OrderBy(_ => random.Next()).First();
        }

        var classification = new ImageClassification(user!, request.Image, classificationType);
        await imageClassificationRepository.AddAsync(classification, cancellationToken);
        return classification;
    }
}

public record GetClassificationCommand(long ChatId, Image Image) : IRequest<ImageClassification>;