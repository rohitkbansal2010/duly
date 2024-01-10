#!/bin/bash
# Read parameters

if [ $# -ne 10 ]; then
  echo "Incorrect number of options. Usage options: -h (hostname), -u (username), -d (database), -p (password), -s (scheme, values: up and down)"
  echo "Example: ./reportportal-db-migration.sh -h localhost -u dba -d mydatabase -p mypassword -s up"
  exit 1
fi

while getopts "h:u:d:p:s:" option
do
  case $option in
    h) hostname=$OPTARG;;
    u) username=$OPTARG;;
    d) database=$OPTARG;;
    p) password=$OPTARG;;
    s) scheme=$OPTARG;;
    *) echo "Error: invalid option"
       exit 1;;
  esac
done
 
# Check tools installation
for tool in git psql
do
  if ! command -v $tool &> /dev/null; then
    echo "Cannot find $tool, check if it is installed."
    exit 1
  fi
done

# Clone migrations repository
if [ -d ./migrations ]; then
  rm -rf migrations
fi
git clone https://github.com/reportportal/migrations.git
cd migrations/migrations

# Get sorted list of scripts for migration
scriptsList=(`ls | sort -n | grep .$scheme.sql`)

# Perform migration
for script in ${scriptsList[*]}
do
  printf "##### Executing script $script #####\n"
  PGPASSWORD=$password psql -h $hostname -U $username -d $database -f $script
  if [ $? -ne 0 ]; then
    echo "Failed to execute script $script"
    exit 1
  fi
done
