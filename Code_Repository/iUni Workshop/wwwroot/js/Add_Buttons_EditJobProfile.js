function AddSkillController() {

    var skillElements = document.getElementsByClassName("SkillClass");
    var skillElementsNumber = skillElements.length;
    var skillDiv = document.getElementById("SkillsDiv");
    if (skillElementsNumber >= 10) {
        alert("At most can you add 10 skills");
        return;
    }

    //1. Create new div for new skill
    var divBox = document.createElement("div");
    divBox.className = "SkillClass";
    divBox.id = "SkillClassId"+skillElementsNumber;
    //1.1 Create space
    var space = document.createElement("p");
    space.innerText = "";
    //1.2 Create skill name box
    var inputSkillName = document.createElement("input");
    inputSkillName.setAttribute("type","text");
    inputSkillName.setAttribute("name","["+skillElementsNumber+"]"+ ".SkillName");
    inputSkillName.onkeyup = function() { getSkillName(this.value) };
    inputSkillName.setAttribute("list","skillList");
    inputSkillName.style.border = "1px solid #ff4000";
    //1.2.1 Data List
    var dataList = document.createElement("datalist");
    dataList.setAttribute("id", "skillList");
    //1.3 Create Required checkbox
    var inputSkillRequired = document.createElement("input");
    inputSkillRequired.setAttribute("type","checkbox");
    inputSkillRequired.setAttribute("name","["+skillElementsNumber+"]"+ ".SkillRequired");
    //1.4 Create Skill name label
    var inputSkillNameLabel = document.createElement("p");
    inputSkillNameLabel.innerText = "Skill Name "+skillElementsNumber;
    //1.5 Create Required label
    var inputSkillLinkLabel = document.createElement("p");
    inputSkillLinkLabel.innerText = "Required "+skillElementsNumber;
    inputSkillLinkLabel.style.marginTop = "5px";
    //1.6 Create remove button
    var buttonSkillButtonR = document.createElement("button");
    buttonSkillButtonR.type = "button";
    buttonSkillButtonR.textContent = "Remove";
    buttonSkillButtonR.style.marginTop = "0px";
    buttonSkillButtonR.style.marginBottom = "20px";
    buttonSkillButtonR.style.marginLeft = "110px";
    buttonSkillButtonR.onclick = function() {RemoveSkill(this)};
    //2. Append sub elements of new skill div
    divBox.appendChild(space);
    divBox.appendChild(inputSkillNameLabel);
    divBox.appendChild(inputSkillName);
    divBox.appendChild(inputSkillLinkLabel);
    divBox.appendChild(inputSkillRequired);
    divBox.appendChild(space);
    divBox.appendChild(buttonSkillButtonR);
    skillDiv.append(divBox);


}

function AddLocationToProfile() {

    var locationElements = document.getElementsByClassName("LocationClass");
    var locationElementsNumber = locationElements.length;
    var locationDiv = document.getElementById("LocationDiv");
    if (locationElementsNumber >= 5) {
        alert("At most can you add 5 locations");
        return;
    }

    //1. Create new div for new skill
    var divBox = document.createElement("div");
    divBox.className = "LocationClass";
    divBox.id = "LocationClassId"+locationElementsNumber;
    //1.1 Create space
    var space = document.createElement("p");
    space.innerText = "";
    //1.2 Data List for Location
    var dataList = document.createElement("datalist");
    dataList.setAttribute("id", "SuburbNameList");
    //1.2.1 Data List for Postcode
    var dataLis = document.createElement("datalist");
    dataLis.setAttribute("id", "PostCodeList");
    //1.2.2 Create Location name box
    var inputLocationName = document.createElement("input");
    inputLocationName.setAttribute("type","text");
    inputLocationName.setAttribute("name","["+locationElementsNumber+"]"+ ".LocationName");
    inputLocationName.onkeyup = function() { GetSuburbName(this.value) };
    inputLocationName.setAttribute("list","SuburbNameList");
    inputLocationName.style.border = "1px solid #ff4000";
    //1.3 Create Postcode input box
    var inputPostcode = document.createElement("input");
    inputPostcode.setAttribute("type","text");
    inputPostcode.setAttribute("name","["+locationElementsNumber+"]"+ ".PostCode");
    inputPostcode.setAttribute("list", "PostCodeList");
    inputPostcode.style.border = "1px solid #ff4000";
    //1.4 Create Skill name label
    var inputLocationNameLabel = document.createElement("p");
    inputLocationNameLabel.innerText = "Location Name "+locationElementsNumber;
    //1.5 Create Skill Certification Link label
    var inputPostcodeLabel = document.createElement("p");
    inputPostcodeLabel.innerText = "Postcode "+locationElementsNumber;
    inputPostcodeLabel.style.marginTop = "5px";
    //1.6 Create remove button
    var buttonLocationButton = document.createElement("button");
    buttonLocationButton.type = "button";
    buttonLocationButton.textContent = "Remove";
    buttonLocationButton.style.marginTop = "5px";
    buttonLocationButton.style.marginBottom = "20px";
    buttonLocationButton.onclick = function() {RemoveLocation(this)};
    //2. Append sub elements of new skill div
    divBox.appendChild(inputLocationNameLabel);
    divBox.appendChild(dataList);
    divBox.appendChild(inputLocationName);
    divBox.appendChild(inputPostcodeLabel);
    divBox.appendChild(dataLis);
    divBox.appendChild(inputPostcode);
    divBox.appendChild(space);
    divBox.appendChild(buttonLocationButton);
    locationDiv.append(divBox);
}

function AddSchoolProfile() {

    var schoolElements = document.getElementsByClassName("SchoolClass");
    var schoolElementsNumber = schoolElements.length;
    var schoolDiv = document.getElementById("SchoolsDiv");
    if (schoolElementsNumber >= 5)
    {
        alert("At most can you add 5 schools");
        return;
    }

    //1. Create new div for new skill
    var divBox = document.createElement("div");
    divBox.className = "SchoolClass";
    divBox.id = "SchoolClassId"+schoolElementsNumber;
    //1.1 Create space
    var space = document.createElement("p");
    space.innerText = "";
    //1.2 Data List for School
    var dataList = document.createElement("datalist");
    dataList.setAttribute("id", "schoolList");
    //1.2.1 Data List for Campus
    var dataLis = document.createElement("datalist");
    dataLis.setAttribute("id", "campusList");
    //1.2.2 Data List for Postcode
    var data = document.createElement("datalist");
    data.setAttribute("id", "campusPostCodeList");
    //1.3 Create School name box
    var inputSchoolName = document.createElement("input");
    inputSchoolName.setAttribute("type","text");
    inputSchoolName.setAttribute("name","["+schoolElementsNumber+"]"+ ".SchoolName");
    inputSchoolName.onkeyup = function() { getSchoolName(this.value) };
    inputSchoolName.setAttribute("list","schoolList");
    inputSchoolName.style.border = "1px solid #ff4000";
    //1.4 Create Campus Location input box
    var inputCampusLocation = document.createElement("input");
    inputCampusLocation.setAttribute("type","text");
    inputCampusLocation.setAttribute("name","["+schoolElementsNumber+"]"+ ".CampusName");
    inputCampusLocation.onkeyup = function() { getSchoolPostCode(this.value) };
    inputCampusLocation.setAttribute("list", "campusList");
    inputCampusLocation.style.border = "1px solid #ff4000";
    //1.5 Create Campus Postcode input box
    var inputCampusLocationPostCode = document.createElement("input");
    inputCampusLocationPostCode.setAttribute("type", "text");
    inputCampusLocationPostCode.setAttribute("name","["+schoolElementsNumber+"]"+ ".CampusPostCode");
    inputCampusLocationPostCode.setAttribute("list", "campusPostCodeList");
    inputCampusLocationPostCode.style.border = "1px solid #ff4000";
    //1.6 Create School name label
    var inputSchoolNameLabel = document.createElement("p");
    inputSchoolNameLabel.innerText = "Campus Name "+ schoolElementsNumber;
    //1.7 Create Campus Location label
    var inputCampusLocationLabel = document.createElement("p");
    inputCampusLocationLabel.innerText = "Location "+ schoolElementsNumber;
    inputCampusLocationLabel.style.marginTop = "5px";
    //1.8 Create Campus Postcode label
    var inputCampusLocationPostCodeLabel = document.createElement("p");
    inputCampusLocationPostCodeLabel.innerText = "Postcode " + schoolElementsNumber;
    inputCampusLocationPostCodeLabel.style.marginTop = "5px";
    //1.9 Create remove button
    var buttonSchoolButton = document.createElement("button");
    buttonSchoolButton.type = "button";
    buttonSchoolButton.textContent = "Remove";
    buttonSchoolButton.style.marginTop = "5px";
    buttonSchoolButton.style.marginBottom = "20px";
    buttonSchoolButton.onclick = function() {RemoveLocation(this)};
    //2. Append sub elements of new skill div
    divBox.appendChild(inputSchoolNameLabel);
    divBox.appendChild(dataList);
    divBox.appendChild(inputSchoolName);
    divBox.appendChild(inputCampusLocationLabel);
    divBox.appendChild(dataLis);
    divBox.appendChild(inputCampusLocation);
    divBox.appendChild(inputCampusLocationPostCodeLabel);
    divBox.appendChild(data);
    divBox.appendChild(inputCampusLocationPostCode);
    divBox.appendChild(space);
    divBox.appendChild(buttonSchoolButton);
    //var last = locationElements[locationElementsNumber - 1];
    //last.append(divBox)
    schoolDiv.append(divBox);
}