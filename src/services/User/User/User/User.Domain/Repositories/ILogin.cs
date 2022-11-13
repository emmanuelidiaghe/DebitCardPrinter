using User.User.Domain.Entities.Login;

namespace User.User.Domain.Repositories
{
    public interface ILogin
    {
        public LoginUtilityResponse UserLogin(LoginRequest login);
    }
}

