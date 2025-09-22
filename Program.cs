
using System.Collections.Concurrent;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Register custom middleware in the correct order
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseMiddleware<LoggingMiddleware>();


// In-memory user store
var users = new ConcurrentDictionary<int, User>();
// Email to userId mapping for fast duplicate checks
var emailToId = new ConcurrentDictionary<string, int>();
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
	emailToId.TryAdd(user.Email.ToLowerInvariant(), i);
	nextId = i + 1;
}


// CRUD Endpoints

// Display initial user message
app.MapGet("/", () =>
    Results.Content(
        @"<html>
            <head>
                <title>Welcome Page</title>
            </head>
            <body>
                <h1>Welcome to the Minimal API!</h1>
                <p>This is a multi-line HTML response.</p>
                <ul>
                    <li>Item 1</li>
                    <li>Item 2</li>
                </ul>
            </body>
        </html>", "text/html")
);

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

// Duplicate email check helper
static bool DuplicateEmail(string email, ConcurrentDictionary<string, int> emailToId, int? currentUserId = null)
{
    var emailKey = email.ToLowerInvariant();
    if (emailToId.TryGetValue(emailKey, out var existingId))
    {
        // If currentUserId is provided (PUT), allow if the email belongs to the same user
        if (currentUserId == null || existingId != currentUserId)
            return true;
    }
    return false;
}

// User data validation helper
static IResult? ValidateUserData(User user)
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
    return null;
}

// Create user
app.MapPost("/users", (User user) =>
{
    var validationResult = ValidateUserData(user);
    if (validationResult != null)
        return validationResult;

    if (DuplicateEmail(user.Email, emailToId))
    {
        return Results.BadRequest("A user with this email already exists.");
    }

    var emailKey = user.Email.ToLowerInvariant();
    var id = nextId;
    emailToId.TryAdd(emailKey, id);
    if (!users.TryAdd(id, user))
    {
        emailToId.TryRemove(emailKey, out _); // rollback
        return Results.Problem("Could not add user.");
    }
    nextId++;
    return Results.Created($"/users/{id}", new { Id = id, User = user });
});

// Update user
app.MapPut("/users/{id:int}", (int id, User updatedUser) =>
{
    var validationResult = ValidateUserData(updatedUser);
    if (validationResult != null)
        return validationResult;

    if (!users.ContainsKey(id))
        return Results.NotFound();

    var emailKey = updatedUser.Email.ToLowerInvariant();
    var currentEmailKey = users[id].Email.ToLowerInvariant();
    if (!emailKey.Equals(currentEmailKey, StringComparison.OrdinalIgnoreCase))
    {
        // Email is being changed, check for duplicates and update mapping
        // First, check for a duplicate email in a thread-safe way using our helper (logic-level check)
        if (DuplicateEmail(updatedUser.Email, emailToId, id))
        {
            return Results.BadRequest("A user with this email already exists.");
        }
        // Second, attempt to add the new email to the dictionary (atomic operation)
        // This is a defensive check in case another thread added the same email between our check and this point
        if (!emailToId.TryAdd(emailKey, id))
        {
            return Results.BadRequest("A user with this email already exists.");
        }
        emailToId.TryRemove(currentEmailKey, out _);
    }
    users[id] = updatedUser;
    return Results.Ok(new { Id = id, User = updatedUser });
});

// Delete user
app.MapDelete("/users/{id:int}", (int id) =>
{
	if (users.TryRemove(id, out var removedUser))
	{
		var emailKey = removedUser.Email.ToLowerInvariant();
		emailToId.TryRemove(emailKey, out _);
		return Results.NoContent();
	}
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
