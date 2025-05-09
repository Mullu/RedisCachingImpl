# Redis Caching with .NET 8 and Docker

1. Install Redis and Run it

   If you're using Windows, install and run Redis using Docker.

   Switch Docker to Linux Mode:
   ```sh
   & 'C:\Program Files\Docker\Docker\DockerCli.exe' -SwitchLinuxEngine

   docker run --name mine-redis -p 6379:6379 -d redis


2. Install dependency nuget packages

   dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis


3. buld and run the application

   3.1. dotnet build
   
   3.2. dotnet run
   
   3.3. Check result using, http://localhost:5000/
    