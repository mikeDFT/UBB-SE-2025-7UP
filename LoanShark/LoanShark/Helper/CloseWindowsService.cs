using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.Helper
{
    public static class CloseWindowService
    {
        public static event Action CloseAllWindows;

        public static void RequestCloseAllWindows()
        {
            CloseAllWindows?.Invoke();
        }
    }
}
