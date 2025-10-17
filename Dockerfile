# Stage 1: Build - Projeyi derlemek için .NET SDK'sýný kullan
# Projenizin .NET sürümüne göre bunu güncelleyebilirsiniz (örn: 8.0, 7.0)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sadece .csproj dosyalarýný ve .sln dosyasýný kopyala
# Bu, Docker katman önbelleðini (layer cache) optimize eder.
# Baðýmlýlýklar deðiþmediði sürece 'dotnet restore' adýmý tekrar çalýþtýrýlmaz.
COPY ["CleanArchitecture.sln", "."]
COPY ["src/Core/CleanArchitecture.Application/CleanArchitecture.Application.csproj", "src/Core/CleanArchitecture.Application/"]
COPY ["src/Core/CleanArchitecture.Domain/CleanArchitecture.Domain.csproj", "src/Core/CleanArchitecture.Domain/"]
COPY ["src/External/CleanArchitecture.Infrastructure/CleanArchitecture.Infrastructure.csproj", "src/External/CleanArchitecture.Infrastructure/"]
COPY ["src/External/CleanArchitecture.Persistance/CleanArchitecture.Persistance.csproj", "src/External/CleanArchitecture.Persistance/"]
COPY ["src/External/CleanArchitecture.Presentation/CleanArchitecture.Presentation.csproj", "src/External/CleanArchitecture.Presentation/"]
COPY ["src/CleanArchitecture.WebApi/CleanArchitecture.WebApi.csproj", "src/CleanArchitecture.WebApi/"]
# Test projesini kopyalamaya gerek yok.

# NuGet paketlerini geri yükle
RUN dotnet restore "CleanArchitecture.sln"

# Projenin geri kalan tüm dosyalarýný kopyala
COPY . .

# WebApi projesini publish et (yayýnla)
WORKDIR "/src/src/CleanArchitecture.WebApi"
RUN dotnet publish "CleanArchitecture.WebApi.csproj" -c Release -o /app/publish --no-restore

# Stage 2: Runtime - Sadece uygulamayý çalýþtýrmak için gerekli dosyalarý içeren küçük imaj
# Bu imaj SDK içermediði için çok daha küçüktür.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render'ýn dýþarýdan gelen isteklere eriþmesi için portu belirt. 
# Genellikle ASP.NET Core uygulamalarý 80 ve 443 portlarýný dinler.
EXPOSE 8080 
# Render gibi platformlar genellikle PORT ortam deðiþkenini kendi ayarlar.
# Bu yüzden Kestrel'i bu deðiþkene göre ayarlamak en iyisidir.
ENV ASPNETCORE_URLS=http://+:8080

# Uygulamayý çalýþtýr
ENTRYPOINT ["dotnet", "CleanArchitecture.WebApi.dll"]