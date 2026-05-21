# Build React
FROM node:20 AS frontend
WORKDIR /app
COPY XA57_Proyecto/configurador-client/package*.json ./
RUN npm install
COPY XA57_Proyecto/configurador-client/ ./
RUN npm run build

# Build ASP.NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend
WORKDIR /app
COPY . .
COPY --from=frontend /app/build ./XA57_Proyecto/wwwroot/
RUN dotnet publish XA57_Proyecto/XA57_Proyecto.csproj -c Release -o /out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=backend /out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "XA57_Proyecto.dll"]
