```markdown
# FinalProjectBackEnd

## Introduction

This is the backend for the Final Project, developed with **.NET Core 8**.

## Prerequisites

- **.NET SDK 8.0** or higher
- **SQL Server**

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/hosseinkalanaki/FinalProjectBackEnd.git
   ```

2. Navigate to the project directory:
   ```bash
   cd FinalProjectBackEnd
   ```

3. Restore dependencies:
   ```bash
   dotnet restore
   ```

4. Update the `appsettings.json` file with your database connection string.

5. Apply database migrations:
   ```bash
   dotnet ef database update
   ```

## Running the Project

Start the application:
```bash
dotnet run
```

Access it at:
```
https://localhost:5001
```
or
```
http://localhost:5000
```

## License

This project is licensed under the MIT License.
```