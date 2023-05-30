using System.Data;
using Ardalis.Specification;
using Domain;

namespace Application.Specifications;

public sealed class ImageInfoByImageSpecification : Specification<Image, ImageInfo>
{
    public ImageInfoByImageSpecification()
    {
        Query.Include(i => i.ImageClassifications).ThenInclude(ic => ic.User);
        Query
            .Select(i => Map(i));
    }

    static ImageInfo Map(Image image)
    {
        var imageInfo = new ImageInfo() { ImageId = image.Id };
        var groupped = image.ImageClassifications.GroupBy(ic => ic.ClassificationType);
        var classes = new List<ClassInfo>();
        foreach (var group in groupped)
        {
            var classInfo = new ClassInfo() { Type = group.Key.Class };
            var sumOfMarks = group.Sum(g => g.Mark);
            if (sumOfMarks < 2.5*group.Count())
                classInfo.Value = 0;
            else
                classInfo.Value = 1;
            classes.Add(classInfo);
        }

        imageInfo.Classes = classes.ToArray();
        return imageInfo;
    }
}