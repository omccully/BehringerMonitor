using System.Windows;
using System.Windows.Controls;

namespace BehringerMonitor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IDisposable
{
    public MainWindowViewModel ViewModel { get; }

    public MainWindow()
    {
        ViewModel = new MainWindowViewModel();
        DataContext = ViewModel;
        InitializeComponent();
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        PasswordBox pwBox = (PasswordBox)sender;
        ViewModel.SettingsTab.GitHubApiKey = pwBox.Password;
    }

    private void PasswordBox_Initialized(object sender, EventArgs e)
    {
        PasswordBox pwBox = (PasswordBox)sender;
        pwBox.Password = ViewModel.SettingsTab.GitHubApiKey;
    }

    public void Dispose()
    {
        ViewModel.Dispose();
    }


}