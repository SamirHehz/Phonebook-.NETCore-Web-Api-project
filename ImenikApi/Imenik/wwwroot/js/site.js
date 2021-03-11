    //prikaz gradova na onchange event combo box-a "Države", kako bi se iz baze izvukli samo oni gradovi koji se nalaze
    //u odabranoj državi
    function PrikazGradova() {

        let DrzavaId = document.getElementById("Drzave").value;

        let url = "/Home/PrikazGradova?DrzavaId=" + DrzavaId;

        let zahtjev = new XMLHttpRequest();

        zahtjev.onload = function () {

            document.getElementById("Ajax").innerHTML = zahtjev.responseText;

        };

        zahtjev.open("GET", url, true);
        zahtjev.send();

    }
    //automatsko punjenje readonly input polja starost, koje se popunjava na osnovu odabranog datuma rođenja
    // u validaciji sam naveo da kontakt mora biti stariji od 10 godina
    function UpdateStarost() {

        var datumRodjenja = document.getElementById("Datum").value;
        var godinaRodjenja = datumRodjenja.substring(0, 4);
        var danasnjiDatum = new Date();
        var danasnjaGodina = danasnjiDatum.getFullYear();

        document.getElementById("Starost").value = danasnjaGodina - godinaRodjenja;

    }
    //funkcija koja se poziva na klik buttona "Otkaži" prilikom dodavanja novog kontakta,
    //ukoliko odustanemo od istog
    function Cancel() {

    window.location.href = "/Home/Index";
    }

    //poziv kontrolera za brisanje kontakta
    function Obrisi(KontaktId) {

        var url = "/Home/Obrisi?Id=" + KontaktId;
        window.location = url;
    }

    //poziv kontrolera koji otvara formu za uređivanje kontakta
    function Uredi(KontaktId) {

        var url = "/Home/Uredi?Id=" + KontaktId;
        window.location = url;
    }
