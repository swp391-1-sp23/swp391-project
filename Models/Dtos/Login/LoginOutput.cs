namespace SWP391.Project.Models.Dtos.Login
{
    public class LoginOutput
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}