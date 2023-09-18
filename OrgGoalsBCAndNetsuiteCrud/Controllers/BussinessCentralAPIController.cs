using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using OrgGoalsBCAndNetsuiteCrud.Core;
using OrgGoalsBCAndNetsuiteCrud.Models;
using System.ComponentModel.DataAnnotations;

namespace OrgGoalsBCAndNetsuiteCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BussinessCentralAPIController : ControllerBase
    {

        #region Customer
        [HttpGet("GetBCCustomerList")]
        public IActionResult GetBCCustomerList()
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    BCClients bCClients = new();
                    var customerListResponse = bCClients.GetBCCustomerList(environmentName, companyId, accessToken).Result;
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

        [HttpGet("GetBCCustomerById")]
        public IActionResult GetBCCustomerById(string customerId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(customerId))
                    {
                        BCClients bCClients = new();
                        var customerResponse = bCClients.GetBCCustomerById(environmentName, companyId, accessToken, customerId).Result;
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

        [HttpDelete("DeleteBCCustomer")]
        public IActionResult DeleteBCCustomer(string customerId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(customerId))
                    {
                        BCClients bCClients = new();
                        var customer = bCClients.GetBCCustomerById(environmentName, companyId, accessToken, customerId).Result;
                        if (string.IsNullOrEmpty(customer.Item2))
                        {
                            if (customer.Item1 != null)
                            {
                                var isSuccess = bCClients.DeleteBCCustomer(environmentName, companyId, accessToken, customer.Item1.id, customer.Item1.odataetag).Result;
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

        [HttpPost("CreateBCCustomer")]
        public IActionResult CreateBCCustomer(BCCustomerInsertUpdateModel insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    BCClients bCClients = new();
                    var customerResponse = bCClients.CreateBCCustomer(insertUpdateModel, environmentName, companyId, accessToken).Result;
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

        [HttpPut("UpdateBCCustomer")]
        public IActionResult UpdateBCCustomer(string customerId, BCCustomerInsertUpdateModel insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(customerId))
                    {
                        BCClients bCClients = new();
                        var customer = bCClients.GetBCCustomerById(environmentName, companyId, accessToken, customerId).Result;
                        if (string.IsNullOrEmpty(customer.Item2))
                        {
                            if (customer.Item1 != null)
                            {
                                var customerResponse = bCClients.UpdateBCCustomer(insertUpdateModel, environmentName, companyId, accessToken, customer.Item1.id, customer.Item1.odataetag).Result;
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
        #endregion


        #region Vendor
        [HttpGet("GetBCVendorList")]
        public IActionResult GetBCVendorList()
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    BCClients bCClients = new();
                    var vendorListResponse = bCClients.GetBCVendorList(environmentName, companyId, accessToken).Result;
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

        [HttpGet("GetBCVendorById")]
        public IActionResult GetBCVendorById(string vendorId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(vendorId))
                    {
                        BCClients bCClients = new();
                        var vendorResponse = bCClients.GetBCVendorById(environmentName, companyId, accessToken, vendorId).Result;
                        if (string.IsNullOrEmpty(vendorResponse.Item2))
                        {
                            if (vendorResponse.Item1 != null)
                            {
                                return Ok(vendorResponse.Item1);
                            }
                            else
                            {
                                return BadRequest($"Venndor detail not found for Customer Id: {vendorId}");
                            }
                        }
                        else
                        {
                            return BadRequest(vendorResponse.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found VendorId");
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

        [HttpDelete("DeleteBCVendor")]
        public IActionResult DeleteBCVendor(string vendorId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(vendorId))
                    {
                        BCClients bCClients = new();
                        var vendor = bCClients.GetBCVendorById(environmentName, companyId, accessToken, vendorId).Result;
                        if (string.IsNullOrEmpty(vendor.Item2))
                        {
                            if (vendor.Item1 != null)
                            {
                                var isSuccess = bCClients.DeleteBCVendor(environmentName, companyId, accessToken, vendor.Item1.id, vendor.Item1.odataetag).Result;
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

        [HttpPost("CreateBCVendor")]
        public IActionResult CreateBCVendor(BCVendorInsertUpdateModel insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    BCClients bCClients = new();
                    var vendorResponse = bCClients.CreateBCVendor(insertUpdateModel, environmentName, companyId, accessToken).Result;
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

        [HttpPut("UpdateBCVendor")]
        public IActionResult UpdateBCVendor(string vendorId, BCVendorInsertUpdateModel insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(vendorId))
                    {
                        BCClients bCClients = new();
                        var vendor = bCClients.GetBCVendorById(environmentName, companyId, accessToken, vendorId).Result;
                        if (string.IsNullOrEmpty(vendor.Item2))
                        {
                            if (vendor.Item1 != null)
                            {
                                var VendorResponse = bCClients.UpdateBCVendor(insertUpdateModel, environmentName, companyId, accessToken, vendor.Item1.id, vendor.Item1.odataetag).Result;
                                if (string.IsNullOrEmpty(VendorResponse.Item2))
                                {
                                    return Ok(VendorResponse.Item1);
                                }
                                else
                                {
                                    return BadRequest(VendorResponse.Item2);
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
        #endregion


        #region Purchase Invoice (Bill)
        #region Purchase Invoice (Bill) Header
        [HttpGet("GetBCPurchaseInvoiceList")]
        public IActionResult GetBCPurchaseInvoiceList()
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    BCClients bCClients = new();
                    var purchaseInvoiceListResponse = bCClients.GetBCPurchaseInvoiceList(environmentName, companyId, accessToken).Result;
                    if (string.IsNullOrEmpty(purchaseInvoiceListResponse.Item2))
                    {
                        return Ok(purchaseInvoiceListResponse.Item1);
                    }
                    else
                    {
                        return BadRequest(purchaseInvoiceListResponse.Item2);
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

        [HttpGet("GetBCPurchaseInvoiceById")]
        public IActionResult GetBCPurchaseInvoiceById(string purchaseInvoiceId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(purchaseInvoiceId))
                    {
                        BCClients bCClients = new();
                        var purchaseInvoiceResponse = bCClients.GetBCPurchaseInvoiceById(environmentName, companyId, accessToken, purchaseInvoiceId).Result;
                        if (string.IsNullOrEmpty(purchaseInvoiceResponse.Item2))
                        {
                            if (purchaseInvoiceResponse.Item1 != null)
                            {
                                return Ok(purchaseInvoiceResponse.Item1);
                            }
                            else
                            {
                                return BadRequest($"Venndor detail not found for Customer Id: {purchaseInvoiceResponse}");
                            }
                        }
                        else
                        {
                            return BadRequest(purchaseInvoiceResponse.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found PurchaseInvoiceId");
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

        [HttpDelete("DeleteBCPurchaseInvoice")]
        public IActionResult DeleteBCPurchaseInvoice(string purchaseInvoiceId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(purchaseInvoiceId))
                    {
                        BCClients bCClients = new();
                        var purchaseInvoice = bCClients.GetBCPurchaseInvoiceById(environmentName, companyId, accessToken, purchaseInvoiceId).Result;
                        if (string.IsNullOrEmpty(purchaseInvoice.Item2))
                        {
                            if (purchaseInvoice.Item1 != null)
                            {
                                var isSuccess = bCClients.DeleteBCPurchaseInvoice(environmentName, companyId, accessToken, purchaseInvoice.Item1.id, purchaseInvoice.Item1.odataetag).Result;
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
                                return BadRequest($"PurchaseInvoice detail not found for Purchase Invoice Id: {purchaseInvoiceId}");
                            }
                        }
                        else
                        {
                            return BadRequest(purchaseInvoice.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found purchaseInvoiceId");
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

        [HttpPost("CreateBCPurchaseInvoice")]
        public IActionResult CreateBCPurchaseInvoice(BCPurchaseInvoiceInsertUpdateModel insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    BCClients bCClients = new();
                    var purchaseInvoiceResponse = bCClients.CreateBCPurchaseInvoice(insertUpdateModel, environmentName, companyId, accessToken).Result;
                    if (string.IsNullOrEmpty(purchaseInvoiceResponse.Item2))
                    {
                        return Ok(purchaseInvoiceResponse.Item1);
                    }
                    else
                    {
                        return BadRequest(purchaseInvoiceResponse.Item2);
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

        [HttpPut("UpdateBCPurchaseInvoice")]
        public IActionResult UpdateBCPurchaseInvoice(string purchaseInvoiceId, BCPurchaseInvoiceInsertUpdateModel insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(purchaseInvoiceId))
                    {
                        BCClients bCClients = new();
                        var purchaseInvoice = bCClients.GetBCPurchaseInvoiceById(environmentName, companyId, accessToken, purchaseInvoiceId).Result;
                        if (string.IsNullOrEmpty(purchaseInvoice.Item2))
                        {
                            if (purchaseInvoice.Item1 != null)
                            {
                                var PurchaseInvoiceResponse = bCClients.UpdateBCPurchaseInvoice(insertUpdateModel, environmentName, companyId, accessToken, purchaseInvoice.Item1.id, purchaseInvoice.Item1.odataetag).Result;
                                if (string.IsNullOrEmpty(PurchaseInvoiceResponse.Item2))
                                {
                                    return Ok(PurchaseInvoiceResponse.Item1);
                                }
                                else
                                {
                                    return BadRequest(PurchaseInvoiceResponse.Item2);
                                }
                            }
                            else
                            {
                                return BadRequest($"PurchaseInvoice detail not found for Purchase Invoice Id: {purchaseInvoiceId}");
                            }
                        }
                        else
                        {
                            return BadRequest(purchaseInvoice.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found purchaseInvoiceId");
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


        #region Purchase Invoice (Bill) Line
        [HttpGet("GetBCPurchaseInvoiceLineList")]
        public IActionResult GetBCPurchaseInvoiceLineList(string purchaseInvoiceId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    BCClients bCClients = new();
                    var purchaseInvoiceLineListResponse = bCClients.GetBCPurchaseInvoiceLineList(environmentName, companyId, accessToken, purchaseInvoiceId).Result;
                    if (string.IsNullOrEmpty(purchaseInvoiceLineListResponse.Item2))
                    {
                        return Ok(purchaseInvoiceLineListResponse.Item1);
                    }
                    else
                    {
                        return BadRequest(purchaseInvoiceLineListResponse.Item2);
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

        [HttpGet("GetBCPurchaseInvoiceLineById")]
        public IActionResult GetBCPurchaseInvoiceLineById(string purchaseInvoiceId, string purchaseInvoiceLineId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(purchaseInvoiceLineId))
                    {
                        BCClients bCClients = new();
                        var purchaseInvoiceLineResponse = bCClients.GetBCPurchaseInvoiceLineById(environmentName, companyId, accessToken, purchaseInvoiceId, purchaseInvoiceLineId).Result;
                        if (string.IsNullOrEmpty(purchaseInvoiceLineResponse.Item2))
                        {
                            if (purchaseInvoiceLineResponse.Item1 != null)
                            {
                                return Ok(purchaseInvoiceLineResponse.Item1);
                            }
                            else
                            {
                                return BadRequest($"Venndor detail not found for Customer Id: {purchaseInvoiceLineResponse}");
                            }
                        }
                        else
                        {
                            return BadRequest(purchaseInvoiceLineResponse.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found PurchaseInvoiceLineId");
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

        [HttpDelete("DeleteBCPurchaseInvoiceLine")]
        public IActionResult DeleteBCPurchaseInvoiceLine(string purchaseInvoiceId, string purchaseInvoiceLineId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(purchaseInvoiceLineId))
                    {
                        BCClients bCClients = new();
                        var purchaseInvoiceLine = bCClients.GetBCPurchaseInvoiceLineById(environmentName, companyId, accessToken, purchaseInvoiceId, purchaseInvoiceLineId).Result;
                        if (string.IsNullOrEmpty(purchaseInvoiceLine.Item2))
                        {
                            if (purchaseInvoiceLine.Item1 != null)
                            {
                                var isSuccess = bCClients.DeleteBCPurchaseInvoiceLine(environmentName, companyId, accessToken, purchaseInvoiceId, purchaseInvoiceLine.Item1.id, purchaseInvoiceLine.Item1.odataetag).Result;
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
                                return BadRequest($"PurchaseInvoiceLine detail not found for Purchase Invoice Id: {purchaseInvoiceLineId}");
                            }
                        }
                        else
                        {
                            return BadRequest(purchaseInvoiceLine.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found PurchaseInvoiceLineId");
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

        [HttpPost("CreateBCPurchaseInvoiceLine")]
        public IActionResult CreateBCPurchaseInvoiceLine(string purchaseInvoiceId, BCPurchaseInvoiceLineInsertUpdateModel insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    BCClients bCClients = new();
                    var purchaseInvoiceLineResponse = bCClients.CreateBCPurchaseInvoiceLine(insertUpdateModel, environmentName, companyId, accessToken, purchaseInvoiceId).Result;
                    if (string.IsNullOrEmpty(purchaseInvoiceLineResponse.Item2))
                    {
                        return Ok(purchaseInvoiceLineResponse.Item1);
                    }
                    else
                    {
                        return BadRequest(purchaseInvoiceLineResponse.Item2);
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

        [HttpPut("UpdateBCPurchaseInvoiceLine")]
        public IActionResult UpdateBCPurchaseInvoiceLine(string purchaseInvoiceId, string purchaseInvoiceLineId, BCPurchaseInvoiceLineInsertUpdateModel insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(purchaseInvoiceLineId))
                    {
                        BCClients bCClients = new();
                        var purchaseInvoiceLine = bCClients.GetBCPurchaseInvoiceLineById(environmentName, companyId, accessToken, purchaseInvoiceId, purchaseInvoiceLineId).Result;
                        if (string.IsNullOrEmpty(purchaseInvoiceLine.Item2))
                        {
                            if (purchaseInvoiceLine.Item1 != null)
                            {
                                var PurchaseInvoiceLineResponse = bCClients.UpdateBCPurchaseInvoiceLine(insertUpdateModel, environmentName, companyId, accessToken, purchaseInvoiceId, purchaseInvoiceLine.Item1.id, purchaseInvoiceLine.Item1.odataetag).Result;
                                if (string.IsNullOrEmpty(PurchaseInvoiceLineResponse.Item2))
                                {
                                    return Ok(PurchaseInvoiceLineResponse.Item1);
                                }
                                else
                                {
                                    return BadRequest(PurchaseInvoiceLineResponse.Item2);
                                }
                            }
                            else
                            {
                                return BadRequest($"PurchaseInvoiceLine detail not found for Purchase Invoice Id: {purchaseInvoiceLineId}");
                            }
                        }
                        else
                        {
                            return BadRequest(purchaseInvoiceLine.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found purchaseInvoiceLineId");
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
        #endregion



        #region Sales Invoice (Bill)
        #region Sales Invoice (Bill) Header
        [HttpGet("GetBCSalesInvoiceList")]
        public IActionResult GetBCSalesInvoiceList()
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    BCClients bCClients = new();
                    var SalesInvoiceListResponse = bCClients.GetBCSalesInvoiceList(environmentName, companyId, accessToken).Result;
                    if (string.IsNullOrEmpty(SalesInvoiceListResponse.Item2))
                    {
                        return Ok(SalesInvoiceListResponse.Item1);
                    }
                    else
                    {
                        return BadRequest(SalesInvoiceListResponse.Item2);
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

        [HttpGet("GetBCSalesInvoiceById")]
        public IActionResult GetBCSalesInvoiceById(string salesInvoiceId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(salesInvoiceId))
                    {
                        BCClients bCClients = new();
                        var salesInvoiceResponse = bCClients.GetBCSalesInvoiceById(environmentName, companyId, accessToken, salesInvoiceId).Result;
                        if (string.IsNullOrEmpty(salesInvoiceResponse.Item2))
                        {
                            if (salesInvoiceResponse.Item1 != null)
                            {
                                return Ok(salesInvoiceResponse.Item1);
                            }
                            else
                            {
                                return BadRequest($"Venndor detail not found for Customer Id: {salesInvoiceResponse}");
                            }
                        }
                        else
                        {
                            return BadRequest(salesInvoiceResponse.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found salesInvoiceId");
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

        [HttpDelete("DeleteBCSalesInvoice")]
        public IActionResult DeleteBCSalesInvoice(string salesInvoiceId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(salesInvoiceId))
                    {
                        BCClients bCClients = new();
                        var salesInvoice = bCClients.GetBCSalesInvoiceById(environmentName, companyId, accessToken, salesInvoiceId).Result;
                        if (string.IsNullOrEmpty(salesInvoice.Item2))
                        {
                            if (salesInvoice.Item1 != null)
                            {
                                var isSuccess = bCClients.DeleteBCSalesInvoice(environmentName, companyId, accessToken, salesInvoice.Item1.id, salesInvoice.Item1.odataetag).Result;
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
                                return BadRequest($"SalesInvoice detail not found for Sales Invoice Id: {salesInvoiceId}");
                            }
                        }
                        else
                        {
                            return BadRequest(salesInvoice.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found salesInvoiceId");
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

        [HttpPost("CreateBCSalesInvoice")]
        public IActionResult CreateBCSalesInvoice(BCSalesInvoiceInsertUpdateModel insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    BCClients bCClients = new();
                    var salesInvoiceResponse = bCClients.CreateBCSalesInvoice(insertUpdateModel, environmentName, companyId, accessToken).Result;
                    if (string.IsNullOrEmpty(salesInvoiceResponse.Item2))
                    {
                        return Ok(salesInvoiceResponse.Item1);
                    }
                    else
                    {
                        return BadRequest(salesInvoiceResponse.Item2);
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

        [HttpPut("UpdateBCSalesInvoice")]
        public IActionResult UpdateBCSalesInvoice(string salesInvoiceId, BCSalesInvoiceInsertUpdateModel insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(salesInvoiceId))
                    {
                        BCClients bCClients = new();
                        var salesInvoice = bCClients.GetBCSalesInvoiceById(environmentName, companyId, accessToken, salesInvoiceId).Result;
                        if (string.IsNullOrEmpty(salesInvoice.Item2))
                        {
                            if (salesInvoice.Item1 != null)
                            {
                                var SalesInvoiceResponse = bCClients.UpdateBCSalesInvoice(insertUpdateModel, environmentName, companyId, accessToken, salesInvoice.Item1.id, salesInvoice.Item1.odataetag).Result;
                                if (string.IsNullOrEmpty(SalesInvoiceResponse.Item2))
                                {
                                    return Ok(SalesInvoiceResponse.Item1);
                                }
                                else
                                {
                                    return BadRequest(SalesInvoiceResponse.Item2);
                                }
                            }
                            else
                            {
                                return BadRequest($"SalesInvoice detail not found for Sales Invoice Id: {salesInvoiceId}");
                            }
                        }
                        else
                        {
                            return BadRequest(salesInvoice.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found salesInvoiceId");
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


        #region Sales Invoice (Bill) Line
        [HttpGet("GetBCSalesInvoiceLineList")]
        public IActionResult GetBCSalesInvoiceLineList(string salesInvoiceId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    BCClients bCClients = new();
                    var salesInvoiceLineListResponse = bCClients.GetBCSalesInvoiceLineList(environmentName, companyId, accessToken, salesInvoiceId).Result;
                    if (string.IsNullOrEmpty(salesInvoiceLineListResponse.Item2))
                    {
                        return Ok(salesInvoiceLineListResponse.Item1);
                    }
                    else
                    {
                        return BadRequest(salesInvoiceLineListResponse.Item2);
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

        [HttpGet("GetBCSalesInvoiceLineById")]
        public IActionResult GetBCSalesInvoiceLineById(string salesInvoiceId, string salesInvoiceLineId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(salesInvoiceLineId))
                    {
                        BCClients bCClients = new();
                        var salesInvoiceLineResponse = bCClients.GetBCSalesInvoiceLineById(environmentName, companyId, accessToken, salesInvoiceId, salesInvoiceLineId).Result;
                        if (string.IsNullOrEmpty(salesInvoiceLineResponse.Item2))
                        {
                            if (salesInvoiceLineResponse.Item1 != null)
                            {
                                return Ok(salesInvoiceLineResponse.Item1);
                            }
                            else
                            {
                                return BadRequest($"Venndor detail not found for Customer Id: {salesInvoiceLineResponse}");
                            }
                        }
                        else
                        {
                            return BadRequest(salesInvoiceLineResponse.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found salesInvoiceLineId");
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

        [HttpDelete("DeleteBCSalesInvoiceLine")]
        public IActionResult DeleteBCSalesInvoiceLine(string salesInvoiceId, string salesInvoiceLineId)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(salesInvoiceLineId))
                    {
                        BCClients bCClients = new();
                        var salesInvoiceLine = bCClients.GetBCSalesInvoiceLineById(environmentName, companyId, accessToken, salesInvoiceId, salesInvoiceLineId).Result;
                        if (string.IsNullOrEmpty(salesInvoiceLine.Item2))
                        {
                            if (salesInvoiceLine.Item1 != null)
                            {
                                var isSuccess = bCClients.DeleteBCSalesInvoiceLine(environmentName, companyId, accessToken, salesInvoiceId, salesInvoiceLine.Item1.id, salesInvoiceLine.Item1.odataetag).Result;
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
                                return BadRequest($"SalesInvoiceLine detail not found for Sales Invoice Id: {salesInvoiceLineId}");
                            }
                        }
                        else
                        {
                            return BadRequest(salesInvoiceLine.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found salesInvoiceLineId");
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

        [HttpPost("CreateBCSalesInvoiceLine")]
        public IActionResult CreateBCSalesInvoiceLine(string salesInvoiceId, BCSalesInvoiceLineInsertUpdateModel insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    BCClients bCClients = new();
                    var salesInvoiceLineResponse = bCClients.CreateBCSalesInvoiceLine(insertUpdateModel, environmentName, companyId, accessToken, salesInvoiceId).Result;
                    if (string.IsNullOrEmpty(salesInvoiceLineResponse.Item2))
                    {
                        return Ok(salesInvoiceLineResponse.Item1);
                    }
                    else
                    {
                        return BadRequest(salesInvoiceLineResponse.Item2);
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

        [HttpPut("UpdateBCSalesInvoiceLine")]
        public IActionResult UpdateBCSalesInvoiceLine(string salesInvoiceId, string salesInvoiceLineId, BCSalesInvoiceLineInsertUpdateModel insertUpdateModel)
        {
            try
            {
                HttpContext.Request.Headers.TryGetValue("environmentName", out StringValues environmentName);
                HttpContext.Request.Headers.TryGetValue("companyId", out StringValues companyId);
                HttpContext.Request.Headers.TryGetValue("accessToken", out StringValues accessToken);
                if (!string.IsNullOrEmpty(environmentName) && !string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(accessToken))
                {
                    if (!string.IsNullOrEmpty(salesInvoiceLineId))
                    {
                        BCClients bCClients = new();
                        var SalesInvoiceLine = bCClients.GetBCSalesInvoiceLineById(environmentName, companyId, accessToken, salesInvoiceId, salesInvoiceLineId).Result;
                        if (string.IsNullOrEmpty(SalesInvoiceLine.Item2))
                        {
                            if (SalesInvoiceLine.Item1 != null)
                            {
                                var SalesInvoiceLineResponse = bCClients.UpdateBCSalesInvoiceLine(insertUpdateModel, environmentName, companyId, accessToken, salesInvoiceId, SalesInvoiceLine.Item1.id, SalesInvoiceLine.Item1.odataetag).Result;
                                if (string.IsNullOrEmpty(SalesInvoiceLineResponse.Item2))
                                {
                                    return Ok(SalesInvoiceLineResponse.Item1);
                                }
                                else
                                {
                                    return BadRequest(SalesInvoiceLineResponse.Item2);
                                }
                            }
                            else
                            {
                                return BadRequest($"SalesInvoiceLine detail not found for Sales Invoice Id: {salesInvoiceLineId}");
                            }
                        }
                        else
                        {
                            return BadRequest(SalesInvoiceLine.Item2);
                        }
                    }
                    else
                    {
                        return BadRequest($"Required parameter not found salesInvoiceLineId");
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
        #endregion
    }
}
