# 请参阅 https://aka.ms/customizecontainer 以了解如何自定义调试容器，以及 Visual Studio 如何使用此 Dockerfile 生成映像以更快地进行调试。

# 此阶段用于在快速模式(默认为调试配置)下从 VS 运行时
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# 此阶段用于生成服务项目
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["common.props", "."]
COPY ["src/Server/Acme.Server.HttpApi.Host/Acme.Server.HttpApi.Host.csproj", "src/Server/Acme.Server.HttpApi.Host/"]
COPY ["src/Server/Acme.Server.Application/Acme.Server.Application.csproj", "src/Server/Acme.Server.Application/"]
COPY ["src/Server/Acme.Server.Application.Contracts/Acme.Server.Application.Contracts.csproj", "src/Server/Acme.Server.Application.Contracts/"]
COPY ["src/Server/Acme.Server.Domain.Shared/Acme.Server.Domain.Shared.csproj", "src/Server/Acme.Server.Domain.Shared/"]
COPY ["src/Server/Acme.Server.Domain/Acme.Server.Domain.csproj", "src/Server/Acme.Server.Domain/"]
COPY ["src/Server/Acme.Server.EntityFrameworkCore/Acme.Server.EntityFrameworkCore.csproj", "src/Server/Acme.Server.EntityFrameworkCore/"]
RUN dotnet restore "./src/Server/Acme.Server.HttpApi.Host/Acme.Server.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/src/Server/Acme.Server.HttpApi.Host"
RUN dotnet build "./Acme.Server.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 此阶段用于发布要复制到最终阶段的服务项目
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Acme.Server.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 此阶段在生产中使用，或在常规模式下从 VS 运行时使用(在不使用调试配置时为默认值)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Acme.Server.HttpApi.Host.dll"]