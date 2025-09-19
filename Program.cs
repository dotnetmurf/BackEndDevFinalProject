
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

// Email format validation helper
static bool IsValidEmail(string email)
{
	try
	{
		var addr = new System.Net.Mail.MailAddress(email);
		return addr.Address == email;
	}
	catch
	{
		return false;
	}
}

// Create user
app.MapPost("/users", (User user) =>
{
	if (string.IsNullOrWhiteSpace(user.FirstName) ||
		string.IsNullOrWhiteSpace(user.LastName) ||
		string.IsNullOrWhiteSpace(user.Email) ||
		string.IsNullOrWhiteSpace(user.Role))
	{
		return Results.BadRequest("All fields are required and cannot be empty.");
	}
	if (user.FirstName.Length > 50)
		return Results.BadRequest("FirstName cannot exceed 50 characters.");
	if (user.LastName.Length > 50)
		return Results.BadRequest("LastName cannot exceed 50 characters.");
	if (user.Email.Length > 75)
		return Results.BadRequest("Email cannot exceed 75 characters.");
	if (user.Role.Length > 10)
		return Results.BadRequest("Role cannot exceed 10 characters.");
	if (!IsValidEmail(user.Email))
	{
		return Results.BadRequest("Email is not in a valid format.");
	}
	if (!string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase) &&
		!string.Equals(user.Role, "User", StringComparison.OrdinalIgnoreCase))
	{
		return Results.BadRequest("Role must be either 'Admin' or 'User'.");
	}
	// Duplicate email check
	if (users.Values.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
	{
		return Results.BadRequest("A user with this email already exists.");
	}
	var id = nextId++;
	if (!users.TryAdd(id, user))
		return Results.Problem("Could not add user.");
	return Results.Created($"/users/{id}", new { Id = id, User = user });
});

// Update user
app.MapPut("/users/{id:int}", (int id, User updatedUser) =>
{
	if (string.IsNullOrWhiteSpace(updatedUser.FirstName) ||
		string.IsNullOrWhiteSpace(updatedUser.LastName) ||
		string.IsNullOrWhiteSpace(updatedUser.Email) ||
		string.IsNullOrWhiteSpace(updatedUser.Role))
	{
		return Results.BadRequest("All fields are required and cannot be empty.");
	}
	if (updatedUser.FirstName.Length > 50)
		return Results.BadRequest("FirstName cannot exceed 50 characters.");
	if (updatedUser.LastName.Length > 50)
		return Results.BadRequest("LastName cannot exceed 50 characters.");
	if (updatedUser.Email.Length > 75)
		return Results.BadRequest("Email cannot exceed 75 characters.");
	if (updatedUser.Role.Length > 10)
		return Results.BadRequest("Role cannot exceed 10 characters.");
	if (!IsValidEmail(updatedUser.Email))
	{
		return Results.BadRequest("Email is not in a valid format.");
	}
	if (!string.Equals(updatedUser.Role, "Admin", StringComparison.OrdinalIgnoreCase) &&
		!string.Equals(updatedUser.Role, "User", StringComparison.OrdinalIgnoreCase))
	{
		return Results.BadRequest("Role must be either 'Admin' or 'User'.");
	}
	// Duplicate email check (exclude current user)
	if (users.Any(kvp => kvp.Key != id && kvp.Value.Email.Equals(updatedUser.Email, StringComparison.OrdinalIgnoreCase)))
	{
		return Results.BadRequest("A user with this email already exists.");
	}
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
