$subscriptionId = ""
$resourceGroup = ""

Select-AzSubscription -SubscriptionId $subscriptionId

New-AzResourceGroupDeployment `
    -ResourceGroupName $resourceGroup `
    -TemplateParameterFile functions-and-keyvault.parameters.json `
    -TemplateFile functions-and-keyvault.json 