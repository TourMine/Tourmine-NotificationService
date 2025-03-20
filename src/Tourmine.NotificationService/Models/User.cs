namespace Tourmine.NotificationService.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public EUserType UserType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public enum EUserType
    {
        Organizer = 1,
        Player = 2
    }
}
