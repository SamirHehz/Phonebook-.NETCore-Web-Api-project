$(function () {


    $.validator.addMethod("isValidEmail", function (value, element) {
        return this.optional(element) ||
            (/^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/i).test(value);
    }, "Molimo unesite validnu email adresu.");

    $.validator.addMethod("isValidPhoneNumber", function (value, element) {
        return this.optional(element) ||
            (/\d{3}\-\d{3}\-\d{3}/).test(value) || (/\d{3}\-\d{3}\-\d{4}/).test(value);
    }, "Molimo unesite validan broj telefona.");

    $.validator.addMethod("isIme", function (value, element) {
        return this.optional(element) ||
            (/^[a-ž]+$/i).test(value) || (/\d{3}\-\d{3}\-\d{4}/).test(value);
    }, "Molimo unesite ime u validnom formatu koji sadrži samo slova.");

    $.validator.addMethod("isPrezime", function (value, element) {
        return this.optional(element) ||
            (/^[a-ž]+$/i).test(value);
    }, "Molimo unesite prezime u validnom formatu koji sadrži samo slova.");

    $.validator.addMethod("isOlderThan", function (value, element) {

        var today = new Date();
        var birthDate = new Date(value);

        var age = today.getFullYear() - birthDate.getFullYear();
        var m = today.getMonth() - birthDate.getMonth();

        if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate()))
            age--;

        if (age >= 10)
            return true;

        return false;
    });


    $("#Dodaj").validate(
        {
            rules: {
                Ime: {
                    required: true,
                    isIme: true,
                    minlength: 3,
                    maxlength: 20
                },
                Prezime:
                {
                    required: true,
                    isPrezime: true,
                    minlength: 3,
                    maxlength: 20
                },
                BrojTelefona:
                {
                    required: true,
                    isValidPhoneNumber: true
                },
                Email:
                {
                    required: true,
                    isValidEmail: true
                },
                Drzave:
                {
                    required: true,
                },
                Gradovi:
                {
                    required: true,
                },
                Spol:
                {
                    required: true
                },
                DatumRodjenja:
                {
                    required: true,
                    isOlderThan:true
                }


            },
            messages:
            {
                Ime: {
                    required: "Molimo unesite Vaše ime!",
                    minlength: "Prekratko",
                    maxlength: "Predugo"
                },
                Prezime: {
                    required: "Molimo unesite Vaše prezime!",
                    minlength: "Prekratko",
                    maxlength: "Predugo"
                },
                BrojTelefona:
                {
                    required: "Molimo unesite Vaš broj telefona!"
                },
                Email:
                {
                    required: "Molimo unesite Vaš Email!"
                },
                Drzave:
                {
                    required: "Molimo odaberite grad iz kojeg dolazite!"
                },
                Gradovi:
                {
                    required: "Molimo odaberite državu iz koje dolazite!"
                },
                Spol:
                {
                    required: "Molimo odaberite spol!"
                },
                DatumRodjenja:
                {
                    required: "Molimo odaberite Vaš datum rođenja!",
                    isOlderThan: "Trebate biti stariji od 10 godina!"
                }
            }
        });
})