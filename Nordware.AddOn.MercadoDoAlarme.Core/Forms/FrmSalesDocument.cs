using Nordware.AddOn.MercadoDoAlarme.Core.Model;
using Nordware.AddOn.MercadoDoAlarme.Core.Static;
using SAPbouiCOM;
using SBO.Hub.Attributes;
using SBO.Hub.Forms;
using SBO.Hub.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using SBO.Hub;

namespace Nordware.AddOn.MercadoDoAlarme.Core.Forms
{
    [Form(FormIDs.NotaFiscalSaida)]
    [Form(FormIDs.PedidoVenda)]
    [Form(FormIDs.Cotacao)]
    public class FrmSalesDocument : SystemForm
    {
        public FrmSalesDocument(ItemEvent itemEvent)
        {
            ItemEventInfo = itemEvent;
        }

        public override bool ItemEvent()
        {
            base.ItemEvent();

            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK && ItemEventInfo.ItemUID == "bt_Profit")
                {
                    try
                    {
                        Matrix mt_Item = Form.Items.Item("38").Specific as Matrix;
                        List<DocumentItemModel> itemList = mt_Item.FillModelListFromMatrix<DocumentItemModel>();
                        //List<DocumentItemModel> itemList = Form.DataSources.DBDataSources.Item(10).FillModelListFromDBDataSource<DocumentItemModel>();
                        FrmSalesProfit frmSalesProfit = new FrmSalesProfit();
                        frmSalesProfit.Show(itemList.Where(m => !String.IsNullOrEmpty(m.ItemCode)).ToList());
                    }
                    catch (Exception ex)
                    {
                        SBOApp.Application.SetStatusBarMessage(ex.Message);
                        //SBOApp.Application.MessageBox($"{t}\n\n{ex.Message}\n\n{ex.StackTrace}");
                        //if (ex.InnerException != null)
                        //    SBOApp.Application.MessageBox($"{ex.InnerException.Message}\n\n{ex.InnerException.StackTrace}");
                    }
                }
            }

            return true;
        }
    }
}