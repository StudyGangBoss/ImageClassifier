using Application.Specifications;
using Domain;
using Infrastructure;
using MediatR;

namespace Application;

public class GetImageHandler:IRequestHandler<GetImageCommand, Image>
{
    private readonly Repository<Image> imageRepository;

    public GetImageHandler(Repository<Image> imageRepository)
    {
        this.imageRepository = imageRepository;
    }

    public async Task<Image> Handle(GetImageCommand request, CancellationToken cancellationToken)
    {
        var image = await imageRepository.FirstOrDefaultAsync(new ImageByIdSpecification(request.ImageId), cancellationToken);
        return image!;
    }
}