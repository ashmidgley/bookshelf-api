#!/bin/bash

date=$(date '+%Y-%m-%d')
container='bookshelf-mssql'
database='Books'
password=''
space=''
dir='/root/scripts'

echo "##### Backing up database"
docker exec $container /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $password -Q "BACKUP DATABASE $database TO DISK='/tmp/$date-bookshelf.bak'"

echo "##### Copying backup file from container to local directory"
docker cp $container:/tmp/$date-bookshelf.bak $dir

echo "##### Removing backup file from container"
docker exec $container /bin/bash -c "rm /tmp/$date-bookshelf.bak"

echo "##### Zipping up bak file"
zip $dir/$date-bookshelf.zip $dir/$date-bookshelf.bak

echo "##### Moving zipped backup to Digital Ocean space"
s3cmd put $dir/$date-bookshelf.zip s3://$space

echo "##### Deleting local backup files"
rm $dir/$date-bookshelf.bak
rm $dir/$date-bookshelf.zip

echo "##### Restart container to avoid annoying login error"
docker stop $container
docker start $container
