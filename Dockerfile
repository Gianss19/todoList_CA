FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY todoList.Domain/todoList.Domain.csproj todoList.Domain/
COPY todoList.Application/todoList.Application.csproj todoList.Application/
COPY todoList.Infrastructure/todoList.Infrastructure.csproj todoList.Infrastructure/
COPY todoList.Presentation/todoList.Api.csproj todoList.Presentation/
RUN dotnet restore todoList.Presentation/todoList.Api.csproj

COPY . .
RUN dotnet publish todoList.Presentation/todoList.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "todoList.Api.dll"]
