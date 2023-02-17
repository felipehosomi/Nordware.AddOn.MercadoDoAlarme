using Nordware.AddOn.MercadoDoAlarme.Core.DAO;
using Nordware.AddOn.MercadoDoAlarme.Core.Model;
using SBO.Hub.DAO;
using System;
using System.Collections.Generic;

namespace Nordware.AddOn.MercadoDoAlarme.Core.BLL
{
    public class SalesProfitBLL
    {
        public List<ProfitModel> GetSalesProfit(List<DocumentItemModel> list)
        {
            CrudDAO crudDAO = new CrudDAO();
            List<ProfitModel> costList = new List<ProfitModel>();

            foreach (var item in list)
            {
                ProfitModel costModel = new ProfitModel();

                costModel = crudDAO.FillModelFromSql<ProfitModel>(String.Format(SQL.Item_GetCost, item.ItemCode));
                costModel.ItemCode = item.ItemCode;
                costModel.Quantity = item.Quantity;
                costModel.ItemPrice = item.ItemPrice - (item.ItemPrice * (item.Discount / 100.00));

                costList.Add(costModel);
            }
            return costList;
        }
    }
}
