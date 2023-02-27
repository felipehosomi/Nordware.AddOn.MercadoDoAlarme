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

        public override bool ItemEvent()
        {
            base.ItemEvent();
            if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
            {
                if (!ItemEventInfo.BeforeAction)
                {
                    if (ItemEventInfo.ActionSuccess)
                    {
                        try
                        {
                            CrudDAO crudDAO = new CrudDAO();
                            ProfitModel costModel = new ProfitModel();

                            string itemCode = Form.DataSources.DBDataSources.Item("OITM").GetValue("ItemCode", 0);
                            if (!string.IsNullOrEmpty(itemCode))
                            {
                                costModel = crudDAO.FillModelFromSql<ProfitModel>(String.Format(SQL.Item_GetCost, itemCode));
                                Form.DataSources.UserDataSources.Item("ud_Cost").Value = costModel.ItemCost.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (!ex.Message.Contains("Collection - Out of boundaries"))
                                SBOApp.Application.SetStatusBarMessage(ex.Message);
                        }
                    }
                }
            }
            //if (ItemEventInfo.BeforeAction && ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
            //{
            //    UserDataSource udCost = null;
            //    try
            //    {
            //        udCost = Form.DataSources.UserDataSources.Item("ud_Cost");
            //    }
            //    catch
            //    {
            //        udCost = Form.DataSources.UserDataSources.Add("ud_Cost", BoDataType.dt_PRICE);
            //    }
            //}
            return true;
        }

        public override bool FormDataEvent()
        {
            base.FormDataEvent();

            if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
            {
                if (!BusinessObjectInfo.BeforeAction)
                {
                    if (BusinessObjectInfo.ActionSuccess)
                    {
                        try
                        {
                            CrudDAO crudDAO = new CrudDAO();
                            ProfitModel costModel = new ProfitModel();

                            string itemCode = Form.DataSources.DBDataSources.Item("OITM").GetValue("ItemCode", 0);
                            if (!string.IsNullOrEmpty(itemCode))
                            {
                                costModel = crudDAO.FillModelFromSql<ProfitModel>(String.Format(SQL.Item_GetCost, itemCode));
                                Form.DataSources.UserDataSources.Item("ud_Cost").Value = costModel.ItemCost.ToString(); 
                            }
                        }
                        catch (Exception ex)
                        {
                            if (!ex.Message.Contains("Collection - Out of boundaries"))
                                SBOApp.Application.SetStatusBarMessage(ex.Message);
                        }
                    }
                } 
            }

            //if (!BusinessObjectInfo.BeforeAction && BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD && BusinessObjectInfo.ActionSuccess)
            //{
            //    try
            //    {
            //        CrudDAO crudDAO = new CrudDAO();
            //        ProfitModel costModel = new ProfitModel();

            //        string itemCode = Form.DataSources.DBDataSources.Item("OITM").GetValue("ItemCode", 0);
            //        costModel = crudDAO.FillModelFromSql<ProfitModel>(String.Format(SQL.Item_GetCost, itemCode));
            //        Form.DataSources.UserDataSources.Item("ud_Cost").Value = costModel.ItemCost.ToString();
            //    }
            //    catch (Exception ex)
            //    {
            //        SBOApp.Application.SetStatusBarMessage(ex.Message);
            //        SBOApp.Application.MessageBox($"{ex.Message}\n\n{ex.StackTrace}");
            //        if (ex.InnerException != null)
            //            SBOApp.Application.MessageBox($"{ex.InnerException.Message}\n\n{ex.InnerException.StackTrace}");
            //    }
            //}
            return true;
        }
    }
}
