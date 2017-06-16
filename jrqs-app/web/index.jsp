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
        <c:choose>
            <c:when test="${empty param.value}">
                <title>jRQS - Home</title>
            </c:when>            
            <c:otherwise>
                <title>jRQS - Search results for "${param.value}"</title>
            </c:otherwise>
        </c:choose>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <link rel="stylesheet" href="css/bootstrap.min.css">
    </head>

    <body style="padding-top: 70px;">

        <noscript>
        <div class="container-fluid" style="background-color:#F44336;color:#fff;height:100px;">
            <h3><span class="glyphicon glyphicon-alert"></span> JavaScript is required</h3>
            <p>Please enable JavaScript in your browser before using jRQS.</p>
        </div>
        </noscript>

        <nav class="navbar navbar-inverse navbar-fixed-top">
            <div class="container-fluid">  
                <div class="navbar-header">
                    <a class="navbar-brand" href="${pageContext.request.contextPath}/">
                        <span class="glyphicon glyphicon-home"></span> Home</a>
                </div>

                <ul class="nav navbar-nav">                    
                    <form class="navbar-form navbar-left" action="${pageContext.request.contextPath}/search" method="get" id="searchRequirementForm" role="form">
                        <div class="input-group">
                            <span class="input-group-addon">Search</span>
                            <input type="hidden" name="by" id="by" value=""/>
                            <input type="text" name="value" id="value" value="${param.value}" 
                                   class="form-control" required="true" placeholder="Type the search phrase"/>
                            <div class="input-group-btn">
                                <button class="btn btn-default" type="submit" onclick="beforeSubmit();">
                                    <i class="glyphicon glyphicon-search"></i>
                                </button>
                            </div>
                        </div>
                    </form>
                </ul>

                <ul class="nav navbar-nav navbar-right">
                    <li>
                        <button class="btn btn-danger navbar-btn" onclick="alert('The Watcher is coming soon!');">
                            <span class="glyphicon glyphicon-eye-open"></span> Watcher
                        </button>
                    </li>
                    <li><a href="http://github.com/kungfux/rqs" title="Give me a star on GitHub if you are enjoying this app :)">
                            <span class="glyphicon glyphicon-star"></span> Star</a></li>
                    <li><a href="help.html" title="Open help">
                            <span class="glyphicon glyphicon-question-sign"></span> Help</a></li>
                </ul>
            </div>
        </nav>

        <div class="container-fluid">
            <c:choose>
                <c:when test="${empty param.value}">
                    <div class="alert alert-info col-md-12">
                        <p><span class="glyphicon glyphicon-info-sign"></span>
                            Hint: Type in the search phrase or exact requirement number and push <kbd>ENTER</kbd> to search.
                            Click <a href="help.html">here</a> for details.</p>
                    </div>
                </c:when>
                <c:otherwise>
                    <h2>Search results for "${param.value}":</h2>
                </c:otherwise>
            </c:choose>

            <c:if test="${not empty param.value && empty requirementsList}">
                <div class="alert alert-danger col-md-12">
                    <p><span class="glyphicon glyphicon-exclamation-sign"></span>
                        No results found!</p>
                </div>
            </c:if>

            <c:if test="${not empty param.value && fn:length(requirementsList) gt 99}">
                <div class="alert alert-warning col-md-12">
                    <p><span class="glyphicon glyphicon-exclamation-sign"></span>
                        Your request returned 100 or more search results but only first 100 are displayed!</p>
                </div>
            </c:if>

            <c:if test="${not empty requirementsList}">
                <table class="table table-striped">
                    <thread>
                        <th title="Action"/>
                        <th title="Requirement Number">FR ID</td>
                        <th title="TMS Task Number">TMS Task</td>
                        <th title="Object Number">Object Number</td>
                        <th title="Text">Text</td>
                        <th title="CCP">CCP</td>
                        <th title="Creation date">Created</td>
                        <th title="Last modified date">Modified</td>
                        <th title="Are created and modified dates equal?"></td>
                        <th title="Status">Status</td>
                        <th title="Boundary">Boundary</td>
                        <th title="Where was requirement taken?">Source</td>
                    </thread>
                    <tbody>
                        <c:forEach var="requirement" items="${requirementsList}">
                            <tr>
                                <td>
                                    <a href="search?by=rowid&value=${requirement.rowid}" title="Share">
                                        <span class="glyphicon glyphicon-share"></span>
                                    </a>
                                </td>
                                <td>${requirement.id}</td>
                                <td>
                                    <a href="search?by=tms&value=${requirement.tmsTask}" title="Search by the ${requirement.tmsTask} TMS task">
                                        ${requirement.tmsTask}
                                    </a>
                                </td>
                                <td>${requirement.objectNumber}</td>
                                <td>${requirement.text}</td>
                                <td>${requirement.ccp}</td>
                                <td>${requirement.created != null && !requirement.created.isEmpty() ?
                                      requirement.created : ""}</td>
                                <td>${requirement.modified != null && !requirement.modified.isEmpty() ?
                                      requirement.modified : ""}</td>
                                <td>
                                    <label>${requirement.created == requirement.modified ? "" : 
                                             "<span class=\"glyphicon glyphicon-pencil\" title=\"The requirement was changed\"></span>"}</label>
                                </td>
                                <td>${requirement.status}</td>
                                <td>${requirement.boundary}</td>
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
    </script>
</html>
<!--
The page is generated at <%=LocalDateTime.now().format(DateTimeFormatter.ofPattern("MM-dd-yyyy HH:mm:ss.SSS"))%>
-->
