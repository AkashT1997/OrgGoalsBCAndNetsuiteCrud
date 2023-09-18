using Newtonsoft.Json;
using RestSharp;
using System.Security.Principal;

namespace OrgGoalsBCAndNetsuiteCrud.Models
{
    #region Netsuite:Get Invoice
    public class NetsuiteInvoiceModel
    {
        public int count { get; set; }
        public bool hasMore { get; set; }
        public List<NetsuiteInvoiceItemId> items { get; set; }
        public int offset { get; set; }
        public int totalResults { get; set; }
    }

    public class NetsuiteInvoiceItemId
    {
        public InvoiceLink[] links { get; set; }
        public string id { get; set; }
    }

    public class InvoiceLink
    {
        public string rel { get; set; }
        public string href { get; set; }
    }
    #endregion

    #region Sample request
    //    {
    //    "entity": {
    //        "internalId": "577"
    //    },
    //    "tranDate": "2023-08-08T05:30:00+05:30",
    //    "tranDateSpecified": true,
    //    "memo": "Sky Test 123456",
    //    "currency": {
    //    "internalId": "1"
    //    },
    //    "subsidiary": {
    //    "internalId": "2"
    //    },
    //    "location": {
    //    "internalId": "2"
    //    },
    //    "dueDate": "2023-07-07T05:30:00+05:30",
    //    "dueDateSpecified": true,
    //    "itemList": {
    //    "item": [
    //            {
    //        "item": {
    //            "internalId": "770"
    //                },
    //                "description": "Sky line item 123",
    //                "amount": 50,
    //                "amountSpecified": true,
    //                "options": null,
    //                "subscriptionLine": null,
    //                "quantity": 5,
    //                "quantitySpecified": true,
    //                "units": {
    //            "internalId": "1"
    //                },
    //                "inventoryDetail": null,
    //                "serialNumbers": null,
    //                "binNumbers": null,
    //                "price": {
    //            "internalId": "1"
    //                },
    //                "department": null,
    //                "licenseCode": null,
    //                "class": null,
    //                "location": {
    //            "internalId": "12"
    //                },
    //                "grossAmt": 100,
    //                "grossAmtSpecified": true,
    //                "tax1Amt": 2.66,
    //                "tax1AmtSpecified": true,
    //                "taxCode": {
    //            "internalId": "6"
    //                },
    //                "taxRate1": 20,
    //                "taxRate1Specified": true
    //            },
    //            {
    //        "item": {
    //            "internalId": "770"
    //                },
    //                "description": "Sky line item 456",
    //                "amount": 50,
    //                "amountSpecified": true,
    //                "options": null,
    //                "subscriptionLine": null,
    //                "quantity": 5,
    //                "quantitySpecified": true,
    //                "units": {
    //            "internalId": "1"
    //                },
    //                "inventoryDetail": null,
    //                "serialNumbers": null,
    //                "binNumbers": null,
    //                "price": {
    //            "internalId": "1"
    //                },
    //                "department": null,
    //                "licenseCode": null,
    //                "class": null,
    //                "location": {
    //            "internalId": "12"
    //                },
    //                "grossAmt": 100,
    //                "grossAmtSpecified": true,
    //                "tax1Amt": 2.66,
    //                "tax1AmtSpecified": true,
    //                "taxCode": {
    //            "internalId": "6"
    //                },
    //                "taxRate1": 20,
    //                "taxRate1Specified": true
    //            }
    //        ]
    //    }
    //}
    #endregion

}
