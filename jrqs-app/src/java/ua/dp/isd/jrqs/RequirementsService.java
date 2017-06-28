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

import java.util.List;

public class RequirementsService {

    private final String rawParamsSearchBy;
    private final String rawParamsUserInput;
    private final String rawParamsLimitBySource;
    private String errorMessage;

    public RequirementsService(String by, String value, String only) {
        rawParamsSearchBy = by;
        rawParamsUserInput = value;
        rawParamsLimitBySource = only;
    }

    public String getErrorMessage() {
        return errorMessage;
    }

    public List<Requirement> getRequirements() {
        if (rawParamsSearchBy == null || rawParamsSearchBy.equals("")) {
            errorMessage = "Invalid request: <b>by</b> parameter is missing in the search request.";
            return null;
        }

        RequirementsDAO dao = new RequirementsDAO();
        switch (rawParamsSearchBy) {
            case "rowid":
                return dao.getRequirementsByRowIds(rawParamsUserInput);
            case "id":
                return dao.getRequirementsByRequirementNumbers(rawParamsUserInput, rawParamsLimitBySource);
            case "tms":
                return dao.getRequirementsByTmsTaskNumbers(rawParamsUserInput, rawParamsLimitBySource);
            case "text":
                return dao.getRequirementsByTextPhrases(rawParamsUserInput, rawParamsLimitBySource);
            default:
                errorMessage = "Invalid request: inappropriate value is defined for search <b>by</b> parameter.";
                break;
        }
        return null;
    }
}
