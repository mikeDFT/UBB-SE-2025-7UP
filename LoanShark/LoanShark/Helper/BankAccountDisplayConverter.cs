using LoanShark.Domain;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanShark.Domain;

namespace LoanShark.Helper
{
    public class BankAccountDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is BankAccount account)
            {
                return string.IsNullOrEmpty(account.name)
                    ? $"{account.iban} ({account.currency})"
                    : account.name;
            }
            return "Unknown Account";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
