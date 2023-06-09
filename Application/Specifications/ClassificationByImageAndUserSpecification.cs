﻿using Ardalis.Specification;
using Domain;

namespace Application.Specifications;

public sealed class ClassificationByImageAndUserSpecification : Specification<Image,ImageClassification[]>
{
    public ClassificationByImageAndUserSpecification(Image image, UserInfo user)
    {
        Query.Include(i=> i.ImageClassifications).ThenInclude(ic=>ic.User);
        Query.Where(i => i.Id == image.Id);
        Query.Select(i => 
            i.ImageClassifications.Where(ic=>ic.User.Id==user.Id).ToArray())
            .Take(1);
    }
}