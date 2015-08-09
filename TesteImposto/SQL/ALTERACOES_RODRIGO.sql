use Teste;

SELECT * FROM NotaFiscal order by Id desc;
SELECT * FROM NotaFiscalItem order by Id desc;





/* =========================================================================================================================== */
/* EXERCÍCIO 3 - NOVOS CAMPOS NA TABELA                                                                                        */
/* =========================================================================================================================== */

ALTER TABLE [dbo].[NotaFiscalItem] ADD
	[BaseIpi] [decimal](18, 5) NULL,
	[AliquotaIpi] [decimal](18, 5) NULL,
	[ValorIpi] [decimal](18, 5) NULL
GO





/* =========================================================================================================================== */
/* EXERCÍCIO 4 - PROCEDURE                                                                                                     */
/* =========================================================================================================================== */

IF OBJECT_ID ( 'dbo.P_CONSULTA_CFOP_AGRUPADO', 'P' ) IS NOT NULL
    DROP PROCEDURE dbo.P_CONSULTA_CFOP_AGRUPADO;
GO


CREATE PROCEDURE [dbo].[P_CONSULTA_CFOP_AGRUPADO]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        CFOP,
        SUM(BASEICMS) AS [Valor Total da Base de ICMS],
        SUM(VALORICMS) AS [Valor Total do ICMS],
        SUM(BASEIPI) AS [Valor Total da Base de IPI],
        SUM(VALORIPI) AS [Valor Total do IPI]
    FROM NOTAFISCALITEM
    GROUP BY CFOP
END
GO


EXEC [dbo].[P_CONSULTA_CFOP_AGRUPADO]
GO





/* =========================================================================================================================== */
/* EXERCÍCIO 7 - NOVOS CAMPOS NA TABELA                                                                                        */
/* =========================================================================================================================== */

ALTER TABLE [dbo].[NotaFiscalItem] ADD
	[PercDesconto] [decimal](18, 5) NULL
GO





/* =========================================================================================================================== */
/* CORREÇÃO DE PROCEDURE DEVIDO À NOVOS CAMPOS                                                                                 */
/* =========================================================================================================================== */

USE [Teste]
GO
/****** Object:  StoredProcedure [dbo].[P_NOTA_FISCAL_ITEM]    Script Date: 08/06/2015 23:05:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[P_NOTA_FISCAL_ITEM]
(
	@pId int,
    @pIdNotaFiscal int,
    @pCfop varchar(5),
    @pTipoIcms varchar(20),
    @pBaseIcms decimal(18,5),
    @pAliquotaIcms decimal(18,5),
    @pValorIcms decimal(18,5),
    @pNomeProduto varchar(50),
    @pCodigoProduto varchar(20),
    @pBaseIpi decimal(18,5),
    @pAliquotaIpi decimal(18,5),
    @pValorIpi decimal(18,5),
    @pPercDesconto decimal(18,5)
)
AS
BEGIN
	IF (@pId = 0)
	BEGIN 		
		INSERT INTO [dbo].[NotaFiscalItem]
           ([IdNotaFiscal]
           ,[Cfop]
           ,[TipoIcms]
           ,[BaseIcms]
           ,[AliquotaIcms]
           ,[ValorIcms]
           ,[NomeProduto]
           ,[CodigoProduto]
           ,[BaseIpi]
           ,[AliquotaIpi]
           ,[ValorIpi]
           ,[PercDesconto])
		VALUES
           (@pIdNotaFiscal,
			@pCfop,
			@pTipoIcms,
			@pBaseIcms,
			@pAliquotaIcms,
			@pValorIcms,
			@pNomeProduto,
			@pCodigoProduto,
			@pBaseIpi,
			@pAliquotaIpi,
			@pValorIpi,
			@pPercDesconto)

		SET @pId = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE [dbo].[NotaFiscalItem]
		SET [IdNotaFiscal] = @pIdNotaFiscal
			,[Cfop] = @pCfop
			,[TipoIcms] = @pTipoIcms
			,[BaseIcms] = @pBaseIcms
			,[AliquotaIcms] = @pAliquotaIcms
			,[ValorIcms] = @pValorIcms
			,[NomeProduto] = @pNomeProduto
			,[CodigoProduto] = @pCodigoProduto
			,[BaseIpi] = @pBaseIpi
			,[AliquotaIpi] = @pAliquotaIpi
			,[ValorIpi] = @pValorIpi
			,[PercDesconto] = @pPercDesconto
		 WHERE Id = @pId
	END	    
END
