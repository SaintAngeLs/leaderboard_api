# Leaderboard API

This project is a CQRS hybrid and Domain-Driven Design (DDD) implementation of a company-wide steps leaderboard application for teams of employees. The API supports creating, incrementing, and deleting counters; adding and deleting teams; and retrieving step count information for teams and individual counters.

> **Note:**  
> - The API uses in-memory storage via Redis so that data is lost on shutdown. Redis is leveraged as a fast, in-memory data store.  
> - A rate limiter middleware is used to throttle incoming requests and ensure fair usage.  
> - API documentation is provided by Scalar and can be accessed at `/scalar/v1`.

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [User Stories](#user-stories)
- [Endpoints and Payloads](#endpoints-and-payloads)
- [Images](#images)
- [Running the Application Locally](#running-the-application-locally)
- [Running with Docker](#running-with-docker)
- [API Documentation with Scalar](#api-documentation-with-scalar)
- [Deployment Details](#deployment-details)
- [Additional Considerations](#additional-considerations)
- [License](#license)

## Overview

The Leaderboard API is built with .NET (C#) and leverages a CQRS hybrid approach along with Domain-Driven Design principles. It separates commands (create, update, delete operations) from queries (data retrieval) and organizes the solution into multiple projects:
- **Leaderboard.Api** – The REST API project.
- **Leaderboard.Application** – Application presentation logic (commands, queries, events, services, and abstractions).
- **Leaderboard.Core** – Domain models, business rules, and repository models.
- **Leaderboard.Infrastructure** – Infrastructure components including Redis integration, middleware for error handling and rate limiting, and external dependency management.

## Architecture

- **CQRS Hybrid:**  
  Commands such as **CreateTeam** and **IncrementCounter** are processed by dedicated domain services, while queries such as **GetAllTeamsQuery** and **GetTeamByIdQuery** are handled by specific query handlers.
  
- **Domain-Driven Design (DDD):**  
  Business logic is encapsulated in domain models (e.g., **Team**, **Counter**) and value objects (e.g., **TeamName**, **StepCount**), allowing clear separation between core business logic, application services, and infrastructure concerns.

- **Redis Leveraging:**  
  Redis is used as an in-memory datastore for high-performance caching and transient storage. The Docker configuration employs a tmpfs volume to clear Redis data upon container restart, though this can be extended to a persistent model if required.

- **Rate Limiter Middleware:**  
  Custom middleware limits the number of requests per client IP within a given time window to prevent abuse and ensure fair resource usage.

## User Stories

The API was designed to address the following user stories:

1. **Counter Management:**
   - **Create a new counter:**  
     *As a user, I want to create a new counter for a team so that steps can be accumulated.*
   - **Increment a counter:**  
     *As a user, I want to increment a counter so that additional steps are counted towards my team's score.*
   - **Delete a counter:**  
     *As a user, I want to delete a counter to manage team members' counters.*

2. **Team Management:**
   - **Add a new team:**  
     *As a user, I want to add a new team to group employees together.*
   - **Delete a team:**  
     *As a user, I want to delete a team to manage team data.*
   - **List all teams:**  
     *As a user, I want to list all teams to compare step counts across teams.*
   - **Retrieve team details:**  
     *As a user, I want to view a specific team by its ID, including its total step count.*

3. **Data Retrieval:**
   - **List all counters in a team:**  
     *As a user, I want to view all counters for a team to see individual contributions.*
   - **Get total steps for a team:**  
     *As a user, I want to retrieve the total step count for a team.*

## Endpoints and Payloads

### **Counters**

- **POST `/api/counters/create`**  
  **Purpose:** Create a new counter for a team.  
  **Payload Example:**
  ```json
  {
    "teamId": "c4d7e8f2-1234-4abc-8d3e-1a2b3c4d5e6f",
    "ownerName": "John Doe"
  }
  ```  
  **Responses:**  
  - `200 OK`: "Counter created successfully."  
  - `404 Not Found`: "Team with ID {teamId} was not found."

- **PUT `/api/counters/increment`**  
  **Purpose:** Increment a counter's step count.  
  **Payload Example:**
  ```json
  {
    "teamId": "c4d7e8f2-1234-4abc-8d3e-1a2b3c4d5e6f",
    "counterId": "d5e6f7a8-2345-4bcd-9e0f-1b2c3d4e5f67",
    "steps": 1000
  }
  ```  
  **Responses:**  
  - `200 OK`: "Counter incremented successfully."  
  - `400 Bad Request`: If the input is invalid.  
  - `404 Not Found`: If the team or counter is not found.

- **DELETE `/api/counters/delete`**  
  **Purpose:** Delete a counter.  
  **Payload Example:**
  ```json
  {
    "teamId": "c4d7e8f2-1234-4abc-8d3e-1a2b3c4d5e6f",
    "counterId": "d5e6f7a8-2345-4bcd-9e0f-1b2c3d4e5f67"
  }
  ```  
  **Responses:**  
  - `200 OK`: "Counter deleted successfully."  
  - `404 Not Found`: If the team or counter is not found.

- **GET `/api/counters/team?teamId={teamId}&pageNumber={pageNumber}&pageSize={pageSize}`**  
  **Purpose:** Retrieve a paginated list of counters for a given team.  
  **Response Example:**
  ```json
  {
    "items": [
      {
        "id": "d5e6f7a8-2345-4bcd-9e0f-1b2c3d4e5f67",
        "teamId": "c4d7e8f2-1234-4abc-8d3e-1a2b3c4d5e6f",
        "ownerName": "John Doe",
        "steps": 1000
      }
    ],
    "totalCount": 1,
    "pageNumber": 1,
    "pageSize": 10
  }
  ```

- **GET `/api/counters/team/{teamId}/totalsteps`**  
  **Purpose:** Retrieve the total steps for all counters of a given team.  
  **Response Example:**
  ```json
  2500
  ```

### **Teams**

- **GET `/api/teams`**  
  **Purpose:** Retrieve a paginated list of all teams.  
  **Response Example:**
  ```json
  {
    "items": [
      {
        "id": "a1b2c3d4-5678-90ab-cdef-1234567890ab",
        "teamName": "Marketing",
        "totalSteps": 5000
      }
    ],
    "totalCount": 1,
    "pageNumber": 1,
    "pageSize": 10
  }
  ```

- **GET `/api/teams/{id}`**  
  **Purpose:** Retrieve details for a specific team by its ID.  
  **Response Example:**
  ```json
  {
    "id": "a1b2c3d4-5678-90ab-cdef-1234567890ab",
    "teamName": "Marketing",
    "totalSteps": 5000
  }
  ```  
  **Responses:**  
  - `200 OK`: Returns the team details.  
  - `404 Not Found`: If the team is not found.

- **POST `/api/teams`**  
  **Purpose:** Create a new team.  
  **Payload Example:**
  ```json
  {
    "teamName": "Marketing"
  }
  ```  
  **Responses:**  
  - `201 Created`: Returns the newly created team details.  
  - `400 Bad Request`: If the team name is empty or invalid.

- **DELETE `/api/teams/delete`**  
  **Purpose:** Delete an existing team.  
  **Payload Example:**
  ```json
  {
    "teamId": "a1b2c3d4-5678-90ab-cdef-1234567890ab"
  }
  ```  
  **Responses:**  
  - `200 OK`: "Team deleted successfully."  
  - `404 Not Found`: If the team is not found.

### **Home**

- **GET `/api/hello`**  
  **Purpose:** Retrieve the API name as a simple test endpoint.  
  **Response Example:**
  ```
  Leaderboard API
  ```

- **GET `/api/error`**  
  **Purpose:** Returns a demonstration error response.  
  **Response Example:**
  ```json
  {
    "code": "error",
    "message": "oops, it's seems the error appeared!"
  }
  ```

## Images

Below are some images that illustrate key aspects of the application:

- **Introduction:**  
  ![Introduction](docs/image_1_intro.png)
- **Create Counter:**  
  ![Create Counter](docs/image_2_create_counter.png)
- **Increment Counter:**  
  ![Increment Counter](docs/image_2_increment_counter.png)
- **Delete Counter:**  
  ![Delete Counter](docs/image_2_delete_counter.png)
- **Retrieve Total Steps for Counters:**  
  ![Retrieve Total Steps](docs/image_2_retrieves_total_steps_for_counters.png)
- **Create New Team:**  
  ![Create New Team](docs/image_3_create_new_team.png)
- **Delete Team:**  
  ![Delete Team](docs/image_3_delete_team.png)
- **Retrieve Team:**  
  ![Retrieve Team](docs/image_3_retrieves_team.png)
- **Teams List:**  
  ![Teams List](docs/image_3_teams_list.png)
- **Retrieve Counters for Team:**  
  ![Retrieve Counters](docs/image__retrieve_counters_for_team.png)

## Running the Application Locally

1. **Clone the repository** and navigate to its root directory.
2. **Run the infrastructure services (Redis):**
   ```bash
   cd scripts
   docker compose -f services.yml up -d
   ```
3. **Restore dependencies and run the API:**
   ```bash
   cd src/Leaderboard.Api
   dotnet run
   ```
4. The API will run on the default URL as configured in your launch settings (e.g., `http://localhost:5000`). You can adjust this via environment variables if needed.

## Running with Docker

There are two ways to run the application using Docker:

### 1. Running Both Infrastructure and API

The provided Docker Compose file in the `scripts` folder (named `infrastructure.yml`) runs both the Redis service and the Leaderboard API.

- **Steps:**
  1. Navigate to the `scripts` folder:
     ```bash
     cd scripts
     ```
  2. Build and run the containers:
     ```bash
     docker compose -f infrastructure.yml up --build -d
     ```
  3. The API will be accessible at [http://localhost:5005](http://localhost:5005).  
     The environment variable `ASPNETCORE_URLS=http://+:5005` ensures that the API listens on port **5005**.
  4. Redis uses a tmpfs volume, so its data is ephemeral and is cleared on container restart.

### 2. Running Individual Services

Alternatively, you can use the `services.yml` (if provided) to run specific services for development or testing purposes.

## API Documentation with Scalar

Scalar is used instead of Swagger for API documentation. Once the API is running, you can view the documentation at:

```
http://35.174.166.92:5005/scalar/v1
```

This endpoint displays detailed documentation for all available endpoints, including request payloads and responses.

---

## Deployment Details

The application is deployed on AWS ECS/Fargate using a Clean Architecture, CQRS, and DDD approach. The deployment was managed by @SaintAngels. All container images are stored in Amazon ECR, and the services are orchestrated using ECS Fargate. The API container runs alongside a Redis container for caching, with AWS Cloud Map service discovery enabling secure internal communication. The public API endpoint is exposed through an Application Load Balancer and Route 53 DNS records for stable, scalable access.

---

## Additional Considerations

- **Clean Architecture:**  
  The solution follows Clean Architecture principles by separating concerns into distinct layers (API, Application, Core, and Infrastructure). This modular design improves testability, maintainability, and scalability, and enables easy extension—such as enhanced authorization using token-based systems combined with context-aware policies derived from HTTP request correlation data.

- **Rate Limiter Middleware:**  
  Custom middleware limits the number of requests per client IP within a specific time window, ensuring fair resource usage and preventing abuse under heavy load.

- **Redis Leveraging:**  
  Redis is used as an in-memory datastore for high-performance caching and transient storage. The current configuration uses a tmpfs volume for ephemeral data, but in production, the repository can be extended to use Redis (or another persistent technology) for long-term data durability.

- **Persistence:**  
  While the application currently relies on in-memory storage, production environments should integrate a persistent storage layer (e.g., SQL Server, PostgreSQL, or a NoSQL solution) to ensure data durability and consistency across restarts.

- **Fault Tolerance & Scalability:**  
  The API is designed to scale and handle high loads. Consider employing Redis clusters for distributed caching, message queues for decoupled processing, and load balancers to evenly distribute traffic. This approach ensures resilience in global contest scenarios with a high volume of requests.

- **Authentication & Authorization:**  
  The API can be enhanced with token-based authentication (e.g., JWT) and context-aware authorization mechanisms, enabling fine-grained control over access to specific endpoints and operations. The modular architecture allows these security features to be integrated without impacting other components.

---

## License

This project is licensed under the MIT License. See [LICENSE.md](LICENSE.md) for details.

