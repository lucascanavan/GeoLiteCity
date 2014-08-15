-- 115.187.204.28 Home
-- 202.59.48.179 www.mcg.org.au
-- 27.121.64.106 www.waca.com.au
-- 210.247.239.153 www.thegabba.com.au

DECLARE @myip int

SELECT @myip = ( 16777216 * 115 ) + ( 65536 * 187 ) + ( 256 * 204 ) + 28
--SELECT @myip = ( 16777216 * 202 ) + ( 65536 * 59 ) + ( 256 * 48 ) + 179
--SELECT @myip = ( 16777216 * 27 ) + ( 65536 * 121 ) + ( 256 * 64 ) + 106
--SELECT @myip = ( 16777216 * 210 ) + ( 65536 * 247 ) + ( 256 * 239 ) + 153

DECLARE @countryCode nchar(2)
DECLARE @region nchar(2)
DECLARE @city nvarchar(255)

SELECT TOP 1 @countryCode = Country, @region = region, @city = city FROM [dbo].[GeoLiteCity-Location] l
JOIN [dbo].[GeoLiteCity-Blocks] b ON b.LocId = l.LocId
WHERE @myip BETWEEN b.StartIpNum AND b.EndIpNum

SELECT @countryCode, @region, @city
