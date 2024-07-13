using Authorizations.Service;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Authorizations;

/// <summary>
/// Interaction logic for RegisterWindow.xaml
/// </summary>
public partial class RegisterWindow : Window
{
    private readonly UserAccountService _userAccountService;
    public bool IsRegistered { get; private set; }
    public string RegisteredLogin { get; private set; }
    public string RegisteredPassword { get; private set; }
  

    public RegisterWindow()
    {
        InitializeComponent();
        _userAccountService = new();
    }

    private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
    {
        if (PasswordBox.Visibility == Visibility.Visible)
        {
            PasswordTextBox.Text = PasswordBox.Password;
            PasswordBox.Visibility = Visibility.Collapsed;
            PasswordTextBox.Visibility = Visibility.Visible;
        }
        else
        {
            PasswordBox.Password = PasswordTextBox.Text;
            PasswordTextBox.Visibility = Visibility.Collapsed;
            PasswordBox.Visibility = Visibility.Visible;
        }
    }

    private void ToggleConfirmPasswordVisibility_Click(object sender, RoutedEventArgs e)
    {
        if (ConfirmPasswordBox.Visibility == Visibility.Visible)
        {
            ConfirmPasswordTextBox.Text = ConfirmPasswordBox.Password;
            ConfirmPasswordBox.Visibility = Visibility.Collapsed;
            ConfirmPasswordTextBox.Visibility = Visibility.Visible;
        }
        else
        {
            ConfirmPasswordBox.Password = ConfirmPasswordTextBox.Text;
            ConfirmPasswordTextBox.Visibility = Visibility.Collapsed;
            ConfirmPasswordBox.Visibility = Visibility.Visible;
        }
    }

    private void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        string login = LoginTextBox.Text;
        string password = PasswordBox.Password;
        string confirmPassword = ConfirmPasswordBox.Password;
        if (string.IsNullOrEmpty(login) & string.IsNullOrEmpty(password) & string.IsNullOrEmpty(confirmPassword)) 
        {
            MessageBox.Show("You have entered a blank Username or Password.", "Error", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            return;
        }

        if (!IsValidLogin(login))
        {
            MessageBox.Show("Login must be at least 8 characters long and contain only Latin letters and numbers.","Eror", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            return;
        }

        if (!IsValidPassword(password))
        {
            MessageBox.Show("Password must be at least 8 characters long, contain one uppercase letter and one number.","Eror",MessageBoxButton.YesNo,MessageBoxImage.Warning);
            return;
        }

        if (password != confirmPassword)
        {
            MessageBox.Show("The password you entered and the password you reentered did not match", "Eror", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            return;
        }
        IsRegistered = true;
        RegisteredLogin = login;
        RegisteredPassword = password;
        _userAccountService.Create(login, password);
        MessageBox.Show("Registration completed successfully");
        this.Close();
    }

    private bool IsValidLogin(string login)
    {
        return login.Length >= 8 && System.Text.RegularExpressions.Regex.IsMatch(login, "^[a-zA-Z0-9]+$");
    }

    private bool IsValidPassword(string password)
    {
        return password.Length >= 8 &&
               System.Text.RegularExpressions.Regex.IsMatch(password, "[A-Z]") &&
               System.Text.RegularExpressions.Regex.IsMatch(password, "[0-9]");
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
