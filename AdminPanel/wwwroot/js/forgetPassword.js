document.forms[0].addEventListener("submit", (event) => {
    event.preventDefault();
    var url = new URL("https://localhost:44386/api/Account/ForegetPassword");

    var u = document.getElementById("Email").value;

    var params = [["Email", u]];
    url.search = new URLSearchParams(params).toString();
    fetch(url, {
        method: "Post",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
    }).then(res => res.json()).then(res => {
        if (res.isSuccess)
            location.replace("../resetPassword.html?Email=" + u + "&Token=" + res.data);
        else {
            alert(res.message);
        }
    });
});
