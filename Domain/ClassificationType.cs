namespace Domain;

public class ClassificationType:IIDEntity
{
    public Guid Id { get; set; }
    public string Question;
    public string Class;
}