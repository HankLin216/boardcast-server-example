# 使用 .NET SDK 7.0 作為基礎映像
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# 設定工作目錄
WORKDIR /app

# 複製 Web API 專案檔案到容器中
COPY . ./

# 執行 dotnet restore 以安裝依賴套件
RUN dotnet restore

# 執行 dotnet publish 以建置應用程式
RUN dotnet publish -c Release -o out

# 使用 .NET Runtime 7.0 作為運行時映像
FROM mcr.microsoft.com/dotnet/aspnet:7.0

# 安裝 curl 套件
RUN apt-get update \
    && apt-get install -y curl \
    && rm -rf /var/lib/apt/lists/*

# 設定工作目錄
WORKDIR /app

# 複製編譯好的應用程式到運行時映像中
COPY --from=build-env /app/out .

# 指定 Web API 監聽的端口
EXPOSE 80

# 啟動應用程式
ENTRYPOINT ["dotnet", "boardcast-server-example.dll"]
