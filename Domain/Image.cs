namespace Domain;

public class Image : IIDEntity
{
    public Guid Id { get; set; }
    public byte[] ImageData;
    public byte[] NeuralImageData;

    private readonly List<ImageClassification> imageClassifications = new();
    public IReadOnlyCollection<ImageClassification> ImageClassifications => imageClassifications.AsReadOnly();

    public Image()
    {
    }
}