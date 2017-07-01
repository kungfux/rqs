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

@WebServlet("/search")
public class RequirementsServlet extends HttpServlet {

    private static final String NEXT_JSP = "/index.jsp";
    private static final String SEARCH_BY_PARAM_NAME = "by";
    private static final String SEARCH_KEYWORDS_PARAM_NAME = "value";
    private static final String LIMIT_BY_SOURCE_PARAM_NAME = "only";
    private static final String REQUIREMENTS_RESPONSE_NAME = "requirementsList";
    private static final String ERROR_RESPONSE_NAME = "errorMessage";
    
    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response) 
            throws ServletException, IOException {
        doGet(request, response);
    }
    
    @Override
    protected void doGet(HttpServletRequest request, HttpServletResponse response) 
            throws ServletException, IOException {
        RequirementsService service = new RequirementsService(
                request.getParameter(SEARCH_BY_PARAM_NAME),
                request.getParameter(SEARCH_KEYWORDS_PARAM_NAME),
                request.getParameter(LIMIT_BY_SOURCE_PARAM_NAME)
        );
        try {
            List<Requirement> foundRequirements = service.getRequirements();
            request.setAttribute(REQUIREMENTS_RESPONSE_NAME, foundRequirements);
        } catch (IllegalArgumentException e) {
            request.setAttribute(ERROR_RESPONSE_NAME, e.getMessage());
        }
        forwardListRequirements(request, response);
    }
    
    private void forwardListRequirements(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        String nextJSP = NEXT_JSP;
        RequestDispatcher dispatcher = getServletContext().getRequestDispatcher(nextJSP);
        dispatcher.forward(request, response);
    }
}
