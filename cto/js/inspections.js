function OnFail(result) {
    window.location.href = "/genericError.html";
}

var ctoUrl = "http://imsd.hres.ca/cto/handler/";
//var ctoUrl = "../handler/";

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function goSearchUrl(lang) {
    var estName = getParameterByName("estName");
    var ref = getParameterByName("ref");
    var prov = getParameterByName("prov");
    var rate = getParameterByName("rate");
    var reg = getParameterByName("reg");
    var regNum = getParameterByName("regNum");
    var act = getParameterByName("act");
    var insFrom = getParameterByName("startDate");
    var insTo = getParameterByName("endDate");
    var cat = getParameterByName("cat");

    var searchUrl = ctoUrl + "searchResult.ashx?estName=" + estName +
        "&ref=" + ref +
        "&prov=" + prov +
        "&rate=" + rate +
        "&reg=" + reg +
        "&regNum=" + regNum +
        "&act=" + act +
        "&insFrom=" + insFrom +
        "&insTo=" + insTo +
        "&cat=" + cat +
        "&lang=" + lang;
    return searchUrl;
}


function goSearchLangUrl(lang) {
    var estName = getParameterByName("estName");
    var ref = getParameterByName("ref");
    var eType = getParameterByName("eType");
    var prov = getParameterByName("prov");
    var rate = getParameterByName("rate");
    var reg = getParameterByName("reg");
    var regNum = getParameterByName("regNum");
    var act = getParameterByName("act");
    var insFrom = getParameterByName("startDate");
    var insTo = getParameterByName("endDate");
    var cat = getParameterByName("cat");
    var langSwitch = lang == 'en' ? "fr" : "en";
    var langUrl = "searchResult-" + langSwitch + ".html?estName=" + estName +
        "&ref=" + ref +       
        "&prov=" + prov +
        "&rate=" + rate +
        "&reg=" + reg +
        "&regNum=" + regNum +
        "&act=" + act +
        "&insFrom=" + insFrom +
        "&insTo=" + insTo +
        "&cat=" + cat +
        "&lang=" + langSwitch;
    return langUrl;
}


function formatedAddress(data) {
        var address;
        if ($.trim(data.street) == '') {
            return "";
        }
        address = data.street + "<br />";

        if (data.city != '') {
            address += data.city + ", ";
        }
        if (data.province != '') {
            address += data.province + "<br />"
        }
        if (data.country != '') {
            address += data.country;
        }
        if (data.postalCode != '') {
            address += ", " + data.postalCode;
        }

        if (address != '') {
            address = address.replace("undefined", "");
            return address;
        }
        return "&nbsp;";
    }

   

function formatedCurrentlyRegistered(currentlyRegistered, lang) {
    if ($.trim(currentlyRegistered) == '')
        {
            return "";
        }
        var YesOui = lang == "en" ? "Yes" : "Oui";
        var NoNon = lang == "en" ? "No" : "Non";
        return currentlyRegistered ? YesOui : NoNon;
    }

    function displayTableList(data, lang) {
        if (data.length == 0) {
            return "";
        }
        var txt = "";
        var i;
        for (i = 0; i < data.length; i++) {
            if ($.trim(data[i].inspectionStartDate) != '') {
                txt += "<tr><td>" + formatedDate(data[i].inspectionStartDate) + "</td>";
            }
               
            if ($.trim(data[i].rating) != '') {
                if (data[i].rating.toLowerCase() == 'i') {
                    if (data[i].reportCard) {
                        txt += "<td><a href=initialReportCard-" + lang + ".html?lang=" + lang + "&insNumber=" + data[i].insepctionNumber + ">" + data[i].ratingDesc + "</a></td>";
                    }
                    else {
                        txt += "<td>" + data[i].ratingDesc + "</td>";
                    }
                }              
                else {
                    if (data[i].reportCard) {
                        txt += "<td><a href=fullReportCard-" + lang + ".html?lang=" + lang + "&insNumber=" + data[i].insepctionNumber + ">" + data[i].ratingDesc + "</a></td>";
                    }
                    else {
                        txt += "<td>" + data[i].ratingDesc + "</td>";
                    }
                }
            }
            else {
                txt += "<td></td>";
            }
                
            if ($.trim(data[i].inspectionType) != '') {
                txt += "<td>" + data[i].inspectionType + "</td></tr>";
            }
        }

        if (txt != '') {
            txt = txt.replace("undefined", "");
            return txt;
        }
        return "&nbsp;";
    }



    function formatedDate(data) {
        if ($.trim(data) == '') {
            return "";
        }
        var data = data.replace("/Date(", "").replace(")/", "");
        if (data.indexOf("+") > 0) {
            data = data.substring(0, data.indexOf("+"));
        }
        else if (data.indexOf("-") > 0) {
            data = data.substring(0, data.indexOf("-"));
        }
        var date = new Date(parseInt(data, 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        return date.getFullYear() + "-" + month + "-" + currentDate;
    }

    function formatedEstablishmentName(insNumber, establishmentName, lang) {
        return '<a href=inspectionDetail-' + lang + '.html?lang=' + lang + '&insNumber=' + insNumber + '>' + establishmentName + '</a>';
    }

    function displayRating(data, lang) {
        if ($.trim(data.rating) == '') {
            return "";
        }

        if (data.reportCard) {
            if (data.rating.toLowerCase() == 'i') {
                    return '<a href=initialReportCard-' + lang + '.html?lang=' + lang + '&insNumber=' + data.insepctionNumber + '>' + data.ratingDesc + '</a>';
            }
            else {           
                if (data.rating.toLowerCase() == 'sr') {
                    return '<a href=./data/report/' + data.insepctionNumber + lang + '.pdf target=\"_blank\">' + data.ratingDesc + '</a>';
                }
                else {
                    return '<a href=fullReportCard-' + lang + '.html?lang=' + lang + '&insNumber=' + data.insepctionNumber + '>' + data.ratingDesc + '</a>';
                }
            }
        }
        return data.ratingDesc;
    }


    function formatedSummaryList(data) {
        var displaySummary;
        if ($.trim(data) == '') {
            return "";
        }

        $.each(data, function (index, record) {
            displaySummary += "<li>" + record + "</li>";
        });

        if (displaySummary != '') {
            displaySummary = displaySummary.replace("undefined", "");
            return "<ul>" + displaySummary + "</ul>";;
        }
        return "";
    }
    function displayOrderedList(data) {
        var displaySummary;
        if ($.trim(data) == '') {
            return "";
        }
        $.each(data, function (index, record) {
            displaySummary += "<li>" + record + "</li>";
        });

        if (displaySummary != '') {
            displaySummary = displaySummary.replace("undefined", "");
            return "<ul>" + displaySummary + "</ul>";;
        }
        return "";
    }
    function displayOutcomeList(data) {
        var displaySummary;
        if ($.trim(data) == '') {
            return "";
        }
        $.each(data, function (index, record) {
            if (index % 2 == 0) {
                displaySummary += record + "<br /><br />";
            }
            else {
                displaySummary += record;
            }
        });
        if (displaySummary != '') {
            displaySummary = displaySummary.replace("undefined", "");
            return displaySummary;
        }
        return "";
    }

