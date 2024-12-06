FROM mcr.microsoft.com/dotnet/runtime:6.0 AS final
MAINTAINER karevin <karevin@sina.com>
WORKDIR /app
ENV INTERVAL 300000
COPY .  /app/
ENTRYPOINT ["dotnet","JavDB.Docker.dll"]
