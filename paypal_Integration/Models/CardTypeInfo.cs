using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayPalIntegration.Models
{
    // Class to hold credit card type information
    public class CardTypeInfo
    {
        public CardTypeInfo(string regEx, int length, CardType type)
        {
            RegEx = regEx;
            Length = length;
            Type = type;
        }

        public string RegEx { get; set; }
        public int Length { get; set; }
        public CardType Type { get; set; }

    }
}