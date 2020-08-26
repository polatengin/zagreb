provider "azurerm" {
  version = "=2.24.0"
  features {}
}

locals {
  project_name              = replace(lower(var.project_name == "" ? random_string.lower.result : var.project_name), " ", "")
  resource_group_name       = "${local.project_name}-rg"
  storage_account_name      = "${local.project_name}st"
  app_service_plan_name     = "${local.project_name}-plan"
}

# ---------------------------------------------------------------------------------------------------------------------
# DEPLOY THE RESOURCE GROUP
# ---------------------------------------------------------------------------------------------------------------------

resource "azurerm_resource_group" "rg" {
  name     = local.resource_group_name
  location = var.location
}

# ---------------------------------------------------------------------------------------------------------------------
# DEPLOY THE AZURE STORAGE ACCOUNT
# ---------------------------------------------------------------------------------------------------------------------

resource "azurerm_storage_account" "storage" {
  name                     = local.storage_account_name
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

# ---------------------------------------------------------------------------------------------------------------------
# DEPLOY THE APP SERVICE PLAN
# ---------------------------------------------------------------------------------------------------------------------

resource "azurerm_app_service_plan" "app_service_plan" {
  name                = local.app_service_plan_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}
