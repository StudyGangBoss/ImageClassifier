using Ardalis.Specification;
using Domain;

namespace Application.Specifications;

public sealed class ImageWithoutMarksSpecification : Specification<Image>
{
    public ImageWithoutMarksSpecification(UserInfo user)
    {
        Query.Include(i => i.ImageClassifications).ThenInclude(ic=>ic.User);
        Query
            .OrderBy(i=> i.ImageClassifications.Count)
            .Where(i => i.ImageClassifications.All(c => c.User.Id != user.Id));
    }
}

public sealed class ImageByIdSpecification : Specification<Image>
{
    public ImageByIdSpecification(Guid imageId)
    {
        Query
            .Where(i => i.Id== imageId);
    }
}