#!/bin/bash

# https://learn.microsoft.com/en-us/cli/azure/deployment/group?view=azure-cli-latest#az-deployment-group-create

az deployment group create \
    --name "Deployment-AppIns" \
    --resource-group "AppInsightsTest" \
    --template-file "main.bicep" \
    --parameters "@main.parameters.json"