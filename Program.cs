
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


// In-memory user store
var users = new ConcurrentDictionary<int, User>();
var nextId = 1;

// Seed with 10 test users
for (int i = 1; i <= 10; i++)
{
	var user = new User
	{
		FirstName = $"TestFirst{i}",
		LastName = $"TestLast{i}",
		Email = $"test{i}@example.com",
		Role = i % 2 == 0 ? "Admin" : "User"
	};
	users.TryAdd(i, user);
	nextId = i + 1;
}


// CRUD Endpoints


// Get all users
app.MapGet("/users", () => Results.Ok(
	users.Select(kvp => new { Id = kvp.Key, User = kvp.Value })
));

// Get user by ID
app.MapGet("/users/{id:int}", (int id) =>
	users.TryGetValue(id, out var user)
		? Results.Ok(new { Id = id, User = user })
		: Results.NotFound()
);

// Create user
app.MapPost("/users", (User user) =>
{
	var id = nextId++;
	if (!users.TryAdd(id, user))
		return Results.Problem("Could not add user.");
	return Results.Created($"/users/{id}", new { Id = id, User = user });
});

// Update user
app.MapPut("/users/{id:int}", (int id, User updatedUser) =>
{
	if (!users.ContainsKey(id))
		return Results.NotFound();
	users[id] = updatedUser;
	return Results.Ok(new { Id = id, User = updatedUser });
});

// Delete user
app.MapDelete("/users/{id:int}", (int id) =>
{
	if (users.TryRemove(id, out _))
		return Results.NoContent();
	return Results.NotFound();
});

app.Run();

// User model
public class User
{
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
	public required string Email { get; set; }
	public required string Role { get; set; }
}
