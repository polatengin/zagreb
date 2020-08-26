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
