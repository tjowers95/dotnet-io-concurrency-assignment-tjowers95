using System;
using System.Collections.Generic;
using System.Text;
using Core;
using IEXTrading.IEXTradingSTOCK_QUOTE;

namespace Server.Stocks
{
    public class Utils
    {
        public static String QuotesToString(
            List<IIEXTradingResponse_STOCK_QUOTE_Content> quotes,
            List<QuoteField> fields
        )
        {
            var quoteString = new StringBuilder();
            foreach (var quote in quotes)
            {
                quoteString.Append("\n").Append(quote.Symbol).Append(" -");
                foreach (var field in fields)
                {
                    switch (field)
                    {
                        case QuoteField.High:
                            quoteString.Append(" High: ").Append(quote.High);
                            break;
                        case QuoteField.Low:
                            quoteString.Append(" Low: ").Append(quote.Low);
                            break;
                        case QuoteField.Close:
                            quoteString.Append(" Close: ").Append(quote.Close);
                            break;
                        case QuoteField.Open:
                            quoteString.Append(" Open: ").Append(quote.Open);
                            break;
                        case QuoteField.ChangePercent:
                            quoteString.Append(" Change Percent: ").Append(quote.ChangePercent);
                            break;
                        case QuoteField.Change:
                            quoteString.Append(" Change: ").Append(quote.Change);
                            break;
                        case QuoteField.LatestPrice:
                            quoteString.Append(" Latest Price: ").Append(quote.LatestPrice);
                            break;
                    }
                }
            }

            return quoteString.ToString();
        }
    }
}