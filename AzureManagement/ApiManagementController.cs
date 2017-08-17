using AzureManagement.Logging.Interfaces;
using Microsoft.Azure;
using Microsoft.Azure.Management.ApiManagement;
using Microsoft.Azure.Management.ApiManagement.SmapiModels;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace AzureManagement
{
    public class ApiManagementController
    {
        private IAzureConfig Config { get; }
        private ILogger Log { get; }
        private SubscriptionCloudCredentials SubscriptionCloudCredentials { get; set; }
        private string ApiManagementName { get; }
        private string ApiManagementResGrpName { get; }

        //This is coming but not released yet
        //https://github.com/Azure/azure-sdk-for-net/blob/psSdkJson6/src/SDKs/ApiManagement/ApiManagement.Tests/ManagementApiTests/ApiTests.cs
        public ApiManagementController(IAzureConfig appConfig, ILogger log, string apiManagementName, string apiManagementResourceGroupName)
        {
            Config = appConfig;
            Log = log;
            ConfigureCredentials();
            ApiManagementName = apiManagementName;
            ApiManagementResGrpName = apiManagementResourceGroupName;
        }



        public void OrchestrateApiAddedToApiManagementService(string apiName, string route, string serviceUrl, string swaggerUrl, string apiManagementProductName)
        {
            //TODO: swagger content in the create isnt supported on this version latest does have it though.
            var swaggerContentFromApi = GetSwaggerApiContent(swaggerUrl); //https://github.com/Azure/azure-sdk-for-net/blob/psSdkJson6/src/SDKs/ApiManagement/ApiManagement.Tests/ManagementApiTests/ApiTests.cs
            var apiId = CreateOrUpdateApi(apiName, route, serviceUrl);
            CreateApiOperationFromSwaggerContent(swaggerContentFromApi);
            CreateOrUpdateProduct(apiManagementProductName, apiId);

        }

        private void CreateApiOperationFromSwaggerContent(string swaggerContentFromApi)
        {


            //var apiCreateOrUpdate = new ApiCreateOrUpdateParameter()
            //{
            //    Path = path,
            //    ContentFormat = ContentFormat.SwaggerJson,
            //    ContentValue = swaggerApiContent
            //};

            //var swaggerApiResponse = testBase.client.Api.CreateOrUpdate(
            //                                                            testBase.rgName,
            //                                                            testBase.serviceName,
            //                                                            swaggerApi,
            //                                                            apiCreateOrUpdate);

        }

        private string GetSwaggerApiContent(string swaggerUrl)
        {
            string swaggerApiContent;

            using (var wc = new WebClient())
            {
                using (var stream = wc.OpenRead(swaggerUrl))
                {
                    if (stream == null)
                    {
                        return string.Empty;
                    }

                    using (var streamreader = new StreamReader(stream))
                    {
                        swaggerApiContent = streamreader.ReadToEnd();
                    }

                    stream.Close();
                }

            }

            return swaggerApiContent;
        }


        private ApiContract GetApiByName(string apiName)
        {
            var apis = GetApisFromServiceManager(ApiManagementName, ApiManagementResGrpName);

            return apis.FirstOrDefault(a => a.Name.Contains(apiName));

        }

        private string CreateOrUpdateApi(string apiName, string route, string serviceUrl)
        {

            var existingApi = GetApiByName(apiName);
            var apiId = (existingApi != null) ? existingApi.Id : Guid.NewGuid().ToString();

            if (existingApi != null)
            {
                Log.Info($"Updating existing Api {existingApi.Id}");
            }

            var apiCreateOrUpdateParams = new ApiCreateOrUpdateParameters
            {
                ApiContract = new ApiContract
                {
                    Name = apiName,
                    Description = $"Description: {apiName}",
                    Path = route,
                    ServiceUrl = serviceUrl,
                    Type = ApiTypeContract.Http,
                    Protocols = new List<ApiProtocolContract>
                    {
                        ApiProtocolContract.Https
                    }

                }

            };

            using (var mc = new ApiManagementClient(SubscriptionCloudCredentials))
            {
                mc.Apis.CreateOrUpdate(ApiManagementResGrpName, ApiManagementName, apiId, apiCreateOrUpdateParams, string.Empty);

            }

            Log.Info($"Successfully Created or Updated Api {apiName} Id: {apiId}");
            return apiId;
        }

        private void CreateOrUpdateProduct(string productName, string apiId)
        {
            var product = GetProductIdFromName(productName);
            using (var mc = new ApiManagementClient(SubscriptionCloudCredentials))
            {
                mc.ProductApis.Add(ApiManagementResGrpName, ApiManagementName, product.Id, apiId);

            }
            Log.Info($"Successfully Created or Updated Product {product.Name} Id: {product.Id}");

        }

        private ProductContract GetProductIdFromName(string productName)
        {
            var products = GetProductsFromServiceManager(ApiManagementName, ApiManagementResGrpName);

            return products.FirstOrDefault(a => a.Name.Contains(productName));

        }

        private IEnumerable<ProductContract> GetProductsFromServiceManager(string apiManagementName, string apiManagementResGrpName)
        {

            using (var mc = new ApiManagementClient(SubscriptionCloudCredentials))
            {
                var listResponse = mc.Products.List(apiManagementResGrpName, apiManagementName, null);

                return listResponse.Result.Values;

            }


        }

        private IEnumerable<ApiContract> GetApisFromServiceManager(string apiManagementName, string apiManagementResGrpName)
        {

            using (var mc = new ApiManagementClient(SubscriptionCloudCredentials))
            {
                var listResponse = mc.Apis.List(apiManagementResGrpName, apiManagementName, null);

                return listResponse.Result.Values;

            }


        }

        private void ConfigureCredentials()
        {
            var credential = new ClientCredential(Config.SpnApplicationId, Config.SpnPassword);
            var authContext = new AuthenticationContext($"https://login.windows.net/{Config.TenantId}/");
            var tokenAuthResult = authContext.AcquireToken("https://management.core.windows.net/", credential);
            SubscriptionCloudCredentials = new TokenCloudCredentials(Config.SubscriptionId, tokenAuthResult.AccessToken);

        }
    }
}
