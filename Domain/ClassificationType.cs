namespace Domain;

public class QuestionInfo:IIDEntity
{
    public string Question;
    public string ClassificationType;

    public Guid Id { get; set; }
}