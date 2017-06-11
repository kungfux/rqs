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
package ua.dp.isd.jrqs;

import java.io.IOException;
import java.util.List;
import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

//@WebServlet(
//        name = "RequirementServlet",
//        urlPatterns = {"/requirement"}
//)
@WebServlet("/search")
public class RequirementsServlet extends HttpServlet {

    RequirementsService requirementService = new RequirementsService();

    @Override
    protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
        searchRequirements(request, response);
    }

    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
        searchRequirements(request, response);
    }

    private void searchRequirements(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
        List<Requirement> result = null;
        String searchText = request.getParameter("phrase");
        String searchBy = request.getParameter("by");
        if (searchBy != null) {
            switch (searchBy) {
                case "id":
                    result = requirementService.getRequirementsByRequirementNumbers(searchText);
                    break;
                case "tms":
                    result = requirementService.getRequirementsByTmsTaskNumbers(searchText);
                    break;
                case "text":
                    result = requirementService.getRequirementsByTextPhrases(searchText);
                    break;
            }
        }
        forwardListRequirements(request, response, result);
    }

    private void forwardListRequirements(HttpServletRequest request, HttpServletResponse response, List requirementList)
            throws ServletException, IOException {
        String nextJSP = "/index.jsp";
        RequestDispatcher dispatcher = getServletContext().getRequestDispatcher(nextJSP);
        request.setAttribute("requirementList", requirementList);
        dispatcher.forward(request, response);
    }
}
