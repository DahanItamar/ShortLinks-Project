let originalUrl = "";

document.getElementById("submitBtn").addEventListener("click", async function (event) {
    event.preventDefault();
    originalUrl = document.getElementById("originalURL").value;
    if (!originalUrl) {
        alert("Error, Invalid URL");
        return;
    }
    await fetch("Api/cutter", {
        method: "POST",
        body: JSON.stringify(originalUrl),
        headers: { "Content-Type": "application/json" }
    })
        .then(Response => Response.text())
        .then(UrlFromResponse => {
            if (UrlFromResponse == 'Error: Invalid URL')
                $("#shortURL").attr('placeholder', UrlFromResponse);
            else
                document.getElementById("shortURL").value = UrlFromResponse;
        })

});

document.getElementById("copyBtn").addEventListener("click", function (event) {
    event.preventDefault();
    let InputRespone = document.getElementById("shortURL").value;
    if (!InputRespone) {
        alert("Error, Empty Respone Input")
        return;
    }
    navigator.clipboard.writeText(InputRespone);
    copyToClipboard();
})

document.getElementById("cleanBTN").addEventListener("click", function (event) {
    event.preventDefault();
    document.getElementById("originalURL").value = null;
    document.getElementById("shortURL").value = null;
    $("#shortURL").attr('placeholder', null);
    originalUrl = "";
})

function copyToClipboard() {
    var message = document.createElement("div");
    message.innerHTML = "Copied to Clipboard";
    message.style.position = "fixed";
    message.style.bottom = "10px";
    message.style.right = "10px";
    message.style.padding = "10px";
    message.style.background = "transparent";
    message.style.color = "#f0ad4e";
    message.style.opacity = "0";
    message.style.transition = "opacity 2s ease-in-out";
    document.body.appendChild(message);
    setTimeout(function () {
        message.style.opacity = "1";
        setTimeout(function () {
            message.style.opacity = "0";
            setTimeout(function () {
                message.remove();
            }, 1000);
        }, 1000);
    }, 10);
}