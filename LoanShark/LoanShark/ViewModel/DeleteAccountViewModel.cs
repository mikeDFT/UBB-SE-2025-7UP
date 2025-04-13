using System.Threading.Tasks;
using LoanShark.Service;

namespace LoanShark.ViewModel
{
    public class DeleteAccountViewModel
    {
        private readonly UserService userService;

        public DeleteAccountViewModel()
        {
            userService = new UserService();
        }

        public async Task<string> DeleteAccount(string password)
        {
            return await userService.DeleteUser(password);
        }
    }
}
