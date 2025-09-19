# VS Code Copilot Instructions

## Introduction

This document outlines the guidelines and best practices for code generation across various programming languages, platforms, and project types. These instructions help ensure that generated code is high-quality, maintainable, secure, and follows industry standards.

## General Coding Principles

- **Keep It Simple**: Prefer simple, readable solutions over complex ones
- **DRY (Don't Repeat Yourself)**: Avoid code duplication
- **YAGNI (You Aren't Gonna Need It)**: Don't add functionality until it's necessary
- **Single Responsibility Principle**: Functions and classes should have only one reason to change
- **Readability over cleverness**: Prioritize code that is easy to understand
- **Consistent naming**: Use meaningful, descriptive names for variables, functions, and classes

## Code Style and Formatting

- Follow language-specific style guides (PEP 8 for Python, Google Style Guides, etc.)
- Use consistent indentation (spaces vs. tabs according to language conventions)
- Limit line length (typically 80-120 characters)
- Use appropriate spacing around operators
- Organize imports logically and alphabetically when applicable
- Group related code blocks logically
- Avoid excessive nesting

## Documentation

- Include descriptive comments for complex logic
- Add docstrings/documentation for functions, classes, and modules
- Document parameters, return values, and exceptions
- Include examples in documentation when helpful
- Explain "why" rather than "what" in comments
- Keep documentation updated with code changes
- Use standardized documentation formats (JSDoc, docstrings, etc.)

## Testing Practices

- Write unit tests for all new functionality
- Follow test-driven development when appropriate
- Cover edge cases and error conditions
- Keep tests independent and deterministic
- Use appropriate mocking for external dependencies
- Aim for high test coverage, especially for critical code paths
- Write integration tests for component interactions
- Include performance tests for critical paths

## Security Considerations

- Validate all user inputs
- Sanitize data before using in SQL queries, HTML output, or command execution
- Implement proper authentication and authorization
- Use secure cryptographic practices
- Avoid hardcoded credentials
- Follow the principle of least privilege
- Be aware of OWASP Top 10 vulnerabilities
- Use parameterized queries for database operations
- Implement proper error handling without leaking sensitive information

## Performance Optimization

- Focus on algorithmic efficiency before micro-optimization
- Consider time and space complexity
- Optimize database queries
- Use appropriate data structures
- Consider caching strategies
- Minimize network requests
- Avoid premature optimization
- Profile code to identify bottlenecks

## Error Handling

- Use specific exception types
- Never silently catch exceptions without proper handling
- Log errors with appropriate context
- Provide meaningful error messages
- Use try-catch blocks appropriately
- Consider recovery strategies
- Fail fast and explicitly

## Version Control Practices

- Write clear, descriptive commit messages
- Keep commits focused and atomic
- Follow conventional commit formats when possible
- Consider branch naming conventions
- Create meaningful PR/MR descriptions

## Platform-Specific Considerations

### Web Development
- Follow responsive design principles
- Ensure accessibility (WCAG compliance)
- Consider cross-browser compatibility
- Implement proper content security policies

### Mobile Development
- Consider battery and resource usage
- Implement responsive UI for different screen sizes
- Handle offline scenarios gracefully

### Cloud Computing
- Design for scalability and resilience
- Implement proper logging and monitoring
- Consider costs of cloud resources
- Use infrastructure as code when possible

## Language-Specific Adaptations

- Respect language idioms and conventions
- Use language-specific features appropriately
- Follow community standards for the language
- Leverage standard libraries before custom implementations

## Code Review Checklist

- Verify functionality matches requirements
- Check for security vulnerabilities
- Ensure appropriate test coverage
- Verify error handling
- Review performance implications
- Check for code style compliance
- Ensure documentation completeness

## Continuous Improvement

- Stay updated with language and framework best practices
- Refactor code when necessary
- Learn from code reviews and feedback
- Balance perfection with pragmatism

## ASP.NET Core Web API Guidelines

### Project Structure
- Use feature-based or vertical slice architecture where appropriate
- Organize controllers, services, and models in a consistent structure
- Keep controllers thin, move business logic to services
- Use interfaces for service abstractions
- Follow a clean architecture approach with clear separation of concerns:
  - Domain/Models
  - Application/Services
  - Infrastructure/Repositories
  - API/Controllers

### Controller Design
- Use API controllers with `[ApiController]` attribute
- Return appropriate ActionResult<T> types
- Follow RESTful naming conventions
- Implement proper status code responses (200, 201, 204, 400, 401, 403, 404, 500)
- Use route attributes consistently
- Avoid excessive action parameters; use request models
- Keep controllers focused on HTTP concerns, not business logic

### API Endpoints
- Design endpoints with clear responsibility
- Use appropriate HTTP methods (GET, POST, PUT, DELETE, PATCH)
- Use proper route naming conventions (plural nouns)
- Implement proper filtering, sorting, and pagination
- Return consistent response structures
- Implement HATEOAS where appropriate

### Dependency Injection
- Register services with appropriate lifetimes (Singleton, Scoped, Transient)
- Use constructor injection
- Avoid service locator pattern
- Configure services in dedicated extension methods
- Register services based on interfaces

### API Versioning
- Implement versioning strategy (URL, query string, or header)
- Use Microsoft.AspNetCore.Mvc.Versioning package
- Document version deprecation policies
- Support multiple versions when necessary

### Middleware
- Use middleware for cross-cutting concerns
- Create custom middleware using the pipeline pattern
- Keep middleware focused and single-purpose
- Order middleware properly in the pipeline
- Consider using middleware for:
  - Exception handling
  - Request/response logging
  - Authentication
  - CORS
  - Response compression

### Model Validation
- Use data annotations for validation
- Implement custom validators when needed
- Return standardized validation error responses
- Validate both route and body parameters
- Implement FluentValidation for complex validation rules

### Authentication and Authorization
- Use JWT or OAuth 2.0 for authentication
- Implement role-based or policy-based authorization
- Use `[Authorize]` and `[AllowAnonymous]` attributes consistently
- Secure endpoints with appropriate scopes/claims
- Implement refresh token mechanisms
- Consider using Identity Server or Auth0 for complex auth scenarios

### API Documentation
- Use Swagger/OpenAPI for documentation
- Document all API endpoints, parameters, and responses
- Include example requests and responses
- Document authentication requirements
- Configure Swagger UI for optimal developer experience
- Consider XML comments for API documentation

### Response Formatting
- Return consistent JSON structure
- Use PascalCase or camelCase consistently
- Configure JSON serialization options globally
- Consider supporting content negotiation
- Implement pagination metadata in responses

### Exception Handling
- Create a global exception handler middleware
- Return standardized error responses
- Map exceptions to appropriate HTTP status codes
- Log exceptions with appropriate context
- Avoid exposing internal exceptions in responses
- Consider using Problem Details (RFC 7807)

### Logging
- Use structured logging with Serilog or NLog
- Log appropriate information for requests and responses
- Include correlation IDs in logs
- Configure different log levels based on environment
- Consider using Application Insights for production monitoring

### Configuration
- Follow the Options pattern for configuration
- Use environment-specific settings
- Secure sensitive configuration with user secrets or Azure Key Vault
- Validate configuration at startup
- Consider using IOptionsSnapshot for reloadable configuration

### Entity Framework Core
- Use the repository pattern where appropriate
- Implement database migrations strategy
- Use async/await for database operations
- Optimize queries with Include and projection
- Implement proper transaction handling
- Consider using specification pattern for complex queries

### Performance
- Implement response caching where appropriate
- Use asynchronous programming throughout
- Consider using memory caching for frequent data
- Implement pagination for large data sets
- Use response compression
- Consider using Minimal APIs for high-performance scenarios

### Testing
- Write unit tests for controllers using xUnit or NUnit
- Create integration tests with TestServer
- Mock external dependencies
- Test both successful and error paths
- Implement API automation tests
- Consider using SpecFlow for BDD testing approach

### Security
- Implement proper CORS policy
- Use HTTPS in all environments
- Implement rate limiting
- Consider using API keys for machine-to-machine communication
- Validate all incoming data
- Implement anti-forgery measures when necessary
- Follow OWASP API Security Top 10

### Deployment
- Use Docker containers for consistent deployment
- Implement CI/CD pipelines
- Configure health checks
- Set up monitoring and alerting
- Implement proper logging in production
- Consider using Azure App Service, Kubernetes, or other cloud services

### Minimal APIs (Net 6+)
- Use for simple, performance-critical endpoints
- Organize endpoints logically
- Implement proper dependency injection
- Use appropriate result types
- Consider using endpoint filters for cross-cutting concerns

### GraphQL (if applicable)
- Consider using Hot Chocolate for GraphQL implementation
- Design schema according to domain
- Implement proper authorization
- Handle errors consistently
- Optimize for N+1 query problems

## Code Examples

### ASP.NET Core Web API Patterns

```csharp
// Controller example with proper patterns
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
    {
        _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            return NotFound();
        }
        return Ok(customer);
    }
}
```

---