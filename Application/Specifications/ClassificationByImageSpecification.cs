using Ardalis.Specification;
using Domain;

namespace Application.Specifications;

public sealed class ClassificationByImageSpecification : Specification<Image,ImageClassification[]>
{
    public ClassificationByImageSpecification(Image image)
    {
        Query.Include(i=> i.ImageClassifications);
        Query.Where(i => i.Id == image.Id);
        
        Query.Select(i => 
            i.ImageClassifications.ToArray())
            .Take(1);
    }
}