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
<%@ taglib prefix="fn" uri="http://java.sun.com/jsp/jstl/functions" %>
<%@ page isELIgnored="false" %>

<%@page import="javax.naming.Context"%>
<%@page import="java.sql.*"%>
<%@page import="javax.sql.*"%>
<%@page import="javax.naming.InitialContext"%>
<%@page import="java.util.Date"%>
<%@page import="java.time.format.DateTimeFormatter"%>
<%@page import="java.time.LocalDateTime"%>
<%@page contentType="text/html" pageEncoding="UTF-8"%>

<!DOCTYPE html>
<html lang="en">
    <head>
        <link rel="stylesheet" href="css/bootstrap.min.css">
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title>jRQS</title>
    </head>
    <body>
        <nav class="navbar navbar-inverse">
            <div class="container-fluid">
                <div class="navbar-header">
                    <img src="favicon.png" class="glyphicon">
                </div>
                <ul class="nav navbar-nav">
                    <li class="active"><a href="#">jRQS</a></li>
                    <li><a href="#">Watcher</a></li>
                </ul>
                <ul class="nav navbar-nav navbar-right">
                    <li><a href="http://github.com/kungfux/rqs" title="Give me a star on GitHub if you enjoyed this app :)">
                            <span class="glyphicon glyphicon-star"></span> Give a star</a></li>
                    <li><a href="help.html" title="Open help">
                            <span class="glyphicon glyphicon-question-sign"></span> Help</a></li>
                </ul>
                <form class="navbar-form navbar-left" action="${pageContext.request.contextPath}/search" method="get" id="searchRequirementForm" role="form">
                    <div class="input-group">
                        <input type="text" name="phrase" id="phrase" value="${param.phrase}" autofocus 
                               class="form-control" required="true" placeholder="Type the search phrase"/>
                        <input type="hidden" name="by" id="by" value=""/>
                        <div class="input-group-btn">
                            <button class="btn btn-default" type="submit" onclick="beforeSubmit();">
                                <i class="glyphicon glyphicon-search"></i>
                            </button>
                        </div>
                    </div>
                </form>
            </div>
        </nav>

        <div class="container-fluid">
            <c:choose>
                <c:when test="${empty param.phrase}">
                    <div class="alert alert-info col-md-12">
                        <p><span class="glyphicon glyphicon-info-sign"></span>
                            Hint: Type in the search phrase or exact requirement number and push <kbd>ENTER</kbd> to search.
                            Click <a href="help.html">here</a> for details.</p>
                    </div>
                </c:when>
                <c:otherwise>
                    <h2>Search results:</h2>
                </c:otherwise>
            </c:choose>

            <c:if test="${not empty param.phrase && empty requirementsList}">
                <div class="alert alert-danger col-md-12">
                    <p><span class="glyphicon glyphicon-exclamation-sign"></span>
                        No results found!</p>
                </div>
            </c:if>

            <c:if test="${not empty param.phrase && fn:length(requirementsList) gt 99}">
                <div class="alert alert-warning col-md-12">
                    <p><p><span class="glyphicon glyphicon-exclamation-sign"></span>
                        Your request returned 100 or more search results but only first 100 are displayed!</p>
                </div>
            </c:if>

            <c:if test="${not empty requirementsList}">
                <table class="table table-striped">
                    <thread>
                        <tr>
                            <td title="Requirement Number">FR ID</td>
                            <td title="TMS Task Number">TMS Task</td>
                            <td title="Object Number">Object Number</td>
                            <td title="Text">Text</td>
                            <td title="CCP">CCP</td>
                            <td title="Created and modified dates">Created / Modified</td>
                            <td title="Are created and modified dates equal?">Is changed?</td>
                            <td title="Status">Status</td>
                            <td title="Where was requirement taken?">Source</td>
                        </tr>
                    </thread>
                    <tbody>
                        <c:forEach var="requirement" items="${requirementsList}">
                            <tr>
                                <td>${requirement.id}</td>
                                <td>
                                    <a href="search?by=tms&phrase=${requirement.tmsTask}" title="Search by TMS task">${requirement.tmsTask}</a>
                                </td>
                                <td>${requirement.objectNumber}</td>
                                <td>${requirement.text}</td>
                                <td>${requirement.ccp}</td>
                                <td>${requirement.created} | ${requirement.modified}</td>
                                <td>
                                    <label><input type="checkbox" value="" disabled ${requirement.created == requirement.modified ? "" : "checked"}></label>
                                </td>
                                <td>${requirement.status}</td>
                                <td>${requirement.source}</td>
                            </tr>
                        </c:forEach>
                    </tbody>
                </table>
            </c:if>
        </div>
    </body>
    <script>
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
    </script>
</html>
<!--
The page is generated at <%=LocalDateTime.now().format(DateTimeFormatter.ofPattern("MM-dd-yyyy HH:mm:ss.SSS"))%>
-->
