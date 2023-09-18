using Newtonsoft.Json;
using OrgGoalsBCAndNetsuiteCrud.Models;
using RestSharp;
using RestSharp.Authenticators.OAuth;
using RestSharp.Authenticators;
using System;
using System.Security.Cryptography;
using System.Text;
using NetSuiteSoapAPIServices;

namespace OrgGoalsBCAndNetsuiteCrud.Core
{
    public class NetSuiteClients
    {
        #region Customer
        public async Task<(List<NetsuiteCustomerItem>, string)> GetNetSuiteCustomerList(NetSuiteAuthModel authModel, List<NetsuiteCustomerItem> netsuiteCustomers, int limit = 200, int offset = 0)
        {
            try
            {
                string Message = string.Empty;
                string query = @"{""q"":""select * from Customer""}";
                //if (CustomerLastSyncDate != null)
                //{
                //    query += $" where accttype = 'Expense' AND lastmodifieddate >= '{CustomerLastSyncDate}' AND lastmodifieddate <= '{DateTime.UtcNow.AddDays(1):dd/MM/yyyy}' ";
                //}
                //else
                //{
                //    query += $" where accttype = 'Expense' ";
                //}
                //query += @" ""}";

                var url = $"{authModel.nsAPIBaseURL}/services/rest/query/v1/suiteql";
                var restClient = CreateRestClient(authModel, url);
                restClient.AddDefaultHeader("Prefer", "transient");
                restClient.AddDefaultQueryParameter("limit", limit.ToString());
                restClient.AddDefaultQueryParameter("offset", offset.ToString());
                var request = new RestRequest(Method.POST);
                request.AddJsonBody(query);
                var response = await restClient.ExecuteAsync(request);

                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var objResponse = JsonConvert.DeserializeObject<NetsuiteCustomerModel>(response.Content.ToString());
                        if (objResponse != null && objResponse.items != null && objResponse.items.Any())
                        {
                            netsuiteCustomers.AddRange(objResponse.items);
                            if (objResponse.totalResults > netsuiteCustomers.Count)
                            {
                                await GetNetSuiteCustomerList(authModel, netsuiteCustomers, limit, offset + limit);
                            }
                        }
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (netsuiteCustomers, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(Customer, string)> GetNetSuiteCustomerById(NetSuiteAuthModel authModel, string customerId)
        {
            try
            {

                string Message = string.Empty;
                string resCustomerId = string.Empty;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
                NetSuitePortTypeClient _service = new();
                _service.Endpoint.Address = new System.ServiceModel.EndpointAddress($"{authModel.nsAPIBaseURL}/services/NetSuitePort_2022_2");
                string nonce = GenerateNonce();
                long timestamp = GenerateTimestamp();
                string signature = GenerateSignature(authModel.nsAccountId, authModel.nsConsumerKey, authModel.nsTokenId, nonce, timestamp, authModel.nsConsumerSecret, authModel.nsTokenSecret);
                TokenPassport tokenPassport = new()
                {
                    account = authModel.nsAccountId,
                    consumerKey = authModel.nsConsumerKey,
                    token = authModel.nsTokenId,
                    nonce = nonce,
                    timestamp = timestamp,
                    signature = new TokenPassportSignature() { algorithm = "HMAC_SHA256", Value = signature },
                };

                RecordRef baseRef = new()
                {
                    internalId = customerId,
                    type = RecordType.customer,
                    typeSpecified = true
                };
                var response = await _service.getAsync(tokenPassport, new ApplicationInfo(), new PartnerInfo(), new NetSuiteSoapAPIServices.Preferences(), baseRef);

                var objResponse = new Customer();
                if (response != null && response.readResponse != null && response.readResponse.status != null)
                {
                    if (response.readResponse.status.isSuccess)
                    {
                        if (response.readResponse.record != null)
                        {
                            objResponse = (Customer)response.readResponse.record;
                        }
                    }
                    else
                    {
                        objResponse = null;
                    }
                }
                if (objResponse != null)
                {
                    return (objResponse, Message);
                }
                else
                {
                    return (null, Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(bool, string)> DeleteNetSuiteCustomer(NetSuiteAuthModel authModel, string customerId)
        {
            try
            {
                bool isSuccess = false;
                string Message = string.Empty;
                var url = $"{authModel.nsAPIBaseURL}/services/rest/record/v1/customer/{customerId}";
                var request = new RestRequest(Method.DELETE);
                var restClient = CreateRestClient(authModel, url);
                var response = await restClient.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent || response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created)
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

        public async Task<(string, string)> CreateNetSuiteCustomer(NetSuiteAuthModel authModel, Customer customer)
        {
            try
            {
                string Message = string.Empty;
                string resCustomerId = string.Empty;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
                NetSuitePortTypeClient _service = new();
                _service.Endpoint.Address = new System.ServiceModel.EndpointAddress($"{authModel.nsAPIBaseURL}/services/NetSuitePort_2022_2");
                string nonce = GenerateNonce();
                long timestamp = GenerateTimestamp();
                string signature = GenerateSignature(authModel.nsAccountId, authModel.nsConsumerKey, authModel.nsTokenId, nonce, timestamp, authModel.nsConsumerSecret, authModel.nsTokenSecret);
                TokenPassport tokenPassport = new()
                {
                    account = authModel.nsAccountId,
                    consumerKey = authModel.nsConsumerKey,
                    token = authModel.nsTokenId,
                    nonce = nonce,
                    timestamp = timestamp,
                    signature = new TokenPassportSignature() { algorithm = "HMAC_SHA256", Value = signature },
                };
                var response = await _service.addAsync(tokenPassport, new ApplicationInfo(), new PartnerInfo(), new NetSuiteSoapAPIServices.Preferences(), customer);

                if (response != null && response.writeResponse != null)
                {
                    if (response.writeResponse.status != null)
                    {
                        if (response.writeResponse.status.isSuccess)
                        {
                            if (response.writeResponse.baseRef != null)
                            {
                                resCustomerId = ((RecordRef)response.writeResponse.baseRef).internalId;
                            }
                        }
                        else
                        {
                            if (response.writeResponse.status.statusDetail != null && response.writeResponse.status.statusDetail.Any())
                            {
                                var statusDetail = response.writeResponse.status.statusDetail[0];
                                Message = statusDetail.message;
                            }
                        }
                    }
                }
                return (resCustomerId, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(string, string)> UpdateNetSuiteCustomer(NetSuiteAuthModel authModel, Customer customer)
        {
            try
            {
                string Message = string.Empty;
                string resCustomerId = string.Empty;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
                NetSuitePortTypeClient _service = new();
                _service.Endpoint.Address = new System.ServiceModel.EndpointAddress($"{authModel.nsAPIBaseURL}/services/NetSuitePort_2022_2");
                string nonce = GenerateNonce();
                long timestamp = GenerateTimestamp();
                string signature = GenerateSignature(authModel.nsAccountId, authModel.nsConsumerKey, authModel.nsTokenId, nonce, timestamp, authModel.nsConsumerSecret, authModel.nsTokenSecret);
                TokenPassport tokenPassport = new()
                {
                    account = authModel.nsAccountId,
                    consumerKey = authModel.nsConsumerKey,
                    token = authModel.nsTokenId,
                    nonce = nonce,
                    timestamp = timestamp,
                    signature = new TokenPassportSignature() { algorithm = "HMAC_SHA256", Value = signature },
                };

                var response = await _service.updateAsync(tokenPassport, new ApplicationInfo(), new PartnerInfo(), new NetSuiteSoapAPIServices.Preferences(), customer);


                if (response != null && response.writeResponse != null)
                {
                    if (response.writeResponse.status != null)
                    {
                        if (response.writeResponse.status.isSuccess)
                        {
                            if (response.writeResponse.baseRef != null)
                            {
                                resCustomerId = ((RecordRef)response.writeResponse.baseRef).internalId;
                            }
                        }
                        else
                        {
                            if (response.writeResponse.status.statusDetail != null && response.writeResponse.status.statusDetail.Any())
                            {
                                var statusDetail = response.writeResponse.status.statusDetail[0];
                                Message = statusDetail.message;
                            }
                        }
                    }
                }
                return (resCustomerId, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion



        #region Vendor
        public async Task<(List<NetsuiteVendorItem>, string)> GetNetSuiteVendorList(NetSuiteAuthModel authModel, List<NetsuiteVendorItem> netsuiteVendors, int limit = 200, int offset = 0)
        {
            try
            {
                string Message = string.Empty;
                string query = @"{""q"":""select v.*,(select LISTAGG(sub.subsidiary,',') from vendorsubsidiaryrelationship sub where sub.entity = v.id) as subsidiary,C.symbol,add.addr1,add.addr2,add.city,add.state,add.zip,add.country from vendor v left join vendoraddressbookentityaddress add on v.defaultbillingaddress = add.nkey left join currency C on C.id = v.currency""}";
                //if (VendorLastSyncDate != null)
                //{
                //    query += $" where accttype = 'Expense' AND lastmodifieddate >= '{VendorLastSyncDate}' AND lastmodifieddate <= '{DateTime.UtcNow.AddDays(1):dd/MM/yyyy}' ";
                //}
                //else
                //{
                //    query += $" where accttype = 'Expense' ";
                //}
                //query += @" ""}";

                var url = $"{authModel.nsAPIBaseURL}/services/rest/query/v1/suiteql";
                var restClient = CreateRestClient(authModel, url);
                restClient.AddDefaultHeader("Prefer", "transient");
                restClient.AddDefaultQueryParameter("limit", limit.ToString());
                restClient.AddDefaultQueryParameter("offset", offset.ToString());
                var request = new RestRequest(Method.POST);
                request.AddJsonBody(query);
                var response = await restClient.ExecuteAsync(request);

                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var objResponse = JsonConvert.DeserializeObject<NetsuiteVendorModel>(response.Content.ToString());
                        if (objResponse != null && objResponse.items != null && objResponse.items.Any())
                        {
                            netsuiteVendors.AddRange(objResponse.items);
                            if (objResponse.totalResults > netsuiteVendors.Count)
                            {
                                await GetNetSuiteVendorList(authModel, netsuiteVendors, limit, offset + limit);
                            }
                        }
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                return (netsuiteVendors, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(Vendor, string)> GetNetSuiteVendorById(NetSuiteAuthModel authModel, string vendorId)
        {
            try
            {

                string Message = string.Empty;
                string resVendorId = string.Empty;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
                NetSuitePortTypeClient _service = new();
                _service.Endpoint.Address = new System.ServiceModel.EndpointAddress($"{authModel.nsAPIBaseURL}/services/NetSuitePort_2022_2");
                string nonce = GenerateNonce();
                long timestamp = GenerateTimestamp();
                string signature = GenerateSignature(authModel.nsAccountId, authModel.nsConsumerKey, authModel.nsTokenId, nonce, timestamp, authModel.nsConsumerSecret, authModel.nsTokenSecret);
                TokenPassport tokenPassport = new()
                {
                    account = authModel.nsAccountId,
                    consumerKey = authModel.nsConsumerKey,
                    token = authModel.nsTokenId,
                    nonce = nonce,
                    timestamp = timestamp,
                    signature = new TokenPassportSignature() { algorithm = "HMAC_SHA256", Value = signature },
                };

                RecordRef baseRef = new()
                {
                    internalId = vendorId,
                    type = RecordType.vendor,
                    typeSpecified = true
                };
                var response = await _service.getAsync(tokenPassport, new ApplicationInfo(), new PartnerInfo(), new NetSuiteSoapAPIServices.Preferences(), baseRef);

                var objResponse = new Vendor();
                if (response != null && response.readResponse != null && response.readResponse.status != null)
                {
                    if (response.readResponse.status.isSuccess)
                    {
                        if (response.readResponse.record != null)
                        {
                            objResponse = (Vendor)response.readResponse.record;
                        }
                    }
                    else
                    {
                        objResponse = null;
                    }
                }
                if (objResponse != null)
                {
                    return (objResponse, Message);
                }
                else
                {
                    return (null, Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(bool, string)> DeleteNetSuiteVendor(NetSuiteAuthModel authModel, string vendorId)
        {
            try
            {
                bool isSuccess = false;
                string Message = string.Empty;
                var url = $"{authModel.nsAPIBaseURL}/services/rest/record/v1/Vendor/{vendorId}";
                var request = new RestRequest(Method.DELETE);
                var restClient = CreateRestClient(authModel, url);
                var response = await restClient.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent || response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created)
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

        public async Task<(string, string)> CreateNetSuiteVendor(NetSuiteAuthModel authModel, Vendor vendor)
        {
            try
            {
                string Message = string.Empty;
                string resVendorId = string.Empty;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
                NetSuitePortTypeClient _service = new();
                _service.Endpoint.Address = new System.ServiceModel.EndpointAddress($"{authModel.nsAPIBaseURL}/services/NetSuitePort_2022_2");
                string nonce = GenerateNonce();
                long timestamp = GenerateTimestamp();
                string signature = GenerateSignature(authModel.nsAccountId, authModel.nsConsumerKey, authModel.nsTokenId, nonce, timestamp, authModel.nsConsumerSecret, authModel.nsTokenSecret);
                TokenPassport tokenPassport = new()
                {
                    account = authModel.nsAccountId,
                    consumerKey = authModel.nsConsumerKey,
                    token = authModel.nsTokenId,
                    nonce = nonce,
                    timestamp = timestamp,
                    signature = new TokenPassportSignature() { algorithm = "HMAC_SHA256", Value = signature },
                };
                var response = await _service.addAsync(tokenPassport, new ApplicationInfo(), new PartnerInfo(), new NetSuiteSoapAPIServices.Preferences(), vendor);

                if (response != null && response.writeResponse != null)
                {
                    if (response.writeResponse.status != null)
                    {
                        if (response.writeResponse.status.isSuccess)
                        {
                            if (response.writeResponse.baseRef != null)
                            {
                                resVendorId = ((RecordRef)response.writeResponse.baseRef).internalId;
                            }
                        }
                        else
                        {
                            if (response.writeResponse.status.statusDetail != null && response.writeResponse.status.statusDetail.Any())
                            {
                                var statusDetail = response.writeResponse.status.statusDetail[0];
                                Message = statusDetail.message;
                            }
                        }
                    }
                }
                return (resVendorId, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(string, string)> UpdateNetSuiteVendor(NetSuiteAuthModel authModel, Vendor vendor)
        {
            try
            {
                string Message = string.Empty;
                string resVendorId = string.Empty;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
                NetSuitePortTypeClient _service = new();
                _service.Endpoint.Address = new System.ServiceModel.EndpointAddress($"{authModel.nsAPIBaseURL}/services/NetSuitePort_2022_2");
                string nonce = GenerateNonce();
                long timestamp = GenerateTimestamp();
                string signature = GenerateSignature(authModel.nsAccountId, authModel.nsConsumerKey, authModel.nsTokenId, nonce, timestamp, authModel.nsConsumerSecret, authModel.nsTokenSecret);
                TokenPassport tokenPassport = new()
                {
                    account = authModel.nsAccountId,
                    consumerKey = authModel.nsConsumerKey,
                    token = authModel.nsTokenId,
                    nonce = nonce,
                    timestamp = timestamp,
                    signature = new TokenPassportSignature() { algorithm = "HMAC_SHA256", Value = signature },
                };

                var response = await _service.updateAsync(tokenPassport, new ApplicationInfo(), new PartnerInfo(), new NetSuiteSoapAPIServices.Preferences(), vendor);


                if (response != null && response.writeResponse != null)
                {
                    if (response.writeResponse.status != null)
                    {
                        if (response.writeResponse.status.isSuccess)
                        {
                            if (response.writeResponse.baseRef != null)
                            {
                                resVendorId = ((RecordRef)response.writeResponse.baseRef).internalId;
                            }
                        }
                        else
                        {
                            if (response.writeResponse.status.statusDetail != null && response.writeResponse.status.statusDetail.Any())
                            {
                                var statusDetail = response.writeResponse.status.statusDetail[0];
                                Message = statusDetail.message;
                            }
                        }
                    }
                }
                return (resVendorId, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region VendorBill
        public async Task<(List<NetsuiteVendorBillItemId>, string)> GetNetSuiteVendorBillList(NetSuiteAuthModel authModel, List<NetsuiteVendorBillItemId> netsuiteVendorBills, int limit = 100, int offset = 0)
        {
            try
            {
                bool isProcess = false;
                List<NetsuiteVendorBillItemId> netsuiteVendorBillIds = new();
                string Message = string.Empty;
                var url = $"{authModel.nsAPIBaseURL}/services/rest/record/v1/vendorBill";
                var request = new RestRequest(Method.GET);
                request.AddParameter("offset", offset.ToString());
                request.AddParameter("limit", limit.ToString());

                var restClient = CreateRestClient(authModel, url);
                var response = await restClient.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var objResponse = JsonConvert.DeserializeObject<NetsuiteVendorBillModel>(response.Content.ToString());
                        if (objResponse != null && objResponse.items != null && objResponse.items.Any())
                        {
                            netsuiteVendorBills.AddRange(objResponse.items);
                            if (objResponse.totalResults > netsuiteVendorBills.Count)
                            {
                                await GetNetSuiteVendorBillList(authModel, netsuiteVendorBills, limit, offset + limit);
                            }
                        }
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }
                
                return (netsuiteVendorBills, Message);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<VendorBill>> SoapAPI_GetNetsuiteVendorBillListAsync(NetSuiteAuthModel authModel, List<NetsuiteVendorBillItemId> netsuiteVendorBills)
        {
            var objResponse = new List<ReadResponse>();
            List<VendorBill> vendorBills = new();
            try
            {
                List<RecordRef> recordRefList = new();
                foreach (var vendorBillItemId in netsuiteVendorBills)
                {
                    RecordRef recordRef = new()
                    {
                        type = RecordType.vendorBill,
                        typeSpecified = true,
                        internalId = vendorBillItemId.id
                    };
                    recordRefList.Add(recordRef);
                }
                if (recordRefList.Any())
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
                    NetSuitePortTypeClient _service = new();
                    _service.Endpoint.Address = new System.ServiceModel.EndpointAddress($"{authModel.nsAPIBaseURL}/services/NetSuitePort_2022_2");
                    string nonce = GenerateNonce();
                    long timestamp = GenerateTimestamp();
                    string signature = GenerateSignature(authModel.nsAccountId, authModel.nsConsumerKey, authModel.nsTokenId, nonce, timestamp, authModel.nsConsumerSecret, authModel.nsTokenSecret);
                    TokenPassport tokenPassport = new()
                    {
                        account = authModel.nsAccountId,
                        consumerKey = authModel.nsConsumerKey,
                        token = authModel.nsTokenId,
                        nonce = nonce,
                        timestamp = timestamp,
                        signature = new TokenPassportSignature() { algorithm = "HMAC_SHA256", Value = signature },
                    };

                    var response = await _service.getListAsync(tokenPassport, new ApplicationInfo(), new PartnerInfo(), new NetSuiteSoapAPIServices.Preferences(), recordRefList.ToArray());

                    if (response != null && response.readResponseList != null && response.readResponseList.status != null)
                    {
                        if (response.readResponseList.status.isSuccess)
                        {
                            if (response.readResponseList.readResponse != null && response.readResponseList.readResponse.Any())
                            {
                                objResponse = response.readResponseList.readResponse.ToList();
                                foreach (var item in objResponse)
                                {
                                    vendorBills.Add((VendorBill)item.record);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vendorBills;
        }

        public async Task<(VendorBill, string)> GetNetSuiteVendorBillById(NetSuiteAuthModel authModel, string vendorBillId)
        {
            try
            {
                string Message = string.Empty;
                string resVendorBillId = string.Empty;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
                NetSuitePortTypeClient _service = new();
                _service.Endpoint.Address = new System.ServiceModel.EndpointAddress($"{authModel.nsAPIBaseURL}/services/NetSuitePort_2022_2");
                string nonce = GenerateNonce();
                long timestamp = GenerateTimestamp();
                string signature = GenerateSignature(authModel.nsAccountId, authModel.nsConsumerKey, authModel.nsTokenId, nonce, timestamp, authModel.nsConsumerSecret, authModel.nsTokenSecret);
                TokenPassport tokenPassport = new()
                {
                    account = authModel.nsAccountId,
                    consumerKey = authModel.nsConsumerKey,
                    token = authModel.nsTokenId,
                    nonce = nonce,
                    timestamp = timestamp,
                    signature = new TokenPassportSignature() { algorithm = "HMAC_SHA256", Value = signature },
                };

                RecordRef baseRef = new()
                {
                    internalId = vendorBillId,
                    type = RecordType.vendorBill,
                    typeSpecified = true
                };
                var response = await _service.getAsync(tokenPassport, new ApplicationInfo(), new PartnerInfo(), new NetSuiteSoapAPIServices.Preferences(), baseRef);

                var objResponse = new VendorBill();
                if (response != null && response.readResponse != null && response.readResponse.status != null)
                {
                    if (response.readResponse.status.isSuccess)
                    {
                        if (response.readResponse.record != null)
                        {
                            objResponse = (VendorBill)response.readResponse.record;
                        }
                    }
                    else
                    {
                        objResponse = null;
                    }
                }
                if (objResponse != null)
                {
                    return (objResponse, Message);
                }
                else
                {
                    return (null, Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(bool, string)> DeleteNetSuiteVendorBill(NetSuiteAuthModel authModel, string vendorBillId)
        {
            try
            {
                bool isSuccess = false;
                string Message = string.Empty;
                var url = $"{authModel.nsAPIBaseURL}/services/rest/record/v1/VendorBill/{vendorBillId}";
                var request = new RestRequest(Method.DELETE);
                var restClient = CreateRestClient(authModel, url);
                var response = await restClient.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent || response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created)
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

        public async Task<(string, string)> CreateNetSuiteVendorBill(NetSuiteAuthModel authModel, VendorBill vendorBill)
        {
            try
            {
                string Message = string.Empty;
                string resVendorBillId = string.Empty;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
                NetSuitePortTypeClient _service = new();
                _service.Endpoint.Address = new System.ServiceModel.EndpointAddress($"{authModel.nsAPIBaseURL}/services/NetSuitePort_2022_2");
                string nonce = GenerateNonce();
                long timestamp = GenerateTimestamp();
                string signature = GenerateSignature(authModel.nsAccountId, authModel.nsConsumerKey, authModel.nsTokenId, nonce, timestamp, authModel.nsConsumerSecret, authModel.nsTokenSecret);
                TokenPassport tokenPassport = new()
                {
                    account = authModel.nsAccountId,
                    consumerKey = authModel.nsConsumerKey,
                    token = authModel.nsTokenId,
                    nonce = nonce,
                    timestamp = timestamp,
                    signature = new TokenPassportSignature() { algorithm = "HMAC_SHA256", Value = signature },
                };
                var response = await _service.addAsync(tokenPassport, new ApplicationInfo(), new PartnerInfo(), new NetSuiteSoapAPIServices.Preferences(), vendorBill);

                if (response != null && response.writeResponse != null)
                {
                    if (response.writeResponse.status != null)
                    {
                        if (response.writeResponse.status.isSuccess)
                        {
                            if (response.writeResponse.baseRef != null)
                            {
                                resVendorBillId = ((RecordRef)response.writeResponse.baseRef).internalId;
                            }
                        }
                        else
                        {
                            if (response.writeResponse.status.statusDetail != null && response.writeResponse.status.statusDetail.Any())
                            {
                                var statusDetail = response.writeResponse.status.statusDetail[0];
                                Message = statusDetail.message;
                            }
                        }
                    }
                }
                return (resVendorBillId, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(string, string)> UpdateNetSuiteVendorBill(NetSuiteAuthModel authModel, VendorBill vendorBill)
        {
            try
            {
                string Message = string.Empty;
                string resVendorBillId = string.Empty;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
                NetSuitePortTypeClient _service = new();
                _service.Endpoint.Address = new System.ServiceModel.EndpointAddress($"{authModel.nsAPIBaseURL}/services/NetSuitePort_2022_2");
                string nonce = GenerateNonce();
                long timestamp = GenerateTimestamp();
                string signature = GenerateSignature(authModel.nsAccountId, authModel.nsConsumerKey, authModel.nsTokenId, nonce, timestamp, authModel.nsConsumerSecret, authModel.nsTokenSecret);
                TokenPassport tokenPassport = new()
                {
                    account = authModel.nsAccountId,
                    consumerKey = authModel.nsConsumerKey,
                    token = authModel.nsTokenId,
                    nonce = nonce,
                    timestamp = timestamp,
                    signature = new TokenPassportSignature() { algorithm = "HMAC_SHA256", Value = signature },
                };

                var response = await _service.updateAsync(tokenPassport, new ApplicationInfo(), new PartnerInfo(), new NetSuiteSoapAPIServices.Preferences(), vendorBill);


                if (response != null && response.writeResponse != null)
                {
                    if (response.writeResponse.status != null)
                    {
                        if (response.writeResponse.status.isSuccess)
                        {
                            if (response.writeResponse.baseRef != null)
                            {
                                resVendorBillId = ((RecordRef)response.writeResponse.baseRef).internalId;
                            }
                        }
                        else
                        {
                            if (response.writeResponse.status.statusDetail != null && response.writeResponse.status.statusDetail.Any())
                            {
                                var statusDetail = response.writeResponse.status.statusDetail[0];
                                Message = statusDetail.message;
                            }
                        }
                    }
                }
                return (resVendorBillId, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region Invoice
        public async Task<(List<NetsuiteInvoiceItemId>, string)> GetNetSuiteInvoiceList(NetSuiteAuthModel authModel, List<NetsuiteInvoiceItemId> netsuiteInvoices, int limit = 10, int offset = 0)
        {
            try
            {
                bool isProcess = false;
                List<NetsuiteInvoiceItemId> netsuiteInvoiceIds = new();
                string Message = string.Empty;
                var url = $"{authModel.nsAPIBaseURL}/services/rest/record/v1/invoice";
                var request = new RestRequest(Method.GET);
                request.AddParameter("offset", offset.ToString());
                request.AddParameter("limit", limit.ToString());

                var restClient = CreateRestClient(authModel, url);
                var response = await restClient.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var objResponse = JsonConvert.DeserializeObject<NetsuiteInvoiceModel>(response.Content.ToString());
                        if (objResponse != null && objResponse.items != null && objResponse.items.Any())
                        {
                            netsuiteInvoices.AddRange(objResponse.items);
                            //if (objResponse.totalResults > netsuiteInvoices.Count)
                            //{
                            //    await GetNetSuiteInvoiceList(authModel, netsuiteInvoices, limit, offset + limit);
                            //}
                        }
                    }
                    else
                    {
                        Message = response.Content;
                    }
                }

                return (netsuiteInvoices, Message);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Invoice>> SoapAPI_GetNetsuiteInvoiceListAsync(NetSuiteAuthModel authModel, List<NetsuiteInvoiceItemId> netsuiteInvoices)
        {
            var objResponse = new List<ReadResponse>();
            List<Invoice> Invoices = new();
            try
            {
                List<RecordRef> recordRefList = new();
                foreach (var InvoiceItemId in netsuiteInvoices)
                {
                    RecordRef recordRef = new()
                    {
                        type = RecordType.invoice,
                        typeSpecified = true,
                        internalId = InvoiceItemId.id
                    };
                    recordRefList.Add(recordRef);
                }
                if (recordRefList.Any())
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
                    NetSuitePortTypeClient _service = new();
                    _service.Endpoint.Address = new System.ServiceModel.EndpointAddress($"{authModel.nsAPIBaseURL}/services/NetSuitePort_2022_2");
                    string nonce = GenerateNonce();
                    long timestamp = GenerateTimestamp();
                    string signature = GenerateSignature(authModel.nsAccountId, authModel.nsConsumerKey, authModel.nsTokenId, nonce, timestamp, authModel.nsConsumerSecret, authModel.nsTokenSecret);
                    TokenPassport tokenPassport = new()
                    {
                        account = authModel.nsAccountId,
                        consumerKey = authModel.nsConsumerKey,
                        token = authModel.nsTokenId,
                        nonce = nonce,
                        timestamp = timestamp,
                        signature = new TokenPassportSignature() { algorithm = "HMAC_SHA256", Value = signature },
                    };

                    var response = await _service.getListAsync(tokenPassport, new ApplicationInfo(), new PartnerInfo(), new NetSuiteSoapAPIServices.Preferences(), recordRefList.ToArray());

                    if (response != null && response.readResponseList != null && response.readResponseList.status != null)
                    {
                        if (response.readResponseList.status.isSuccess)
                        {
                            if (response.readResponseList.readResponse != null && response.readResponseList.readResponse.Any())
                            {
                                objResponse = response.readResponseList.readResponse.ToList();
                                foreach (var item in objResponse)
                                {
                                    Invoices.Add((Invoice)item.record);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Invoices;
        }

        public async Task<(Invoice, string)> GetNetSuiteInvoiceById(NetSuiteAuthModel authModel, string invoiceId)
        {
            try
            {
                string Message = string.Empty;
                string resInvoiceId = string.Empty;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
                NetSuitePortTypeClient _service = new();
                _service.Endpoint.Address = new System.ServiceModel.EndpointAddress($"{authModel.nsAPIBaseURL}/services/NetSuitePort_2022_2");
                string nonce = GenerateNonce();
                long timestamp = GenerateTimestamp();
                string signature = GenerateSignature(authModel.nsAccountId, authModel.nsConsumerKey, authModel.nsTokenId, nonce, timestamp, authModel.nsConsumerSecret, authModel.nsTokenSecret);
                TokenPassport tokenPassport = new()
                {
                    account = authModel.nsAccountId,
                    consumerKey = authModel.nsConsumerKey,
                    token = authModel.nsTokenId,
                    nonce = nonce,
                    timestamp = timestamp,
                    signature = new TokenPassportSignature() { algorithm = "HMAC_SHA256", Value = signature },
                };

                RecordRef baseRef = new()
                {
                    internalId = invoiceId,
                    type = RecordType.invoice,
                    typeSpecified = true
                };
                var response = await _service.getAsync(tokenPassport, new ApplicationInfo(), new PartnerInfo(), new NetSuiteSoapAPIServices.Preferences(), baseRef);

                var objResponse = new Invoice();
                if (response != null && response.readResponse != null && response.readResponse.status != null)
                {
                    if (response.readResponse.status.isSuccess)
                    {
                        if (response.readResponse.record != null)
                        {
                            objResponse = (Invoice)response.readResponse.record;
                        }
                    }
                    else
                    {
                        objResponse = null;
                    }
                }
                if (objResponse != null)
                {
                    return (objResponse, Message);
                }
                else
                {
                    return (null, Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(bool, string)> DeleteNetSuiteInvoice(NetSuiteAuthModel authModel, string invoiceId)
        {
            try
            {
                bool isSuccess = false;
                string Message = string.Empty;
                var url = $"{authModel.nsAPIBaseURL}/services/rest/record/v1/Invoice/{invoiceId}";
                var request = new RestRequest(Method.DELETE);
                var restClient = CreateRestClient(authModel, url);
                var response = await restClient.ExecuteAsync(request);
                if (response != null && response.Content != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent || response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created)
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

        public async Task<(string, string)> CreateNetSuiteInvoice(NetSuiteAuthModel authModel, Invoice invoice)
        {
            try
            {
                string Message = string.Empty;
                string resInvoiceId = string.Empty;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
                NetSuitePortTypeClient _service = new();
                _service.Endpoint.Address = new System.ServiceModel.EndpointAddress($"{authModel.nsAPIBaseURL}/services/NetSuitePort_2022_2");
                string nonce = GenerateNonce();
                long timestamp = GenerateTimestamp();
                string signature = GenerateSignature(authModel.nsAccountId, authModel.nsConsumerKey, authModel.nsTokenId, nonce, timestamp, authModel.nsConsumerSecret, authModel.nsTokenSecret);
                TokenPassport tokenPassport = new()
                {
                    account = authModel.nsAccountId,
                    consumerKey = authModel.nsConsumerKey,
                    token = authModel.nsTokenId,
                    nonce = nonce,
                    timestamp = timestamp,
                    signature = new TokenPassportSignature() { algorithm = "HMAC_SHA256", Value = signature },
                };
                var response = await _service.addAsync(tokenPassport, new ApplicationInfo(), new PartnerInfo(), new NetSuiteSoapAPIServices.Preferences(), invoice);

                if (response != null && response.writeResponse != null)
                {
                    if (response.writeResponse.status != null)
                    {
                        if (response.writeResponse.status.isSuccess)
                        {
                            if (response.writeResponse.baseRef != null)
                            {
                                resInvoiceId = ((RecordRef)response.writeResponse.baseRef).internalId;
                            }
                        }
                        else
                        {
                            if (response.writeResponse.status.statusDetail != null && response.writeResponse.status.statusDetail.Any())
                            {
                                var statusDetail = response.writeResponse.status.statusDetail[0];
                                Message = statusDetail.message;
                            }
                        }
                    }
                }
                return (resInvoiceId, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(string, string)> UpdateNetSuiteInvoice(NetSuiteAuthModel authModel, Invoice invoice)
        {
            try
            {
                string Message = string.Empty;
                string resInvoiceId = string.Empty;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
                NetSuitePortTypeClient _service = new();
                _service.Endpoint.Address = new System.ServiceModel.EndpointAddress($"{authModel.nsAPIBaseURL}/services/NetSuitePort_2022_2");
                string nonce = GenerateNonce();
                long timestamp = GenerateTimestamp();
                string signature = GenerateSignature(authModel.nsAccountId, authModel.nsConsumerKey, authModel.nsTokenId, nonce, timestamp, authModel.nsConsumerSecret, authModel.nsTokenSecret);
                TokenPassport tokenPassport = new()
                {
                    account = authModel.nsAccountId,
                    consumerKey = authModel.nsConsumerKey,
                    token = authModel.nsTokenId,
                    nonce = nonce,
                    timestamp = timestamp,
                    signature = new TokenPassportSignature() { algorithm = "HMAC_SHA256", Value = signature },
                };

                var response = await _service.updateAsync(tokenPassport, new ApplicationInfo(), new PartnerInfo(), new NetSuiteSoapAPIServices.Preferences(), invoice);


                if (response != null && response.writeResponse != null)
                {
                    if (response.writeResponse.status != null)
                    {
                        if (response.writeResponse.status.isSuccess)
                        {
                            if (response.writeResponse.baseRef != null)
                            {
                                resInvoiceId = ((RecordRef)response.writeResponse.baseRef).internalId;
                            }
                        }
                        else
                        {
                            if (response.writeResponse.status.statusDetail != null && response.writeResponse.status.statusDetail.Any())
                            {
                                var statusDetail = response.writeResponse.status.statusDetail[0];
                                Message = statusDetail.message;
                            }
                        }
                    }
                }
                return (resInvoiceId, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region Common
        public static RestClient CreateRestClient(NetSuiteAuthModel authModel, string apiurl)
        {
            var client = new RestClient(apiurl);
            var authenticator = OAuth1Authenticator.ForAccessToken(
                            consumerKey: authModel.nsConsumerKey,
                            consumerSecret: authModel.nsConsumerSecret,
                            token: authModel.nsTokenId,
                            tokenSecret: authModel.nsTokenSecret,
                            OAuthSignatureMethod.HmacSha256);
            authenticator.Realm = authModel.nsAccountId;
            client.Authenticator = authenticator;

            return client;
        }
        public static long GenerateTimestamp()
        {
            //computing for timestamp
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            string timestamp = unixTimestamp.ToString();
            return Convert.ToInt64(timestamp);
        }
        public static string GenerateNonce()
        {
            //Computing for nonce
            RNGCryptoServiceProvider rng = new();
            byte[] data = new byte[20];
            rng.GetBytes(data);
            int value = Math.Abs(BitConverter.ToInt32(data, 0));
            return value.ToString();
        }
        public static string GenerateSignature(string accountId, string consumerKey, string tokenKey, string nonce, long timestamp, string consumerSecret, string tokenSecret)
        {
            string baseString = accountId + "&" + consumerKey + "&" + tokenKey + "&" + nonce + "&" + timestamp;
            string key = consumerSecret + "&" + tokenSecret;
            ASCIIEncoding encoding = new();
            byte[] keyByte = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(baseString);
            using (HMACSHA256 myhmacsha1 = new(keyByte))
            {
                byte[] hashmessage = myhmacsha1.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
        #endregion
    }

}
