using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SangataWeb.Models
{
    public class ViewModel
    {
        public Users? User_Login { get; set; }
        public List<AssetMain>? vAssetType { get; set; }
        public List<AssetMain>? vAssetModel { get; set; }
        public List<ServicePrice>? vServicePrice { get; set; }
        public List<Technician>? vTechnician { get; set; }
        public List<Technician>? vTechnicianRate { get; set; }
        public List<Project>? vProject { get; set; }
        public List<RepairGroup>? vRepairGroup { get; set; }
        public AssetMain? vAssetMain { get; set; }
        public List<Service>? vServices { get; set; }
        public List<Repair>? vRepair { get; set; }
        public Repair? vOneRepair { get; set; }
        public List<AssetMain>? vAssetMainFind { get; set; }
        public DailyRequest? vDailyRequest { get; set; }
        public List<DailyRequestLabour>? vDailyRequestLabour { get; set; }
        public List<DailyRequestMaterial>? vDailyRequestMaterial { get; set; }
        public List<Store>? vStore { get; set; }
        public Store? vOneStore { get; set; }
        public List<DailyRequest>? vRequestFind { get; set; }
        public List<ClientRef>? vClientRef { get; set; }
        public List<Location>? vLocation { get; set; }
        public List<Customer>? vCustomer { get; set; }
        public List<Customer>? vCompany { get; set; }
        public List<OrderType>? vOrderType { get; set; }
        public List<CompleteBy>? vCompleteBy { get; set; }
        public List<Repair>? vMainFind { get; set; }
        public Repair? vRepairRequest { get; set; }
        public List<DailyRequestLabour>? vRepairRequestLabour { get; set; }
        public List<DailyRequestMaterial>? vRepairRequestMaterial { get; set; }
        public List<Repair>? vRepairFind { get; set; }
        public List<CCSCode>? vCCSCode { get; set; }
        public List<CCSCodeNew>? vCCSCodeNew { get; set; }
        public List<Unit>? vUnit { get; set; }
        public List<Rack>? vRack { get; set; }
        public List<Bin>? vBin { get; set; }
        public StoreSum? vStoreSum { get; set; }
        public List<Store>? vStoreFind { get; set; }
        public List<vWINo>? vJob { get; set; }
        public List<Foreman>? vForeman { get; set; }
        public List<StoreMan>? vStoreMan { get; set; }
        public InternalReqOUT? vInternalReqOUT { get; set; }
        public List<PreparedBy>? vPreparedBy { get; set; }
        public List<Currency>? vCurrency { get; set; }
        public List<RecBy>? vRecBy { get; set; }
        public List<InternalReqSubOUT>? vInternalReqSubOUT { get; set; }
        public List<InternalReqOUT>? vIRequestOutFind { get; set; }
        public List<ListMaterialOutSummary>? vListMaterialOutSummary { get; set; }
        public List<ListMaterialOut>? vListMaterialOut { get; set; }
        public InternalReq? vInternalReq { get; set; }
        public List<InternalReqSub>? vInternalReqSub { get; set; }
        public List<InternalReq>? vIRequestInFind { get; set; }
        public List<SupplierList>? vSupplierList { get; set; }
        public Adjustment? vAdjustment { get; set; }
        public List<AdjustmentSub>? vAdjustmentSub { get; set; }
        public List<Adjustment>? vAdjustmentFind { get; set; }
        public Shipping? vShipping { get; set; }
        public List<vShippingSub>? vShippingSub { get; set; }
        public List<vStoreIssueSub>? vStoreIssueSub { get; set; }
        public List<Shipping>? vShippingFind { get; set; }
        public List<vShippingPONo>? vShippingPONo { get; set; }
        public List<FreightType>? vFreightType { get; set; }
        public List<vOrReqNo>? vOrReqNo { get; set; }
        public Docket? vDocket { get; set; }
        public List<vDocketSub>? vDocketSub { get; set; }
        public List<Docket>? vDocketFind { get; set; }
        public List<vShippingPONo>? vDocketPONo { get; set; }
        public BackCharge? vBackCharge { get; set; }
        public List<BackChargeSub>? vBackChargeSub { get; set; }
        public List<BackCharge>? vBackChargeFind { get; set; }
        public Store? vStockDetail { get; set; }
        public List<StockIn>? vStockIn { get; set; }
        public List<StockOut>? vStockOut { get; set; }
        public List<ReportMaster>? vReportMaster { get; set; }
    }
}