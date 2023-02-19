const originalInput = document.getElementById("originalURL");
const shortInput = document.getElementById("shortURL");
const note = document.getElementById("formNote");

function setNote(text, isError) {
    note.textContent = text || "";
    note.classList.toggle("is-error", !!isError);
}

document.getElementById("submitBtn").addEventListener("click", async function (event) {
    event.preventDefault();
    const originalUrl = originalInput.value.trim();
    if (!originalUrl) {
        setNote("Enter a URL first.", true);
        return;
    }
    setNote("Cutting…");
    try {
        const response = await fetch("Api/cutter", {
            method: "POST",
            body: JSON.stringify(originalUrl),
            headers: { "Content-Type": "application/json" }
        });
        const text = await response.text();
        if (!response.ok) {
            shortInput.value = "";
            setNote("Invalid URL — it must start with http:// or https://", true);
            return;
        }
        shortInput.value = text;
        setNote("Done. Same URL always returns the same short link.");
    } catch {
        setNote("Server unreachable.", true);
    }
});

document.getElementById("copyBtn").addEventListener("click", function () {
    if (!shortInput.value) {
        setNote("Nothing to copy yet.", true);
        return;
    }
    navigator.clipboard.writeText(shortInput.value);
    setNote("Copied to clipboard.");
});

document.getElementById("cleanBTN").addEventListener("click", function () {
    originalInput.value = "";
    shortInput.value = "";
    setNote("");
    originalInput.focus();
});
