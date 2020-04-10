#!/bin/bash

echo "##### Starting database container"
docker start bookshelf-mssql
echo "##### Stopping API container"
docker stop bookshelf-api
echo "##### Removing old API container instance"
docker rm bookshelf-api
echo "##### Building new image"
docker build -t bookshelf-api .
echo "##### Starting new API container"
docker run -d -p 5000:80 --name bookshelf-api --link bookshelf-mssql:bookshelf-mssql bookshelf-api