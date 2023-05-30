namespace Application;

public class ImageInfo
{
    public Guid ImageId;
    public ClassInfo[] Classes;
}

public class ClassInfo
{
    public string Type;
    public int Value;
}