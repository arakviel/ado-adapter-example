using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;

namespace AdoNetAdapterDemoWpf;

public class UsersViewModel : INotifyPropertyChanged
{
    private readonly string _connectionString = "Server=.\\SQLEXPRESS;Database=Demo1;Integrated Security=True;TrustServerCertificate=True;";
    private DataTable _usersTable;
    private SqlDataAdapter _adapter;

    public ObservableCollection<User> Users { get; set; }

    public UsersViewModel()
    {
        Users = new ObservableCollection<User>();
        LoadUsers();
    }

    public void LoadUsers()
    {
        _adapter = new SqlDataAdapter("SELECT * FROM Users", _connectionString);
        new SqlCommandBuilder(_adapter);
        _usersTable = new DataTable();
        _adapter.Fill(_usersTable);

        foreach (DataRow row in _usersTable.Rows)
        {
            Users.Add(new User
            {
                Id = (int)row["Id"],
                Name = row["Name"].ToString(),
                Email = row["Email"].ToString()
            });
        }
    }

    public void AddUser(User user)
    {
        var newRow = _usersTable.NewRow();
        newRow["Name"] = user.Name;
        newRow["Email"] = user.Email;
        _usersTable.Rows.Add(newRow);

        Users.Add(user);
    }

    public void UpdateUser(User user)
    {
        var row = _usersTable.Select($"Id = {user.Id}").FirstOrDefault();
        if (row != null)
        {
            row["Name"] = user.Name;
            row["Email"] = user.Email;
        }
    }

    public void DeleteUser(User user)
    {
        var row = _usersTable.Select($"Id = {user.Id}").FirstOrDefault();
        if (row != null)
        {
            row.Delete();
            Users.Remove(user);
        }
    }

    public void SaveChanges()
    {
        using var connection = new SqlConnection(_connectionString);
        _adapter.Update(_usersTable);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}