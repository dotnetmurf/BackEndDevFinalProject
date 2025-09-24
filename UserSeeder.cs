using System.Collections.Concurrent;

public static class UserSeeder
{
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
