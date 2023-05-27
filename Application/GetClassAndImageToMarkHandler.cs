using Domain;
using MediatR;

namespace Application;

public class GetClassAndImageToMarkHandler : IRequestHandler<GetClassAndImageToMarkCommand, ImageClassification>
{
    private ISender sender;

    public GetClassAndImageToMarkHandler(ISender sender)
    {
        this.sender = sender;
    }

    public async Task<ImageClassification> Handle(GetClassAndImageToMarkCommand request, CancellationToken cancellationToken)
    {
        var getImageToClassifyResult=await sender.Send(new GetImageToClassifyCommand(request.ChatId));
        var classification=await sender.Send(new GetClassificationCommand(request.ChatId, getImageToClassifyResult.Image), cancellationToken);

        return classification;
    }
}