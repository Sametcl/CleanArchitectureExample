# Stage 1: Build - Projeyi derlemek için .NET SDK'sýný kullan
# Projenizin .NET sürümüne göre bunu güncelleyebilirsiniz (örn: 8.0, 7.0)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sadece .csproj dosyalarýný ve .sln dosyasýný kopyala
# Bu, Docker katman önbelleðini (layer cache) optimize eder.
# DÜZELTME: "src/", "Core/", "External/" gibi sanal yollar kaldýrýldý.
COPY ["CleanArchitecture.sln", "."]
COPY ["CleanArchitecture.Application/CleanArchitecture.Application.csproj", "CleanArchitecture.Application/"]
COPY ["CleanArchitecture.Domain/CleanArchitecture.Domain.csproj", "CleanArchitecture.Domain/"]
COPY ["CleanArchitecture.Infrastructure/CleanArchitecture.Infrastructure.csproj", "CleanArchitecture.Infrastructure/"]
COPY ["CleanArchitecture.Persistance/CleanArchitecture.Persistance.csproj", "CleanArchitecture.Persistance/"]
COPY ["CleanArchitecture.Presentation/CleanArchitecture.Presentation.csproj", "CleanArchitecture.Presentation/"]
COPY ["CleanArchitecture.WebApi/CleanArchitecture.WebApi.csproj", "CleanArchitecture.WebApi/"]

# NuGet paketlerini geri yükle
RUN dotnet restore "CleanArchitecture.sln"

# Projenin geri kalan tüm dosyalarýný kopyala
COPY . .

# WebApi projesini publish et (yayýnla)
# DÜZELTME: WORKDIR yolu düzeltildi.
WORKDIR "/src/CleanArchitecture.WebApi"
RUN dotnet publish "CleanArchitecture.WebApi.csproj" -c Release -o /app/publish --no-restore

# Stage 2: Runtime - Sadece uygulamayý çalýþtýrmak için gerekli dosyalarý içeren küçük imaj
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render'ýn dýþarýdan gelen isteklere eriþmesi için portu belirt.
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Uygulamayý çalýþtýr
ENTRYPOINT ["dotnet", "CleanArchitecture.WebApi.dll"]