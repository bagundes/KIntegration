CREATE OR ALTER PROCEDURE SPKS1_PRICELIST RETURNS (
	CODTABELA INTEGER,
	CODBARRA VARCHAR(13),
	PRECOVAR NUMERIC,
	PROM_VAREJO NUMERIC,
	DTINICIOPROM DATE,
	DTFIMPROM DATE,
	DESCONTO NUMERIC,
	DESCONTOBLOQ NUMERIC,
	PRECOMAX NUMERIC,
	COMISSAOV NUMERIC,
	DATA TIMESTAMP
)
AS
/*VERSAO=001*/
declare variable VNOMETABELA varchar(8);
BEGIN
   FOR SELECT R.RDB$RELATION_NAME
       FROM RDB$RELATIONS r
       WHERE r.RDB$SYSTEM_FLAG = 0 AND
             R.RDB$RELATION_NAME LIKE 'TAB%' AND
             CHARACTER_LENGTH(TRIM(r.RDB$RELATION_NAME)) = 8
       ORDER BY R.RDB$RELATION_NAME
   INTO :vNomeTabela DO
   BEGIN
       CODTABELA = CAST( SUBSTRING(vNomeTabela FROM 4 FOR CHARACTER_LENGTH(:vNomeTabela)) AS INTEGER);

       FOR EXECUTE STATEMENT '  SELECT ' ||vNomeTabela || '.CODBARRA                                                           '||
                             '        ,' ||vNomeTabela || '.PRECOVAR                                                           '||
                             '        ,' ||vNomeTabela || '.PROM_VAREJO                                                        '||
                             '        ,' ||vNomeTabela || '.DTINICIOPROM                                                       '||
                             '        ,' ||vNomeTabela || '.DTFIMPROM                                                          '||
                             '        ,' ||vNomeTabela || '.DESCONTO                                                           '||
                             '        ,(' ||vNomeTabela || '.DESCONTO + 0.01)                                                  '||
                             '        ,(' ||vNomeTabela || '.PRECOVAR +                                                        '||
                             '         ((' ||vNomeTabela || '.PRECOVAR * (SELECT COALESCE(PMVENDA,0) FROM CONFIG_BLINK))/100)) '||
                             '        ,' ||vNomeTabela || '.COMISSAOV                                                          '||
                             '        ,' ||vNomeTabela || '.DATA                                                               '||
                             '  FROM '||vNomeTabela                                                                             ||
                             ' INNER JOIN PRODUTO                                                                              '||
                             '    ON PRODUTO.CODBARRA = ' ||vNomeTabela || '.CODBARRA                                          '||
                             '   AND (PRODUTO.TIPOPRODUTO=0 OR PRODUTO.TIPOPRODUTO = 2)                                        '||
                             '   AND (PRODUTO.INATIVO = ''' || 'F' || ''')                                                     '
                             INTO :CODBARRA,
                                  :PRECOVAR,    
                                  :PROM_VAREJO, 
                                  :DTINICIOPROM,
                                  :DTFIMPROM,   
                                  :DESCONTO,    
                                  :DESCONTOBLOQ,
                                  :PRECOMAX,
                                  :COMISSAOV, 
                                  :DATA DO
                              BEGIN
                                  SUSPEND;
                              END
   END
END