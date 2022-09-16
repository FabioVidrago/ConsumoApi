using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConsoleApi
{
    public class Trades
    {
        public int Id { get; set; }
        public int Mts { get; set; }
        public Double Amount { get; set; }
        public Double Price { get; set; }
    }
}
