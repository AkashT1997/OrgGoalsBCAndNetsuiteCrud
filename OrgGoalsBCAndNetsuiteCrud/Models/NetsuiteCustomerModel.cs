﻿using Newtonsoft.Json;
using RestSharp;
using System.Security.Principal;

namespace OrgGoalsBCAndNetsuiteCrud.Models
{
    #region Netsuite:Get Customer
    public class NetsuiteCustomerModel
    {
        public List<NetsuiteCustomerLink> links { get; set; }
        public int count { get; set; }
        public bool hasMore { get; set; }
        public List<NetsuiteCustomerItem> items { get; set; }
        public int offset { get; set; }
        public int totalResults { get; set; }
    }

    public class NetsuiteCustomerLink
    {
        public string rel { get; set; }
        public string href { get; set; }
    }

    public class NetsuiteCustomerItem
    {
        public object[] links { get; set; }
        public string alcoholrecipienttype { get; set; }
        public string altname { get; set; }
        public string balancesearch { get; set; }
        public string companyname { get; set; }
        public string creditholdoverride { get; set; }
        public string currency { get; set; }
        public string custentity_2663_customer_refund { get; set; }
        public string custentity_2663_direct_debit { get; set; }
        public string custentity_alf_customer_hide_total_vat { get; set; }
        public string custentity_atlas_high_impact { get; set; }
        public string custentity_naw_trans_need_approval { get; set; }
        public string dateclosed { get; set; }
        public string datecreated { get; set; }
        public string defaultallocationstrategy { get; set; }
        public string defaultshippingaddress { get; set; }
        public string duplicate { get; set; }
        public string emailpreference { get; set; }
        public string emailtransactions { get; set; }
        public string entityid { get; set; }
        public string entitynumber { get; set; }
        public string entitystatus { get; set; }
        public string entitytitle { get; set; }
        public string faxtransactions { get; set; }
        public string firstorderdate { get; set; }
        public string firstsaledate { get; set; }
        public string giveaccess { get; set; }
        public string globalsubscriptionstatus { get; set; }
        public string id { get; set; }
        public string isautogeneratedrepresentingentity { get; set; }
        public string isbudgetapproved { get; set; }
        public string isinactive { get; set; }
        public string isperson { get; set; }
        public string language { get; set; }
        public string lastmodifieddate { get; set; }
        public string lastorderdate { get; set; }
        public string lastsaledate { get; set; }
        public string oncredithold { get; set; }
        public string overduebalancesearch { get; set; }
        public string printtransactions { get; set; }
        public string probability { get; set; }
        public string receivablesaccount { get; set; }
        public string searchstage { get; set; }
        public string shipcomplete { get; set; }
        public string shippingcarrier { get; set; }
        public string unbilledorderssearch { get; set; }
        public string weblead { get; set; }
        public string defaultbillingaddress { get; set; }
        public string custentity_alf_mop_default { get; set; }
        public string custentity_celigo_etail_cust_exported { get; set; }
        public string custentity_celigo_is_updated_via_shp { get; set; }
        public string parent { get; set; }
        public string custentity_celigo_etail_channel { get; set; }
        public string custentity_celigo_etail_cust_id { get; set; }
        public string custentity_celigo_shopify_store { get; set; }
        public string email { get; set; }
        public string externalid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phone { get; set; }
        public string salutation { get; set; }
        public string representingsubsidiary { get; set; }
        public string url { get; set; }
    }
    #endregion


    #region Sample request
    //    {
    //    "internalId":"1740",
    //    "firstName": "Satva",
    //    "middleName": "Customer",
    //    "lastName": "123",
    //    "companyName": "Satva Customer",
    //    "subsidiary": {
    //        "internalId": "1"
    //    }
    //}
    #endregion

}
