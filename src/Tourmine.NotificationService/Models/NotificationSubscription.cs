namespace Tourmine.NotificationService.Models
{
    public class NotificationSubscription
    {
        public Guid TournamentId { get; set; }
        public Guid UserId { get; set; }
    }
}
