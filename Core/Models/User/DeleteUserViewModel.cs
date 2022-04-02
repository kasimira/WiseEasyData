namespace Core.Models.User
{
    public class DeleteUserViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
