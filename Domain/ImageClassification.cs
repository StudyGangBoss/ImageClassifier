namespace Domain;

public class ImageClassification : IDEntity
{
    public UserInfo User;
    public Image Image;

    public ImageClassification(Guid id, UserInfo user, Image image, string classificationType, int mark)
    {
        User = user;
        Image = image;
        ClassificationType = classificationType;
        Mark = mark;
        Id = id;
    }

    public string ClassificationType { get; set; }
    public int Mark { get; set; }
    public Guid Id { get; set; }
}