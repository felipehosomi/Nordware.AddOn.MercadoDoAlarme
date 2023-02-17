IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_RESSARCIMENTO_ST_ENTRADA')
	DROP PROCEDURE SP_RESSARCIMENTO_ST_ENTRADA
GO

CREATE PROCEDURE SP_RESSARCIMENTO_ST_ENTRADA
(
	@ItemCode NVARCHAR(100)
)
AS
BEGIN
	SELECT TOP 3
		OPCH.U_chaveacesso	[ChaveAcesso],
		OPCH.Serial,
		PCH1.CFOPCode	[CFOP],
		OPCH.DocDate,
		OPCH.CardCode,
		OPCH.CardName,
		PCH12.TaxId0	[CNPJ],
		PCH12.StateB	[State],
		PCH1.ItemCode,
		PCH1.Dscription	[ItemName],
		PCH1.CodeBars,
		OITM.SalUnitMsr	[Uom],
		PCH1.Quantity,
		PCH1.Price,
		PCH1.LineTotal,
		ICMS.BaseSum	BaseIcms,
		ICMS.TaxRate	IcmsRate,
		ICMS.TaxSum		Icms,
		ICMS_ST.BaseSum	BaseIcmsST,
		ICMS_ST.TaxRate	IcmsRateST,
		ICMS_ST.TaxSum	IcmsST
	FROM OPCH WITH(NOLOCK)
		INNER JOIN PCH1 WITH(NOLOCK)
			ON PCH1.DocEntry = OPCH.DocEntry
		INNER JOIN OITM WITH(NOLOCK)
			ON OITM.ItemCode = PCH1.ItemCode
		INNER JOIN PCH12 WITH(NOLOCK)
			ON PCH12.DocEntry = OPCH.DocEntry
		INNER JOIN 
		(
				SELECT PCH4.DocEntry, PCH4.LineNum, MAX(TaxRate) TaxRate, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum FROM PCH4 WITH(NOLOCK)
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY PCH4.DocEntry, PCH4.LineNum
		) ICMS
			ON ICMS.DocEntry = OPCH.DocEntry
			AND ICMS.LineNum = PCH1.LineNum
		INNER JOIN 
		(
				SELECT PCH4.DocEntry, PCH4.LineNum,  MAX(TaxRate) TaxRate, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum FROM PCH4 WITH(NOLOCK)
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
				GROUP BY PCH4.DocEntry, PCH4.LineNum
		) ICMS_ST
			ON ICMS_ST.DocEntry = OPCH.DocEntry
			AND ICMS_ST.LineNum = PCH1.LineNum
		LEFT JOIN [DBInvOne].[dbo].[DocReceived] NF
			ON NF.DocEntry = OPCH.DocENtry
			AND NF.DocTypeId = 18	
	WHERE PCH1.ItemCode = @ItemCode
END