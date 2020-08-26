#!/usr/bin/env bash

# This script deploys Azure Function App and related resources to Azure

# Stop on error
set -e

HELP="
$(basename "$0") [--project-name project_name] [--location location]

commands:
  -h [--help]                 show this help text
  -p [--project-name]         name of the project, it'll be use to generate names for resources, leave blank to auto-generate a name
  -l [--location]             location of the resources, westeurope, eastus, e.g.
"

while [[ "$#" -gt 0 ]]
do
  case $1 in
    -h | --help)
      echo "$HELP"
      exit 0
    ;;
    -p | --project-name)
      PROJECT_NAME=$2
    ;;
    -l | --location)
      LOCATION=$2
    ;;
  esac
  shift
done

if [ -z "$LOCATION" ]
then
  echo "It should be a valid location, you can execute following command and get list of locations"
  echo "az account list-locations --query \"[].{DisplayName:displayName, Name:name}\" -o table"
  read -p "Location : " LOCATION
fi

cd terraform

# download providers and initialize terraform
terraform init
