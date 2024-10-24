using RealtyMarket.Models.RealtyEntity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyMarket.Models
{
    public class RealtyFilter
    {
        public string AdType { get; set; }
        public string Region { get; set; }
        public double StartCost { get; set; }
        public double EndCost { get; set; }
        public RealtySort Sort { get; set; } = RealtySort.New;
    }
}
