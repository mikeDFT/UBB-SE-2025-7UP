using System;
using LoanShark.Domain;
using Microsoft.UI.Xaml.Data;

namespace LoanShark.Helper
{
    public class BankAccountDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is BankAccount account)
            {
                return string.IsNullOrEmpty(account.Name)
                    ? $"{account.Iban} ({account.Currency})"
                    : account.Name;
            }
            return "Unknown Account";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
