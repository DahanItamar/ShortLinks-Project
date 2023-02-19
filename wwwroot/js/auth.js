// Show/hide password toggles on the auth pages.
document.querySelectorAll(".pass-toggle").forEach(function (btn) {
    const input = document.getElementById(btn.getAttribute("data-target"));
    if (!input) return;
    btn.addEventListener("click", function () {
        const show = input.type === "password";
        input.type = show ? "text" : "password";
        btn.setAttribute("aria-pressed", String(show));
        btn.classList.toggle("is-on", show);
        input.focus({ preventScroll: true });
    });
});
