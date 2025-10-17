# Stage 1: Build - Projeyi derlemek i�in .NET SDK's�n� kullan
# Projenizin .NET s�r�m�ne g�re bunu g�ncelleyebilirsiniz (�rn: 8.0, 7.0)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sadece .csproj dosyalar�n� ve .sln dosyas�n� kopyala
# Bu, Docker katman �nbelle�ini (layer cache) optimize eder.
# Ba��ml�l�klar de�i�medi�i s�rece 'dotnet restore' ad�m� tekrar �al��t�r�lmaz.
COPY ["CleanArchitecture.sln", "."]
COPY ["src/Core/CleanArchitecture.Application/CleanArchitecture.Application.csproj", "src/Core/CleanArchitecture.Application/"]
COPY ["src/Core/CleanArchitecture.Domain/CleanArchitecture.Domain.csproj", "src/Core/CleanArchitecture.Domain/"]
COPY ["src/External/CleanArchitecture.Infrastructure/CleanArchitecture.Infrastructure.csproj", "src/External/CleanArchitecture.Infrastructure/"]
COPY ["src/External/CleanArchitecture.Persistance/CleanArchitecture.Persistance.csproj", "src/External/CleanArchitecture.Persistance/"]
COPY ["src/External/CleanArchitecture.Presentation/CleanArchitecture.Presentation.csproj", "src/External/CleanArchitecture.Presentation/"]
COPY ["src/CleanArchitecture.WebApi/CleanArchitecture.WebApi.csproj", "src/CleanArchitecture.WebApi/"]
# Test projesini kopyalamaya gerek yok.

# NuGet paketlerini geri y�kle
RUN dotnet restore "CleanArchitecture.sln"

# Projenin geri kalan t�m dosyalar�n� kopyala
COPY . .

# WebApi projesini publish et (yay�nla)
WORKDIR "/src/src/CleanArchitecture.WebApi"
RUN dotnet publish "CleanArchitecture.WebApi.csproj" -c Release -o /app/publish --no-restore

# Stage 2: Runtime - Sadece uygulamay� �al��t�rmak i�in gerekli dosyalar� i�eren k���k imaj
# Bu imaj SDK i�ermedi�i i�in �ok daha k���kt�r.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render'�n d��ar�dan gelen isteklere eri�mesi i�in portu belirt. 
# Genellikle ASP.NET Core uygulamalar� 80 ve 443 portlar�n� dinler.
EXPOSE 8080 
# Render gibi platformlar genellikle PORT ortam de�i�kenini kendi ayarlar.
# Bu y�zden Kestrel'i bu de�i�kene g�re ayarlamak en iyisidir.
ENV ASPNETCORE_URLS=http://+:8080

# Uygulamay� �al��t�r
ENTRYPOINT ["dotnet", "CleanArchitecture.WebApi.dll"]