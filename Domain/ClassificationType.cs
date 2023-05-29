namespace Domain;

public class ClassificationType : IIDEntity
{
    public ClassificationType()
    {
    }

    public ClassificationType(string question, string classType)
    {
        Question = question;
        Class = classType;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
    public string Question;
    public string Class;
}