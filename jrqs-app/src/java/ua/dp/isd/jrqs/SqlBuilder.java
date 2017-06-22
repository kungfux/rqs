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

public class SqlBuilder {

    public enum SelectBy {
        ROWID,
        FRID,
        TMSTask,
        Text
    }

    private static final String sqlSelectTemplate
            = "select id, fr_id, fr_tms_task, fr_object, fr_text, ccp, created, modified, status, boundary, source from requirements %s limit 100;";

    private final SelectBy selectBy;
    private String[] sqlParameters;
    private String[] sqlSourceParameters;

    public SqlBuilder(SelectBy selectBy) {
        this.selectBy = selectBy;
    }

    public String getSql() {
        return String.format(sqlSelectTemplate, getWhereStatement());
    }

    public String[] getParametersList() {
        return sqlParameters;
    }
    
    public String[] getSourceParametersList() {
        return sqlSourceParameters;
    }
    
    public void addCommaSeparatedParameters(String commaSeparatedParameters) {
        if (commaSeparatedParameters == null) {
            return;
        }

        sqlParameters = commaSeparatedParameters.toLowerCase().split(",");
        
        int i = 0;
        for (String s: sqlParameters) {
            switch (selectBy) {
                case ROWID:
                case FRID:
                    sqlParameters[i++] = s.trim();
                    break;
                case TMSTask:
                case Text:
                    sqlParameters[i++] = String.format("%%%s%%", s.trim());
                    break;
            }
        }
    }

    public void addCommaSeparatedSourcesToFilter(String commaSeparatedSources) {
        if (commaSeparatedSources == null) {
            return;
        }

        sqlSourceParameters = commaSeparatedSources.toLowerCase().split(",");
        int i = 0;
        for (String s : sqlSourceParameters) {
            sqlSourceParameters[i++] = s.trim();
        }
    }

    private String getWhereStatement() {
        StringBuilder sqlWhere = new StringBuilder();
        
        switch (selectBy) {
            case ROWID:
                sqlWhere.append(String.format("where id in (%s)", 
                        generatePlaceholders("?", ", ", sqlParameters.length)));
                break;
            case FRID:
                sqlWhere.append(String.format("where lower(fr_id) in (%s)", 
                        generatePlaceholders("?", ", ", sqlParameters.length)));
                break;
            case TMSTask:
                sqlWhere.append(String.format("where %s", 
                        generatePlaceholders("lower(fr_tms_task) like ?", " and ", sqlParameters.length)));
                break;
            case Text:
            default:
                sqlWhere.append(String.format("where %s", 
                        generatePlaceholders("lower(fr_text) like ?", " and ", sqlParameters.length)));
                break;
        }
        
        if (sqlSourceParameters != null) {
            sqlWhere.append(String.format("and lower(source) in (%s)", 
                    generatePlaceholders("?", ",", sqlSourceParameters.length)));
        }

        return sqlWhere.toString();
    }
    
    private String generatePlaceholders(String condition, String separator, Integer count) {
        StringBuilder sb = new StringBuilder();
        sb.append(condition);

        for (int i = 1; i < count; i++) {
            sb.append(separator);
            sb.append(condition);
        }
        
        return sb.toString();
    }
}
