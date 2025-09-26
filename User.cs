/*
    User.cs - TechHive Solutions User Management API
    -----------------------------------------------
    This file defines the User model class for the User Management API.

    Main Features:
    - Represents a user entity with FirstName, LastName, Email, and Role properties.
    - All properties are required for validation and data integrity.
    - Used for in-memory storage, API requests, and responses.
    - Supports CRUD operations and user management features throughout the application.

    This class is the core data structure for user records in the API.
*/
public class User
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
}
