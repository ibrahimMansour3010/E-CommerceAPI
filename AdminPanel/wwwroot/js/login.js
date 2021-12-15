window.addEventListener("load", () => {
  if (localStorage.getItem("Token")) {
    location.replace("../Profile.html");
  }
});
document.forms[0].addEventListener("submit", (e) => {
  e.preventDefault();

  let u = document.getElementById("Username").value;
  let p = document.getElementById("Password").value;
  let r = document.getElementById("RememberME").checked;
  let role = document.getElementById("role").options[document.getElementById("role").selectedIndex].text;

  var url = new URL("https://localhost:44386/api/Account/Login");

  var params = [
    ["Username", u],
    ["Password", p],
    ["IsRemembered", r],
    ["UserRole", role],
  ];
  url.search = new URLSearchParams(params).toString();
  console.log(url);
  fetch(url, {
    method: "Post",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
    },
  })
    .then((res) => res.json())
    .then((res) => {
      console.log(res)
      localStorage.setItem("Token", res.data);
      alert("Welcome " + u);
      location.replace("../Profile.html");
    });
});
