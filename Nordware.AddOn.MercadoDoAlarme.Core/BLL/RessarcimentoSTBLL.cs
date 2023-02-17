using ClosedXML.Excel;
using Nordware.AddOn.MercadoDoAlarme.Core.Model;
using SBO.Hub.DAO;
using SBO.Hub.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Nordware.AddOn.MercadoDoAlarme.Core.BLL
{
    public class RessarcimentoSTBLL
    {
        public string Generate(string bplId, DateTime dateFrom, DateTime dateTo, string fileName)
        {
            try
            {
                CrudDAO crudDAO = new CrudDAO();

                string sql = @" DECLARE @DateFrom DATETIME
                                DECLARE @DateTo DATETIME

                                SET @DateFrom = CAST('{1}' AS DATETIME)
                                SET @DateTo = CAST('{2}' AS DATETIME)

                                EXEC SP_RESSARCIMENTO_ST_SAIDA {0}, @DateFrom, @DateTo";

                sql = String.Format(sql, bplId, dateFrom.ToString("yyyyMMdd"), dateTo.ToString("yyyyMMdd"));

                List<RessarcimentoSTModel> listSaida = crudDAO.FillModelListFromSql<RessarcimentoSTModel>(sql);
                IEnumerable<IGrouping<string, RessarcimentoSTModel>> groupedByItem = listSaida.GroupBy(m => m.ItemCode);

                var wb = new XLWorkbook($"{Application.StartupPath}\\Excel\\RessarcimentoSTBase.xlsx");
                var ws = wb.Worksheet(1);

                int index = 2;

                foreach (var listByItem in groupedByItem)
                {
                    sql = $"EXEC SP_RESSARCIMENTO_ST_Entrada '{listByItem.Key}'";
                    sql = String.Format(sql, bplId, dateFrom.ToString("yyyyMMdd"), dateTo.ToString("yyyyMMdd"));
                    List<RessarcimentoSTModel> listEntrada = crudDAO.FillModelListFromSql<RessarcimentoSTModel>(sql);

                    DataTable dtbItem = listByItem.ToList().ConvertToDataTable<RessarcimentoSTModel>("S" + listByItem.Key);
                    dtbItem.Columns["ChaveAcesso"].ColumnName = "Chave NFe";
                    dtbItem.Columns["Serial"].ColumnName = "Nº NFe";
                    dtbItem.Columns["DocDate"].ColumnName = "Data NFe";
                    dtbItem.Columns["CardCode"].ColumnName = "Cód. Cliente";
                    dtbItem.Columns["CardName"].ColumnName = "Nome Cliente";
                    dtbItem.Columns["CNPJ"].ColumnName = "CNPJ Cliente";
                    dtbItem.Columns["State"].ColumnName = "UF Cliente";
                    dtbItem.Columns["ItemCode"].ColumnName = "Cód. Item";
                    dtbItem.Columns["ItemName"].ColumnName = "Descrição Item";
                    dtbItem.Columns["CodeBars"].ColumnName = "Código de Barras";
                    dtbItem.Columns["Uom"].ColumnName = "Unidade de Medida";
                    dtbItem.Columns["Quantity"].ColumnName = "Quantidade";
                    dtbItem.Columns["Price"].ColumnName = "Preço Unitário";
                    dtbItem.Columns["LineTotal"].ColumnName = "Valor Total";
                    dtbItem.Columns["BaseIcms"].ColumnName = "BC ICMS Próprio";
                    dtbItem.Columns["IcmsRate"].ColumnName = "Aliquota ICMS Próprio";
                    dtbItem.Columns["Icms"].ColumnName = "Valor ICMS Próprio";
                    dtbItem.Columns["BaseIcmsST"].ColumnName = "BC ICMS ST";
                    dtbItem.Columns["IcmsRateST"].ColumnName = "Aliquota ICMS ST";
                    dtbItem.Columns["IcmsST"].ColumnName = "Valor ICMS ST";

                    ws.Cell(index, "B").Value = "NF Saída";
                    ws.Range($"B{index}:V{index}").Merge();
                    ws.Cell(index, "B").Style.Font.Bold = true;
                    ws.Cell(index, "B").Style.Font.FontColor = XLColor.White;
                    ws.Cell(index, "B").Style.Fill.BackgroundColor = XLColor.DarkBlue;

                    index++;
                    ws.Cell(index, "B").InsertTable(dtbItem);

                    index += dtbItem.Rows.Count + 1;

                    DataTable dtbEntradas = listEntrada.ConvertToDataTable<RessarcimentoSTModel>("E" + listByItem.Key);
                    dtbEntradas.Columns["ChaveAcesso"].ColumnName = "Chave NFe";
                    dtbEntradas.Columns["Serial"].ColumnName = "Nº NFe";
                    dtbEntradas.Columns["DocDate"].ColumnName = "Data NFe";
                    dtbEntradas.Columns["CardCode"].ColumnName = "Cód. Cliente";
                    dtbEntradas.Columns["CardName"].ColumnName = "Nome Cliente";
                    dtbEntradas.Columns["CNPJ"].ColumnName = "CNPJ Cliente";
                    dtbEntradas.Columns["State"].ColumnName = "UF Cliente";
                    dtbEntradas.Columns["ItemCode"].ColumnName = "Cód. Item";
                    dtbEntradas.Columns["ItemName"].ColumnName = "Descrição Item";
                    dtbEntradas.Columns["CodeBars"].ColumnName = "Código de Barras";
                    dtbEntradas.Columns["Uom"].ColumnName = "Unidade de Medida";
                    dtbEntradas.Columns["Quantity"].ColumnName = "Quantidade";
                    dtbEntradas.Columns["Price"].ColumnName = "Preço Unitário";
                    dtbEntradas.Columns["LineTotal"].ColumnName = "Valor Total";
                    dtbEntradas.Columns["BaseIcms"].ColumnName = "BC ICMS Próprio";
                    dtbEntradas.Columns["IcmsRate"].ColumnName = "Aliquota ICMS Próprio";
                    dtbEntradas.Columns["Icms"].ColumnName = "Valor ICMS Próprio";
                    dtbEntradas.Columns["BaseIcmsST"].ColumnName = "BC ICMS ST";
                    dtbEntradas.Columns["IcmsRateST"].ColumnName = "Aliquota ICMS ST";
                    dtbEntradas.Columns["IcmsST"].ColumnName = "Valor ICMS ST";

                    index++;
                    ws.Cell(index, "B").Value = "NF Entrada";
                    ws.Cell(index, "B").Style.Font.Bold = true;
                    ws.Cell(index, "B").Style.Font.FontColor = XLColor.White;
                    ws.Cell(index, "B").Style.Fill.BackgroundColor = XLColor.DarkBlue;

                    ws.Range($"B{index}:V{index}").Merge();

                    index++;
                    ws.Cell(index, "B").InsertTable(dtbEntradas);

                    index += dtbEntradas.Rows.Count + 2;
                }

                ws.ShowGridLines = false;
                ws.Columns().AdjustToContents();

                wb.SaveAs(fileName);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return String.Empty;
        }


    }
}
