# Dotnet Microservice Project Template
## Overview
This ongoing project involves developing a microservice based on Clean and Vertical
Slice Architecture principles. It includes Dockerization, 
health checks, messaging service integration, and Kubernetes orchestration for
scalable deployment. The goal is to establish a modular, maintainable architecture 
that optimizes development efficiency and ensures robust system reliability in environments.

## Architecture
This architecture is inspired by **Clean and Vertical Slice Architecture** principles,
focusing on **feature** encapsulation to enhance modularity and maintainability.
By organizing the codebase into distinct layers—each with a specific
responsibility—it promotes a clear separation of concerns
This structured approach ensures that core business logic remains independent
of external dependencies, facilitating better scalability and adaptability.
### Core Layer
Contains the essential components like domain entities, endpoint definitions, exceptions, and logging utilities.
### Features Layer
Each feature is self-contained with commands, domain models, and queries to encapsulate functionality.
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

1. Clone the Repository

``` bash
git clone https://github.com/alkhateebali/Microservices.Net.git
cd Microservices.Net
```
2. Install the template

Ensure that you are in the root directory of the template:
```bash
# install the template 
dotnet new install . 

#Result:
Success:  Microservices.Net installed the following templates:
Template Name          Short Name     Language  Tags
---------------------  -------------  --------  ---------------------
Microservice Template  microtemplate  [C#]      Web/API/Microservices
```
3. Create a new project using the template

Navigate to the target directory where you want to create a new project from the template:

```bash
# Create a new project using the template
dotnet new microtemplate -o MicroserviceName

# Change to the new project directory
cd MicroserviceName

```
### Template Options

you can run below command to explore all options 

```bash
dotnet new microtemplate -h 
```

#### Options
*  Include RabbitMQ messaging feature 

```bash
dotnet new microtemplate -o MicroserviceName --messaging true
```
*  Include  Redis caching feature
   
To include Redis caching in your microservice, ensure that Redis
is installed or running as a Docker container. You can use the following 
command to create a new microservice with Redis caching enabled:

```bash
dotnet new microtemplate -o MicroserviceName --redis true
```
To run Redis as a Docker container, you can use the following command

```bash
docker run --name redis -d -p 6379:6379 redis:alpine
```
more options coming soon 
4. Run the project 

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the project
dotnet run
```

## Configuration

- App Settings: Configure your settings in appsettings.json.
- Environment Variables: Set up necessary environment variables for sensitive configurations.

## Running Tests

```bash
# Navigate to the test project
cd Microservice.Unit.Tests

# Run tests
dotnet test
```
## API Documentation
Swagger is integrated to provide interactive API documentation. Once the project is running, navigate to /swagger to explore and test the API endpoints.


## Contribution
Feel free to submit issues or pull requests. We welcome contributions to improve this project.

## License
This project is licensed under the Proprietary License. See the [LICENSE](LICENSE) file for more details.

## Acknowledgements
- [MediatR](https://github.com/jbogard/MediatR)
- [Swagger](https://swagger.io/)
- [xUnit](https://xunit.net/)
- [Moq](https://github.com/devlooped/moq)

## Contact
For any inquiries or feedback, please contact devbyali@outlook.com.




