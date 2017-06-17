/*
 *  jRQS
 *  Java Implementation of RQS
 *  Copyright (C) Alexander Fuks 2013-2017
 *  
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *  
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *  
 *  You should have received a copy of the GNU General Public License along
 *  with this program; if not, write to the Free Software Foundation, Inc.,
 *  51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
 *  
 *  Alexander Fuks, hereby disclaims all copyright
 *  interest in the program "jRQS"
 *  (which makes passes at compilers)
 *  written by Alexander Fuks.
 * 
 *  Alexander Fuks <mailto:Alexander.V.Fuks@gmail.com>, 08 June 2017.
 */

function isSearchById(text) {
    var input = text;
    var pattern = new RegExp(/(fr|nfr)\d+/i);
    return pattern.test(input);
}

function isSearchByTmsTask(text) {
    var input = text;
    var pattern = new RegExp(/\w{3,7}-\d+/i);
    return pattern.test(input);
}

overrideSubmit = function () {
    var searchBox = document.getElementById("searchRequirementForm");
    var searchText = document.getElementById("value");
    var searchBy = document.getElementById("by");

    searchText.value = searchText.value.replace(/(^\s+|\s+$)/, "").replace(/(\s+,\s+|\s+,|,\s+)/g, ",");

    if (searchText.value !== "") {
        if (isSearchById(searchText.value)) {
            searchBy.value = "id";
        } else if (isSearchByTmsTask(searchText.value)) {
            searchBy.value = "tms";
        } else {
            searchBy.value = "text";
        }
        searchBox.submit();
    }
};

function getParameterByName(name) {
    url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
    if (!results)
        return null;
    if (!results[2])
        return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function highlight() {
    if (getParameterByName("by") !== "text")
        return;
    var keywords = getParameterByName("value");
    if (keywords === null)
        return;
    keywords = keywords.split(",");
    var markInstance = new Mark(document.querySelectorAll("table tr td:nth-child(5)"));
    markInstance.mark(keywords, {
        "element": "mark",
        "separateWordSearch": false
    });
};
highlight();
