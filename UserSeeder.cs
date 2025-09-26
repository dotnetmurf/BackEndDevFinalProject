/*
    UserSeeder.cs - TechHive Solutions User Management API
    -----------------------------------------------------
    This file defines static helper methods for seeding and initializing the in-memory user store.

    Main Features:
    - Provides CreateUserStore to initialize user and email dictionaries with test users.
    - Implements SeedUsers to populate the store with sample user data for development and testing.
    - Supports fast duplicate email checks and user ID assignment.
    - Used by Program.cs to set up the initial state of the API's user management system.

    This class helps ensure consistent test data and efficient user store management.
*/
using System.Collections.Concurrent;

public static class UserSeeder
{
    public static (ConcurrentDictionary<int, User> users, ConcurrentDictionary<string, int> emailToId, int nextId) CreateUserStore(int startId = 1, int count = 50)
    {
        var users = new ConcurrentDictionary<int, User>();
        var emailToId = new ConcurrentDictionary<string, int>();
        var nextId = SeedUsers(users, emailToId, startId, count);
        return (users, emailToId, nextId);
    }
    public static int SeedUsers(ConcurrentDictionary<int, User> users, ConcurrentDictionary<string, int> emailToId, int startId = 1, int count = 10)
    {
        int nextId = startId;
        for (int i = 1; i <= count; i++)
        {
            var user = new User
            {
                FirstName = $"testFirstName{i}",
                LastName = $"testLastName{i}",
                Email = $"testEmail{i}@example.com",
                Role = i % 2 == 0 ? "Admin" : "User"
            };
            users.TryAdd(nextId, user);
            emailToId.TryAdd(user.Email.ToLowerInvariant(), nextId);
            nextId++;
        }
        return nextId;
    }
}
