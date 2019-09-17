$subscriptionId = ""
$resourceGroup = ""

Select-AzSubscription -SubscriptionId $subscriptionId

New-AzResourceGroupDeployment `
    -ResourceGroupName $resourceGroup `
    -TemplateParameterFile logic-app-refresh.parameters.json `
    -TemplateFile logic-app-refresh.json 