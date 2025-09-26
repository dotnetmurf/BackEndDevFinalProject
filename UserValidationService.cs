/*
    UserValidationService.cs - TechHive Solutions User Management API
    ---------------------------------------------------------------
    This file defines static helper methods for validating user data and enforcing business rules.

    Main Features:
    - Validates user input for required fields, length limits, and allowed values.
    - Checks for duplicate emails in the user store, supporting both create and update scenarios.
    - Verifies email format using .NET's MailAddress class.
    - Returns standardized error responses for invalid data.
    - Used by Program.cs to ensure data integrity and prevent invalid or duplicate user records.

    This class centralizes validation logic for consistent and maintainable user management.
*/
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;

public static class UserValidationService
{
    public static bool IsValidEmail(string email)
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

    public static bool DuplicateEmail(string email, ConcurrentDictionary<string, int> emailToId, int? currentUserId = null)
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

    public static IResult? ValidateUserData(User user)
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
}
