# OrderService
Optimization for legacy order service

## Tech stack:
- [MediatR](https://github.com/jbogard/MediatR): to implement CQRS and Interceptor pattern (view `Features/Test` and `Config/Behaviors`).

## Development
### EF migrations
``dotnet ef migrations add MigrationName --startup-project Connector.WebApi/ --project Connector.Common
``

``dotnet ef database update --startup-project Connector.WebApi/ --project Connector.Common
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