function sorsolas(tipus) {
    szamok = fetch("http://localhost:5297/lotto", {
        method: "POST",
        body: JSON.stringify(tipus),
        headers: {
            "Content-type": "application/json; charset=UTF-8"
        }
    })
        .then((valasz) => valasz.json())
        .then((adatok) => {
            document.getElementById(tipus).textContent = adatok.join(", ");
        });
}
