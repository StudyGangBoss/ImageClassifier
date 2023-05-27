using Domain;
using JetBrains.Annotations;
using MediatR;

namespace Application;
[UsedImplicitly]
public class GetClassAndImageToMarkHandler : IRequestHandler<GetClassAndImageToMarkCommand, GetClassAndImageResult>
{
    private readonly ISender sender;

    public GetClassAndImageToMarkHandler(ISender sender)
    {
        this.sender = sender;
    }

    public async Task<GetClassAndImageResult> Handle(GetClassAndImageToMarkCommand request, CancellationToken cancellationToken)
    {
        var getImageToClassifyResult=await sender.Send(new GetImageToClassifyCommand(request.ChatId));
        var classification=await sender.Send(new GetClassificationCommand(request.ChatId, getImageToClassifyResult.Image), cancellationToken);

        return new GetClassAndImageResult(classification,getImageToClassifyResult.Image);
    }
}
public record GetClassAndImageToMarkCommand(long ChatId) : IRequest<GetClassAndImageResult>;

public record GetClassAndImageResult(ImageClassification ImageClassification, Image Image);
