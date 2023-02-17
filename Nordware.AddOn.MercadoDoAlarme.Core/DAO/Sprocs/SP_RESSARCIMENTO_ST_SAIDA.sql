IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_RESSARCIMENTO_ST_SAIDA')
	DROP PROCEDURE SP_RESSARCIMENTO_ST_SAIDA
GO

CREATE PROCEDURE SP_RESSARCIMENTO_ST_SAIDA
(
	@BPLId		INT,
	@DateFrom	DATETIME,
	@DateTo		DATETIME
)
AS
BEGIN
	SELECT	
		NF.KeyNfe	[ChaveAcesso],
		OINV.Serial,
		INV1.CFOPCode	[CFOP],
		OINV.DocDate,
		OINV.CardCode,
		OINV.CardName,
		INV12.TaxId0	[CNPJ],
		INV12.StateB	[State],
		INV1.ItemCode,
		INV1.Dscription	[ItemName],
		INV1.CodeBars,
		OITM.SalUnitMsr	[Uom],
		INV1.Quantity,
		INV1.Price,
		INV1.LineTotal,
		ICMS.BaseSum	BaseIcms,
		ICMS.TaxRate	IcmsRate,
		ICMS.TaxSum		Icms,
		ICMS_ST.BaseSum	BaseIcmsST,
		ICMS_ST.TaxRate	IcmsRateST,
		ICMS_ST.TaxSum	IcmsST
	FROM OINV WITH(NOLOCK)
		INNER JOIN INV1 WITH(NOLOCK)
			ON INV1.DocEntry = OINV.DocEntry
		INNER JOIN OITM WITH(NOLOCK)
			ON OITM.ItemCode = INV1.ItemCode
		INNER JOIN INV12 WITH(NOLOCK)
			ON INV12.DocEntry = OINV.DocEntry
		INNER JOIN 
		(
				SELECT INV4.DocEntry, INV4.LineNum, MAX(TaxRate) TaxRate, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum FROM INV4 WITH(NOLOCK)
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'
				GROUP BY INV4.DocEntry, INV4.LineNum
		) ICMS
			ON ICMS.DocEntry = OINV.DocEntry
			AND ICMS.LineNum = INV1.LineNum
		INNER JOIN 
		(
				SELECT INV4.DocEntry, INV4.LineNum,  MAX(TaxRate) TaxRate, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum FROM INV4 WITH(NOLOCK)
					INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = INV4.StaType
					INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'
				GROUP BY INV4.DocEntry, INV4.LineNum
		) ICMS_ST
			ON ICMS_ST.DocEntry = OINV.DocEntry
			AND ICMS_ST.LineNum = INV1.LineNum
		LEFT JOIN [DBInvOne].[dbo].[Process] NF
			ON NF.DocEntry = OINV.DocEntry
			AND NF.DocType = 13
	WHERE OINV.BPLId = @BPLId
	AND OINV.DocDate BETWEEN @DateFrom AND @DateTo
END