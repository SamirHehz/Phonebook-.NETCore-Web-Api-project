using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imenik.ViewModels
{
    public class DodajVM
    {
        public int KontaktId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Spol { get; set; }
        public string Email { get; set; }
        public string BrojTelefona { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public int Starost { get; set; }
        public int DrzavaId { get; set; }
        public int GradId { get; set; }
        public List<SelectListItem> Gradovi { get; set; }
        public List<SelectListItem> Drzave { get; set; }
        public List<SelectListItem> Spolovi { get; set; }
    }
}
