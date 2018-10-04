
function GetSuburbName(str) {
    var suburbSelectBox = document.getElementById("SuburbNameList");
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function() {
        if (this.readyState == 4 && this.status == 200) {
            var suburbs = JSON.parse(this.responseText);
            suburbSelectBox.innerHTML = "";
            for (var i = 0; i < suburbs.length; i++) {
                var btn = document.createElement("OPTION");
                btn.value = suburbs[i];
                if (str == suburbs[i]) {
                    GetPostCode(str);
                }
                suburbSelectBox.appendChild(btn);
            }
        }
    };
    xmlHttp.open("GET", "/Suburb/GetSuburb/" + str, true);
    xmlHttp.send();
}

function GetPostCode(suburbName) {
    var postCodeSelectBox = document.getElementById("PostCodeList");
    postCodeSelectBox.innerHTML = "";
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function() {
        if (this.readyState === 4 && this.status === 200) {
            var postCodes = JSON.parse(this.responseText);
            for (var i = 0; i < postCodes.length; i++) {
                var postcode = document.createElement("OPTION");
                postcode.value = postCodes[i];
                postCodeSelectBox.appendChild(postcode);
            }
        }
    };
    xmlHttp.open("GET", "/Suburb/GetPostCode/" + suburbName, true);
    xmlHttp.send();
}

function GetSchoolName(schoolName) {
    var schoolSelectBox = document.getElementById("SchoolNameList");
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function() {
        if (this.readyState === 4 && this.status === 200) {
            var names = JSON.parse(this.responseText);
            schoolSelectBox.innerHTML = "";
            for (var i = 0; i < names.length; i++) {
                var btn = document.createElement("OPTION");
                btn.value = names[i];
                if (schoolName == names[i]) {
                    GetSchoolDomainExtension(schoolName);
                }
                schoolSelectBox.appendChild(btn);
            }
        }
    };
    xmlHttp.open("GET", "/School/GetSchoolName/" + schoolName, true);
    xmlHttp.send();
}

function GetSchoolDomainExtension(schoolName) {
    var schoolSelectBox = document.getElementById("DomainList");
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function() {
        if (this.readyState === 4 && this.status === 200) {
            var extensions = JSON.parse(this.responseText);
            schoolSelectBox.innerHTML = "";
            for (var i = 0; i < extensions.length; i++) {
                var btn = document.createElement("OPTION");
                btn.value = extensions[i];
                schoolSelectBox.appendChild(btn);
            }
        }
    };
    xmlHttp.open("GET", "/School/GetSchoolDomainExtension/" + schoolName, true);
    xmlHttp.send();
}

function CleanValue(e) {
    e.value = "";
}
