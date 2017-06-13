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
            = "select id, fr_id, fr_tms_task, fr_object, fr_text, ccp, created, modified, status, source from requirements %s limit 100;";

    public List<Requirement> getRequirementsByRequirementNumbers(String RequirementNumbers) {
        RequirementNumbers = RequirementNumbers.toLowerCase();
        String[] exposedRequirementNumbers = RequirementNumbers.split(",");
        return getRequirements(String.format(SQL, "where lower(fr_id) in (?)"), exposedRequirementNumbers);
    }

    public List<Requirement> getRequirementsByTmsTaskNumbers(String TmsTaskNumbers) {
        TmsTaskNumbers = TmsTaskNumbers.toLowerCase();
        String[] exposedTmsTaskNumbers = TmsTaskNumbers.split(",");
        return getRequirements(String.format(SQL, "where lower(fr_tms_task) in (?)"), exposedTmsTaskNumbers);
    }

    public List<Requirement> getRequirementsByTextPhrases(String TextPhrases) {
        TextPhrases = TextPhrases.toLowerCase();
        return getRequirements(String.format(SQL, "where lower(fr_text) like (?)"), new String[]{("%" + TextPhrases + "%")});
    }

    private List<Requirement> getRequirements(String sql, String[] arguments) {
        StringBuilder argsBuilder = new StringBuilder();
        argsBuilder.append("?");

        if (arguments != null && arguments.length > 1) {
            for (int i = 0; i <= arguments.length; i++) {
                argsBuilder.append(",?");
            }
        }

        List<Requirement> requirements = null;

        Connection connection = null;
        PreparedStatement statement = null;
        ResultSet set = null;

        try {
            Context initContext = new InitialContext();
            Context context = (Context) initContext.lookup("java:/comp/env");
            DataSource dataSource = (DataSource) context.lookup("jdbc/sqlite");

            connection = dataSource.getConnection();
            statement = connection.prepareStatement(sql.replace("?", argsBuilder.toString()));

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
                        set.getString(10)
                ));
            }
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
