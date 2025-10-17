# Stage 1: Build - Projeyi derlemek i�in .NET SDK's�n� kullan
# Projenizin .NET s�r�m�ne g�re bunu g�ncelleyebilirsiniz (�rn: 8.0, 7.0)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sadece .csproj dosyalar�n� ve .sln dosyas�n� kopyala
# Bu, Docker katman �nbelle�ini (layer cache) optimize eder.
# D�ZELTME: "src/", "Core/", "External/" gibi sanal yollar kald�r�ld�.
COPY ["CleanArchitecture.sln", "."]
COPY ["CleanArchitecture.Application/CleanArchitecture.Application.csproj", "CleanArchitecture.Application/"]
COPY ["CleanArchitecture.Domain/CleanArchitecture.Domain.csproj", "CleanArchitecture.Domain/"]
COPY ["CleanArchitecture.Infrastructure/CleanArchitecture.Infrastructure.csproj", "CleanArchitecture.Infrastructure/"]
COPY ["CleanArchitecture.Persistance/CleanArchitecture.Persistance.csproj", "CleanArchitecture.Persistance/"]
COPY ["CleanArchitecture.Presentation/CleanArchitecture.Presentation.csproj", "CleanArchitecture.Presentation/"]
COPY ["CleanArchitecture.WebApi/CleanArchitecture.WebApi.csproj", "CleanArchitecture.WebApi/"]

# NuGet paketlerini geri y�kle
RUN dotnet restore "CleanArchitecture.sln"

# Projenin geri kalan t�m dosyalar�n� kopyala
COPY . .

# WebApi projesini publish et (yay�nla)
# D�ZELTME: WORKDIR yolu d�zeltildi.
WORKDIR "/src/CleanArchitecture.WebApi"
RUN dotnet publish "CleanArchitecture.WebApi.csproj" -c Release -o /app/publish --no-restore

# Stage 2: Runtime - Sadece uygulamay� �al��t�rmak i�in gerekli dosyalar� i�eren k���k imaj
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render'�n d��ar�dan gelen isteklere eri�mesi i�in portu belirt.
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Uygulamay� �al��t�r
ENTRYPOINT ["dotnet", "CleanArchitecture.WebApi.dll"]