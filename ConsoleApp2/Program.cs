using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Threading;

class Program
{
    static List<User> users = new List<User>();
    static bool isChanged = false;
    private static Timer? autosaveTimer;

    static void AutoSave(object state)
    {
        if(isChanged == true)
        {
            SaveUsersToFile();
            //Console.WriteLine("Auto saved successfully!\n");
        }
        //else
        //{
        //    Console.WriteLine("There is nothing to change!\n");
        //}
    }

    static void Main()
    {
        try
        {
            var autoEvent = new AutoResetEvent(false);
            autosaveTimer = new Timer(AutoSave, autoEvent, 0, 10000);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        

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
                    Console.Clear();
                    Console.WriteLine("Adding new user\n");
                    AddNewUser();
                    break;
                case "2":
                    Console.Clear();
                    ShowAllUsers();
                    break;
                case "3":
                    Console.Clear();
                    ShowUserByNameOrEmail();
                    break;
                case "4":
                    Console.Clear();
                    Console.WriteLine("Deleting user\n");
                    DeleteUser();
                    break;
                case "5":
                    Console.Clear();
                    Console.WriteLine("Updating user\n");
                    UpdateUser();
                    break;
                case "6":
                    Console.Clear();
                    SaveUsersToFile();
                    Console.Clear();
                    break;
                case "7":
                    Console.Clear();
                    SaveUsersToFile();
                    Environment.Exit(0);
                    Console.Clear();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid option. Please try again.\n");
                    break;
            }
        }
    }

    static void AddNewUser()
    {
        User newUser = new User();

        Console.Write("Enter user name: ");
        string name = Console.ReadLine();
        if(!string.IsNullOrWhiteSpace(name))
        {
            newUser.Name = name;
        }
        else
        {
            bool isNull = true;
            do
            {
                Console.WriteLine("Enter proper name!\n");
                Console.Write("Enter user name: ");
                name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    newUser.Name = name;
                    isNull = false;
                }
            } while (isNull);
        }

        Console.Write("Enter user email: ");
        string email = Console.ReadLine();
        if(Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
        {
            foreach (var user in users)
            {
                if(user.Email == email)
                {
                    do
                    {
                        Console.WriteLine("Enter proper or unique email!\n");
                        Console.Write("Enter user email: ");
                        email = Console.ReadLine();
                        if(Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                        {
                            newUser.Email = email;
                        }
                    } while (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"));
                }
                break;
            }
            newUser.Email = email;
        }
        else
        {
            do
            {
                Console.WriteLine("Enter proper or unique email!\n");
                Console.Write("Enter user email: ");
                email = Console.ReadLine();
                if (Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    newUser.Email = email;
                }
            } while (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"));
        }

        users.Add(newUser);
        isChanged = true;

        Console.Clear();
        Console.WriteLine("User added successfully.\n");
    }

    static void ShowAllUsers()
    {
        if (users.Count > 0)
        {
            Console.WriteLine("All Users:\n");
            foreach (var user in users)
            {
                Console.WriteLine($"Name: {user.Name}, Email: {user.Email}");
            }
            Console.WriteLine();
        }
        else
        {
            Console.Clear();
            Console.WriteLine("No users found.\n");
        }
    }

    static void ShowUserByNameOrEmail()
    {
        Console.Write("Enter user name or email to search: ");
        string searchTerm = Console.ReadLine().ToLower();

        var foundUsers = users.Where(user => user.Name.ToLower().Contains(searchTerm) || user.Email.ToLower().Contains(searchTerm)).ToList();

        if (foundUsers.Count > 0)
        {
            Console.WriteLine("Matching Users:\n");
            foreach (var user in foundUsers)
            {
                Console.WriteLine($"Name: {user.Name}, Email: {user.Email}");
            }
            Console.WriteLine();
        }
        else
        {
            Console.Clear();
            Console.WriteLine("No matching users found.\n");
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
            isChanged = true;

            Console.Clear();
            Console.WriteLine("User deleted successfully.\n");
        }
        else
        {
            Console.Clear();
            Console.WriteLine("User not found.\n");
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

            if (!string.IsNullOrWhiteSpace(newName))
            {
                userToUpdate.Name = newName;
            }
            else
            {
                bool isNull = true;
                do
                {
                    Console.WriteLine("Enter proper name!\n");
                    Console.Write("Enter new user name: ");
                    newName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newName))
                    {
                        userToUpdate.Name = newName;
                        isNull = false;
                    }
                } while (isNull);
            }
            isChanged = true;

            Console.Clear();
            Console.WriteLine("User updated successfully.\n");
        }
        else
        {
            Console.Clear();
            Console.WriteLine("User not found.\n");
        }
    }

    static void SaveUsersToFile()
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<User>));
        try
        {
            using (Stream stream = File.Create("Users.xml"))
            {
                xmlSerializer.Serialize(stream, users);
                isChanged = false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + "\n");
        }
    }

    static void LoadUsersFromFile()
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<User>));
        try
        {
            if (File.Exists("Users.xml"))
            {
                using (Stream stream = File.OpenRead("Users.xml"))
                {
                    List<User> newUsers = (List<User>)xmlSerializer.Deserialize(stream);
                    users = newUsers;
                }
            }
            else
            {
                SaveUsersToFile();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + "\n");
        }
    }
}

public class User
{
    public string Name { get; set; }
    public string Email { get; set; }

    public User()
    {
        Name = "";
        Email = "";
    }
    public User(string name, string email)
    {
        Name = name;
        Email = email;
    }
}