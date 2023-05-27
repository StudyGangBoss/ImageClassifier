using Ardalis.Specification;
using Domain;

namespace Application.Specifications;

public sealed class ImageWithMinimumMarksSpecification : Specification<Image>
{
    public ImageWithMinimumMarksSpecification()
    {
        Query.OrderBy(i=>i.ImageClassifications.Count);
    }
}