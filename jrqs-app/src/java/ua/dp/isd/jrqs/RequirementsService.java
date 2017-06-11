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
    
//    public List<Requirement> getAllRequirements() {
//        return new RequirementsList().getAllRequirements();
//    }
    
    public List<Requirement> getRequirementsByRequirementNumbers(String RequirementNumbers) {
        return new RequirementsList().getRequirementsByRequirementNumbers(RequirementNumbers);
    }
    
    public List<Requirement> getRequirementsByTmsTaskNumbers(String TmsTaskNumbers) {
        return new RequirementsList().getRequirementsByTmsTaskNumbers(TmsTaskNumbers);
    }
    
    public List<Requirement> getRequirementsByTextPhrases(String TextPhrases) {
        return new RequirementsList().getRequirementsByTextPhrases(TextPhrases);
    }
}
