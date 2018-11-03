function RemoveSkill(element) {
    var all = document.getElementsByClassName('SkillClass');
    if (all.length === 1) {
        alert("At Least have 1 skill");
        return;
    }

    var skillDiv = element.parentNode;
    skillDiv.remove(skillDiv);
    var remains = document.getElementsByClassName('SkillClass');
    for (var i = 0; i < remains.length; i++) {
        remains[i].id = "SkillClassId" + i;
        remains[i].children[1].name = "[" + i + "]" + ".SkillName";
        remains[i].children[3].name = "[" + i + "]" + ".CertificationLink";
    }
}

function RemoveLocation(element) {
    var all = document.getElementsByClassName('LocationClass');

    var locationDiv = element.parentNode;
    locationDiv.remove(locationDiv);
    var remain = document.getElementsByClassName('LocationClass');
    for (var i = 0; i < remain.length; i++) {
        remain[i].id = "LocationClassId" + i;
        remain[i].children[1].name = "[" + i + "]" + ".LocationName";
        remain[i].children[3].name = "[" + i + "]" + ".PostCode";
    }
}

function RemoveSchool(element) {
    var all = document.getElementsByClassName('SchoolClass');

    var schoolDiv = element.parentNode;
    schoolDiv.remove(schoolDiv);
    var remain = document.getElementsByClassName('SchoolClass');
    for (var i = 0; i < remain.length; i++) {
        remain[i].id = "SchoolClassId" + i;
        remain[i].children[1].name = "[" + i + "]" + ".SchoolName";
        remain[i].children[3].name = "[" + i + "]" + ".CampusName";
        remain[i].children[5].name = "[" + i + "]" + ".CampusPostCode";
    }
}