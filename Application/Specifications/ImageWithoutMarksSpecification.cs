using Ardalis.Specification;
using Domain;

namespace Application.Specifications;

public sealed class ImageWithoutMarksSpecification : Specification<Image>
{
    public ImageWithoutMarksSpecification(UserInfo user)
    {
        Query.Where(i => i.ImageClassifications.All(c => c.User.Id != user.Id));
    }
}