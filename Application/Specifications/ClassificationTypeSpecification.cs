using Ardalis.Specification;
using Domain;

namespace Application.Specifications;

public sealed class ClassificationTypeSpecification : Specification<ClassificationType>
{
    public ClassificationTypeSpecification()
    {
        Query.Where(i => true);
    }
}