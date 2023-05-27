using Application.Specifications;
using Domain;
using Infrastructure;
using JetBrains.Annotations;
using MediatR;

namespace Application;
[UsedImplicitly]
public class AddMarkHandler:IRequestHandler<SetMarkCommand>
{
    private readonly Repository<ImageClassification> imageClassificationRepository;

    public AddMarkHandler(Repository<ImageClassification> imageClassificationRepository)
    {
        this.imageClassificationRepository = imageClassificationRepository;
    }

    public async Task Handle(SetMarkCommand request, CancellationToken cancellationToken)
    {
        var imageClassification = await imageClassificationRepository.FirstOrDefaultAsync(new ClassificationsByIdSpecification(request.ClassificationId), cancellationToken);
        if (imageClassification != null)
        {
            imageClassification.Mark = request.Mark;
            await imageClassificationRepository.UpdateAsync(imageClassification, cancellationToken);
        }
    }
}

public record SetMarkCommand(Guid ClassificationId, int Mark) : IRequest;