using Authorizations.Service;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Authorizations;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly UserAccountService _userAccountService;
    private string filePath = "C:\\Users\\Пользователь\\OneDrive\\Ishchi stol\\RememberMeCheck.txt";

    public MainWindow()
    {
        InitializeComponent();
        _userAccountService = new();
        LoadCredentials();
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

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        string login = LoginTextBox.Text;
        string password = PasswordBox.Password;
        if(login ==string.Empty &  password == string.Empty)
        {
            MessageBox.Show("You have not entered a Username or Password", "Error",MessageBoxButton.YesNo, MessageBoxImage.Warning);
            return;
        }
        var credentials = LoadUsers();
        if (credentials.ContainsKey(login))
        {
            if (RememberMeCheckBox.IsChecked == true)
            {
                SaveCredentials(login, password);
                
            }

            MessageBox.Show("You entered correctly","Successfully", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
        else
        {
            MessageBox.Show("You entered the login or password incorrectly", "Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
        }
    }
    private void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        var registerWindow = new RegisterWindow();
        registerWindow.ShowDialog();

        if (registerWindow.IsRegistered)
        {
            LoginTextBox.Text = registerWindow.RegisteredLogin;
            PasswordBox.Password = registerWindow.RegisteredPassword;
        }
    }

    private void LoadCredentials()
    {
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            if (lines.Length >= 2)
            {
                LoginTextBox.Text = lines[0];
                PasswordBox.Password = lines[1];
            }
        }
    }

    public void SaveCredentials(string login, string password)
    {
        File.WriteAllLines(filePath, new[] { login, password });
        
        //userNameAndPasswords.Add(login, password);
    }

    private Dictionary<string, string> LoadUsers()
    {
        var userAccounts=_userAccountService.GetUserAccount();
        
        var result= new Dictionary<string, string>();
        foreach (var user in userAccounts)
        {
            result.Add(user.UserName, user.Password);
        }
        return result;
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

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return HashPassword(password) == hashedPassword;
    }
}