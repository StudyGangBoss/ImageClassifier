﻿namespace Domain;

public class UserInfo : IIDEntity
{
    public Guid Id { get; set; }
    public long ChatId { get; set; }
    private readonly List<ImageClassification> imageClassifications = new();
    public IReadOnlyCollection<ImageClassification> ImageClassifications => imageClassifications.AsReadOnly();
    public UserInfo()
    {
    }
    
    public UserInfo(long chatId)
    {
        ChatId = chatId;
        Id = Guid.NewGuid();
    }
}