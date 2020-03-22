namespace Bookshelf.Core
{
    public interface IEmailHelper
    {
        void SendResetToken(string email, string url);
    }
}