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
        <!--<script src="js/jquery-3.2.1.min.js"></script>-->
        <!--<script src="js/bootstrap.min.js"></script>-->
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title>Java Requirement Searcher</title>
    </head>
    <body>
        <nav class="navbar navbar-inverse">
            <div class="container-fluid">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbar">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span> 
                    </button>
                    <p class="navbar-text">QA</p>
                </div>
                <div class="collapse navbar-collapse" id="myNavbar">
                    <ul class="nav navbar-nav">
                        <li class="active"><a href="#">jRQS</a></li>
                    </ul>
                    <ul class="nav navbar-nav navbar-right">
                        <li><a href="http://github.com/kungfux/rqs"><span class="glyphicon glyphicon-star"></span> Give a star</a></li>
                    </ul>
                </div>
            </div>
        </nav>
        <div class="container-fluid">
            <h2>Search Requirements</h2>
            <!-- Search form -->
            <form action="${pageContext.request.contextPath}/search" method="get" id="searchRequirementForm" role="form">
                <div class="form-group col-md-4">
                    <input type="text" name="phrase" id="phrase" value="${param.phrase}" class="form-control" required="true" placeholder="Type the search phrase"/>
                    <input type="hidden" name="by" id="by" value=""/>
                </div>
                <button type="submit" class="btn btn-info" onclick="beforeSubmit();">
                    <span class="glyphicon glyphicon-search"></span> Search
                </button>
                <br/>
                <br/>
            </form>

            <!-- Welcome message -->
            <c:if test="${empty param.phrase}">
                <div class="alert alert-info col-md-5">
                    Type in the search phrase or exact requirement number and click Search.
                    Check <a href="help.html">help</a> for details.
                </div>
            </c:if>
            
            <!-- No results found message -->
            <c:if test="${not empty param.phrase && empty requirementsList}">
                <div class="alert alert-danger col-md-5">
                    No results found!
                </div>
            </c:if>

            <!-- Search results -->
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
                                    <label><input type="checkbox" value="" disabled ${requirement.created == requirement.modified ? "checked" : ""}></label>
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
        };
    </script>
</html>
