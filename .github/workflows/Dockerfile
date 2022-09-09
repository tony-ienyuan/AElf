FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base

ARG FOLDER_NAME=TestNet_Build_Images_AELF_Node

WORKDIR /app

COPY [".", "/app"]

RUN apt-get -y update && \
    apt-get -y install \
    protobuf-compiler \
    unzip && \
    dotnet publish AElf.sln /p:NoBuild=false --configuration Release -o /app/docker/ 

FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /app

ENV LD_LIBRARY_PATH /app

COPY --from=base /app/docker .

RUN echo 0x3f > /proc/self/coredump_filter

ENTRYPOINT ["dotnet", "AElf.Launcher.dll"]
