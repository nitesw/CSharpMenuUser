using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static List<User> users = new List<User>();
    static string filename = "users.txt";

    static void Main()
    {
        LoadUsersFromFile();

        while (true)
        {
            Console.WriteLine("User Management System");
            Console.WriteLine("1. Add New User");
            Console.WriteLine("2. Show All Users");
            Console.WriteLine("3. Show User by Name/Email");
            Console.WriteLine("4. Delete User");
            Console.WriteLine("5. Update User");
            Console.WriteLine("6. Save to File");
            Console.WriteLine("7. Exit");

            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            
            switch (choice)
            {
                
                case "1":
                    AddNewUser();
                    break;
                case "2":
                    ShowAllUsers();
                    break;
                case "3":
                    ShowUserByNameOrEmail();
                    break;
                case "4":
                    DeleteUser();
                    break;
                case "5":
                    UpdateUser();
                    break;
                case "6":
                    SaveUsersToFile();
                    break;
                case "7":
                    SaveUsersToFile();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void AddNewUser()
    {
        Console.Write("Enter user name: ");
        string name = Console.ReadLine();

        Console.Write("Enter user email: ");
        string email = Console.ReadLine();

        User newUser = new User(name, email);
        users.Add(newUser);

        Console.WriteLine("User added successfully.");
    }

    static void ShowAllUsers()
    {
        if (users.Count > 0)
        {
            Console.WriteLine("All Users:");
            foreach (var user in users)
            {
                Console.WriteLine($"Name: {user.Name}, Email: {user.Email}");
            }
        }
        else
        {
            Console.WriteLine("No users found.");
        }
    }

    static void ShowUserByNameOrEmail()
    {
        Console.Write("Enter user name or email to search: ");
        string searchTerm = Console.ReadLine().ToLower();

        var foundUsers = users.Where(user => user.Name.ToLower().Contains(searchTerm) || user.Email.ToLower().Contains(searchTerm)).ToList();

        if (foundUsers.Count > 0)
        {
            Console.WriteLine("Matching Users:");
            foreach (var user in foundUsers)
            {
                Console.WriteLine($"Name: {user.Name}, Email: {user.Email}");
            }
        }
        else
        {
            Console.WriteLine("No matching users found.");
        }
    }

    static void DeleteUser()
    {
        Console.Write("Enter user email to delete: ");
        string userEmail = Console.ReadLine();

        User userToDelete = users.FirstOrDefault(user => user.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

        if (userToDelete != null)
        {
            users.Remove(userToDelete);
            Console.WriteLine("User deleted successfully.");
        }
        else
        {
            Console.WriteLine("User not found.");
        }
    }

    static void UpdateUser()
    {
        Console.Write("Enter user email to update: ");
        string userEmail = Console.ReadLine();

        User userToUpdate = users.FirstOrDefault(user => user.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

        if (userToUpdate != null)
        {
            Console.Write("Enter new user name: ");
            string newName = Console.ReadLine();
            userToUpdate.Name = newName;

            Console.WriteLine("User updated successfully.");
        }
        else
        {
            Console.WriteLine("User not found.");
        }
    }

    static void SaveUsersToFile()
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (var user in users)
            {
                writer.WriteLine($"{user.Name},{user.Email}");
            }
        }

        Console.WriteLine("Users saved to file successfully.");
    }

    static void LoadUsersFromFile()
    {
        if (File.Exists(filename))
        {
            string[] lines = File.ReadAllLines(filename);

            foreach (var line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2)
                {
                    string name = parts[0];
                    string email = parts[1];

                    User loadedUser = new User(name, email);
                    users.Add(loadedUser);
                }
            }
        }
    }
}

class User
{
    public string Name { get; set; }
    public string Email { get; set; }

    public User(string name, string email)
    {
        Name = name;
        Email = email;
    }
}




