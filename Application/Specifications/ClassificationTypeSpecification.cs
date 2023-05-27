using Ardalis.Specification;
using Domain;

namespace Application.Specifications;

public sealed class ClassificationTypeSpecification : Specification<ClassificationType>
{
    public ClassificationTypeSpecification(ImageClassification[] classifications)
    {
        var classIds = classifications.Select(c => c.ClassificationType.Id);
        Query.Where(i => !classIds.Contains(i.Id));
    }
}