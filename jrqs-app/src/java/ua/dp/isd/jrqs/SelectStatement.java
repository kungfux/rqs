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

public class SelectStatement {

    private final String SQL_SELECT_TEMPLATE
            = "select id, fr_id, fr_tms_task, fr_object, fr_text, ccp, created, modified, status, boundary, source from requirements %s limit 100;";

    private final String[] Values;
    private final String[] Sources;
    private final String SqlSelectStatement;

    public String[] getValues() {
        return Values;
    }

    public String[] getSources() {
        return Sources;
    }

    public String getSqlSelectStatement() {
        return SqlSelectStatement;
    }

    public SelectStatement(String SearchByField, Boolean ExactMatches, String RawValues, String LimitBySource) {
        Values = RawValues != null ? splitParameters(RawValues) : null;
        Sources = LimitBySource != null ? splitParameters(LimitBySource) : null;

        String escapedValues;
        String whereClause;

        if (Values != null) {
            if (ExactMatches) {
                escapedValues = generateSqlParameters("?", ",", Values.length);
                whereClause = String.format("where lower(%s) in (%s)", SearchByField, escapedValues);
            } else {
                wrapUpByPercentages();
                escapedValues = generateSqlParameters(String.format("lower(%s) like ?", SearchByField), " and ", Values.length);
                whereClause = String.format("where %s", escapedValues);
            }

            if (LimitBySource != null && !LimitBySource.equals("")) {
                String escapedSources = generateSqlParameters("?", ",", Sources.length);
                whereClause = String.format(whereClause + " and lower(source) in (%s)", escapedSources);
            }
            SqlSelectStatement = String.format(SQL_SELECT_TEMPLATE, whereClause);
        } else {
            SqlSelectStatement = null;
        }
    }

    private String[] splitParameters(String RawValues) {
        return RawValues.toLowerCase().split(",");
    }

    private String generateSqlParameters(String SqlCondition, String Separator, int Count) {
        StringBuilder argsBuilder = new StringBuilder();
        argsBuilder.append(SqlCondition);

        for (int i = 1; i < Count; i++) {
            argsBuilder.append(Separator);
            argsBuilder.append(SqlCondition);
        }
        return argsBuilder.toString();
    }

    private void wrapUpByPercentages() {
        for (int i = 0; i < Values.length; i++) {
            Values[i] = String.format("%%%s%%", Values[i]);
        }
    }
}
