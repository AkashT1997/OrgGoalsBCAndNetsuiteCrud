using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrgGoalsBCAndNetsuiteCrud.Common;
using OrgGoalsBCAndNetsuiteCrud.Models;
using OrgGoalsBCAndNetsuiteCrud.Models.MongoDB;
using OrgGoalsBCAndNetsuiteCrud.MongoDB_Services;
using RestSharp;
using System.ComponentModel.Design;

namespace OrgGoalsBCAndNetsuiteCrud.Core
{
    public class BCClients
    {
        private readonly BCAuthService _bCAuthService;

        public BCClients(BCAuthService bCAuthService)
        {
            _bCAuthService = bCAuthService;
        }

        public BCClients()
        {
        }

        #region BC Auth
        public string MakeAuthorizationRequest(string businessCentralClientId)
        {
            try
            {
                string authorizationRequestURI = $"https://login.microsoftonline.com/common/oauth2/authorize";
                string authorizationRequest = string.Format("{0}?resource={1}&scope={2}&client_id={3}&redirect_uri={4}&response_type={5}&state={6}",
                    authorizationRequestURI,
                    "https://api.businesscentral.dynamics.com",
                    Uri.EscapeDataString("https://api.businesscentral.dynamics.com/.default"),
                    businessCentralClientId,
                    AppConfiguration.BusinessCentralRedirectUri,
                    "code",
                    ""
                    );
                return authorizationRequest;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BCAuthModel> GetBussinessCentralBearerToken(string code, string businessCentralClientId, string businessCentralClientSecret)
        {
            try
            {
                BCAuthModel obj = new BCAuthModel();
                var client = new RestClient($"https://login.microsoftonline.com/common/oauth2/token")
                {
                    Timeout = -1
                };
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("grant_type", "authorization_code");
                request.AddParameter("code", code);
                request.AddParameter("client_id", businessCentralClientId);
                request.AddParameter("client_secret", businessCentralClientSecret);
                request.AddParameter("redirect_uri", AppConfiguration.BusinessCentralRedirectUri);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        obj = JsonConvert.DeserializeObject<BCAuthModel>(response.Content);
                    }
                }
                if (obj != null)
                {
                    return obj;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<BCAuthModel> GetBussinessCentralRefreshToken(string refresh_token)
        {
            try
            {
                BCAuthModel obj = new BCAuthModel();
                var client = new RestClient($"https://login.microsoftonline.com/common/oauth2/token")
                {
                    Timeout = -1
                };
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Cookie", "buid=0.ATQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA.AQABAAEAAAD--DLA3VO7QrddgJg7WevrAyL0LOcQOJTF5VPV7sHjf-NQcJWTx81YInuQ5k9V_2SWVbifsjlLfD7-1-zImfnKOFmFle9EdRyFyTy6HUrzWW2S8fTwpT-k3Mn_-r3KBfogAA; esctx=PAQABAAEAAAD--DLA3VO7QrddgJg7WevrMkYjua7kgCO5YvOxd6aG6WVaSMpVZ-vRnevMrWQoo1nYbe0quHFqPJpqARwSfRZmz7aMlP7JBwwioLKScqum0X_3UUoswfmz2kxM_Etec7Xw1SWJpx6d0Wq88jw7Tg4ShgSHjCGpuTkwNdStjdlOmw_iNT9PDeaH6FjWtSu8lzsgAA; fpc=AoUNWawFi3tPidJmCI-KMJHuyHgGAwAAACaaNNsOAAAAP62OVAEAAADSmzTbDgAAAA; stsservicecookie=estsfd; x-ms-gateway-slice=estsfd");
                request.AddParameter("grant_type", "refresh_token");
                request.AddParameter("refresh_token", refresh_token);
                request.AddParameter("client_id", AppConfiguration.BusinessCentralClientId);
                request.AddParameter("client_secret", AppConfiguration.BusinessCentralClientSecret);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        obj = JsonConvert.DeserializeObject<BCAuthModel>(response.Content);
                    }
                }
                if (obj != null)
                {
                    return obj;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public async Task BCConfirmRefreshToken(Organization organization)
        public async Task BCConfirmRefreshToken(BCAuthModel bCAuthModel)
        {
            try
            {
                //if (organization.AccessTokenExpire == null || organization.AccessTokenExpire <= DateTime.UtcNow.AddMinutes(3))
                //{
                await BusinessCentralRefreshToken(bCAuthModel);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task BusinessCentralRefreshToken(BCAuthModel bCAuthModel)
        {
            try
            {
                var objRefreshToken = await GetBussinessCentralRefreshToken(bCAuthModel.refresh_token);
                if (objRefreshToken != null)
                {
                    if (objRefreshToken != null && !string.IsNullOrEmpty(objRefreshToken.access_token) && !string.IsNullOrEmpty(objRefreshToken.refresh_token))
                    {
                        bCAuthModel.access_token = objRefreshToken.access_token;
                        bCAuthModel.refresh_token = objRefreshToken.refresh_token;
                        _bCAuthService.UpdateAsync(bCAuthModel.id, bCAuthModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region Customer
        public async Task<(BCCustomerList, string)> GetBCCustomerList(string environmentName, string companyId, string accessToken)
        {
            try
            {
                BCCustomerList objCustomers = new();
                string Message = string.Empty;

                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/customers");
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        objCustomers = JsonConvert.DeserializeObject<BCCustomerList>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                if (objCustomers != null)
                {
                    return (objCustomers, Message);
                }
                else
                {
                    return (new BCCustomerList(), Message);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(BCCustomer, string)> GetBCCustomerById(string environmentName, string companyId, string accessToken, string customerId)
        {
            try
            {
                BCCustomer objCustomer = new();
                string Message = string.Empty;
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/customers({customerId})");

                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        objCustomer = JsonConvert.DeserializeObject<BCCustomer>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                if (objCustomer != null)
                {
                    return (objCustomer, Message);
                }
                else
                {
                    return (new BCCustomer(), Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(bool, string)> DeleteBCCustomer(string environmentName, string companyId, string accessToken, string customerId, string eTag)
        {
            try
            {
                bool isSuccess = false;
                string Message = string.Empty;
                var request = new RestRequest(Method.DELETE);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/customers({customerId})");
                request.AddHeader("If-Match", eTag.Replace("\\", ""));
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (isSuccess, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(BCCustomer, string)> CreateBCCustomer(BCCustomerInsertUpdateModel insertUpdateModel, string environmentName, string companyId, string accessToken)
        {
            try
            {
                BCCustomer bCCustomerResponse = new();
                string Message = string.Empty;
                var request = new RestRequest(Method.POST);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/customers");


                request.AddHeader("Authorization", "Bearer " + accessToken);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(insertUpdateModel);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        bCCustomerResponse = JsonConvert.DeserializeObject<BCCustomer>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (bCCustomerResponse, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(BCCustomer, string)> UpdateBCCustomer(BCCustomerInsertUpdateModel insertUpdateModel, string environmentName, string companyId, string accessToken, string customerId, string eTag)
        {
            try
            {
                BCCustomer bCCustomerResponse = new();
                string Message = string.Empty;
                var request = new RestRequest(Method.PATCH);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/customers({customerId})");
                request.AddHeader("If-Match", eTag.Replace("\\", ""));
                request.AddHeader("Authorization", "Bearer " + accessToken);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(insertUpdateModel);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        bCCustomerResponse = JsonConvert.DeserializeObject<BCCustomer>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (bCCustomerResponse, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region Vendor
        public async Task<(BCVendorList, string)> GetBCVendorList(string environmentName, string companyId, string accessToken)
        {
            try
            {
                BCVendorList objVendors = new();
                string Message = string.Empty;

                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/vendors");
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        objVendors = JsonConvert.DeserializeObject<BCVendorList>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                if (objVendors != null)
                {
                    return (objVendors, Message);
                }
                else
                {
                    return (new BCVendorList(), Message);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(BCVendor, string)> GetBCVendorById(string environmentName, string companyId, string accessToken, string vendorId)
        {
            try
            {
                BCVendor objVendor = new();
                string Message = string.Empty;
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/vendors({vendorId})");

                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        objVendor = JsonConvert.DeserializeObject<BCVendor>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                if (objVendor != null)
                {
                    return (objVendor, Message);
                }
                else
                {
                    return (new BCVendor(), Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(bool, string)> DeleteBCVendor(string environmentName, string companyId, string accessToken, string vendorId, string eTag)
        {
            try
            {
                bool isSuccess = false;
                string Message = string.Empty;
                var request = new RestRequest(Method.DELETE);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/vendors({vendorId})");
                request.AddHeader("If-Match", eTag.Replace("\\", ""));
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (isSuccess, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(BCVendor, string)> CreateBCVendor(BCVendorInsertUpdateModel insertUpdateModel, string environmentName, string companyId, string accessToken)
        {
            try
            {
                BCVendor bCVendorResponse = new();
                string Message = string.Empty;
                var request = new RestRequest(Method.POST);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/vendors");


                request.AddHeader("Authorization", "Bearer " + accessToken);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(insertUpdateModel);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        bCVendorResponse = JsonConvert.DeserializeObject<BCVendor>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (bCVendorResponse, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(BCVendor, string)> UpdateBCVendor(BCVendorInsertUpdateModel insertUpdateModel, string environmentName, string companyId, string accessToken, string vendorId, string eTag)
        {
            try
            {
                BCVendor bCVendorResponse = new();
                string Message = string.Empty;
                var request = new RestRequest(Method.PATCH);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/vendors({vendorId})");
                request.AddHeader("If-Match", eTag.Replace("\\", ""));
                request.AddHeader("Authorization", "Bearer " + accessToken);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(insertUpdateModel);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        bCVendorResponse = JsonConvert.DeserializeObject<BCVendor>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (bCVendorResponse, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region Purchase Invoice (Bill)
        #region Purchase Invoice (Bill) Header
        public async Task<(BCPurchaseInvoiceList, string)> GetBCPurchaseInvoiceList(string environmentName, string companyId, string accessToken)
        {
            try
            {
                BCPurchaseInvoiceList objPurchaseInvoices = new();
                string Message = string.Empty;

                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/purchaseInvoices");
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        objPurchaseInvoices = JsonConvert.DeserializeObject<BCPurchaseInvoiceList>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                if (objPurchaseInvoices != null)
                {
                    return (objPurchaseInvoices, Message);
                }
                else
                {
                    return (new BCPurchaseInvoiceList(), Message);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(BCPurchaseInvoice, string)> GetBCPurchaseInvoiceById(string environmentName, string companyId, string accessToken, string purchaseInvoiceId)
        {
            try
            {
                BCPurchaseInvoice objPurchaseInvoice = new();
                string Message = string.Empty;
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/purchaseInvoices({purchaseInvoiceId})");

                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        objPurchaseInvoice = JsonConvert.DeserializeObject<BCPurchaseInvoice>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                if (objPurchaseInvoice != null)
                {
                    return (objPurchaseInvoice, Message);
                }
                else
                {
                    return (new BCPurchaseInvoice(), Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(bool, string)> DeleteBCPurchaseInvoice(string environmentName, string companyId, string accessToken, string purchaseInvoiceId, string eTag)
        {
            try
            {
                bool isSuccess = false;
                string Message = string.Empty;
                var request = new RestRequest(Method.DELETE);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/purchaseInvoices({purchaseInvoiceId})");
                request.AddHeader("If-Match", eTag.Replace("\\", ""));
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (isSuccess, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(BCPurchaseInvoice, string)> CreateBCPurchaseInvoice(BCPurchaseInvoiceInsertUpdateModel insertUpdateModel, string environmentName, string companyId, string accessToken)
        {
            try
            {
                BCPurchaseInvoice bCPurchaseInvoiceResponse = new();
                string Message = string.Empty;
                var request = new RestRequest(Method.POST);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/purchaseInvoices");


                request.AddHeader("Authorization", "Bearer " + accessToken);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(insertUpdateModel);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        bCPurchaseInvoiceResponse = JsonConvert.DeserializeObject<BCPurchaseInvoice>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (bCPurchaseInvoiceResponse, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(BCPurchaseInvoice, string)> UpdateBCPurchaseInvoice(BCPurchaseInvoiceInsertUpdateModel insertUpdateModel, string environmentName, string companyId, string accessToken, string purchaseInvoiceId, string eTag)
        {
            try
            {
                BCPurchaseInvoice bCPurchaseInvoiceResponse = new();
                string Message = string.Empty;
                var request = new RestRequest(Method.PATCH);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/purchaseInvoices({purchaseInvoiceId})");
                request.AddHeader("If-Match", eTag.Replace("\\", ""));
                request.AddHeader("Authorization", "Bearer " + accessToken);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(insertUpdateModel);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        bCPurchaseInvoiceResponse = JsonConvert.DeserializeObject<BCPurchaseInvoice>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (bCPurchaseInvoiceResponse, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Purchase Invoice (Bill) Line
        public async Task<(BCPurchaseInvoiceLineList, string)> GetBCPurchaseInvoiceLineList(string environmentName, string companyId, string accessToken, string purchaseInvoiceId)
        {
            try
            {
                BCPurchaseInvoiceLineList objPurchaseInvoiceLines = new();
                string Message = string.Empty;

                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/purchaseInvoices({purchaseInvoiceId})/purchaseInvoiceLines");
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        objPurchaseInvoiceLines = JsonConvert.DeserializeObject<BCPurchaseInvoiceLineList>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                if (objPurchaseInvoiceLines != null)
                {
                    return (objPurchaseInvoiceLines, Message);
                }
                else
                {
                    return (new BCPurchaseInvoiceLineList(), Message);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(BCPurchaseInvoiceLine, string)> GetBCPurchaseInvoiceLineById(string environmentName, string companyId, string accessToken, string purchaseInvoiceId, string PurchaseInvoiceLineId)
        {
            try
            {
                BCPurchaseInvoiceLine objPurchaseInvoiceLine = new();
                string Message = string.Empty;
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/purchaseInvoices({purchaseInvoiceId})/purchaseInvoiceLines({PurchaseInvoiceLineId})");

                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        objPurchaseInvoiceLine = JsonConvert.DeserializeObject<BCPurchaseInvoiceLine>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                if (objPurchaseInvoiceLine != null)
                {
                    return (objPurchaseInvoiceLine, Message);
                }
                else
                {
                    return (new BCPurchaseInvoiceLine(), Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(bool, string)> DeleteBCPurchaseInvoiceLine(string environmentName, string companyId, string accessToken, string purchaseInvoiceId, string PurchaseInvoiceLineId, string eTag)
        {
            try
            {
                bool isSuccess = false;
                string Message = string.Empty;
                var request = new RestRequest(Method.DELETE);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/purchaseInvoices({purchaseInvoiceId})/purchaseInvoiceLines({PurchaseInvoiceLineId})");
                request.AddHeader("If-Match", eTag.Replace("\\", ""));
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (isSuccess, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(BCPurchaseInvoiceLine, string)> CreateBCPurchaseInvoiceLine(BCPurchaseInvoiceLineInsertUpdateModel insertUpdateModel, string environmentName, string companyId, string accessToken, string purchaseInvoiceId)
        {
            try
            {
                BCPurchaseInvoiceLine bCPurchaseInvoiceLineResponse = new();
                string Message = string.Empty;
                var request = new RestRequest(Method.POST);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/purchaseInvoices({purchaseInvoiceId})/purchaseInvoiceLines");


                request.AddHeader("Authorization", "Bearer " + accessToken);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(insertUpdateModel);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        bCPurchaseInvoiceLineResponse = JsonConvert.DeserializeObject<BCPurchaseInvoiceLine>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (bCPurchaseInvoiceLineResponse, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(BCPurchaseInvoiceLine, string)> UpdateBCPurchaseInvoiceLine(BCPurchaseInvoiceLineInsertUpdateModel insertUpdateModel, string environmentName, string companyId, string accessToken, string purchaseInvoiceId, string PurchaseInvoiceLineId, string eTag)
        {
            try
            {
                BCPurchaseInvoiceLine bCPurchaseInvoiceLineResponse = new();
                string Message = string.Empty;
                var request = new RestRequest(Method.PATCH);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/purchaseInvoices({purchaseInvoiceId})/purchaseInvoiceLines({PurchaseInvoiceLineId})");
                request.AddHeader("If-Match", eTag.Replace("\\", ""));
                request.AddHeader("Authorization", "Bearer " + accessToken);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(insertUpdateModel);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        bCPurchaseInvoiceLineResponse = JsonConvert.DeserializeObject<BCPurchaseInvoiceLine>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (bCPurchaseInvoiceLineResponse, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #endregion



        #region Sales Invoice (Invoice)
        #region Sales Invoice (Invoice) Header
        public async Task<(BCSalesInvoiceList, string)> GetBCSalesInvoiceList(string environmentName, string companyId, string accessToken)
        {
            try
            {
                BCSalesInvoiceList objSalesInvoices = new();
                string Message = string.Empty;

                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/salesInvoices");
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        objSalesInvoices = JsonConvert.DeserializeObject<BCSalesInvoiceList>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                if (objSalesInvoices != null)
                {
                    return (objSalesInvoices, Message);
                }
                else
                {
                    return (new BCSalesInvoiceList(), Message);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(BCSalesInvoice, string)> GetBCSalesInvoiceById(string environmentName, string companyId, string accessToken, string salesInvoiceId)
        {
            try
            {
                BCSalesInvoice objSalesInvoice = new();
                string Message = string.Empty;
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/salesInvoices({salesInvoiceId})");

                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        objSalesInvoice = JsonConvert.DeserializeObject<BCSalesInvoice>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                if (objSalesInvoice != null)
                {
                    return (objSalesInvoice, Message);
                }
                else
                {
                    return (new BCSalesInvoice(), Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(bool, string)> DeleteBCSalesInvoice(string environmentName, string companyId, string accessToken, string salesInvoiceId, string eTag)
        {
            try
            {
                bool isSuccess = false;
                string Message = string.Empty;
                var request = new RestRequest(Method.DELETE);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/salesInvoices({salesInvoiceId})");
                request.AddHeader("If-Match", eTag.Replace("\\", ""));
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (isSuccess, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(BCSalesInvoice, string)> CreateBCSalesInvoice(BCSalesInvoiceInsertUpdateModel insertUpdateModel, string environmentName, string companyId, string accessToken)
        {
            try
            {
                BCSalesInvoice bCSalesInvoiceResponse = new();
                string Message = string.Empty;
                var request = new RestRequest(Method.POST);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/salesInvoices");


                request.AddHeader("Authorization", "Bearer " + accessToken);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(insertUpdateModel);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        bCSalesInvoiceResponse = JsonConvert.DeserializeObject<BCSalesInvoice>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (bCSalesInvoiceResponse, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(BCSalesInvoice, string)> UpdateBCSalesInvoice(BCSalesInvoiceInsertUpdateModel insertUpdateModel, string environmentName, string companyId, string accessToken, string salesInvoiceId, string eTag)
        {
            try
            {
                BCSalesInvoice bCSalesInvoiceResponse = new();
                string Message = string.Empty;
                var request = new RestRequest(Method.PATCH);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/salesInvoices({salesInvoiceId})");
                request.AddHeader("If-Match", eTag.Replace("\\", ""));
                request.AddHeader("Authorization", "Bearer " + accessToken);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(insertUpdateModel);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        bCSalesInvoiceResponse = JsonConvert.DeserializeObject<BCSalesInvoice>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (bCSalesInvoiceResponse, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Sales Invoice (Invoice) Line
        public async Task<(BCSalesInvoiceLineList, string)> GetBCSalesInvoiceLineList(string environmentName, string companyId, string accessToken, string salesInvoiceId)
        {
            try
            {
                BCSalesInvoiceLineList objSalesInvoiceLines = new();
                string Message = string.Empty;

                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/salesInvoices({salesInvoiceId})/salesInvoiceLines");
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        objSalesInvoiceLines = JsonConvert.DeserializeObject<BCSalesInvoiceLineList>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                if (objSalesInvoiceLines != null)
                {
                    return (objSalesInvoiceLines, Message);
                }
                else
                {
                    return (new BCSalesInvoiceLineList(), Message);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(BCSalesInvoiceLine, string)> GetBCSalesInvoiceLineById(string environmentName, string companyId, string accessToken, string salesInvoiceId, string salesInvoiceLineId)
        {
            try
            {
                BCSalesInvoiceLine objSalesInvoiceLine = new();
                string Message = string.Empty;
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/salesInvoices({salesInvoiceId})/salesInvoiceLines({salesInvoiceLineId})");

                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        objSalesInvoiceLine = JsonConvert.DeserializeObject<BCSalesInvoiceLine>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                if (objSalesInvoiceLine != null)
                {
                    return (objSalesInvoiceLine, Message);
                }
                else
                {
                    return (new BCSalesInvoiceLine(), Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(bool, string)> DeleteBCSalesInvoiceLine(string environmentName, string companyId, string accessToken, string salesInvoiceId, string salesInvoiceLineId, string eTag)
        {
            try
            {
                bool isSuccess = false;
                string Message = string.Empty;
                var request = new RestRequest(Method.DELETE);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/salesInvoices({salesInvoiceId})/salesInvoiceLines({salesInvoiceLineId})");
                request.AddHeader("If-Match", eTag.Replace("\\", ""));
                request.AddHeader("Authorization", "Bearer " + accessToken);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (isSuccess, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(BCSalesInvoiceLine, string)> CreateBCSalesInvoiceLine(BCSalesInvoiceLineInsertUpdateModel insertUpdateModel, string environmentName, string companyId, string accessToken, string salesInvoiceId)
        {
            try
            {
                BCSalesInvoiceLine bCSalesInvoiceLineResponse = new();
                string Message = string.Empty;
                var request = new RestRequest(Method.POST);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/salesInvoices({salesInvoiceId})/salesInvoiceLines");


                request.AddHeader("Authorization", "Bearer " + accessToken);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(insertUpdateModel);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        bCSalesInvoiceLineResponse = JsonConvert.DeserializeObject<BCSalesInvoiceLine>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (bCSalesInvoiceLineResponse, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(BCSalesInvoiceLine, string)> UpdateBCSalesInvoiceLine(BCSalesInvoiceLineInsertUpdateModel insertUpdateModel, string environmentName, string companyId, string accessToken, string salesInvoiceId, string salesInvoiceLineId, string eTag)
        {
            try
            {
                BCSalesInvoiceLine bCSalesInvoiceLineResponse = new();
                string Message = string.Empty;
                var request = new RestRequest(Method.PATCH);
                var client = new RestClient($"https://api.businesscentral.dynamics.com/v2.0/{environmentName}/api/v2.0/companies({companyId})/salesInvoices({salesInvoiceId})/salesInvoiceLines({salesInvoiceLineId})");
                request.AddHeader("If-Match", eTag.Replace("\\", ""));
                request.AddHeader("Authorization", "Bearer " + accessToken);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(insertUpdateModel);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        bCSalesInvoiceLineResponse = JsonConvert.DeserializeObject<BCSalesInvoiceLine>(response.Content);
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (bCSalesInvoiceLineResponse, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #endregion
    }
}
