#!/bin/bash

# https://learn.microsoft.com/en-us/cli/azure/deployment/group?view=azure-cli-latest#az-deployment-group-create

az deployment group create \
    --name "Deployment-AppIns-Container" \
    --resource-group "AppInsightsTest" \
    --template-file "main-linux-container.bicep" \
    --parameters "@main.parameters.json"