using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using IEXTrading;
using IEXTrading.IEXTradingSTOCK_QUOTE;

namespace Server.Stocks
{
    public static class Api
    {
        private static Task<IIEXTradingResponse_STOCK_QUOTE> FetchQuote(string tickerSymbol)
        {
            var connection = IEXTradingConnection.Instance;
            return connection.GetQueryObject_STOCK_QUOTE().QueryAsync(tickerSymbol);
        }

        public static async Task<List<IIEXTradingResponse_STOCK_QUOTE_Content>> FetchQuotes(QuoteRequest request)
        {
            var tasks = request.Symbols.Select(FetchQuote);
            var quotes = await Task.WhenAll(tasks);
            return quotes.Select(q => q.Data).ToList();
        }
    }
}