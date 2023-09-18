using Newtonsoft.Json;
using RestSharp;
using System.Security.Principal;

namespace OrgGoalsBCAndNetsuiteCrud.Models
{
    #region BC:Get Sales Invoice
    public class BCSalesInvoiceList
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        public List<BCSalesInvoice> value { get; set; }
    }

    public class BCSalesInvoice
    {
        [JsonProperty("@odata.etag")]
        public string odataetag { get; set; }
        public string id { get; set; }
        public string number { get; set; }
        public string externalDocumentNumber { get; set; }
        public string invoiceDate { get; set; }
        public string postingDate { get; set; }
        public string dueDate { get; set; }
        public string customerPurchaseOrderReference { get; set; }
        public string customerId { get; set; }
        public string customerNumber { get; set; }
        public string customerName { get; set; }
        public string billToName { get; set; }
        public string billToCustomerId { get; set; }
        public string billToCustomerNumber { get; set; }
        public string shipToName { get; set; }
        public string shipToContact { get; set; }
        public string sellToAddressLine1 { get; set; }
        public string sellToAddressLine2 { get; set; }
        public string sellToCity { get; set; }
        public string sellToCountry { get; set; }
        public string sellToState { get; set; }
        public string sellToPostCode { get; set; }
        public string billToAddressLine1 { get; set; }
        public string billToAddressLine2 { get; set; }
        public string billToCity { get; set; }
        public string billToCountry { get; set; }
        public string billToState { get; set; }
        public string billToPostCode { get; set; }
        public string shipToAddressLine1 { get; set; }
        public string shipToAddressLine2 { get; set; }
        public string shipToCity { get; set; }
        public string shipToCountry { get; set; }
        public string shipToState { get; set; }
        public string shipToPostCode { get; set; }
        public string currencyId { get; set; }
        public string shortcutDimension1Code { get; set; }
        public string shortcutDimension2Code { get; set; }
        public string currencyCode { get; set; }
        public string orderId { get; set; }
        public string orderNumber { get; set; }
        public string paymentTermsId { get; set; }
        public string shipmentMethodId { get; set; }
        public string salesperson { get; set; }
        public bool pricesIncludeTax { get; set; }
        public float remainingAmount { get; set; }
        public float discountAmount { get; set; }
        public bool discountAppliedBeforeTax { get; set; }
        public float totalAmountExcludingTax { get; set; }
        public float totalTaxAmount { get; set; }
        public float totalAmountIncludingTax { get; set; }
        public string status { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
    }
    #endregion

    #region BC:Create Update Sales Invoice
    public class BCSalesInvoiceInsertUpdateModel
    {
        //public string? number { get; set; }
        public string? externalDocumentNumber { get; set; }
        public string? invoiceDate { get; set; }
        public string? postingDate { get; set; }
        public string? dueDate { get; set; }
        public string? customerPurchaseOrderReference { get; set; }
        public string? customerId { get; set; }
        public string? customerNumber { get; set; }
        //public string? customerName { get; set; }
        //public string? billToName { get; set; }
        public string? billToCustomerId { get; set; }
        public string? billToCustomerNumber { get; set; }
        public string? shipToName { get; set; }
        public string? shipToContact { get; set; }
        public string? sellToAddressLine1 { get; set; }
        public string? sellToAddressLine2 { get; set; }
        public string? sellToCity { get; set; }
        public string? sellToCountry { get; set; }
        public string? sellToState { get; set; }
        public string? sellToPostCode { get; set; }
        //public string? billToAddressLine1 { get; set; }
        //public string? billToAddressLine2 { get; set; }
        //public string? billToCity { get; set; }
        //public string? billToCountry { get; set; }
        //public string? billToState { get; set; }
        //public string? billToPostCode { get; set; }
        public string? shipToAddressLine1 { get; set; }
        public string? shipToAddressLine2 { get; set; }
        public string? shipToCity { get; set; }
        public string? shipToCountry { get; set; }
        public string? shipToState { get; set; }
        public string? shipToPostCode { get; set; }
        public string? currencyId { get; set; }
        public string? shortcutDimension1Code { get; set; }
        public string? shortcutDimension2Code { get; set; }
        public string? currencyCode { get; set; }
        //public string? orderId { get; set; }
        //public string? orderNumber { get; set; }
        public string? paymentTermsId { get; set; }
        public string? shipmentMethodId { get; set; }
        public string? salesperson { get; set; }
        //public bool? pricesIncludeTax { get; set; }
        //public float? remainingAmount { get; set; }
        public float? discountAmount { get; set; }
        //public bool? discountAppliedBeforeTax { get; set; }
        //public float? totalAmountExcludingTax { get; set; }
        //public float? totalTaxAmount { get; set; }
        //public float? totalAmountIncludingTax { get ; set; }
        //public string? status { get; set; }
        //public DateTime? lastModifiedDateTime { get; set; }
        public string? phoneNumber { get; set; }
        public string? email { get; set; }
    }
    #endregion


    #region BC:Get Sales Invoice Line
    public class BCSalesInvoiceLineList
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        public List<BCSalesInvoiceLine> value { get; set; }
    }

    public class BCSalesInvoiceLine
    {
        [JsonProperty("@odata.etag")]
        public string odataetag { get; set; }
        public string id { get; set; }
        public string documentId { get; set; }
        public int sequence { get; set; }
        public string itemId { get; set; }
        public string accountId { get; set; }
        public string lineType { get; set; }
        public string lineObjectNumber { get; set; }
        public string description { get; set; }
        public string description2 { get; set; }
        public string unitOfMeasureId { get; set; }
        public string unitOfMeasureCode { get; set; }
        public float quantity { get; set; }
        public float unitPrice { get; set; }
        public float discountAmount { get; set; }
        public float discountPercent { get; set; }
        public bool discountAppliedBeforeTax { get; set; }
        public float amountExcludingTax { get; set; }
        public string taxCode { get; set; }
        public float taxPercent { get; set; }
        public float totalTaxAmount { get; set; }
        public float amountIncludingTax { get; set; }
        public float invoiceDiscountAllocation { get; set; }
        public float netAmount { get; set; }
        public float netTaxAmount { get; set; }
        public float netAmountIncludingTax { get; set; }
        public string shipmentDate { get; set; }
        public string itemVariantId { get; set; }
        public string locationId { get; set; }
    }
    #endregion

    #region BC:Create Update Sales Invoice Line
    public class BCSalesInvoiceLineInsertUpdateModel
    {
        public string? itemId { get; set; }
        public string? lineType { get; set; }
        public string? lineObjectNumber { get; set; }
        public string? description { get; set; }
        public string? description2 { get; set; }
        public string? unitOfMeasureId { get; set; }
        public string? unitOfMeasureCode { get; set; }
        public float? quantity { get; set; }
        public float? unitPrice { get; set; }
        public float? discountAmount { get; set; }
        public float? discountPercent { get; set; }
        public string? taxCode { get; set; }
        public string? shipmentDate { get; set; }
        public string? itemVariantId { get; set; }
        public string? locationId { get; set; }
    }
    #endregion


}
