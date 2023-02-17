using System;

namespace Nordware.AddOn.MercadoDoAlarme.Core.Model
{
    public class RessarcimentoSTModel
    {
        public string ChaveAcesso { get; set; }
        public int Serial { get; set; }
        public string CFOP { get; set; }
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string CNPJ { get; set; }
        public string State { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string CodeBars { get; set; }
        public string Uom { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double LineTotal { get; set; }
        public double BaseIcms { get; set; }
        public double Icms { get; set; }
        public double IcmsRate { get; set; }
        public double BaseIcmsST { get; set; }
        public double IcmsST { get; set; }
        public double IcmsRateST { get; set; }

    }
}
