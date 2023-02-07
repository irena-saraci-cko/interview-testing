#get base sdks image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /

# copy csproj and restore
COPY ./src/PaymentGateway.Api/PaymentGateway.Api.csproj /src/PaymentGateway.Api/
COPY ./src/PaymentGateway.Application/PaymentGateway.Application.csproj /src/PaymentGateway.Application/
COPY ./src/PaymentGateway.BankAcquirer/PaymentGateway.BankAcquirer.csproj /src/PaymentGateway.BankAcquirer/
COPY ./src/PaymentGateway.Domain/PaymentGateway.Domain.csproj /src/PaymentGateway.Domain/
COPY ./src/PaymentGateway.Common/PaymentGateway.Common.csproj /src/PaymentGateway.Common/

RUN dotnet restore /src/PaymentGateway.Api/PaymentGateway.Api.csproj

#copy the rest of the files and publish release
COPY ./src /src
WORKDIR /src/PaymentGateway.Api/
RUN dotnet publish -c Release -o out /p:Version=1.0.0


#generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
EXPOSE 5067
EXPOSE 7092
COPY --from=build /src/PaymentGateway.Api/out .

#how do we want the container to start
ENTRYPOINT ["dotnet", "PaymentGateway.Api.dll" ]