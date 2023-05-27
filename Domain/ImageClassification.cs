namespace Domain;

public class ImageClassification : IIDEntity
{
    public UserInfo User;
    public Image Image;
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

    public ClassificationType ClassificationType { get; set; }
    public int Mark { get; set; }
    public Guid Id { get; set; }
}