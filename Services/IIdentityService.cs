using System.Threading.Tasks;
using PostBook.Contracts.Requests;
using PostBook.Domains;

namespace PostBook.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(UserRegisterRequest request);
        Task<AuthenticationResult> LoginAsync(UserLoginRequest request);
        Task<AuthenticationResult> RefreshAsync(RefreshTokenRequest request);
    }
}