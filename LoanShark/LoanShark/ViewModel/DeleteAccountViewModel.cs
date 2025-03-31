using System.Threading.Tasks;
using LoanShark.Service;

namespace LoanShark.ViewModel
{
    public class DeleteAccountViewModel
    {
        private readonly UserService _userService;

        public DeleteAccountViewModel()
        {
            _userService = new UserService();
        }

        public async Task<string> DeleteAccount(string password)
        {
            return await _userService.DeleteUser(password);
        }
    }
}
