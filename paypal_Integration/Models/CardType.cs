using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayPalIntegration.Models
{
    
    public enum CardType
    {
        Unknown = 0,
        MasterCard = 1,
        VISA = 2,
        Amex = 3,
        Discover = 4,
        DinersClub = 5,
        JCB = 6,
        enRoute = 7
    }
}