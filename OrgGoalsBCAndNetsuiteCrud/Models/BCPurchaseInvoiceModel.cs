using Newtonsoft.Json;
using RestSharp;
using System.Security.Principal;

namespace OrgGoalsBCAndNetsuiteCrud.Models
{
    #region BC:Get Purchase Invoice
    public class BCPurchaseInvoiceList
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        public List<BCPurchaseInvoice> value { get; set; }
    }

    public class BCPurchaseInvoice
    {
        [JsonProperty("@odata.etag")]
        public string odataetag { get; set; }
        public string id { get; set; }
        public string number { get; set; }
        public string invoiceDate { get; set; }
        public string postingDate { get; set; }
        public string dueDate { get; set; }
        public string vendorInvoiceNumber { get; set; }
        public string vendorId { get; set; }
        public string vendorNumber { get; set; }
        public string vendorName { get; set; }
        public string payToName { get; set; }
        public string payToContact { get; set; }
        public string payToVendorId { get; set; }
        public string payToVendorNumber { get; set; }
        public string shipToName { get; set; }
        public string shipToContact { get; set; }
        public string buyFromAddressLine1 { get; set; }
        public string buyFromAddressLine2 { get; set; }
        public string buyFromCity { get; set; }
        public string buyFromCountry { get; set; }
        public string buyFromState { get; set; }
        public string buyFromPostCode { get; set; }
        public string shipToAddressLine1 { get; set; }
        public string shipToAddressLine2 { get; set; }
        public string shipToCity { get; set; }
        public string shipToCountry { get; set; }
        public string shipToState { get; set; }
        public string shipToPostCode { get; set; }
        public string payToAddressLine1 { get; set; }
        public string payToAddressLine2 { get; set; }
        public string payToCity { get; set; }
        public string payToCountry { get; set; }
        public string payToState { get; set; }
        public string payToPostCode { get; set; }
        public string shortcutDimension1Code { get; set; }
        public string shortcutDimension2Code { get; set; }
        public string currencyId { get; set; }
        public string currencyCode { get; set; }
        public string orderId { get; set; }
        public string orderNumber { get; set; }
        public bool pricesIncludeTax { get; set; }
        public float discountAmount { get; set; }
        public bool discountAppliedBeforeTax { get; set; }
        public float totalAmountExcludingTax { get; set; }
        public float totalTaxAmount { get; set; }
        public float totalAmountIncludingTax { get; set; }
        public string status { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
    }
    #endregion

    #region BC:Create Update Purchase Invoice
    public class BCPurchaseInvoiceInsertUpdateModel
    {
        public string? invoiceDate { get; set; }
        public string? postingDate { get; set; }
        public string? dueDate { get; set; }
        public string? vendorInvoiceNumber { get; set; }
        public string? vendorId { get; set; }
        public string? vendorNumber { get; set; }
        public string? payToVendorId { get; set; }
        public string? payToVendorNumber { get; set; }
        public string? shipToName { get; set; }
        public string? shipToContact { get; set; }
        public string? buyFromAddressLine1 { get; set; }
        public string? buyFromAddressLine2 { get; set; }
        public string? buyFromCity { get; set; }
        public string? buyFromCountry { get; set; }
        public string? buyFromState { get; set; }
        public string? buyFromPostCode { get; set; }
        public string? shipToAddressLine1 { get; set; }
        public string? shipToAddressLine2 { get; set; }
        public string? shipToCity { get; set; }
        public string? shipToCountry { get; set; }
        public string? shipToState { get; set; }
        public string? shipToPostCode { get; set; }
        public string? currencyId { get; set; }
        public string? currencyCode { get; set; }
        public bool? pricesIncludeTax { get; set; }
        public float? discountAmount { get; set; }
        public bool? discountAppliedBeforeTax { get; set; }
        public float? totalAmountIncludingTax { get; set; }
    }
    #endregion


    #region BC:Get Purchase Invoice Line
    public class BCPurchaseInvoiceLineList
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        public List<BCPurchaseInvoiceLine> value { get; set; }
    }

    public class BCPurchaseInvoiceLine
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
        public float unitCost { get; set; }
        public float quantity { get; set; }
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
        public string expectedReceiptDate { get; set; }
        public string itemVariantId { get; set; }
        public string locationId { get; set; }
    }
    #endregion

    #region BC:Create Update Purchase Invoice Line
    public class BCPurchaseInvoiceLineInsertUpdateModel
    {
        public string? accountId { get; set; }
        public string? lineType { get; set; }
        public string? lineObjectNumber { get; set; }
        public string? description { get; set; }
        public string? description2 { get; set; }
        public string? unitOfMeasureId { get; set; }
        public string? unitOfMeasureCode { get; set; }
        public float? unitCost { get; set; }
        public float? quantity { get; set; }
        public float? discountAmount { get; set; }
        public float? discountPercent { get; set; }
        public bool? discountAppliedBeforeTax { get; set; }
        public float? amountExcludingTax { get; set; }
        public string? taxCode { get; set; }
        public float? amountIncludingTax { get; set; }
        public float? invoiceDiscountAllocation { get; set; }
        public float? netAmount { get; set; }
        public float? netTaxAmount { get; set; }
        public float? netAmountIncludingTax { get; set; }
        public string? expectedReceiptDate { get; set; }
        public string? itemVariantId { get; set; }
        public string? locationId { get; set; }
    }
    #endregion


}
