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
            errorMessage = "The request does not contain 'by' parameter and treated as invalid!";
            return null;
        }
        switch (rawParamsSearchBy) {
            case "rowid":
                return getRequirementsByRowId(rawParamsUserInput);
            case "id":
                return getRequirementsByRequirementNumbers(rawParamsUserInput, rawParamsLimitBySource);
            case "tms":
                return getRequirementsByTmsTaskNumbers(rawParamsUserInput, rawParamsLimitBySource);
            case "text":
            default:
                return getRequirementsByTextPhrases(rawParamsUserInput, rawParamsLimitBySource);
        }
    }

    private List<Requirement> getRequirementsByRowId(String RequirementRowIds) {
        return new RequirementsDAO().getRequirementsByRowIds(RequirementRowIds);
    }

    private List<Requirement> getRequirementsByRequirementNumbers(String RequirementNumbers, String LimitBySource) {
        return new RequirementsDAO().getRequirementsByRequirementNumbers(RequirementNumbers, LimitBySource);
    }

    private List<Requirement> getRequirementsByTmsTaskNumbers(String TmsTaskNumbers, String LimitBySource) {
        return new RequirementsDAO().getRequirementsByTmsTaskNumbers(TmsTaskNumbers, LimitBySource);
    }

    private List<Requirement> getRequirementsByTextPhrases(String TextPhrases, String LimitBySource) {
        return new RequirementsDAO().getRequirementsByTextPhrases(TextPhrases, LimitBySource);
    }
}
