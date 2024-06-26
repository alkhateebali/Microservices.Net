#.Net Microservice Project Template
## Overview
This project is a microservice designed using modern .NET practices. It includes features such as CQRS, MediatR for handling commands and queries, event sourcing, and dependency injection. The project also includes comprehensive unit tests, API versioning, health checks, and authentication.

## Architecture
### Core Layer
Contains the essential components like domain entities, endpoint definitions, exceptions, and logging utilities.
Features Layer
### Each feature is self-contained with commands, domain models, and queries to encapsulate functionality.
Persistence Layer
Manages database context, entity configurations, event publishing, and repository interfaces.
### Infrastructure Layer
The Infrastructure Layer handles all cross-cutting concerns and dependencies specific to microservices, such as health checks, messaging brokers, and other essential services.
### Configuration Layer
Handles application settings and environment configurations, facilitating easy setup and management.

## Features 
- CQRS and MediatR: Separates command and query responsibilities, improving code maintainability and scalability.
- Event Sourcing: Captures changes to an application state as a sequence of events.
- Dependency Injection: Manages dependencies to improve modularity and testability.
- Authentication and Authorization.
- Dynamic endpoints generation.
- Metrics and Monitoring: Integrate tools like Prometheus and Grafana for monitoring.
- Health Checks: Monitors the health of the application and its dependencies.
- Swagger: Provides interactive API documentation.
- Unit Tests: Ensures code quality and reliability.

## Prerequisites
- .NET 8 SDK or higher
- Docker (optional, for containerization)
- An IDE such as Visual Studio, Rider or VS Code

## Getting Started
Clone the Repository

``` bash
git clone https://github.com/yourusername/your-repo-name.git
cd your-repo-name
```
## Configuration

- App Settings: Configure your settings in appsettings.json.
- Environment Variables: Set up necessary environment variables for sensitive configurations.

# Build and Run

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the project
dotnet run
```

## Running Tests

```bash
# Navigate to the test project
cd Microservice.Unit.Tests

# Run tests
dotnet test
```
## API Documentation
Swagger is integrated to provide interactive API documentation. Once the project is running, navigate to /swagger to explore and test the API endpoints.

## Template Status
This project is currently being structured as a reusable template. Progress is ongoing, and updates will be made regularly to enhance its functionality and usability.

## Contribution
Feel free to submit issues or pull requests. We welcome contributions to improve this project.

## License
This project is licensed under the MIT License. See the LICENSE file for more details.

## Acknowledgements
- MediatR
- Swagger
- xUnit
- Moq

##Contact
For any inquiries or feedback, please contact devbyali@outlook.com.




