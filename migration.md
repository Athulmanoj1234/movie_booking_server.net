## EF Core Migrations — CommonServicesLibrary

### 1. Overview

The `ApplicationDbContext` and entity models have been moved from the main `movie_booking` API project into the `CommonServicesLibrary` class library. Therefore, EF Core migration commands must target the class library while using the API project as the startup project.

### 2. Creating a New Migration

Run the following command from the `CommonServicesLibrary` directory:

```powershell
dotnet ef migrations add "migration-name" --project . --startup-project ..\movie_booking --context CommonServicesLibrary.data.ApplicationDbContext
```

**Command explanation:**

- `migrations add "migration-name"` — Creates a new migration with the specified name.
- `--project .` — Specifies the current project (`CommonServicesLibrary`) as the target project where migration files are generated.
- `--startup-project ..\movie_booking` — Specifies the `movie_booking` API project as the startup project.
- `--context CommonServicesLibrary.data.ApplicationDbContext` — Specifies the `ApplicationDbContext` located in the `CommonServicesLibrary.data` namespace.

**Why is `--startup-project` required?**

The `movie_booking` API project contains the application startup configuration and registers the `ApplicationDbContext` through dependency injection.

The context is configured in `CommonServicesLibrary` using `AddDbContextFactory<ApplicationDbContext>()` and `UseSqlServer()`. EF Core uses the startup project's configuration and service registration to discover the database connection and create the context at design time.

The migration is generated in `CommonServicesLibrary`, while the API project provides the startup configuration needed to create the context.

### 3. Applying Migrations to the Database

After creating and verifying the migration, run the following command from the `CommonServicesLibrary` directory:

```powershell
dotnet ef database update --project . --startup-project ..\movie_booking --context CommonServicesLibrary.data.ApplicationDbContext
```

**Command explanation:**

- `database update` — Applies pending migrations to the configured database.
- `--project .` — Specifies `CommonServicesLibrary` as the migration target project.
- `--startup-project ..\movie_booking` — Uses the API project's startup configuration to create the context and obtain the database connection.
- `--context CommonServicesLibrary.data.ApplicationDbContext` — Specifies the context whose migrations should be applied.

### 4. Important Notes

- Run these commands from the `CommonServicesLibrary` directory when using the paths shown above.
- Use a descriptive migration name, such as `AddShowDimensions`.
- Review the generated migration before applying it to the database.
- Ensure existing migrations and the model snapshot are correctly maintained when moving the context between projects.
- Back up important data before applying schema changes to a production database.