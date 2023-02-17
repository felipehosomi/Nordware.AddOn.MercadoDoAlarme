using Nordware.AddOn.MercadoDoAlarme.Core.DAO;
using Nordware.AddOn.MercadoDoAlarme.Core.Model;
using Nordware.AddOn.MercadoDoAlarme.Core.Static;
using SAPbouiCOM;
using SBO.Hub;
using SBO.Hub.Attributes;
using SBO.Hub.DAO;
using SBO.Hub.Forms;
using SBO.Hub.SBOHelpers;
using System;

namespace Nordware.AddOn.MercadoDoAlarme.Core.Forms
{
    [Form(FormIDs.CadastroItem)]
    public class FrmItemMasterData : SystemForm
    {
        public FrmItemMasterData(ItemEvent itemEvent)
        {
            ItemEventInfo = itemEvent;
        }

        public FrmItemMasterData(BusinessObjectInfo businessObjectInfo)
        {
            BusinessObjectInfo = businessObjectInfo;
        }

        //public override bool ItemEvent()
        //{
        //    if (ItemEventInfo.BeforeAction && ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
        //    {
        //        Form.DataSources.UserDataSources.Add("ud_Cost", BoDataType.dt_PRICE);
        //    }
        //    return true;
        //}

        public override bool FormDataEvent()
        {
            if (!BusinessObjectInfo.BeforeAction && BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
            {
                try
                {
                    CrudDAO crudDAO = new CrudDAO();
                    ProfitModel costModel = new ProfitModel();

                    string itemCode = Form.DataSources.DBDataSources.Item("OITM").GetValue("ItemCode", 0);
                    costModel = crudDAO.FillModelFromSql<ProfitModel>(String.Format(SQL.Item_GetCost, itemCode));
                    Form.DataSources.UserDataSources.Item("ud_Cost").Value = costModel.ItemCost.ToString();
                }
                catch (Exception ex)
                {
                    SBOApp.Application.SetStatusBarMessage(ex.Message);
                }
            }
            return true;
        }
    }
}
