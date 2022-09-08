FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /app

ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

ENV LD_LIBRARY_PATH /app

COPY ./build/node /app

ENTRYPOINT ["dotnet", "AElf.Launcher.dll"]

RUN echo 0x3f > /proc/self/coredump_filter
