document.forms[0].addEventListener("submit", () => {
    let Email = location.href.split("?")[1].split("&")[0].split("=")[1];
    let Token = location.href.split("?")[1].split("&")[1].split("=")[1];
    var url = new URL("https://localhost:44386/api/Account/ResetPassword");

    var password = document.getElementById("Password").value;
    var ConfirmPassword = document.getElementById("ConfirmPassword").value;

    var params = [
        ["Email", Email],
        ["Token", Token],
        ["Password", password],
        ["ConfirmPassword", ConfirmPassword],
    ];
    url.search = new URLSearchParams(params).toString();
    fetch(url, {
        method: "Post",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
    })
        .then((res) => res.json())
        .catch(err => console.log(err))
        .then((res) => {
            console.log(res);
            if (res.isSuccess) {
                alert(res.message);
                location.replace("../login.html");
            }
        });
});
