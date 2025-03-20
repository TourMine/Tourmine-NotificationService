namespace Tourmine.NotificationService.Models
{
    public class Tournament
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Game { get; set; }
        public int Plataform { get; set; }
        public int MaxTeams { get; set; }
        public int TeamsType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Prize { get; set; }
        public int SubscriptionType { get; set; }
        public int Status { get; set; }
        public string? Description { get; set; }
        public Guid Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }

    public enum EPlataforms
    {
        PC = 1,
        PS4 = 2,
        PS5 = 3,
        XBOX_ONE = 4,
        XBOX_SERIES = 5,
        SWITCH = 6,
        MOBILE = 7
    }

    public enum EParticipantsType
    {
        SINGLE = 1,
        DUO = 2,
        SQUAD = 3
    }

    public enum ESubscriptionType
    {
        FREE = 1,
        PAID = 2
    }

    public enum ETournamentStatus
    {
        Planning = 1,
        Open = 2,
        InProgress = 3,
        Finished = 4
    }
}
