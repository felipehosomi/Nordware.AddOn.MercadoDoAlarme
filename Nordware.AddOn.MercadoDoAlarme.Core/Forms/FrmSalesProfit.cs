using Nordware.AddOn.MercadoDoAlarme.Core.BLL;
using Nordware.AddOn.MercadoDoAlarme.Core.Model;
using SAPbouiCOM;
using SBO.Hub.Forms;
using SBO.Hub.UI;
using System.Collections.Generic;
using System.Linq;

namespace Nordware.AddOn.MercadoDoAlarme.Core.Forms
{
    public class FrmSalesProfit : BaseForm
    {
        private Form Form;

        public Form Show(List<DocumentItemModel> list)
        {
            Form = (Form)base.Show();
            Form.Freeze(true);
            SalesProfitBLL salesProfitBLL = new SalesProfitBLL();
            List<ProfitModel> profitList = salesProfitBLL.GetSalesProfit(list);

            Form.DataSources.DataTables.Item("dt_Profit").FillTable(profitList);
            Grid gr_Profit = Form.Items.Item("gr_Profit").Specific as Grid;
            gr_Profit.AutoResizeColumns();

            EditTextColumn cl_PvTotal = (EditTextColumn)gr_Profit.Columns.Item("PV Total");
            cl_PvTotal.ColumnSetting.SumType = BoColumnSumType.bst_Auto;

            EditTextColumn cl_PcTotal = (EditTextColumn)gr_Profit.Columns.Item("PC Total");
            cl_PcTotal.ColumnSetting.SumType = BoColumnSumType.bst_Auto;

            EditTextColumn cl_LucroBruto = (EditTextColumn)gr_Profit.Columns.Item("Lucro Bruto");
            cl_LucroBruto.ColumnSetting.SumType = BoColumnSumType.bst_Auto;

            EditTextColumn cl_LucroPercentual = (EditTextColumn)gr_Profit.Columns.Item("% Lucro");
            cl_LucroPercentual.ColumnSetting.SumType = BoColumnSumType.bst_Manual;
            cl_LucroPercentual.ColumnSetting.SumValue = (profitList.Sum(m => m.Profit) / profitList.Sum(m => m.TotalCost) * 100.00).ToString("f2");

            Form.EnableMenu("784", true);

            Form.Freeze(false);
            return Form;
        }
    }
}