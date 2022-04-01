namespace Core.Models.User
{
    public class AllUsersViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }
         
        public DateTime JoinedDate { get; set; }

        public IEnumerable<string> Role { get; set; }

    }
}
