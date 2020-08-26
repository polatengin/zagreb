provider "azurerm" {
  version = "=2.24.0"
  features {}
}

locals {
  project_name              = replace(lower(var.project_name == "" ? random_string.lower.result : var.project_name), " ", "")
  resource_group_name       = "${local.project_name}-rg"
}

# ---------------------------------------------------------------------------------------------------------------------
# DEPLOY THE RESOURCE GROUP
# ---------------------------------------------------------------------------------------------------------------------

resource "azurerm_resource_group" "rg" {
  name     = local.resource_group_name
  location = var.location
}

