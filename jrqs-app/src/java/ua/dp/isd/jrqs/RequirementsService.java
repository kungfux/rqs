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

    public RequirementsService(String by, String value, String only) {
        rawParamsSearchBy = by;
        rawParamsUserInput = value;
        rawParamsLimitBySource = only;
    }

    public List<Requirement> getRequirements() {
        if (rawParamsSearchBy == null || rawParamsSearchBy.equals("")) {
            throw new IllegalArgumentException("Invalid request: by parameter is missing in the request.");
        }
        SearchBy by = detectSearchBy(rawParamsSearchBy);
        RequirementsDAO dao = new RequirementsDAO();
        return dao.getRequirements(by, rawParamsUserInput, rawParamsLimitBySource);
    }
    
    private SearchBy detectSearchBy(String by) {
        switch (by) {
            case "rowid":
                return SearchBy.ROWID;
            case "id":
                return SearchBy.FRID;
            case "tms":
               return SearchBy.TMSTask;
            case "text":
                return SearchBy.Text;
        }
        throw new IllegalArgumentException("Invalid request: inappropriate value is defined for by parameter.");
    }
}
