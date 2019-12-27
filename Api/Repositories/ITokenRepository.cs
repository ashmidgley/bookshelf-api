namespace Api
{
    public interface ITokenRepository
    {
        bool Authenticate(LoginDto login);
    }
}