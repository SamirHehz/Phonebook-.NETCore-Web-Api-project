using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using novi=System.Web.Http;
using Imenik.EntityModels;
using Imenik.ViewModels;
using ImenikApi.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImenikApi.Controllers
{
    public class ImenikController : Controller
    {
        //konekcija na bazu
        private readonly DBContext db;
        public ImenikController(DBContext _db)
        {
            db = _db;
        }

        //api koji dobavlja sve postojeće kontakte iz baze
        [Route("api/Imenik")]
        [HttpGet]
        public List<PrikazVM> Get()
        {
            //kreiranje modela te popunjavanje istog sa podacima iz baze
            List<PrikazVM> model = new List<PrikazVM>();
            foreach (var k in db.Kontakti)
            {
                model.Add(new PrikazVM
                {
                    Ime = k.Ime,
                    Prezime = k.Prezime,
                    BrojTelefona = k.BrojTelefona,
                    DatumRodjenja = k.DatumRodjenja,
                    Email = k.Email,
                    Starost = k.Starost,
                    Spol = k.Spol,
                    Grad = db.Gradovi.Find(k.GradId).Naziv,
                    Drzava = k.Drzava,
                    KontaktId=k.Id
                });
            }
            return model;
        }

        //api koji dobavlja sve države iz baze
        [Route("api/Drzave")]
        [HttpGet]
        public List<DrzaveVM> GetDrzave()
        {
            //kreiranje modela, te popunjavanje istog podacima iz baze
            List<DrzaveVM> model = new List<DrzaveVM>();
            foreach (var d in db.Drzave)
            {
                model.Add(new DrzaveVM
                {
                    DrzavaId = d.Id,
                    Naziv = d.Naziv
                });
            }
            return model;
        }

        //api koji dobavlja sve gradove iz baze na osnovu odabrane države
        [HttpGet("api/Grad/{DrzavaId}")]
        public List<GradoviVM> GetGradovi(int DrzavaId)
        {
            List<GradoviVM> model = new List<GradoviVM>();

            //popunjavanje modela sa gradovima, ali sa filterom na DrzavaId, 
            //kako bi se iz baze povukli smo oni gradovi koji se nalaze u odabranoj državi.
            foreach (var g in db.Gradovi.Where(s=>s.DrzavaId== DrzavaId).ToList())
            {
                model.Add(new GradoviVM
                {
                    GradId = g.Id,
                    Naziv = g.Naziv,
                    DrzavaId = g.DrzavaId
                });
            }

            return model;
        }

        //api koji dodaje novi kontakt u bazu
        [Route("api/post")]
        [HttpPost]
        public IActionResult Snimi(DodajVM m)
        {
            //pravljenje nove instance Kontakt, popunjavanje informacijama, te spašavanje u bazu
            Kontakt novi = new Kontakt
            {
                Ime = m.Ime,
                Prezime = m.Prezime,
                GradId = m.GradId,
                Drzava = db.Drzave.Find(m.DrzavaId).Naziv,
                Email = m.Email,
                BrojTelefona = m.BrojTelefona,
                Spol = m.Spol,
                DatumRodjenja = m.DatumRodjenja,
                Starost = m.Starost
            };

            db.Kontakti.Add(novi);
            db.SaveChanges();

            return Ok();
        }
        //api koji briše odabrani kontakt iz baze
        [HttpDelete("api/Obrisi/{Id}")]
        public async Task<IActionResult> Obrisi(int Id)
        {
            //pronalaženje kontakta
            var kontakt = db.Kontakti.Find(Id);

            if(kontakt==null)
            {
                return NotFound();
            }

            //brisanje kontakta i spremanje promjena u bazu
            db.Kontakti.Remove(kontakt);
            await db.SaveChangesAsync();

            return NoContent();
        }
        //api koji dobavlja odabrani kontakt iz baze, na osnovu id-a, koji sam koristio da popunim formu za uređivanje kontakta,
        //tj. da popunim formu za uređivanje kontakta sa informacijama koje pripadaju odabranom kontaktu
        [HttpGet("api/GetById/{Id}")]
        public DodajVM GetById(int Id)
        {
            //pronalađenje kontakta u bazi
            Kontakt k = db.Kontakti.Find(Id);

            //popunjavanje ViewModela
            DodajVM model = new DodajVM
            {
                Ime = k.Ime,
                Prezime = k.Prezime,
                BrojTelefona = k.BrojTelefona,
                Email = k.Email,
                Starost = k.Starost,
                Spol = k.Spol,
                GradId = k.GradId,
                DrzavaId = db.Drzave.Where(s => s.Naziv == k.Drzava).FirstOrDefault().Id,
                DatumRodjenja = k.DatumRodjenja,
                KontaktId = k.Id,
                Drzave = db.Drzave.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Naziv }).ToList(),
                Gradovi = db.Gradovi.Where(s => s.DrzavaId == db.Drzave.Where(s=>s.Naziv==k.Drzava).FirstOrDefault().Id).Select(s => new SelectListItem { Value=s.Id.ToString(),Text=s.Naziv}).ToList()
            };

            return model;
        }

        //api koji služi za uređivanje odabranog kontakta
        [Route("api/put")]
        [HttpPut]
        public IActionResult Uredi(DodajVM m)
        {
            //pronalaženje kontakta u bazi
            Kontakt k = db.Kontakti.Find(m.KontaktId);

            //popunjavanje novih vrijednosti
            k.Ime = m.Ime;
            k.Prezime = m.Prezime;
            k.GradId = m.GradId;
            k.Drzava = db.Drzave.Find(m.DrzavaId).Naziv;
            k.Email = m.Email;
            k.BrojTelefona = m.BrojTelefona;
            k.Spol = m.Spol;
            k.DatumRodjenja = m.DatumRodjenja;
            k.Starost = m.Starost;
            
            //spašavanje u bazu
            db.Kontakti.Update(k);
            db.SaveChanges();
            
            return Ok();
        }
    }
}
