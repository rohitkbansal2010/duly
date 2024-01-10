#!/bin/bash
# Read parameters

if [ $# -ne 8 ]; then
  echo "Incorrect number of options. Usage options: -h (hostname), -u (username), -d (database), -p (password)"
  echo "Example: ./backup-psql-database.sh -h localhost -u dba -d mydatabase -p mypassword"
  exit 1
fi

while getopts "h:u:d:p:" option
do
  case $option in
    h) hostname=$OPTARG;;
    u) username=$OPTARG;;
    d) database=$OPTARG;;
    p) password=$OPTARG;;
    *) echo "Error: invalid option"
       exit 1;;
  esac
done
 
# Check pg_dump installation
if ! command -v pg_dump &> /dev/null; then
  echo "Cannot find pg_dump, check if it is installed."
  exit 1
fi

# Backup path variables
backupDirectory=~/dbbackup
backupFile=$database-backup-$(date +"%F-%H%M%S").gz
if [ ! -d $backupDirectory ]; then
  mkdir -p $backupDirectory
  if [ $? -ne 0 ]; then
    echo "Failed to create backup directory $backupDirectory."
    exit 1
  fi
fi

# Create backup
echo "Creating backup of the database $database into $backupDirectory/$backupFile"
PGPASSWORD=$password pg_dump -h $hostname -U $username -d $database | gzip > $backupDirectory/$backupFile
if [ $? -ne 0 ]; then
  echo "Failed to create backup"
  exit 1
fi

