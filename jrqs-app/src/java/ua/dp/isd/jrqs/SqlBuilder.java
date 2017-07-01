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

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.stream.Stream;

public class SqlBuilder {

    private static final int MAX_PARAMETERS = 100;
    private static final int MAX_SEARCH_RESULTS = 100;
    private static final String SQL_SELECT_TEMPLATE
            = "select * from requirements %s limit " + MAX_SEARCH_RESULTS + ";";

    private final SearchBy selectBy;
    private String[] sqlParameters;
    private String[] sqlSourceParameters;

    public SqlBuilder(SearchBy selectBy) {
        this.selectBy = selectBy;
    }

    public String getSql() {
        return String.format(SQL_SELECT_TEMPLATE, getWhereStatement());
    }

    public String[] getParametersList() {
        if (sqlParameters != null) {
            if (sqlSourceParameters != null) {
                return Stream.concat(Arrays.stream(sqlParameters), Arrays.stream(sqlSourceParameters))
                        .toArray(String[]::new);
            }
            return sqlParameters;
        }
        return null;
    }

    public void addCommaSeparatedParameters(String commaSeparatedParameters) {
        if (commaSeparatedParameters == null) {
            return;
        }

        String[] splitted = commaSeparatedParameters.toLowerCase().split(",");
        if (splitted.length > MAX_PARAMETERS) {
            throw new IllegalArgumentException("The number of maximum allowed arguments have been exceeded.");
        } else {
            sqlParameters = splitted;
        }

        int i = 0;
        for (String s : sqlParameters) {
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

        if (selectBy == SearchBy.FRID) {
            expandRange();
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
                sqlWhere.append(String.format("where %s in (%s)",
                        RequirementsTableMapping.getRowIdColumnName(),
                        generatePlaceholders("?", ", ", sqlParameters.length)));
                break;
            case FRID:
                sqlWhere.append(String.format("where lower(%s) in (%s)",
                        RequirementsTableMapping.getRequirementNumberColumnName(),
                        generatePlaceholders("?", ", ", sqlParameters.length)));
                break;
            case TMSTask:
                sqlWhere.append(String.format("where %s",
                        generatePlaceholders(
                                String.format("lower(%s) like ?",
                                        RequirementsTableMapping.getTmsTaskColumnName()),
                                " and ", sqlParameters.length)));
                break;
            case Text:
            default:
                sqlWhere.append(String.format("where %s",
                        generatePlaceholders(
                                String.format("lower(%s) like ?",
                                        RequirementsTableMapping.getTextColumnName()),
                                " and ", sqlParameters.length)));
                break;
        }

        if (sqlSourceParameters != null) {
            sqlWhere.append(String.format("and lower(%s) in (%s)",
                    RequirementsTableMapping.getSourceColumnName(),
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

    private void expandRange() {
        List<String> result = new ArrayList();
        for (String s : sqlParameters) {
            if (!s.contains("-")) {
                result.add(s);
                continue;
            }
            String prefix = getSubstringByRegex(s, "^(\\w*?)\\d+");
            String from = getSubstringByRegex(s, "^\\w*?(\\d+)-");
            String to = getSubstringByRegex(s, "^\\w*?\\d+-\\w*?(\\d+)");
            if (from == null || to == null) {
                continue;
            }
            Integer fromNum = parseInt(from);
            Integer toNum = parseInt(to);
            if (fromNum == null || toNum == null) {
                continue;
            }
            if (fromNum - toNum > MAX_PARAMETERS || toNum - fromNum > MAX_PARAMETERS) {
                throw new IllegalArgumentException("The number of maximum allowed arguments have been exceeded.");
            }
            if (fromNum > toNum) {
                toNum = fromNum + toNum;
                fromNum = toNum - fromNum;
                toNum = toNum - fromNum;
            }
            for (int i = fromNum; i <= toNum; i++) {
                result.add(prefix.concat(String.valueOf(i)));
            }
        }
        sqlParameters = result.toArray(new String[result.size()]);
    }

    private String getSubstringByRegex(String string, String expression) {
        Pattern pattern = Pattern.compile(expression);
        Matcher matcher = pattern.matcher(string);
        if (matcher.find()) {
            return matcher.group(1);
        }
        return null;
    }

    private Integer parseInt(String value) {
        try {
            return Integer.parseInt(value);
        } catch (NumberFormatException e) {
            // ignore
        }
        return null;
    }
}
