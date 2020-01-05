namespace Api
{
    public class TokenDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
        public string Error { get; set; }
    }
}