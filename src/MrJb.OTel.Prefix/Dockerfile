FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base

# ports
EXPOSE 8080

# should be downloaded in build so this isn't installed on final image.
RUN apt-get update && apt-get install -y wget

# install stackfiy retrace
RUN mkdir -m 0755 -p /stackify-dotnet-apm && \
    cd /stackify-dotnet-apm && \
    wget http://download.stackify.com/stackify-dotnet-apm/stackify-dotnet-apm-install-linux-latest.tar.gz && \
    tar -zxvf stackify-dotnet-apm-install-linux-latest.tar.gz && \
    cd stackify-dotnet-apm-install-linux && \
    ./install.sh --docker && \
    cd .. && \
    rm -rf stackify-dotnet-apm-install-linux && \
    rm stackify-dotnet-apm-install-linux-latest.tar.gz

ENV CORECLR_PROFILER={cf0d821e-299b-5307-a3d8-b283c03916da}
ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER_PATH="/stackify-dotnet-apm/StackifyProfiler.so"

# build
FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["src/MrJb.OTel.Prefix/MrJb.OTel.Prefix.csproj", "src/MrJb.OTel.Prefix/"]
COPY ["src/MrJB.OTel.Prefix.Data/MrJB.OTel.Prefix.Data.csproj", "src/MrJB.OTel.Prefix.Data/"]
COPY ["src/MrJB.OTel.Prefix.OTel/MrJB.OTel.Prefix.OTel.csproj", "src/MrJB.OTel.Prefix.OTel/"]
RUN dotnet restore "src/MrJb.OTel.Prefix/MrJb.OTel.Prefix.csproj"
COPY . .
WORKDIR "/src/src/MrJb.OTel.Prefix"
RUN dotnet build "MrJb.OTel.Prefix.csproj" -c Release -o /app/build

# publish
FROM build AS publish
RUN dotnet publish "MrJb.OTel.Prefix.csproj" -c Release -o /app/publish /p:UseAppHost=false

# final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MrJb.OTel.Prefix.dll"]