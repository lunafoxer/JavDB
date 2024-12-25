FROM mcr.microsoft.com/dotnet/runtime:6.0 AS final
MAINTAINER karevin <karevin@sina.com>
WORKDIR /app
ENV INTERVAL 15000
ENV DELAY 3000
ENV TZ Asia/Shanghai
COPY .  /app/
ENTRYPOINT ["dotnet","JavDB.Docker.dll"]
