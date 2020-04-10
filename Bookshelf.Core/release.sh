#!/bin/bash

docker start bookshelf-mssql
docker stop bookshelf-api
docker rm bookshelf-api
docker build -t bookshelf-api .
docker run -d -p 5000:80 --name bookshelf-api --link bookshelf-mssql:bookshelf-mssql bookshelf-api