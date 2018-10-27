FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY . .
RUN dotnet restore "ShiftManagement.Web/ShiftManagement.Web.csproj"
WORKDIR "/src/ShiftManagement.Web"
RUN dotnet build "ShiftManagement.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ShiftManagement.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ShiftManagement.Web.dll"]