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

import java.util.Objects;

public class Requirement {
    private final long rowid;
    private final String id;
    private final String tmsTask;
    private final String objectNumber;
    private final String text;
    private final String ccp;
    private final String created;
    private final String modified;
    private final Boolean isChanged;
    private final String status;
    private final String boundary;
    private final String source;
       
    public Requirement(long rowid, String id, String tmsTask, String objectNumber, 
            String text, String ccp, String created, String modified, String status,
            String boundary, String source) {
        this.rowid = rowid;
        this.id = id;
        this.tmsTask = tmsTask;
        this.objectNumber = objectNumber;
        this.text = text;
        this.ccp = ccp;
        this.created = getNullIfEmpty(created);
        this.modified = getNullIfEmpty(modified);
        this.isChanged = isChanged(this.created, this.modified);
        this.status = status;
        this.boundary = boundary;
        this.source = source;
    }
    
    public long getRowid() {
        return rowid;
    }
    
    public String getId() {
        return id;
    }
    
    public String getTmsTask() {
        return tmsTask;
    }
    
    public String getObjectNumber() {
        return objectNumber;
    }
    
    public String getText() {
        return text;
    }
    
    public String getCcp() {
        return ccp;
    }
    
    public String getCreated() {
        return created;
    }
    
    public String getModified() {
        return modified;
    }
    
    public Boolean getIsChanged() {
        return isChanged;
    }
    
    public String getStatus() {
        return status;
    }
    
    public String getBoundary() {
        return boundary;
    }
    
    public String getSource() {
        return source;
    }
    
    private String getNullIfEmpty (String value) {
        if (value == null)
            return null;
        if ("".equals(value.trim()))
            return null;
        else
            return value;
    }
    
    private Boolean isChanged (String value1, String value2) {
        return !Objects.equals(value1, value2);
    }
}
