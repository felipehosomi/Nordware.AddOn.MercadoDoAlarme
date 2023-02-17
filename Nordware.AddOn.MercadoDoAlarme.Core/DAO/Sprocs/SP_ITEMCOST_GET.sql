ALTER PROCEDURE SP_ITEMCOST_GET
(
	@ItemCode NVARCHAR(100)
)
AS
BEGIN
	SELECT TOP 1
		OITM.ItemName,
		PCH1.Price + (PCH1.DistribSum / PCH1.Quantity) + (ISNULL(PCH4.TaxSum, 0) / PCH1.Quantity) + (ISNULL(IPF1.TtlCostLC, 0) / PCH1.Quantity) ItemCost
	FROM OPCH
		INNER JOIN PCH1 WITH(NOLOCK) ON PCH1.DocEntry = OPCH.DocEntry
		INNER JOIN OITM WITH(NOLOCK) ON OITM.ItemCode = PCH1.ItemCode
		LEFT JOIN IPF1 WITH(NOLOCK) ON IPF1.BaseEntry = PCH1.DocEntry AND IPF1.OrigLine = PCH1.LineNum AND IPF1.BaseType = PCH1.ObjType
		LEFT JOIN 
		(
			SELECT DocEntry, LineNum, SUM(TaxSum - DeductTax) TaxSum 
			FROM PCH4 WITH(NOLOCK) 
			GROUP BY DocEntry, LineNum
		) PCH4 ON PCH4.DocEntry = PCH1.DocEntry AND PCH4.LineNum = PCH1.LineNum

	WHERE PCH1.ItemCode = @ItemCode
	AND PCH1.Usage = 14
	AND OPCH.CANCELED = 'N'
	ORDER BY OPCH.DocDate DESC
END