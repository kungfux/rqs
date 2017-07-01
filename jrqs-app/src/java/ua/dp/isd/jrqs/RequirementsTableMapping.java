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
 *  Alexander Fuks <mailto:Alexander.V.Fuks@gmail.com>, 01 July 2017.
 */
package ua.dp.isd.jrqs;

public final class RequirementsTableMapping {
    private static final String ROWID = "id";
    private static final String REQ_NUMBER = "fr_id";
    private static final String TMS_TASK = "fr_tms_task";
    private static final String OBJECT_NUMBER = "fr_object";
    private static final String TEXT = "fr_text";
    private static final String CCP = "ccp";
    private static final String CREATED = "created";
    private static final String MODIFIED = "modified";
    private static final String STATUS = "status";
    private static final String BOUNDARY = "boundary";
    private static final String SOURCE = "source";
    
    private RequirementsTableMapping() {
        // disallow to instantiate
    }
    
    public static String getRowIdColumnName() {
        return ROWID;
    }
    
    public static String getRequirementNumberColumnName() {
        return REQ_NUMBER;
    }
    
    public static String getTmsTaskColumnName() {
        return TMS_TASK;
    }
    
    public static String getObjectNumberColumnName() {
        return OBJECT_NUMBER;
    }
    
    public static String getTextColumnName() {
        return TEXT;
    }
    
    public static String getCcpColumnName() {
        return CCP;
    }
    
    public static String getCreatedColumnName() {
        return CREATED;
    }
    
    public static String getModifiedColumnName() {
        return MODIFIED;
    }
    
    public static String getStatusColumnName() {
        return STATUS;
    }
    
    public static String getBoundaryColumnName() {
        return BOUNDARY;
    }
    
    public static String getSourceColumnName() {
        return SOURCE;
    }
}
