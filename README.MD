
# Ticket Management System

This is a full-stack ticket management system built with a **React frontend**, a **.NET backend (ASP.NET Core)**, and a **PostgreSQL database**.

## Table of Contents
1. [Technologies Used](#technologies-used)
2. [Project Structure](#project-structure)
3. [Prerequisites](#prerequisites)
4. [Backend Setup (ASP.NET Core)](#backend-setup-aspnet-core)
5. [Frontend Setup (React)](#frontend-setup-react)
6. [Database Setup (PostgreSQL)](#database-setup-postgresql)
7. [Running the Application](#running-the-application)
8. [Additional Information](#additional-information)

---

## Technologies Used

- **Frontend**: React, TypeScript, MaterialUI
- **Backend**: .NET 8 (C#), entity framework
- **Database**: PostgreSQL

---

## Project Structure

The project is structured as follows:

```
/tickets-backend       # Backend (.NET API and tests)
  ├── API.Tests        # Unit tests for the API
  ├── ticket-api       # ASP.NET Core API project
  ├── .net.sln         # Solution file
/tickets-frontend      # Frontend (React)
README.MD              # Project documentation
```

---

## Prerequisites

Before running the application, make sure you have the following installed:

- **Node.js 20.x.x** (for the frontend)
- **.NET SDK 8.x.x** (for the backend)
- **PostgreSQL** (for the database): 
- **Docker** (optional but recommended for easy database setup)

```bash
# 1. Install Node.js 20.x.x
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -
sudo apt-get install -y nodejs

# 2. Install .NET SDK 8.x.x
wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0

# 3. Install dotnet-ef for migrations
dotnet tool install --global dotnet-ef

# 4. Add the dotnet tools path for the current session
export PATH="$PATH:/home/ubuntu/.dotnet/tools"

# 5. Install PostgreSQL
sudo apt-get update
sudo apt-get install -y postgresql postgresql-contrib

# 6. (Optional) Install Docker for easy PostgreSQL setup
sudo apt-get update
sudo apt-get install -y apt-transport-https ca-certificates curl software-properties-common
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg
echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt-get update
sudo apt-get install -y docker-ce docker-ce-cli containerd.io

# 7. (Optional) Set up PostgreSQL using Docker
docker run --name postgres -e POSTGRES_PASSWORD=your_password -p 5432:5432 -d postgres

```
---

## Backend Setup (ASP.NET Core)

### 1. Install Dependencies
Navigate to the `tickets-backend/ticket-api` directory and install the necessary packages:

```bash
cd tickets-backend/ticket-api
dotnet restore
```

### 2. Set Up Database Connection String
In the `appsettings.json` file located in the `ticket-api` project, configure the PostgreSQL connection:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=your_database_name;Username=your_username;Password=your_password"
}
```

### 3. Run Database Migrations
Make sure your PostgreSQL database is up and running, then apply the migrations to create the necessary tables:

```bash
dotnet ef database update
```

### 4. Running the Backend
To start the backend, run the following command:

```bash
dotnet run
```

The backend API will be available at `http://localhost:5007` (or see the console).
or you can explore it by using this link http://54.91.30.24:5007/swagger/index.html where i deploy the .net app in ec2 instance, this link will redirect you to the swagger documentation.

---

## Frontend Setup (React)

### 1. Install Dependencies
Navigate to the `tickets-frontend` directory and install the dependencies:

```bash
cd tickets-frontend
npm install
```

### 2. Update API Endpoints
Ensure that the frontend is configured to point to the backend API. Open the `.env` file (create one if necessary) in the `tickets-frontend` directory and set the API URL:

```env
REACT_APP_API_URL=http://localhost:5000/api
```

### 3. Running the Frontend
To start the React development server, run:

```bash
npm start
```

The frontend will be available at `http://localhost:3000`.

---

## Database Setup (PostgreSQL)

### Option 1: Manual Setup
1. Install PostgreSQL and create a new database:

   ```bash
   createdb your_database_name
   ```

2. Set the PostgreSQL username and password if you haven't already.

3. Update the connection string in the `appsettings.json` file in the backend.

### Option 2: Using Docker (Recommended)
If you prefer using Docker, you can set up a PostgreSQL container by running:

```bash
docker run --name postgres-db -e POSTGRES_USER=your_username -e POSTGRES_PASSWORD=your_password -e POSTGRES_DB=your_database_name -p 5432:5432 -d postgres
```

Once the container is running, your database will be accessible for the backend API.

---

## Running the Application

1. **Run the Backend**: Start the backend by running:

   ```bash
   dotnet run
   ```

   The backend will be available at `http://localhost:5000`.

2. **Run the Frontend**: Start the frontend by running:

   ```bash
   npm run start
   ```

   The frontend will be available at `http://localhost:3000`.

Access the application via `http://localhost:3000`. The frontend should communicate with the backend API hosted at `http://localhost:5000/api`.

---

## Additional Information

- **Frontend Build**: To build the frontend for production, run:

  ```bash
  npm run build
  ```

  This will create an optimized production build in the `build/` directory.

- **Backend Testing**: The backend includes unit tests located in the `API.Tests` folder. To run the tests, execute:

  ```bash
  dotnet test
  ```

---
