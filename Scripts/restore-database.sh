#!/bin/bash

backup='../../db/data.bak'
container='bookshelf-mssql'
dbname='Books'
password=''

echo "##### Copying backup file from local folder into container"
docker cp $backup $container:/tmp/data.bak

echo "##### Dropping database if it exists"
docker exec -it $container /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $password -Q "DROP DATABASE $dbname"

echo "##### Restoring database"
docker exec -it $container /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $password -Q "RESTORE DATABASE $dbname FROM DISK='/tmp/data.bak'"

echo "##### Removing backup file from container"
docker exec -it $container /bin/bash -c "rm /tmp/data.bak"
