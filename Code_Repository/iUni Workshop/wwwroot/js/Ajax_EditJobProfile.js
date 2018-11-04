function getSkillName(skillName) {
    var postCodeSelectBox = document.getElementById("skillList");
    postCodeSelectBox.innerHTML = "";
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function() {
        if (this.readyState === 4 && this.status === 200) {
            var skills = JSON.parse(this.responseText);
            for (var i = 0; i < skills.length; i++) {
                var skill = document.createElement("OPTION");
                skill.value = skills[i];
                postCodeSelectBox.appendChild(skill);
            }
        }
    };
    xmlHttp.open("GET", "/Skill/GetSkillName/" + skillName, true);
    xmlHttp.send();
}

window.onload = function getFieldName() {
    var postCodeSelectBox = document.getElementById("fieldList");
    postCodeSelectBox.innerHTML = "";
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function() {
        if (this.readyState === 4 && this.status === 200) {
            var fields = JSON.parse(this.responseText);
            for (var i = 0; i < fields.length; i++) {
                var field = document.createElement("OPTION");
                field.value = fields[i];
                field.innerText = fields[i];
                postCodeSelectBox.appendChild(field);
            }
        }
    };
    xmlHttp.open("GET", "/Field/GetAllFields/", true);
    xmlHttp.send();
}

function getSchoolName(schoolName) {
    var schoolSelectBox = document.getElementById("schoolList");
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var suburbs = JSON.parse(this.responseText);
            schoolSelectBox.innerHTML="";
            for (var i = 0; i < suburbs.length; i++) {
                var btn = document.createElement("OPTION");
                btn.value = suburbs[i];
                if (schoolName == suburbs[i]) {
                    getSchoolSuburb(schoolName);
                }
                schoolSelectBox.appendChild(btn);
            }
        }
    };
    xmlHttp.open("GET", "/School/GetSchoolName/" + schoolName, true);
    xmlHttp.send();
}

function getSchoolSuburb(schoolName) {
    var schoolSelectBox = document.getElementById("campusList");
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var suburbs = JSON.parse(this.responseText);
            schoolSelectBox.innerHTML="";
            for (var i = 0; i < suburbs.length; i++) {
                var btn = document.createElement("OPTION");
                btn.value = suburbs[i].suburbName;
                schoolSelectBox.appendChild(btn);
            }
        }
    };
    xmlHttp.open("GET", "/School/GetCampus/" + schoolName, true);
    xmlHttp.send();
}

function getSchoolPostCode(suburbName) {
    var schoolName = document.getElementById("SchoolName").value;
    var postCode = document.getElementById("CampusPostCode");
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var suburbs = JSON.parse(this.responseText);
            for (var i = 0; i < suburbs.length; i++) {
                var btn = document.createElement("OPTION");
                btn.value = suburbs[i].suburbName;
                if (suburbName == suburbs[i].suburbName) {
                    postCode.value = suburbs[i].postCode;
                }
//                    schoolSelectBox.appendChild(btn);
            }
        }
    };
    xmlHttp.open("GET", "/School/GetCampus/" + schoolName, true);
    xmlHttp.send();
}