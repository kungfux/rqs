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

public class Requirement {
    private long rowid;
    private String id;
    private String tmsTask;
    private String objectNumber;
    private String text;
    private String ccp;
    private String created;
    private String modified;
    private String status;
    private String boundary;
    private String source;
       
    public Requirement(long rowid, String id, String tmsTask, String objectNumber, 
            String text, String ccp, String created, String modified, String status,
            String boundary, String source) {
        this.rowid = rowid;
        this.id = id;
        this.tmsTask = tmsTask;
        this.objectNumber = objectNumber;
        this.text = text;
        this.ccp = ccp;
        this.created = created;
        this.modified = modified;
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
    
    public String getStatus() {
        return status;
    }
    
    public String getBoundary() {
        return boundary;
    }
    
    public String getSource() {
        return source;
    }
}
