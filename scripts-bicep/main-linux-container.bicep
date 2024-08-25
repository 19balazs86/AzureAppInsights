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

// --> App Service plan: Linux
// https://learn.microsoft.com/en-us/azure/templates/microsoft.web/serverfarms

resource hostingPlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: webAppName
  location: rgLocation
  kind: 'linux'
  sku: { // Or you can use: B1-Basic
    name: 'F1'
    tier: 'Free'
  }
  properties: {
    reserved: true // When you have linux
  }
}

// --> Webb App with docker container
// https://learn.microsoft.com/en-us/azure/templates/microsoft.web/sites

// - Examle: https://samcogan.com/creating-an-azure-web-app-or-function-running-a-container-with-bicep

resource webApp 'Microsoft.Web/sites@2022-09-01' = {
  name: webAppName
  location: rgLocation
  kind: 'app'
  properties: {
    serverFarmId: hostingPlan.id
    httpsOnly: true
    siteConfig: {
      minTlsVersion: '1.2' // optional
      linuxFxVersion: 'DOCKER|index.docker.io/19balazs86/app-insights-web-api'
      appSettings: [
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: moduleAppInsights.outputs.appInsights_ConnectionString
        }
      ]
    }
  }
}

output url string = webApp.properties.defaultHostName