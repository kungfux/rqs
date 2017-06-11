<%--
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
--%>

<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<%@ page isELIgnored="false" %>

<%@page import="javax.naming.Context"%>
<%@page import="java.sql.*"%>
<%@page import="javax.sql.*"%>
<%@page import="javax.naming.InitialContext"%>
<%@page import="java.util.Date"%>
<%@page contentType="text/html" pageEncoding="UTF-8"%>

<!DOCTYPE html>
<html>
    <head>
        <link rel="stylesheet" href="css/bootstrap.min.css">
        <!--<script src="js/bootstrap.min.js"></script>-->
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title>Java Requirement Searcher</title>
    </head>
    <body>
        <div class="container">
            <h2>Search Requirements</h2>
            <!-- Search form -->
            <form action="${pageContext.request.contextPath}/search" method="get" id="searchRequirementForm" role="form">
                <div class="form-group col-xs-5">
                    <input type="text" name="phrase" id="phrase" value="${param.phrase}" class="form-control" required="true" placeholder="Type the search phrase"/>
                    <input type="hidden" name="by" id="by" value=""/>
                </div>
                <button type="submit" class="btn btn-info" onclick="beforeSubmit();">
                    <span class="glyphicon glyphicon-search"></span>
                    Search
                </button>
            </form>
            <!-- End of Search form-->
            <c:choose>
                <c:when test="${not empty requirementList}">
                    <table class="table table-striped">
                        <!--<thread>-->
                        <tr>
                            <td>FR ID</td>
                            <td>TMS Task</td>
                            <td>Object Number</td>
                            <td>Text</td>
                            <td>CCP</td>
                            <td>Created</td>
                            <td>Modified</td>
                            <td>Is modified?</td>
                            <td>Status</td>
                            <td>Source</td>
                        </tr>
                        <!--</thread>-->
                        <c:forEach var="requirement" items="${requirementList}">
                            <tr>
                                <td>${requirement.id}</td>
                                <td>${requirement.tmsTask}</td>
                                <td>${requirement.objectNumber}</td>
                                <td>${requirement.text}</td>
                                <td>${requirement.ccp}</td>
                                <td>${requirement.created}</td>
                                <td>${requirement.modified}</td>
                                <td>${requirement.created == requirement.modified}</td>
                                <td>${requirement.status}</td>
                                <td>${requirement.source}</td>
                            </tr>
                        </c:forEach>
                    </table>
                </c:when>
                <c:otherwise>
                    <br>
                    <div>
                        Type in the search phrase or exact requirement number. Check <a href="help.html">help</a> for details.
                    </div>
                </c:otherwise>
            </c:choose>
        </div>
    </body>
    <script type="text/javascript">
    function isSearchById(text) {
        var input = text;
        var pattern = new RegExp(/(fr|nfr)\d+/i);
        return pattern.test(input);
    }

    function isSearchByTmsTask(text) {
        var input = text;
        var pattern = new RegExp(/\w{4,7}-\d+/i);
        return pattern.test(input);
    }

    beforeSubmit = function () {
        var searchBox = document.getElementById("searchRequirementForm");
        var searchText = document.getElementById("phrase");
        var searchBy = document.getElementById("by");

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
    }
    </script>
</html>
