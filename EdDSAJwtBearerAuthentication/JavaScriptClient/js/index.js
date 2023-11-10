const SSUrl = "https://localhost:44315";
const ResourceWebAPIUrl = "https://localhost:44345/";

document.getElementById("login").addEventListener("click", LoginAsync);
document.getElementById("getdata").addEventListener("click", function (){ GetDataAsync("getdata") });
document.getElementById("getadmindata").addEventListener("click", function () { GetDataAsync("getadmindata") });
document.getElementById("getaccountantdata").addEventListener("click", function (){ GetDataAsync("getaccountantdata") });
document.getElementById("getsellerdata").addEventListener("click", function () { GetDataAsync("getsellerdata") });

async function GetTokenAsync() {
    let result = {};
    const userCredentials = {
        email: document.getElementById("email").value,
        password: document.getElementById("password").value
    };

    const response = await fetch(`${SSUrl}/login`,
        {
            method: "post",
            headers:
            {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(userCredentials)
        });

    if (response.ok) {
        result = {
            "token": await response.text(),
            "errormessage": "=]"
        }
    }
    else {
        const details = await response.json();
        result = {
            "token": null,
            "errormessage": details.title
        }
    }

    return result;
}

async function LoginAsync() {
    const result = await GetTokenAsync();
    if (result.token != null) {
        document.getElementById("tokenValue").value = result.token;
        const fullName = GetClaimValue(result.token, "firstName") + " " + GetClaimValue(result.token, "lastName");
        document.getElementById("fullName").innerHTML = fullName;
        const roles = GetClaimValue(result.token, "role");
        document.getElementById("roles").innerHTML = "(" + roles.join() + ")";
        document.getElementById("dataContainer").style.display = "inline";
        document.getElementById("buttonsContainer").style.display = "inline";
        document.getElementById("information").innerHTML = "";
    }    
    else {
        document.getElementById("tokenValue").value = null;
        document.getElementById("dataContainer").style.display = "none";
        document.getElementById("buttonsContainer").style.display = "none";
        document.getElementById("information").innerHTML = result.errormessage;
    }
}

function GetClaimValue(token, claimType) {
    const payLoadString = atob(token.split(".")[1]);
    const payLoad = JSON.parse(payLoadString);
    const value = payLoad[claimType];
    return value;
}

async function GetDataAsync(requestUri) {
    let result = "";
    let headers = {};
    const token = document.getElementById("tokenValue").value.trim();

    if (token) {
        headers = {
            "Content-Type": "application/json",
            "Authorization": `bearer ${token}`
        }
    }
    else {
       headers = {
            "Content-Type": "application/json"
        }
    }

    const response = await fetch(`${ResourceWebAPIUrl}${requestUri}`, { method : "get", headers : headers });

    if (response.ok) {
        result = await response.text();
    }
    else {
        result = await response.status;
    }

    document.getElementById("information").innerHTML = result;
} 