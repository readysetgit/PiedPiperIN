using PiedPiperIN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PiedPiperIN.Models
{
    public class DashboardViewModel
    {
        public List<product> Product { get; set; }
        public List<cart_view> Cart { get; set; }
      
    }
}