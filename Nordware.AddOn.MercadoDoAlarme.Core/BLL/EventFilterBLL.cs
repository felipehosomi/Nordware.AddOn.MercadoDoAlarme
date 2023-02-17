using Nordware.AddOn.MercadoDoAlarme.Core.Static;
using SBO.Hub.Helpers;
using SBO.Hub.SBOHelpers;

namespace Nordware.AddOn.MercadoDoAlarme.Core.BLL
{
    public class EventFilterBLL
    {
        public static void CreateEvents()
        {
            EventFilterHelper.SetFormEvent(FormIDs.RessarcimentoST, SAPbouiCOM.BoEventTypes.et_CLICK);

            EventFilterHelper.SetFormEvent(FormIDs.CadastroItem, SAPbouiCOM.BoEventTypes.et_FORM_LOAD);
            EventFilterHelper.SetFormEvent(FormIDs.NotaFiscalSaida, SAPbouiCOM.BoEventTypes.et_FORM_LOAD);
            EventFilterHelper.SetFormEvent(FormIDs.PedidoVenda, SAPbouiCOM.BoEventTypes.et_FORM_LOAD);
            EventFilterHelper.SetFormEvent(FormIDs.Cotacao, SAPbouiCOM.BoEventTypes.et_FORM_LOAD);

            EventFilterHelper.SetFormEvent(FormIDs.NotaFiscalSaida, SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterHelper.SetFormEvent(FormIDs.PedidoVenda, SAPbouiCOM.BoEventTypes.et_CLICK);
            EventFilterHelper.SetFormEvent(FormIDs.Cotacao, SAPbouiCOM.BoEventTypes.et_CLICK);

            EventFilterHelper.SetFormEvent(FormIDs.CadastroItem, SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD);

            EventFilterHelper.EnableEvents();
        }
    }
}
