param appName string

var rgLocation = resourceGroup().location
var webAppName = '${appName}-${uniqueString(resourceGroup().id, 'just-a-name')}'

// --> Module: LogAnalytics workspace + Application Insights
// https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/modules

module moduleAppInsights 'module-AppInsightsLogAnalytics.bicep' = {
  name: 'Insights-Deployment'
  params: {
    appName: webAppName
  }
}

// --> App Service plan
// https://learn.microsoft.com/en-us/azure/templates/microsoft.web/serverfarms

resource hostingPlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: webAppName
  location: rgLocation
  kind: '' // Leave it empty for windows. Otherwise: 'linux' and required to set properties.reserved: true
  sku: {
    name: 'F1'
    tier: 'Free'
  }
  properties: {}
}

// --> Webb App
// https://learn.microsoft.com/en-us/azure/templates/microsoft.web/sites

// - Simple App Service - Linux: https://github.com/Azure/azure-quickstart-templates/blob/master/quickstarts/microsoft.web/app-service-docs-linux
// - Simple App Service - Windows: https://github.com/Azure/azure-quickstart-templates/tree/master/quickstarts/microsoft.web/app-service-docs-windows

resource webApp 'Microsoft.Web/sites@2022-09-01' = {
  name: webAppName
  location: rgLocation
  kind: 'app'
  properties: {
    serverFarmId: hostingPlan.id
    httpsOnly: true
    siteConfig: {
      minTlsVersion: '1.2' // optional
      netFrameworkVersion: 'v8.0' // optional
      appSettings: [
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: moduleAppInsights.outputs.appInsights_ConnectionString
        }
      ]
    }
  }
}

// --> Source control
// https://learn.microsoft.com/en-us/azure/templates/microsoft.web/sites/sourcecontrols

// resource sourceControl 'Microsoft.Web/sites/sourcecontrols@2022-09-01' = {
//   parent: webApp
//   name: 'web-app-source'
//   properties: {
//     repoUrl: repoURL
//     branch: branch
//     isManualIntegration: true
//   }
// }

output url string = webApp.properties.defaultHostName