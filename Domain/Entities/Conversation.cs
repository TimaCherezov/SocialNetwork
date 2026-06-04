using Domain.Enums;

namespace Domain.Entities;

public class Conversation
{
    public Guid Id { get; set; }
    public ConversationType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? LastMessageId { get; set; }
    public Message? LastMessage { get; set; }

    public Guid? DirectUserAId { get; set; }
    public Guid? DirectUserBId { get; set; }

    public ICollection<ConversationParticipant> Participants { get; set; } = new List<ConversationParticipant>();
}