using Application.Specifications;
using Domain;
using Infrastructure;
using JetBrains.Annotations;
using MediatR;

namespace Application;

[UsedImplicitly]
public class AddImageHandler : IRequestHandler<AddImageCommand, AddImageResult>
{
    private readonly Repository<Image> imageRepository;
    private readonly IReadRepository<UserInfo> userInfoRepository;

    public AddImageHandler(Repository<Image> imageRepository, IReadRepository<UserInfo> userInfoRepository)
    {
        this.imageRepository = imageRepository;
        this.userInfoRepository = userInfoRepository;
    }

    public async Task<AddImageResult> Handle(AddImageCommand request, CancellationToken cancellationToken)
    {
        var user = await userInfoRepository.FirstOrDefaultAsync(new UserByChatSpecification(request.ChatId), cancellationToken);
        if (user!.Role != Role.Admin)
            return new AddImageResult(null);

        var array = request.ImageStream.ToArray();
        await request.ImageStream.DisposeAsync();
        var image = new Image(array);
        await imageRepository.AddAsync(image, cancellationToken);
        return new AddImageResult(image);
    }
}

public record AddImageCommand(long ChatId, MemoryStream ImageStream) : IRequest<AddImageResult>;

public record AddImageResult(Image? Image);