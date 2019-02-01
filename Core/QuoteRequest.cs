using System;
using System.Collections.Generic;

namespace Core
{
    [Serializable]
    public class QuoteRequest
    {
        public List<QuoteField> Fields { get; set; }
        public List<string> Symbols { get; set; }
    }
}