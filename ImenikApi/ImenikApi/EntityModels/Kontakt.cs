using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imenik.EntityModels
{
    public class Kontakt
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string BrojTelefona { get; set; }
        public string Spol { get; set; }
        public string Email { get; set; }
        public int GradId { get; set; }
        public Grad Grad { get; set; }
        public string Drzava { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public int Starost { get; set; }
    }
}
