using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imenik.ViewModels
{
    public class DrzaveVM
    {
        public int DrzavaId { get; set; }
        public string Naziv { get; set; }
        public List<SelectListItem> Drzave { get; set; }
    }
}
