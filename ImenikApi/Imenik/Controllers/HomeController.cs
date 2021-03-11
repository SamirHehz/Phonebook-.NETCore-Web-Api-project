using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Imenik.Models;
using Imenik.Helper;
using Imenik.ViewModels;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Net.Http.Headers;

namespace Imenik.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //Ostvarivanje veze sa api projektom
        public ImenikAPI _api = new ImenikAPI();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //prikaz postojećih kontakata u bazi, kontroler koji poziva api koji dobavlja sve kontakte iz baze
        public async Task<IActionResult> Index()
        {
            List<PrikazVM> model = new List<PrikazVM>();

            HttpClient client = _api.Inital();
            HttpResponseMessage res = await client.GetAsync("api/imenik");

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<List<PrikazVM>>(result);
            }

            return View(model);
        }

        //prikaz forme za dodavanje novog kontakta, uključujući popunjavanje combo box-a "Države"
        //i combo box-a "Gradovi" na osnovu države koja je odabrana,
        //te popunjavanje combo boxa-a spolovi
        public async Task<IActionResult> Dodaj(int KontaktId)
        {
            DodajVM model = new DodajVM();

            //Kreiranje spolova
            List<string> Spolovi = new List<string> { "Muški", "Ženski" };

            //Popunjavanje combo box-a spolovi
            model.Spolovi = Spolovi.ConvertAll
            (
                s =>
                {
                    return new SelectListItem { Value = s, Text = s };
                }
            ).ToList();

            //Kreiranje država i popunjavanje combo box-a "Države" pomoću api-a
            List<DrzaveVM> drzave = new List<DrzaveVM>();

            HttpClient client = _api.Inital();
            HttpResponseMessage res = await client.GetAsync("api/Drzave");

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                drzave = JsonConvert.DeserializeObject<List<DrzaveVM>>(result);
            };

            model.Drzave = drzave.Select
                (
                    s => new SelectListItem
                    {
                        Value = s.DrzavaId.ToString(),
                        Text = s.Naziv
                    }
                ).ToList();

            model.KontaktId = KontaktId;

            return View(model);
        }

        //popunjavanje comboboxa "Gradovi" pomoću api-a, a na osnovu prethodno odabrane države, tj. prikazat će se samo oni gradovi koji se nalaze u odabranoj državi
        public async Task<IActionResult> PrikazGradova(int DrzavaId)
        {
            List<GradoviVM> gradovi = new List<GradoviVM>();
            HttpClient client = _api.Inital();
            HttpResponseMessage res = await client.GetAsync($"api/Grad/{DrzavaId}");

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                gradovi = JsonConvert.DeserializeObject<List<GradoviVM>>(result);
            };

            GradoviVM model = new GradoviVM();

            //popunjavanje modela sa gradovima koje smo dobavili pomoću api-a
            model.Gradovi = gradovi.Select
                (
                    s => new SelectListItem
                    {
                        Value = s.GradId.ToString(),
                        Text = s.Naziv
                    }
                ).ToList();

            return View(model);
        }
        //kontroler koji poziva post api koji će dodati novi kontakt u bazu
        [HttpPost]
        public IActionResult Snimi(DodajVM m)
        {
            HttpClient client = _api.Inital();

            var url = "/api/post?Ime=" + m.Ime + "&Prezime=" + m.Prezime + "&GradId=" + m.GradId + "&DrzavaId=" + m.DrzavaId + "&DatumRodjenja=" + m.DatumRodjenja.ToString("MM.dd.yyyy") + "&Starost=" + m.Starost + "&Spol=" + m.Spol + "&BrojTelefona=" + m.BrojTelefona + "&Email=" + m.Email;
            //odradio sam na ovaj način, budući da nikako nije htjelo da odradi sa url-om "/api/post", pa sam došao do ovoga rješenja

            var postTask = client.PostAsJsonAsync(url, m);
            postTask.Wait();

            var result = postTask.Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        //kontroler koji poziva delete api, te briše odabrani kontakt
        public async Task<IActionResult> Obrisi(int Id)
        {
            HttpClient client = _api.Inital();
            HttpResponseMessage res = await client.DeleteAsync($"api/Obrisi/{Id}");
            return RedirectToAction("Index");
        }
        //kontroler koji otvara formu za uređivanje odabranog kontakta. Prvo se koristi getById api koji dobavlja informacije o odabranom kontaktu,
        //također se popunjava combobox spolovi
        public async Task<IActionResult> Uredi(int Id)
        {
            DodajVM model = new DodajVM();

            HttpClient client = _api.Inital();
            HttpResponseMessage res = await client.GetAsync($"api/GetById/{Id}");

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<DodajVM>(result);
            }

            List<string> Spolovi = new List<string> { "Muški", "Ženski" };
            model.Spolovi = Spolovi.ConvertAll
                    (
                        s =>
                        {
                            return new SelectListItem { Value = s, Text = s };
                        }
                    ).ToList();

            return View(model);
        }
        //kontroler koji poziva api za uređivanje odabranog kontakta
        [HttpPost]
        public IActionResult SpasiUredjivanje(DodajVM m)
        {
            HttpClient client = _api.Inital();

            var url = "/api/put?KontaktId="+m.KontaktId+"&Ime="+m.Ime+"&GradId="+m.GradId+"&DrzavaId="+m.DrzavaId+"&DatumRodjenja="+m.DatumRodjenja.ToString("MM.dd.yyyy")+ "&Spol="+m.Spol+"&Prezime="+m.Prezime+"&BrojTelefona="+m.BrojTelefona+"&Email="+m.Email+"&Starost="+m.Starost;
            //odradio sam na ovaj način, budući da nikako nije htjelo da odradi sa url-om "/api/post", pa sam došao do ovoga rješenja

            var putTask = client.PutAsJsonAsync(url, m);
            putTask.Wait();

            var result = putTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
