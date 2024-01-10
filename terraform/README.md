
# Setup

## Requirements
  * Install terraform version  >= v0.15
     [Terraform downloads](https://www.terraform.io/downloads.html)

  * Install azure cli
     [Azure cli downloads](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-linux?pivots=apt)


---
### Basic project setup

1. Login to azure cli using command
    ```
    az login
    ```

1. Provision the infrastructure using Terraform.

    Once you have downloaded the required version of **Terraform**, navigate
    to gcp directory and run:

    ```
    cd terraform
    terraform init
    ```

    This will intialize terraform and download gcp plugin and modules for
    terraform and initialize workspace(s).

    **If you are working on multiple projects using same terraform script
      then first you need to activate project then reconfigure terraform.
      env**
    ```
    terraform init --reconfigure
    ```

    Once terraform has initialized, you need to select the workspace for which
    you need to provision the infrastructure.

    To see the list of available workspaces do:
    ```terraform workspace select <workspace-name>```

    Once the workspace is selected, validate / plan what resources terraform
    is going to create/delete/update using:
    ```
    terraform plan -var-file=dev.variables.tfvars
    ```

    Once you are satisfied with what changes terraform will do, you can apply
    your changes using:
    ```
    terraform apply -var-file=dev.variables.tfvars
    ```

---


# How Tos

## Terraform

### How To create new Terraform workspace

`terraform workspace new <workspace-name>`

### How To delete a Terraform workspace

`terraform workspace delete <workspace-name>`

### How To List Terraform workspaces

`terraform workspace list`

### How To Select Terraform workspace

`terraform workspace select <workspace-name>`

### How To Find which workspace is currently active

`terraform workspace show`

## How to initialise the working directory for terraform

`terraform init`



## How to create execution plan for dev workspace

`terraform plan -var-file=dev.variables.tfvars`

## How to apply the changes for dev workspace

`terraform apply -var-file=dev.variables.tfvars`

## How to destroy all resources for dev workspace

`terraform destroy -var-file=dev.variables.tfvars`

## How To change infrastructure for staging

`terraform apply -var-file=staging.variables.tfvars`

---

