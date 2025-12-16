using System.Windows;

namespace BehringerMonitor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel { get; }

    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new MainWindowViewModel();
        DataContext = ViewModel;
    }
}