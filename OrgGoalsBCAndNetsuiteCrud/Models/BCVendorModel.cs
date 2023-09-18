using Newtonsoft.Json;
using RestSharp;
using System.Security.Principal;

namespace OrgGoalsBCAndNetsuiteCrud.Models
{
    #region BC:Get Vendor
    public class BCVendorList
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        public List<BCVendor> value { get; set; }
    }

    public class BCVendor
    {
        [JsonProperty("@odata.etag")]
        public string odataetag { get; set; }
        public string id { get; set; }
        public string number { get; set; }
        public string displayName { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string postalCode { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string taxRegistrationNumber { get; set; }
        public string currencyId { get; set; }
        public string currencyCode { get; set; }
        public string irs1099Code { get; set; }
        public string paymentTermsId { get; set; }
        public string paymentMethodId { get; set; }
        public bool taxLiable { get; set; }
        public string blocked { get; set; }
        public float balance { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
    }
    #endregion

    #region BC:Create Update Vendor
    public class BCVendorInsertUpdateModel
    {
        public string? number { get; set; }
        public string? displayName { get; set; }
        public string? addressLine1 { get; set; }
        public string? addressLine2 { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? country { get; set; }
        public string? postalCode { get; set; }
        public string? phoneNumber { get; set; }
        public string? email { get; set; }
        public string? website { get; set; }
        public string? taxRegistrationNumber { get; set; }
        public string? currencyId { get; set; }
        public string? currencyCode { get; set; }
        public string? irs1099Code { get; set; }
        public string? paymentTermsId { get; set; }
        public string? paymentMethodId { get; set; }
        public bool? taxLiable { get; set; }
        public string? blocked { get; set; }
        //public float balance { get; set; }
        //public DateTime lastModifiedDateTime { get; set; }
    }
    #endregion

}
