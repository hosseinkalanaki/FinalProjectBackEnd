# FinalProjectBackEnd

## Introduction

This repository contains the backend implementation of the Final Project, developed using **.NET Core 8**.

## Prerequisites

Ensure the following are installed on your system:

- **.NET SDK**: Version 8.0 or higher
- **SQL Server**: For database integration
- **Visual Studio 2022** (or later) or any preferred IDE with .NET Core support

## Installation

1. **Clone the repository**:

   ```bash
   git clone https://github.com/hosseinkalanaki/FinalProjectBackEnd.git
   ```

2. **Navigate to the project directory**:

   ```bash
   cd FinalProjectBackEnd
   ```

3. **Restore dependencies**:

   ```bash
   dotnet restore
   ```

4. **Update the database connection string**:  
   Open `appsettings.json` and configure the `ConnectionStrings` section with your SQL Server settings.

5. **Apply migrations** (if applicable):

   ```bash
   dotnet ef database update
   ```

## Running the Project

To run the application locally, execute:

```bash
dotnet run
```

The application will be available at:

```
https://localhost:5001
```

or

```
http://localhost:5000
```

## Project Structure

- **`Models/`**: Contains the project's data models.
- **`SignalRWebpack/`**: Configuration files for SignalR and Webpack.
- **`MyApp.sln`**: Solution file for the backend.

## Useful Commands

- **Run the application**:

  ```bash
  dotnet run
  ```

- **Build the project**:

  ```bash
  dotnet build
  ```

- **Apply migrations**:

  ```bash
  dotnet ef database update
  ```

- **Create a new migration**:

  ```bash
  dotnet ef migrations add MigrationName
  ```

## Contributing

We welcome contributions! Please ensure all changes follow the project's coding standards and best practices. Open a pull request for review.

## License

This project is licensed under the MIT License. For details, see the `LICENSE` file.
``` 