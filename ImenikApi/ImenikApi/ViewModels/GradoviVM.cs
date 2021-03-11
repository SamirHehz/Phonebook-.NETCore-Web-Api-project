using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imenik.ViewModels
{
    public class GradoviVM
    {
        public int GradId { get; set; }
        public int DrzavaId { get; set; }
        public string  Naziv { get; set; }
        public List<SelectListItem> Gradovi { get; set; }
    }
}
