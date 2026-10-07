function sorsolas(tipus) {
    fetch("http://localhost:5297/lotto", {
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

function lekeres() {
    fetch("http://localhost:5297/lotto", {
        method: "GET",
    })
        .then((valasz) => valasz.json())
        .then((adatok) => {
            for (let i = 0; i < 3; i++) {
                if (adatok[i] !== null) {
                    const szoveg = document.getElementById("szoveg" + i);

                    szoveg.textContent = adatok[i].join("\n");
                    szoveg.style.height = "auto";
                    szoveg.style.height = szoveg.scrollHeight + "px";
                    szoveg.classList.remove("rejtett");
                }
            }
        })
}
