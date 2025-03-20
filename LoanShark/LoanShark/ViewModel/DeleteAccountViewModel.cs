using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanShark.Service;

namespace LoanShark.ViewModel
{
    public class DeleteAccountViewModel
    {
        private readonly UserService _userService;
        private readonly int _currentUserId;

        public DeleteAccountViewModel(int currentUserId)
        {
            _userService = new UserService();
            _currentUserId = currentUserId;
        }

        public string DeleteAccount(string password)
        {
            return _userService.DeleteUser(_currentUserId, password);
        }
    }
}
