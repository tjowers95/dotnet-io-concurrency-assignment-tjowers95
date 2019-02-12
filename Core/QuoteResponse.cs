using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core
{
    [Serializable]
    [XmlRoot("QuoteResponse")]
    public class QuoteResponse
    {
        [XmlArray(ElementName = "quotes"), XmlArrayItem("quote")]
        public List<Quote> Quote;

        public QuoteResponse()
        {
            this.Quote = new List<Quote>();
        }
    }

    public class Quote
    {
        [XmlElement("symbol")]
        public string symbol;
        [XmlElement("open")]
        public string open;
        [XmlElement("high")]
        public string high;
        [XmlElement("low")]
        public string low;
        [XmlElement("close")]
        public string close;
    }
    
}