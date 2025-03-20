using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using LoanShark.Data;
using LoanShark.Domain;
using Microsoft.UI.Xaml;

namespace LoanShark.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private string? welcomeText;
        
        public event PropertyChangedEventHandler? PropertyChanged;

        public MainPageViewModel()
        {
            InitializeWelcomeText();
        }

        public string WelcomeText
        {
            get => this.welcomeText ?? "Welcome, user";
            set
            {
                if (this.welcomeText != value)
                {
                    this.welcomeText = value;
                    OnPropertyChanged();
                }
            }
        }

        private void InitializeWelcomeText()
        {
            try {
                string? firstName = UserSession.Instance.GetUserData("first_name");
                this.WelcomeText = firstName != null ? $"Welcome back, {firstName}" : "Welcome, user";
            }
            catch (Exception ex) {
                this.WelcomeText = "Welcome, user";
                Debug.Print($"Error getting user data: {ex.Message}");
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
