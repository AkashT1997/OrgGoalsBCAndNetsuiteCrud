using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NetSuiteSoapAPIServices;
using OrgGoalsBCAndNetsuiteCrud.Core;
using OrgGoalsBCAndNetsuiteCrud.Models;
using System.ComponentModel.Design;

namespace OrgGoalsNetSuiteAndNetsuiteCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetSuiteAPIController : ControllerBase
    {
        #region Customer
        [HttpGet("GetNetSuiteCustomerList")]
        public IActionResult GetNetSuiteCustomerList()
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    NetSuiteAuthModel authModel = new()
                    {
                        nsAccountId = nsAccountId,
                        nsAPIBaseURL = nsAPIBaseURL,
                        nsTokenId = nsTokenId,
                        nsTokenSecret = nsTokenSecret,
                        nsConsumerKey = nsConsumerKey,
                        nsConsumerSecret = nsConsumerSecret
                    };
                    NetSuiteClients netSuiteClients = new();
                    var customerListResponse = netSuiteClients.GetNetSuiteCustomerList(authModel, new List<NetsuiteCustomerItem>()).Result;
                    if (string.IsNullOrEmpty(customerListResponse.Item2))
                    {
                        return Ok(customerListResponse.Item1);
                    }
                    else
                    {
                        return BadRequest(customerListResponse.Item2);
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetNetSuiteCustomerById")]
        public IActionResult GetNetSuiteCustomerById(string customerId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    if (!string.IsNullOrEmpty(customerId))
                    {
                        NetSuiteAuthModel authModel = new()
                        {
                            nsAccountId = nsAccountId,
                            nsAPIBaseURL = nsAPIBaseURL,
                            nsTokenId = nsTokenId,
                            nsTokenSecret = nsTokenSecret,
                            nsConsumerKey = nsConsumerKey,
                            nsConsumerSecret = nsConsumerSecret
                        };
                        NetSuiteClients netSuiteClients = new();
                        var customerResponse = netSuiteClients.GetNetSuiteCustomerById(authModel, customerId).Result;
                        if (string.IsNullOrEmpty(customerResponse.Item2))
                        {
                            if (customerResponse.Item1 != null)
                            {
                                return Ok(customerResponse.Item1);
                            }
                            else
                            {
                                return BadRequest($"Customer detail not found for Customer Id: {customerId}");
                            }
                        }
                        else
                        {
                            return BadRequest(customerResponse.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found customerId");
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteNetSuiteCustomer")]
        public IActionResult DeleteNetSuiteCustomer(string customerId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    if (!string.IsNullOrEmpty(customerId))
                    {
                        NetSuiteAuthModel authModel = new()
                        {
                            nsAccountId = nsAccountId,
                            nsAPIBaseURL = nsAPIBaseURL,
                            nsTokenId = nsTokenId,
                            nsTokenSecret = nsTokenSecret,
                            nsConsumerKey = nsConsumerKey,
                            nsConsumerSecret = nsConsumerSecret
                        };
                        NetSuiteClients netSuiteClients = new();
                        var customer = netSuiteClients.GetNetSuiteCustomerById(authModel, customerId).Result;
                        if (string.IsNullOrEmpty(customer.Item2))
                        {
                            if (customer.Item1 != null)
                            {
                                var isSuccess = netSuiteClients.DeleteNetSuiteCustomer(authModel, customerId).Result;
                                if (string.IsNullOrEmpty(isSuccess.Item2))
                                {
                                    return Ok("Success");
                                }
                                else
                                {
                                    return BadRequest(isSuccess.Item2);
                                }
                            }
                            else
                            {
                                return BadRequest($"Customer detail not found for Customer Id: {customerId}");
                            }
                        }
                        else
                        {
                            return BadRequest(customer.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found customerId");
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateNetSuiteCustomer")]
        public IActionResult CreateNetSuiteCustomer(Customer insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    NetSuiteAuthModel authModel = new()
                    {
                        nsAccountId = nsAccountId,
                        nsAPIBaseURL = nsAPIBaseURL,
                        nsTokenId = nsTokenId,
                        nsTokenSecret = nsTokenSecret,
                        nsConsumerKey = nsConsumerKey,
                        nsConsumerSecret = nsConsumerSecret
                    };
                    NetSuiteClients netSuiteClients = new();
                    var customerResponse = netSuiteClients.CreateNetSuiteCustomer(authModel, insertUpdateModel).Result;
                    if (string.IsNullOrEmpty(customerResponse.Item2))
                    {
                        return Ok(customerResponse.Item1);
                    }
                    else
                    {
                        return BadRequest(customerResponse.Item2);
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("UpdateNetSuiteCustomer")]
        public IActionResult UpdateNetSuiteCustomer(Customer insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    if (insertUpdateModel != null && !string.IsNullOrEmpty(insertUpdateModel.internalId))
                    {
                        NetSuiteAuthModel authModel = new()
                        {
                            nsAccountId = nsAccountId,
                            nsAPIBaseURL = nsAPIBaseURL,
                            nsTokenId = nsTokenId,
                            nsTokenSecret = nsTokenSecret,
                            nsConsumerKey = nsConsumerKey,
                            nsConsumerSecret = nsConsumerSecret
                        };
                        NetSuiteClients netSuiteClients = new();
                        var customerResponse = netSuiteClients.UpdateNetSuiteCustomer(authModel, insertUpdateModel).Result;
                        if (string.IsNullOrEmpty(customerResponse.Item2))
                        {
                            return Ok(customerResponse.Item1);
                        }
                        else
                        {
                            return BadRequest(customerResponse.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found customer Id (internalId)");
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


        #region Vendor
        [HttpGet("GetNetSuiteVendorList")]
        public IActionResult GetNetSuiteVendorList()
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    NetSuiteAuthModel authModel = new()
                    {
                        nsAccountId = nsAccountId,
                        nsAPIBaseURL = nsAPIBaseURL,
                        nsTokenId = nsTokenId,
                        nsTokenSecret = nsTokenSecret,
                        nsConsumerKey = nsConsumerKey,
                        nsConsumerSecret = nsConsumerSecret
                    };
                    NetSuiteClients netSuiteClients = new();
                    var vendorListResponse = netSuiteClients.GetNetSuiteVendorList(authModel, new List<NetsuiteVendorItem>()).Result;
                    if (string.IsNullOrEmpty(vendorListResponse.Item2))
                    {
                        return Ok(vendorListResponse.Item1);
                    }
                    else
                    {
                        return BadRequest(vendorListResponse.Item2);
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetNetSuiteVendorById")]
        public IActionResult GetNetSuiteVendorById(string vendorId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    if (!string.IsNullOrEmpty(vendorId))
                    {
                        NetSuiteAuthModel authModel = new()
                        {
                            nsAccountId = nsAccountId,
                            nsAPIBaseURL = nsAPIBaseURL,
                            nsTokenId = nsTokenId,
                            nsTokenSecret = nsTokenSecret,
                            nsConsumerKey = nsConsumerKey,
                            nsConsumerSecret = nsConsumerSecret
                        };
                        NetSuiteClients netSuiteClients = new();
                        var vendorResponse = netSuiteClients.GetNetSuiteVendorById(authModel, vendorId).Result;
                        if (string.IsNullOrEmpty(vendorResponse.Item2))
                        {
                            if (vendorResponse.Item1 != null)
                            {
                                return Ok(vendorResponse.Item1);
                            }
                            else
                            {
                                return BadRequest($"Vendor detail not found for Vendor Id: {vendorId}");
                            }
                        }
                        else
                        {
                            return BadRequest(vendorResponse.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found vendorId");
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteNetSuiteVendor")]
        public IActionResult DeleteNetSuiteVendor(string vendorId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    if (!string.IsNullOrEmpty(vendorId))
                    {
                        NetSuiteAuthModel authModel = new()
                        {
                            nsAccountId = nsAccountId,
                            nsAPIBaseURL = nsAPIBaseURL,
                            nsTokenId = nsTokenId,
                            nsTokenSecret = nsTokenSecret,
                            nsConsumerKey = nsConsumerKey,
                            nsConsumerSecret = nsConsumerSecret
                        };
                        NetSuiteClients netSuiteClients = new();
                        var vendor = netSuiteClients.GetNetSuiteVendorById(authModel, vendorId).Result;
                        if (string.IsNullOrEmpty(vendor.Item2))
                        {
                            if (vendor.Item1 != null)
                            {
                                var isSuccess = netSuiteClients.DeleteNetSuiteVendor(authModel, vendorId).Result;
                                if (string.IsNullOrEmpty(isSuccess.Item2))
                                {
                                    return Ok("Success");
                                }
                                else
                                {
                                    return BadRequest(isSuccess.Item2);
                                }
                            }
                            else
                            {
                                return BadRequest($"Vendor detail not found for Vendor Id: {vendorId}");
                            }
                        }
                        else
                        {
                            return BadRequest(vendor.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found vendorId");
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateNetSuiteVendor")]
        public IActionResult CreateNetSuiteVendor(Vendor insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    NetSuiteAuthModel authModel = new()
                    {
                        nsAccountId = nsAccountId,
                        nsAPIBaseURL = nsAPIBaseURL,
                        nsTokenId = nsTokenId,
                        nsTokenSecret = nsTokenSecret,
                        nsConsumerKey = nsConsumerKey,
                        nsConsumerSecret = nsConsumerSecret
                    };
                    NetSuiteClients netSuiteClients = new();
                    var vendorResponse = netSuiteClients.CreateNetSuiteVendor(authModel, insertUpdateModel).Result;
                    if (string.IsNullOrEmpty(vendorResponse.Item2))
                    {
                        return Ok(vendorResponse.Item1);
                    }
                    else
                    {
                        return BadRequest(vendorResponse.Item2);
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("UpdateNetSuiteVendor")]
        public IActionResult UpdateNetSuiteVendor(Vendor insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    if (insertUpdateModel != null && !string.IsNullOrEmpty(insertUpdateModel.internalId))
                    {
                        NetSuiteAuthModel authModel = new()
                        {
                            nsAccountId = nsAccountId,
                            nsAPIBaseURL = nsAPIBaseURL,
                            nsTokenId = nsTokenId,
                            nsTokenSecret = nsTokenSecret,
                            nsConsumerKey = nsConsumerKey,
                            nsConsumerSecret = nsConsumerSecret
                        };
                        NetSuiteClients netSuiteClients = new();
                        var vendorResponse = netSuiteClients.UpdateNetSuiteVendor(authModel, insertUpdateModel).Result;
                        if (string.IsNullOrEmpty(vendorResponse.Item2))
                        {
                            return Ok(vendorResponse.Item1);
                        }
                        else
                        {
                            return BadRequest(vendorResponse.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found Vendor Id (internalId)");
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


        #region VendorBill
        [HttpGet("GetNetSuiteVendorBillList")]
        public IActionResult GetNetSuiteVendorBillList()
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    NetSuiteAuthModel authModel = new()
                    {
                        nsAccountId = nsAccountId,
                        nsAPIBaseURL = nsAPIBaseURL,
                        nsTokenId = nsTokenId,
                        nsTokenSecret = nsTokenSecret,
                        nsConsumerKey = nsConsumerKey,
                        nsConsumerSecret = nsConsumerSecret
                    };
                    NetSuiteClients netSuiteClients = new();
                    var vendorBillListResponse = netSuiteClients.GetNetSuiteVendorBillList(authModel, new List<NetsuiteVendorBillItemId>()).Result;
                    if (string.IsNullOrEmpty(vendorBillListResponse.Item2))
                    {
                        var mainResponse = new List<VendorBill>();
                        if (vendorBillListResponse.Item1.Any())
                        {
                            foreach (var item in vendorBillListResponse.Item1)
                            {
                                var vendorData = netSuiteClients.GetNetSuiteVendorBillById(authModel, item.id).Result;
                                if (vendorData.Item1 != null && string.IsNullOrEmpty(vendorData.Item2))
                                {
                                    mainResponse.Add(vendorData.Item1);
                                }
                            }
                        }
                        return Ok(mainResponse);
                    }
                    else
                    {
                        return BadRequest(vendorBillListResponse.Item2);
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetNetSuiteVendorBillById")]
        public IActionResult GetNetSuiteVendorBillById(string vendorBillId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    if (!string.IsNullOrEmpty(vendorBillId))
                    {
                        NetSuiteAuthModel authModel = new()
                        {
                            nsAccountId = nsAccountId,
                            nsAPIBaseURL = nsAPIBaseURL,
                            nsTokenId = nsTokenId,
                            nsTokenSecret = nsTokenSecret,
                            nsConsumerKey = nsConsumerKey,
                            nsConsumerSecret = nsConsumerSecret
                        };
                        NetSuiteClients netSuiteClients = new();
                        var vendorBillResponse = netSuiteClients.GetNetSuiteVendorBillById(authModel, vendorBillId).Result;
                        if (string.IsNullOrEmpty(vendorBillResponse.Item2))
                        {
                            if (vendorBillResponse.Item1 != null)
                            {
                                return Ok(vendorBillResponse.Item1);
                            }
                            else
                            {
                                return BadRequest($"VendorBill detail not found for VendorBill Id: {vendorBillId}");
                            }
                        }
                        else
                        {
                            return BadRequest(vendorBillResponse.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found vendorBillId");
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteNetSuiteVendorBill")]
        public IActionResult DeleteNetSuiteVendorBill(string vendorBillId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    if (!string.IsNullOrEmpty(vendorBillId))
                    {
                        NetSuiteAuthModel authModel = new()
                        {
                            nsAccountId = nsAccountId,
                            nsAPIBaseURL = nsAPIBaseURL,
                            nsTokenId = nsTokenId,
                            nsTokenSecret = nsTokenSecret,
                            nsConsumerKey = nsConsumerKey,
                            nsConsumerSecret = nsConsumerSecret
                        };
                        NetSuiteClients netSuiteClients = new();
                        var vendorBill = netSuiteClients.GetNetSuiteVendorBillById(authModel, vendorBillId).Result;
                        if (string.IsNullOrEmpty(vendorBill.Item2))
                        {
                            if (vendorBill.Item1 != null)
                            {
                                var isSuccess = netSuiteClients.DeleteNetSuiteVendorBill(authModel, vendorBillId).Result;
                                if (string.IsNullOrEmpty(isSuccess.Item2))
                                {
                                    return Ok("Success");
                                }
                                else
                                {
                                    return BadRequest(isSuccess.Item2);
                                }
                            }
                            else
                            {
                                return BadRequest($"VendorBill detail not found for VendorBill Id: {vendorBillId}");
                            }
                        }
                        else
                        {
                            return BadRequest(vendorBill.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found vendorBillId");
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateNetSuiteVendorBill")]
        public IActionResult CreateNetSuiteVendorBill(VendorBill insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    NetSuiteAuthModel authModel = new()
                    {
                        nsAccountId = nsAccountId,
                        nsAPIBaseURL = nsAPIBaseURL,
                        nsTokenId = nsTokenId,
                        nsTokenSecret = nsTokenSecret,
                        nsConsumerKey = nsConsumerKey,
                        nsConsumerSecret = nsConsumerSecret
                    };
                    NetSuiteClients netSuiteClients = new();
                    var vendorBillResponse = netSuiteClients.CreateNetSuiteVendorBill(authModel, insertUpdateModel).Result;
                    if (string.IsNullOrEmpty(vendorBillResponse.Item2))
                    {
                        return Ok(vendorBillResponse.Item1);
                    }
                    else
                    {
                        return BadRequest(vendorBillResponse.Item2);
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("UpdateNetSuiteVendorBill")]
        public IActionResult UpdateNetSuiteVendorBill(VendorBill insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    if (insertUpdateModel != null && !string.IsNullOrEmpty(insertUpdateModel.internalId))
                    {
                        NetSuiteAuthModel authModel = new()
                        {
                            nsAccountId = nsAccountId,
                            nsAPIBaseURL = nsAPIBaseURL,
                            nsTokenId = nsTokenId,
                            nsTokenSecret = nsTokenSecret,
                            nsConsumerKey = nsConsumerKey,
                            nsConsumerSecret = nsConsumerSecret
                        };
                        NetSuiteClients netSuiteClients = new();
                        var vendorBillResponse = netSuiteClients.UpdateNetSuiteVendorBill(authModel, insertUpdateModel).Result;
                        if (string.IsNullOrEmpty(vendorBillResponse.Item2))
                        {
                            return Ok(vendorBillResponse.Item1);
                        }
                        else
                        {
                            return BadRequest(vendorBillResponse.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found VendorBill Id (internalId)");
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


        #region Invoice
        [HttpGet("GetNetSuiteInvoiceList")]
        public IActionResult GetNetSuiteInvoiceList()
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    NetSuiteAuthModel authModel = new()
                    {
                        nsAccountId = nsAccountId,
                        nsAPIBaseURL = nsAPIBaseURL,
                        nsTokenId = nsTokenId,
                        nsTokenSecret = nsTokenSecret,
                        nsConsumerKey = nsConsumerKey,
                        nsConsumerSecret = nsConsumerSecret
                    };
                    NetSuiteClients netSuiteClients = new();
                    var invoiceListResponse = netSuiteClients.GetNetSuiteInvoiceList(authModel, new List<NetsuiteInvoiceItemId>()).Result;
                    if (string.IsNullOrEmpty(invoiceListResponse.Item2))
                    {
                        var mainResponse = new List<Invoice>();
                        if (invoiceListResponse.Item1.Any())
                        {
                            foreach (var item in invoiceListResponse.Item1)
                            {
                                var vendorData = netSuiteClients.GetNetSuiteInvoiceById(authModel, item.id).Result;
                                if (vendorData.Item1 != null && string.IsNullOrEmpty(vendorData.Item2))
                                {
                                    mainResponse.Add(vendorData.Item1);
                                }
                            }
                        }
                        return Ok(mainResponse);
                    }
                    else
                    {
                        return BadRequest(invoiceListResponse.Item2);
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetNetSuiteInvoiceById")]
        public IActionResult GetNetSuiteInvoiceById(string invoiceId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    if (!string.IsNullOrEmpty(invoiceId))
                    {
                        NetSuiteAuthModel authModel = new()
                        {
                            nsAccountId = nsAccountId,
                            nsAPIBaseURL = nsAPIBaseURL,
                            nsTokenId = nsTokenId,
                            nsTokenSecret = nsTokenSecret,
                            nsConsumerKey = nsConsumerKey,
                            nsConsumerSecret = nsConsumerSecret
                        };
                        NetSuiteClients netSuiteClients = new();
                        var invoiceResponse = netSuiteClients.GetNetSuiteInvoiceById(authModel, invoiceId).Result;
                        if (string.IsNullOrEmpty(invoiceResponse.Item2))
                        {
                            if (invoiceResponse.Item1 != null)
                            {
                                return Ok(invoiceResponse.Item1);
                            }
                            else
                            {
                                return BadRequest($"Invoice detail not found for Invoice Id: {invoiceId}");
                            }
                        }
                        else
                        {
                            return BadRequest(invoiceResponse.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found invoiceId");
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteNetSuiteInvoice")]
        public IActionResult DeleteNetSuiteInvoice(string invoiceId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    if (!string.IsNullOrEmpty(invoiceId))
                    {
                        NetSuiteAuthModel authModel = new()
                        {
                            nsAccountId = nsAccountId,
                            nsAPIBaseURL = nsAPIBaseURL,
                            nsTokenId = nsTokenId,
                            nsTokenSecret = nsTokenSecret,
                            nsConsumerKey = nsConsumerKey,
                            nsConsumerSecret = nsConsumerSecret
                        };
                        NetSuiteClients netSuiteClients = new();
                        var invoice = netSuiteClients.GetNetSuiteInvoiceById(authModel, invoiceId).Result;
                        if (string.IsNullOrEmpty(invoice.Item2))
                        {
                            if (invoice.Item1 != null)
                            {
                                var isSuccess = netSuiteClients.DeleteNetSuiteInvoice(authModel, invoiceId).Result;
                                if (string.IsNullOrEmpty(isSuccess.Item2))
                                {
                                    return Ok("Success");
                                }
                                else
                                {
                                    return BadRequest(isSuccess.Item2);
                                }
                            }
                            else
                            {
                                return BadRequest($"Invoice detail not found for Invoice Id: {invoiceId}");
                            }
                        }
                        else
                        {
                            return BadRequest(invoice.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found invoiceId");
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateNetSuiteInvoice")]
        public IActionResult CreateNetSuiteInvoice(Invoice insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    NetSuiteAuthModel authModel = new()
                    {
                        nsAccountId = nsAccountId,
                        nsAPIBaseURL = nsAPIBaseURL,
                        nsTokenId = nsTokenId,
                        nsTokenSecret = nsTokenSecret,
                        nsConsumerKey = nsConsumerKey,
                        nsConsumerSecret = nsConsumerSecret
                    };
                    NetSuiteClients netSuiteClients = new();
                    var invoiceResponse = netSuiteClients.CreateNetSuiteInvoice(authModel, insertUpdateModel).Result;
                    if (string.IsNullOrEmpty(invoiceResponse.Item2))
                    {
                        return Ok(invoiceResponse.Item1);
                    }
                    else
                    {
                        return BadRequest(invoiceResponse.Item2);
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("UpdateNetSuiteInvoice")]
        public IActionResult UpdateNetSuiteInvoice(Invoice insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("nsAccountId", out StringValues nsAccountId);
                HttpContext.Request.Headers.TryGetValue("nsAPIBaseURL", out StringValues nsAPIBaseURL);
                HttpContext.Request.Headers.TryGetValue("nsTokenId", out StringValues nsTokenId);
                HttpContext.Request.Headers.TryGetValue("nsTokenSecret", out StringValues nsTokenSecret);
                HttpContext.Request.Headers.TryGetValue("nsConsumerKey", out StringValues nsConsumerKey);
                HttpContext.Request.Headers.TryGetValue("nsConsumerSecret", out StringValues nsConsumerSecret);

                if (!string.IsNullOrEmpty(nsAccountId) && !string.IsNullOrEmpty(nsAPIBaseURL) && !string.IsNullOrEmpty(nsTokenId) && !string.IsNullOrEmpty(nsTokenSecret) && !string.IsNullOrEmpty(nsConsumerKey) && !string.IsNullOrEmpty(nsConsumerSecret))
                {
                    if (insertUpdateModel != null && !string.IsNullOrEmpty(insertUpdateModel.internalId))
                    {
                        NetSuiteAuthModel authModel = new()
                        {
                            nsAccountId = nsAccountId,
                            nsAPIBaseURL = nsAPIBaseURL,
                            nsTokenId = nsTokenId,
                            nsTokenSecret = nsTokenSecret,
                            nsConsumerKey = nsConsumerKey,
                            nsConsumerSecret = nsConsumerSecret
                        };
                        NetSuiteClients netSuiteClients = new();
                        var invoiceResponse = netSuiteClients.UpdateNetSuiteInvoice(authModel, insertUpdateModel).Result;
                        if (string.IsNullOrEmpty(invoiceResponse.Item2))
                        {
                            return Ok(invoiceResponse.Item1);
                        }
                        else
                        {
                            return BadRequest(invoiceResponse.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found Invoice Id (internalId)");
                    }
                }
                else
                {
                    return Unauthorized($"Missing or invalid authorization detail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
