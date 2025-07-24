
using LiveChat.Entities;
using LiveChat.Repositories.Interfaces;
using LiveChat.Services.Interfaces;

namespace LiveChat.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetOrCreateGuestUser(string username)
        {
            try
            {
                var user = await _userRepository.GetGuestByUsername(username);
                if(user is not null)
                {
                    return user;
                }
                User newUser = new User { Username = username, IsGuest = true };
                await _userRepository.AddUser(newUser);
                await _userRepository.SaveChangesAsync();
                return newUser;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
