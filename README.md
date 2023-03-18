---
Group: 1
Members:
  - Hue Nhan
  - Le Xuan Tu
  - Phan Thanh Ngan
  - Le Tri Hao
---

# SWP391

## Required services

### MSSQL

> Run before dev server

```sh
docker run --rm --name mssql --env-file $PWD/EnvFiles/.env.mssql --volume $PWD/ContainerizedData/mssql:/var/opt/mssql --publish 1433:1433 mcr.microsoft.com/mssql/server:latest
```

### Minio

> Run before dev server

```sh
docker run --rm --name minio --env-file $PWD/EnvFiles/.env.minio --volume $PWD/ContainerizedData/minio:/data --publish 9000:9000 --publish 9090:9090 quay.io/minio/minio:latest server /data --console-address ":9090"
```

## Development

> Run after every code pull

```sh
dotnet restore
```

> Run after every code pull

```sh
dotnet tools restore
```

> Run after every code pull

```sh
dotnet dotnet-ef database update
```

> Run to check if there's any error

```sh
dotnet build
```

> Run dev server

```sh
dotnet watch run
```
