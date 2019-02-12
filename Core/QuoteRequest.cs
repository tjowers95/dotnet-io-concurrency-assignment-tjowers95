using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core
{
    [Serializable]
    [XmlRoot("QuoteRequest")]
    public class QuoteRequest
    {
        public List<QuoteField> Fields { get; set; }
        public List<string> Symbols { get; set; }
        public int Interval { get; set; }
    }
}