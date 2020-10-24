namespace PostBook.Contracts.Requests
{
    public class UserLoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}