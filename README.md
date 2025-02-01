# Multitenancy ScaffoldCore - A Boilerplate for Multi-Tenant Applications

[![Build and Tests](https://github.com/Vrossi28/multitenancy-scaffold-core/actions/workflows/workflow.yaml/badge.svg)](https://github.com/Vrossi28/multitenancy-scaffold-core/actions/workflows/workflow.yaml)

## Introduction

ScaffoldCore is a **.NET 6.0 Web API** boilerplate designed to accelerate the development of new projects by providing a solid foundation with essential functionalities. It includes built-in **multi-tenancy support**, **account management**, **email sending**, and **asynchronous job processing using Hangfire**.

This project is containerized with **Docker**, utilizes **PostgreSQL** as the database, follows **CQRS architecture with MediatR**, and supports **Entity Framework Core's Code-First Migrations**.

---

1. [Features](#features)

   - Multi-Tenancy
   - Tenant Management
   - Account Management
   - Email Sending
   - Asynchronous Job Processing (Hangfire)
   - Database Migrations (EF Core)

2. [Technologies Used](#technologies-used)

   - .NET Core 6.0
   - Hangfire
   - PostgreSQL
   - Docker & Docker Compose
   - CQRS Pattern
   - MediatR
   - Entity Framework Core
   - Swagger
   - AutoMapper

3. [Getting Started](#getting-started)

   - Prerequisites
     - .NET 6 SDK
     - Docker & Docker Compose
   - Running the Project
     - Clone and Setup Instructions
     - Running via Docker
     - Running Locally
   - Accessing API Documentation

4. [Docker Compose Overview](#docker-compose-overview)

   - `docker-compose.yml` Explanation
   - Environment Variables
     - ASPNETCORE_ENVIRONMENT
     - ASPNETCORE_URLS
     - ConnectionStrings\_\_DefaultConnection
     - EmailService\_\_SenderEmail
     - Jwt\_\_SecretKey
     - Hangfire\_\_Password

5. [Generating a Development Certificate](#generating-a-development-certificate)

   - Creating a Self-Signed Certificate
   - Exporting the Certificate
   - Docker Configuration for HTTPS

6. [Contributing](#contributing)

   - How to Contribute
   - Changes in the Database (Code-First Approach)
     - Creating a Migration
     - Updating the Database

7. [Contributors](#contributors)

   - List of Contributors

8. [License](#license)
   - Apache-2.0 license

## Features

- **Multi-Tenancy**: Ensures all data is isolated by tenant.
- **Tenant Management**: Create and manage tenants dynamically.
- **Account Management**: Create, manage and associate accounts with tenants.
- **Email Sending**: Includes an example of sending contact request emails.
- **Asynchronous Job Processing**: Integrated with **Hangfire** for background jobs.
- **Database Migrations**: Uses **EF Core** for database schema management.

## Technologies Used

- **.NET Core 6.0** – Backend framework.
- **Hangfire** – Background job processing.
- **PostgreSQL** – Relational database.
- **Docker** – Containerization.
- **Docker Compose** – Orchestration of containers for both the API and database.
- **CQRS Pattern** – Command and Query Responsibility Segregation.
- **MediatR** – Implementation of CQRS.
- **Entity Framework Core** – ORM for database access, using Code-First migrations.
- **Swagger** – API documentation.
- **AutoMapper** – DTO mapping.

## Getting Started

### Prerequisites

Make sure you have the following installed:

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker & Docker Compose](https://www.docker.com/get-started)

### Running the Project

1. Clone the repository:

   ```bash
   git clone https://github.com/Vrossi28/multitenancy-scaffold-core.git
   cd multitenancy-scaffold-core
   ```

2. Build and run the application

   1. Using Docker

      - The following command will start both the **Web API** and **PostgreSQL** containers:

      ```bash
      docker-compose up --build
      ```

   2. Local development - Make sure to update the connection strings to map to your local Postgres database.
      - After configuring the connection to the database you can run the project using the command:
      ```bash
        $ dotnet run --configuration debug
      ```

3. Access the API documentation:

   - Open [http://localhost:8080/swagger](http://localhost:8080/swagger) in your browser.

## Docker Compose Overview

Here is the `docker-compose.yml` file used in this project:

```yaml
version: "3"
services:
  vrossi.scaffoldcore.webapi:
    build:
      context: .
      dockerfile: Vrossi.ScaffoldCore.WebApi/Dockerfile
    ports:
      - "8080:8080"
      - "1443:1443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=http://+:8080;https://+:1443
      - ASPNETCORE_Kestrel__Certificates__Default__Password=Password
      - ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
      - ConnectionStrings__DefaultConnection=Server=vrossi.scaffoldcore.db;Port=5432;Database=VrossiScaffoldDB;User Id=postgres;Password=postgres
      - EmailService__SenderEmail=youremail@domain.com
      - EmailService__SenderPassword=Password
      - Jwt__SecretKey=SecretKey
      - Hangfire__Password=Password
    volumes:
      - ~/.aspnet/https:/https:ro
    depends_on:
      - vrossi.scaffoldcore.db

  vrossi.scaffoldcore.db:
    build:
      context: .
      dockerfile: Vrossi.ScaffoldCore.Infrastructure/Config/Dockerfile
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
      POSTGRES_DB: VrossiScaffoldDB
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data
      - ./init-scripts:/docker-entrypoint-initdb.d
    restart: unless-stopped

volumes:
  postgres_data:
```

### Environment Variables

| Variable                                              | Description                                                             |
| ----------------------------------------------------- | ----------------------------------------------------------------------- |
| `ASPNETCORE_ENVIRONMENT`                              | Defines the application environment (Development, Staging, Production). |
| `ASPNETCORE_URLS`                                     | Defines the URLs on which the application will listen (HTTP and HTTPS). |
| `ASPNETCORE_Kestrel__Certificates__Default__Password` | Password for the HTTPS certificate (`.pfx`).                            |
| `ASPNETCORE_Kestrel__Certificates__Default__Path`     | Path to the HTTPS certificate (`.pfx`).                                 |
| `ConnectionStrings__DefaultConnection`                | Connection string for PostgreSQL database.                              |
| `EmailService__SenderEmail`                           | Sender email for notifications.                                         |
| `EmailService__SenderPassword`                        | Password for the sender email.                                          |
| `Jwt__SecretKey`                                      | Secret key for JWT authentication.                                      |
| `Hangfire__Password`                                  | Password for Hangfire dashboard authentication.                         |

## Generating a Development Certificate

To enable HTTPS in development, you need to generate a **self-signed certificate**.

### Step 1: Create the Certificate

Run the following command:

```bash
dotnet dev-certs https --trust
```

This will generate and trust a development certificate on your machine.

### Step 2: Export the Certificate

```bash
dotnet dev-certs https --export-path ./aspnetapp.pfx --password "yourPassword"
```

This command will:

- Export the certificate to `aspnetapp.pfx`.
- Set `"yourPassword"` as the password.

### Step 3: Configure Docker

Ensure your `docker-compose.yml` mounts the certificate correctly:

```yaml
volumes:
  - ./aspnetapp.pfx:/https/aspnetapp.pfx:ro
```

Update the **environment variables**:

```yaml
environment:
  - ASPNETCORE_Kestrel__Certificates__Default__Password=yourPassword
  - ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
```

Now, restart your containers:

```bash
docker-compose down
docker-compose up --build
```

## Contributing

Contributions are welcome! Feel free to open an issue or submit a pull request.

### Changes in database

This project uses code first approach. If you want to change database entities, it's necessary to properly configure it at `ScaffoldCoreContext` class.

#### Create migration

After you do your changes, create the migration by giving a short description and running the command:

```bash
$ dotnet ef migrations add "ShortDescription" --project Vrossi.ScaffoldCore.Infrastructure --startup-project Vrossi.ScaffoldCore.WebApi --output-dir Persistence\Migrations --verbose
```

#### Update database

After create the migration, update your database using the command:

```bash
$ dotnet ef database update --project Vrossi.ScaffoldCore.Infrastructure --startup-project Vrossi.ScaffoldCore.WebApi --verbose
```

## Contributors

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
    <tbody>
        <tr>
            <td align="center">
                <a href="https://www.linkedin.com/in/vinicius-rossi-br/?locale=en_US">
                    <img src="https://avatars.githubusercontent.com/u/57651321?s=400&u=8d5bd045263f2a42ad3e3a4a61dbd26270501ea3&v=4" width="100px;" alt="Vinicius Rossi"/>
                    <br />
                    <sub><b>Vinicius Rossi</b></sub>
                </a> 
            </td>
        </tr>
    </tbody>
</table>

## License

This project is licensed under the [Apache-2.0 license](LICENSE).
