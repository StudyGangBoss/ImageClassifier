using Application.Specifications;
using Domain;
using Infrastructure;
using MediatR;

namespace Application;

public class GetImageInfoHandler:IRequestHandler<GetImageInfoCommand, List<ImageInfo>>
{
    private readonly Repository<Image> imageRepository;

    public GetImageInfoHandler(Repository<Image> imageRepository)
    {
        this.imageRepository = imageRepository;
    }

    public async Task<List<ImageInfo>> Handle(GetImageInfoCommand request, CancellationToken cancellationToken)
    {
        var imageClassifications = await imageRepository.ListAsync(new ImageInfoByImageSpecification(), cancellationToken);
        return imageClassifications!;
    }
}
public record GetImageInfoCommand() : IRequest<List<ImageInfo>>;

public record GetImageCommand(Guid ImageId) : IRequest<Image>;
