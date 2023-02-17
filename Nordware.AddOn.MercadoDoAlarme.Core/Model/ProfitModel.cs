using SBO.Hub.Attributes;

namespace Nordware.AddOn.MercadoDoAlarme.Core.Model
{
    public class ProfitModel
    {
        [HubModel(UIFieldName = "Código", ColumnName = "ItemCode")]
        public string ItemCode { get; set; }

        [HubModel(UIFieldName = "Descrição", ColumnName = "Dscription")]
        public string ItemName { get; set; }

        [HubModel(UIFieldName = "PC Unit", AutoFill = false)]
        public double ItemCost { get; set; }

        [HubModel(UIFieldName = "PV Unit", ColumnName = "Price")]
        public double ItemPrice { get; set; }

        [HubModel(UIFieldName = "Quantidade", ColumnName = "Quantity")]
        public double Quantity { get; set; }

        [HubModel(UIFieldName = "PC Total", AutoFill = false)]
        public double TotalCost
        {
            get
            {
                return ItemCost * Quantity;
            }
        }

        [HubModel(UIFieldName = "PV Total", AutoFill = false)]
        public double TotalPrice
        {
            get
            {
                return ItemPrice * Quantity;
            }
        }

        [HubModel(UIFieldName = "Lucro Bruto", AutoFill = false)]
        public double Profit
        {
            get
            {
                return TotalPrice - TotalCost;
            }
        }

        [HubModel(UIFieldName = "% Lucro", AutoFill = false)]
        public double ProfitPercentage
        {
            get
            {
                return ((ItemPrice / ItemCost) - 1) * 100;
            }
        }
    }
}
