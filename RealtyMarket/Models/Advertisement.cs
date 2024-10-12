using RealtyMarket.Models.RealtyEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyMarket.Models
{
    public class Advertisement
    {
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Категория")]
        public RealtyCategory RealtyCategory { get; set; }

        public Realty Realty { get; set; }

    }
}
