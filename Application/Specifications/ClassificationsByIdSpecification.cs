using Ardalis.Specification;
using Domain;

namespace Application.Specifications;

public sealed class ClassificationsByIdSpecification : Specification<ImageClassification>
{
    public ClassificationsByIdSpecification(Guid id)
    {
        Query.Where(c => c.Id == id);
    }
}