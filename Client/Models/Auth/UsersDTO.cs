namespace Client.Models.Auth
{
    public class UsersDTO
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
