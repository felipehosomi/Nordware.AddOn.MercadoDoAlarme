using SBO.Hub.Attributes;

namespace Nordware.AddOn.MercadoDoAlarme.Core.Model
{
    public class DocumentItemModel
    {
        [HubModel(UIFieldName = "1")]
        public string ItemCode { get; set; }

        [HubModel(UIFieldName = "11")]
        public double Quantity { get; set; }

        [HubModel(UIFieldName = "15")]
        public double Discount { get; set; }

        [HubModel(UIFieldName = "14")]
        public double ItemPrice { get; set; }
    }
}
