using Nordware.AddOn.MercadoDoAlarme.Core.BLL;
using Nordware.AddOn.MercadoDoAlarme.Core.DAO;
using SAPbouiCOM;
using SBO.Hub;
using SBO.Hub.Forms;
using SBO.Hub.UI;
using SBO.Hub.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nordware.AddOn.MercadoDoAlarme.Core.Forms
{
    class FrmRessarcimentoST : BaseForm
    {
        Form Form;

        public override object Show()
        {
            Form = (Form)base.Show();

            ComboBox cb_Branch = (ComboBox)Form.Items.Item("cb_Branch").Specific;
            cb_Branch.AddValuesFromQuery(SQL.Branch_Get);
            cb_Branch.Select(0, BoSearchKey.psk_Index);

            return Form;
        }

        public FrmRessarcimentoST()
        {

        }

        public FrmRessarcimentoST(MenuEvent menuEvent)
        {
            MenuEventInfo = menuEvent;
        }

        public FrmRessarcimentoST(ItemEvent itemEvent)
        {
            ItemEventInfo = itemEvent;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                Form = SBOApp.Application.Forms.GetForm(ItemEventInfo.FormTypeEx, ItemEventInfo.FormTypeCount);
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "bt_Gen")
                    {
                        string branch = Form.DataSources.UserDataSources.Item("ud_Branch").Value;
                        DateTime dateFrom;
                        DateTime dateTo;

                        if (!DateTime.TryParseExact(Form.DataSources.UserDataSources.Item("ud_DtFrom").Value, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateFrom))
                        {
                            SBOApp.Application.SetStatusBarMessage("'Data de' deve ser informada");
                            return false;
                        }
                        if (!DateTime.TryParseExact(Form.DataSources.UserDataSources.Item("ud_DtTo").Value, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTo))
                        {
                            SBOApp.Application.SetStatusBarMessage("'Data até' deve ser informada");
                            return false;
                        }

                        string directory = Form.DataSources.UserDataSources.Item("ud_Dir").Value;
                        if (!Directory.Exists(directory))
                        {
                            SBOApp.Application.SetStatusBarMessage("Diretório não encontrado");
                            return false;
                        }
                        RessarcimentoSTBLL ressarcimentoSTBLL = new RessarcimentoSTBLL();
                        string fileName = Path.Combine(directory, $"RessacimentoST_{DateTime.Now.ToString("ddMMyyyy_HHmm")}.xlsx");

                        string error = ressarcimentoSTBLL.Generate(branch, dateFrom, dateTo, fileName);
                        if (!String.IsNullOrEmpty(error))
                        {
                            SBOApp.Application.SetStatusBarMessage(error);
                        }
                        else
                        {
                            if (SBOApp.Application.MessageBox($"Arquivo gerado: {fileName}. Deseja abrir o arquivo?", 1, "Sim", "Não") == 1)
                            {
                                System.Diagnostics.Process.Start(fileName);
                            }
                        }
                    }
                    if (ItemEventInfo.ItemUID == "bt_Dir")
                    {
                        DialogUtil dialogUtil = new DialogUtil();
                        Form.DataSources.UserDataSources.Item("ud_Dir").Value = dialogUtil.FolderBrowserDialog();
                    }
                }
            }
            return true;
        }
    }
}
