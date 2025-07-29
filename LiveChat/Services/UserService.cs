
using System.Threading.Tasks;
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

        public async Task<User> Create(User user)
        {
            User userCreated = await _userRepository.Create(user);
            return userCreated;
        }

        public async Task<User?> GetByEmail(string email)
        {
            User? user = await _userRepository.GetByEmail(email);
            return user;
        }

        public async Task<User?> GetByUsername(string username)
        {
            User? user = await _userRepository.GetByUsername(username);
            return user;
        }

        public async Task<User> GetOrCreateGuestUser(string username)
        {
            var user = await _userRepository.GetGuestByUsername(username);
            if(user is not null)
            {
                return user;
            }
            User newUser = new User { Username = username };
            await _userRepository.AddUser(newUser);
            await _userRepository.SaveChangesAsync();
            return newUser;
        }


    }
}
