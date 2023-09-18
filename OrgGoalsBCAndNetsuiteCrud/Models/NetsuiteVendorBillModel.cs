using Newtonsoft.Json;
using RestSharp;
using System.Security.Principal;

namespace OrgGoalsBCAndNetsuiteCrud.Models
{
    #region Netsuite:Get VendorBill
    public class NetsuiteVendorBillModel
    {
        public int count { get; set; }
        public bool hasMore { get; set; }
        public List<NetsuiteVendorBillItemId> items { get; set; }
        public int offset { get; set; }
        public int totalResults { get; set; }
    }

    public class NetsuiteVendorBillItemId
    {
        public VendorBillLink[] links { get; set; }
        public string id { get; set; }
    }

    public class VendorBillLink
    {
        public string rel { get; set; }
        public string href { get; set; }
    }
    #endregion


    #region Sample request
    //{
    //    "internalId":"1312300",
    //    "entity": {
    //        "internalId": "173"
    //    },
    //    "tranId": "SkyLane",
    //    "tranDate": "2023-08-30T04:30:00+05:30",
    //    "tranDateSpecified": true,
    //    "memo": "Sky Test 123",
    //    "currency": {
    //    "internalId": "1"
    //    },
    //    "subsidiary": {
    //    "internalId": "2"
    //    },
    //    "location": {
    //    "internalId": "2"
    //    },
    //    "expenseList": {
    //    "expense": [
    //            {
    //        "orderDoc": 0,
    //                "orderDocSpecified": false,
    //                "orderLine": 0,
    //                "orderLineSpecified": false,
    //                "category": null,
    //                "account": {
    //            "internalId": "408"
    //                },
    //                "amount": 1500,
    //                "amountSpecified": true,
    //                "taxAmount": 0,
    //                "taxAmountSpecified": false,
    //                "tax1Amt": 15,
    //                "tax1AmtSpecified": true,
    //                "memo": "test line item sky",
    //                "grossAmt": 2010,
    //                "grossAmtSpecified": true,
    //                "taxDetailsReference": null,
    //                "department": {
    //            "internalId": "1"
    //                },
    //                "class": {
    //            "internalId": "8"
    //                },
    //                "location": null,
    //                "customer": null,
    //                "taxCode": {
    //            "internalId": "5"
    //                },
    //                "taxRate2": 0,
    //                "taxRate2Specified": false,
    //                "amortizationSched": {
    //            "internalId": "1"
    //                },
    //                "amortizStartDate": "2023-08-29T04:30:00+05:30",
    //                "amortizStartDateSpecified": true,
    //                "amortizationEndDate": "2024-07-31T04:30:00+05:30",
    //                "amortizationEndDateSpecified": true,
    //                "amortizationResidual": null,
    //                "customFieldList": null
    //            },
    //			{
    //        "orderDoc": 0,
    //                "orderDocSpecified": false,
    //                "orderLine": 0,
    //                "orderLineSpecified": false,
    //                "category": null,
    //                "account": {
    //            "internalId": "408"
    //                },
    //                "amount": 1800,
    //                "amountSpecified": true,
    //                "taxAmount": 0,
    //                "taxAmountSpecified": false,
    //                "tax1Amt": 18,
    //                "tax1AmtSpecified": true,
    //                "memo": "test line item sky 2",
    //                "grossAmt": 2010,
    //                "grossAmtSpecified": true,
    //                "taxDetailsReference": null,
    //                "department": {
    //            "internalId": "1"
    //                },
    //                "class": {
    //            "internalId": "8"
    //                },
    //                "location": null,
    //                "customer": null,
    //                "taxCode": {
    //            "internalId": "5"
    //                },
    //                "taxRate2": 0,
    //                "taxRate2Specified": false,
    //                "amortizationSched": {
    //            "internalId": "1"
    //                },
    //                "amortizStartDate": "2023-08-29T04:30:00+05:30",
    //                "amortizStartDateSpecified": true,
    //                "amortizationEndDate": "2024-07-31T04:30:00+05:30",
    //                "amortizationEndDateSpecified": true,
    //                "amortizationResidual": null,
    //                "customFieldList": null
    //            }
    //        ]
    //    }
    //}
    #endregion

}
