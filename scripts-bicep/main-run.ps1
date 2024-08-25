New-AzResourceGroupDeployment `
    -name "Deployment-AppIns" `
    -ResourceGroupName "AppInsightsTest" `
    -TemplateFile "main.bicep" `
    -TemplateParameterFile "main.parameters.json"