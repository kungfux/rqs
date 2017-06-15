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

import java.sql.*;
import java.util.*;
import javax.naming.*;
import javax.sql.DataSource;

public class RequirementsList {

    private static final String SQL
            = "select id, fr_id, fr_tms_task, fr_object, fr_text, ccp, created, modified, status, boundary, source from requirements %s limit 100;";

    public List<Requirement> getRequirementsByRowIds(String RowIds) {
        String[] exposedRowIds = RowIds.split(",");
        String parameters = generateSqlParameters("?", ",", exposedRowIds.length);
        String whereClause = String.format("where id in (%s)", parameters);
        return getRequirements(String.format(SQL, whereClause), exposedRowIds);
    }

    public List<Requirement> getRequirementsByRequirementNumbers(String RequirementNumbers) {
        RequirementNumbers = RequirementNumbers.toLowerCase();
        String[] exposedRequirementNumbers = RequirementNumbers.split(",");
        String parameters = generateSqlParameters("?", ",", exposedRequirementNumbers.length);
        String whereClause = String.format("where lower(fr_id) in (%s)", parameters);
        return getRequirements(String.format(SQL, whereClause), exposedRequirementNumbers);
    }

    public List<Requirement> getRequirementsByTmsTaskNumbers(String TmsTaskNumbers) {
        TmsTaskNumbers = TmsTaskNumbers.toLowerCase();
        String[] exposedTmsTaskNumbers = TmsTaskNumbers.split(",");
        String parameters = generateSqlParameters("?", ",", exposedTmsTaskNumbers.length);
        String whereClause = String.format("where lower(fr_tms_task) in (%s)", parameters);
        return getRequirements(String.format(SQL, whereClause), exposedTmsTaskNumbers);
    }

    public List<Requirement> getRequirementsByTextPhrases(String TextPhrases) {
        TextPhrases = TextPhrases.toLowerCase();
        String[] exposedTextPhrases = TextPhrases.split(",");
        for (int i = 0; i < exposedTextPhrases.length; i++) {
            exposedTextPhrases[i] = "%" + exposedTextPhrases[i] + "%";
        }
        String parameters = generateSqlParameters("lower(fr_text) like ?", " and ", exposedTextPhrases.length);
        String whereClause = String.format("where %s", parameters);
        return getRequirements(String.format(SQL, whereClause), exposedTextPhrases);
    }

    private String generateSqlParameters(String SqlCondition, String Separator, int Times) {
        StringBuilder argsBuilder = new StringBuilder();
        argsBuilder.append(SqlCondition);

        for (int i = 1; i < Times; i++) {
            argsBuilder.append(Separator);
            argsBuilder.append(SqlCondition);
        }
        return argsBuilder.toString();
    }

    private List<Requirement> getRequirements(String sql, String[] arguments) {
        List<Requirement> requirements = null;

        Connection connection = null;
        PreparedStatement statement = null;
        ResultSet set = null;

        try {
            Context initContext = new InitialContext();
            Context context = (Context) initContext.lookup("java:/comp/env");
            DataSource dataSource = (DataSource) context.lookup("jdbc/sqlite");

            connection = dataSource.getConnection();
            statement = connection.prepareStatement(sql);

            if (arguments != null) {
                for (int i = 0; i < arguments.length; i++) {
                    statement.setString(i + 1, arguments[i]);
                }
            }

            set = statement.executeQuery();

            if (set.isBeforeFirst()) {
                requirements = new ArrayList<>();
            }
            while (set.next()) {
                requirements.add(new Requirement(
                        set.getLong(1),
                        set.getString(2),
                        set.getString(3),
                        set.getString(4),
                        set.getString(5),
                        set.getString(6),
                        set.getString(7),
                        set.getString(8),
                        set.getString(9),
                        set.getString(10),
                        set.getString(11)
                ));
            }

            set.close();
            set = null;
            statement.close();
            statement = null;
            connection.close();
            connection = null;
        } catch (NamingException | SQLException e) {
            logError(e);
        } finally {
            if (set != null) {
                try {
                    set.close();
                } catch (SQLException e) {
                    logError(e);
                }
                set = null;
            }
            if (statement != null) {
                try {
                    statement.close();
                } catch (SQLException e) {
                    logError(e);
                }
                statement = null;
            }
            if (connection != null) {
                try {
                    connection.close();
                } catch (SQLException e) {
                    logError(e);
                }
                connection = null;
            }
        }
        return requirements;
    }

    private void logError(Exception e) {
        System.out.println(e.getMessage());
    }
}
