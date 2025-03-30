# Use official .NET SDK base image
FROM mcr.microsoft.com/dotnet/sdk:9.0.200-alpine3.20 AS base

# Metadata
LABEL maintainer="hello@djjm.io"
LABEL version="1.0"
LABEL description="Custom .NET SDK image (dotnet/sdk:9.0.200-alpine3.20-arm64v8) with pre-installed NuGet packages"

# Set working directory
WORKDIR /

# Create dummy project to cache NuGet packages
RUN dotnet new console -n TempProject \
    && cd TempProject \
    && dotnet add package Blazored.LocalStorage --version 4.5.0 \
    && dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 9.0.3 \
    && dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.1 \
    && dotnet add package MudBlazor --version 8.4.0 \
    && dotnet add package nanoid --version 3.1.0 \
    && dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 9.0.4 \
    && dotnet add package QRCoder --version 1.6.0 \
    && dotnet add package Swashbuckle.AspNetCore --version 7.2.0 \
    && dotnet add package System.IdentityModel.Tokens.Jwt --version 8.7.0 \
    && dotnet add package System.Security.Claims --version 4.3.0 \
    && dotnet add package Extensions.MudBlazor.StaticInput --version 3.2.0 \
    && dotnet add package Microsoft.AspNetCore.Authentication.MicrosoftAccount --version 9.0.3 \
    && dotnet add package Microsoft.AspNetCore.Components.WebAssembly.Server --version 9.0.3 \
    && dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore --version 9.0.3 \
    && dotnet add package Microsoft.AspNetCore.Identity.UI --version 9.0.3 \
    && dotnet add package Microsoft.EntityFrameworkCore --version 9.0.3 \
    && dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.3 \
    && dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.3 \
    && dotnet add package Microsoft.Extensions.Configuration --version 9.0.3 \
    && dotnet add package Microsoft.IdentityModel.Tokens --version 8.6.1 \
    && dotnet add package Microsoft.AspNetCore.OpenApi --version 9.0.3 \
    && dotnet add package System.IdentityModel.Tokens.Jwt --version 8.6.1 \
    && dotnet restore \
    && cd .. \
    && rm -rf TempProject
