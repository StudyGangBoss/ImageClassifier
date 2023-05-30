namespace Domain;

public class ImageClassification : IIDEntity
{
    public UserInfo User;
    public Image Image;
    public ClassificationType ClassificationType { get; set; }
    public Guid ClassificationTypeId { get; set; }
    public Guid ImageId { get; set; }
    public Guid UserId;

    public ImageClassification( UserInfo user, Image image, ClassificationType classificationType)
    {
        User = user;
        Image = image;
        ClassificationType = classificationType;
        UserId = user.Id;
        Id = Guid.NewGuid();
    }
    
    public ImageClassification( )
    {
    }

    public int Mark { get; set; } = -1;

    public Guid Id { get; set; }
}