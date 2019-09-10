$subscriptionId = "84717e9e-5b58-4068-a903-2604a799282c"
$resourceGroup = "azure-enterprise-consumption-dev"

Select-AzSubscription -SubscriptionId $subscriptionId

New-AzResourceGroupDeployment `
    -ResourceGroupName $resourceGroup `
    -TemplateParameterFile functions-and-keyvault.parameters.json `
    -TemplateFile functions-and-keyvault.json 