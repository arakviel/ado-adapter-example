using System.ComponentModel;
using System.Windows;

namespace AdoNetAdapterDemoWpf;

public partial class MainWindow : Window
{
    private readonly UsersViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new UsersViewModel();
        DataContext = _viewModel;
        Closing += MainWindow_Closing;
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var newUser = new User { Name = "New User", Email = "user@example.com" };
        _viewModel.AddUser(newUser);
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (UsersGrid.SelectedItem is User selectedUser)
        {
            selectedUser.Name = "Updated Name"; // You may add edit dialog for user inputs
            selectedUser.Email = "updated@example.com";
            _viewModel.UpdateUser(selectedUser);
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (UsersGrid.SelectedItem is User selectedUser)
        {
            _viewModel.DeleteUser(selectedUser);
        }
    }

    private void MainWindow_Closing(object sender, CancelEventArgs e)
    {
        _viewModel.SaveChanges();
    }
}