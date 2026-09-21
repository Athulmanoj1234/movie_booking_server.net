#multiple staging is there so build and run image is used in here 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /APP

#copy everything inside the csproj to the working firectory of the container
COPY . ./

#it installs all the nuget packages that the application depends upon
RUN dotnet restore
 
#build and publish a release
RUN dotnet publish -o out

#build run time image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /APP

#copy from build stage and all the resources in the /App/out and copy to the curent working directory ie /APP
COPY --from=build /APP/out .

ENTRYPOINT ["dotnet", "movie_booking.dll"]