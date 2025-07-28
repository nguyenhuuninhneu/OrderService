# OrderService
Optimization for legacy order service

## Tech stack:
- [MediatR](https://github.com/jbogard/MediatR): to implement CQRS pattern (view `Features/Order` and `Config/`).
- EF Core + SQLite with Code First to avoid SQL injection.
- The design for easy to test, maintainable, and can scale to Web API if need.

## Development
### EF migrations
``dotnet ef migrations add MigrationName --startup-project LegacyOrdereService/ --project LegacyOrdereService.Data
``

``dotnet ef database update --startup-project LegacyOrdereService/ --project LegacyOrdereService.Data
``

- Maximum 2 levels nested code is allowed, the following is **not allowed**:
```
if (x)
{
    if (y)
    {
        if (z)
        {
        
        }    
    }
}
```
- Only 1 level of abstraction is allowed, the following is **not allowed**:
```
interface IGrandParent
{
    
}

interface IParent : IGrandParent
{
    
}

class Child : IParent
{
    
}
```