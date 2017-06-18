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
import java.util.logging.Logger;
import javax.naming.*;
import javax.sql.DataSource;

public class RequirementsList {
   
    public List<Requirement> getRequirementsByRowIds(String RowIds) {
        SelectStatement sql = new SelectStatement("id", Boolean.TRUE, RowIds, null);
        return getRequirements(sql.getSqlSelectStatement(), sql.getValues(), null);
    }

    public List<Requirement> getRequirementsByRequirementNumbers(String RequirementNumbers, String LimitBySource) {
        SelectStatement sql = new SelectStatement("fr_id", Boolean.TRUE, RequirementNumbers, LimitBySource);
        return getRequirements(sql.getSqlSelectStatement(), sql.getValues(), sql.getSources());
    }

    public List<Requirement> getRequirementsByTmsTaskNumbers(String TmsTaskNumbers, String LimitBySource) {
        SelectStatement sql = new SelectStatement("fr_tms_task", Boolean.FALSE, TmsTaskNumbers, LimitBySource);
        return getRequirements(sql.getSqlSelectStatement(), sql.getValues(), sql.getSources());
    }

    public List<Requirement> getRequirementsByTextPhrases(String Keywords, String LimitBySource) {
        SelectStatement sql = new SelectStatement("fr_text", Boolean.FALSE, Keywords, LimitBySource);
        return getRequirements(sql.getSqlSelectStatement(), sql.getValues(), sql.getSources());
    }    

    private List<Requirement> getRequirements(String sql, String[] arguments, String[] limitBySource) {
        List<Requirement> requirements = null;

        if (sql == null)
            return requirements;
        
        Connection connection = null;
        PreparedStatement statement = null;
        ResultSet set = null;

        try {
            Context initContext = new InitialContext();
            Context context = (Context) initContext.lookup("java:/comp/env");
            DataSource dataSource = (DataSource) context.lookup("jdbc/sqlite");

            connection = dataSource.getConnection();
            statement = connection.prepareStatement(sql);

            int i = 1;
            if (arguments != null) {
                for (String argValue : arguments) {
                    statement.setString(i++, argValue);
                }
                if (limitBySource != null) {
                    for (String argSource: limitBySource) {
                        statement.setString(i++, argSource);
                    }
                }
//                for (int i = 0; i < arguments.length; i++) {
//                    statement.setString(i + 1, arguments[i]);
//                }
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
        Logger.getGlobal().severe(e.toString());
    }
}
