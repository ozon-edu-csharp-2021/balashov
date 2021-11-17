﻿FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/OzonEdu.MerchandiseService/OzonEdu.MerchandiseService.csproj", "OzonEdu.MerchandiseService/"]
RUN dotnet restore "OzonEdu.MerchandiseService/OzonEdu.MerchandiseService.csproj"
COPY /src .
WORKDIR "/src/OzonEdu.MerchandiseService"
RUN dotnet build "OzonEdu.MerchandiseService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OzonEdu.MerchandiseService.csproj" -c Release -o /app/publish
COPY "entrypoint.sh" "/app/publish/."

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
#EXPOSE 5000
#EXPOSE 5002

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

#ENTRYPOINT ["dotnet", "OzonEdu.MerchandiseService.dll"]
RUN chmod +x entrypoint.sh
CMD /bin/bash entrypoint.sh
