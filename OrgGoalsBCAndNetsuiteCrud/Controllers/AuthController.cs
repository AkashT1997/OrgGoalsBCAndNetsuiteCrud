using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Newtonsoft.Json;
using OrgGoalsBCAndNetsuiteCrud.Core;
using OrgGoalsBCAndNetsuiteCrud.Models.MongoDB;
using OrgGoalsBCAndNetsuiteCrud.MongoDB_Services;
using System.Globalization;

namespace OrgGoalsBCAndNetsuiteCrud.Controllers
{
    public class AuthController : Controller
    {
        private readonly BCAuthService _bCAuthService;

        public AuthController(BCAuthService bCAuthService)
        {
            _bCAuthService = bCAuthService;
        }

        public IActionResult MakeAuthorizationRequest(BCAuthModel bCAuthModel)
        {
            //var data = _bCAuthService.GetByFilterAsync("").Result;
            var id = TempData["BCAuthDataId"] ?? "";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                bCAuthModel = _bCAuthService.GetByIdAsync(id.ToString()).Result;
                //var list = _bCAuthService.GetAllAsync().Result;
                _ = _bCAuthService.DeleteAsync(id.ToString());
            }
            return View(bCAuthModel);
        }

        [HttpPost]
        public IActionResult BCMakeAuthorizationRequest(string client_id, string secret_id)
        {
            if (!string.IsNullOrEmpty(client_id) && !string.IsNullOrEmpty(secret_id))
            {
                BCClients bCClients = new(_bCAuthService);
                var authorizationRequest = bCClients.MakeAuthorizationRequest(client_id);
                TempData["client_id"] = client_id;
                TempData["secret_id"] = secret_id;
                return Redirect(authorizationRequest);
            }
            else
            {
                return RedirectToAction("MakeAuthorizationRequest", new BCAuthModel());
            }
        }

        #region Business Central
        public async Task<IActionResult> BusinessCentralCallback(string code, string state, string session_state, string error, string error_description)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(code))
                {
                    BCClients bCClients = new(_bCAuthService);
                    var client_id = TempData["client_id"] ?? "";
                    var secret_id = TempData["secret_id"] ?? "";
                    if (!string.IsNullOrEmpty(client_id.ToString()) && !string.IsNullOrEmpty(secret_id.ToString()))
                    {
                        var BCAuthData = await bCClients.GetBussinessCentralBearerToken(code, client_id.ToString(), secret_id.ToString());
                        if (BCAuthData != null)
                        {
                            await _bCAuthService.CreateAsync(BCAuthData);
                            TempData["BCAuthDataId"] = BCAuthData.id;
                            return RedirectToAction("MakeAuthorizationRequest");
                        }
                        else
                        {
                            return RedirectToAction("ErrorPage", "Connection", new { message = "Authentication issue when getting bearer token from BC using code (GetBussinessCentralBearerToken)" });
                        }
                    }
                    else
                    {
                        return RedirectToAction("MakeAuthorizationRequest");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(error_description))
                    {
                        return RedirectToAction("ErrorPage", "Connection", new { message = "Authentication issue when connect BC : " + error_description });
                    }
                    else
                    {
                        return RedirectToAction("ErrorPage", "Connection", new { message = "Authentication issue." });
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
