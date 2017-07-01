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

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.Logger;
import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import javax.sql.DataSource;

public class RequirementsDAO {

    public List<Requirement> getRequirements(SearchBy selectBy, String keywords, String limitBySources) {
        SqlBuilder sqlb = new SqlBuilder(selectBy);
        sqlb.addCommaSeparatedParameters(keywords);
        sqlb.addCommaSeparatedSourcesToFilter(limitBySources);
        String sql = sqlb.getSql();
        String[] arguments = sqlb.getParametersList();
        String[] limitBySource = sqlb.getSourceParametersList();
        return getRequirementsFromDB(sql, arguments, limitBySource);
    }
    
    private List<Requirement> getRequirementsFromDB(String sql, String[] arguments, String[] limitBySource) {
        if (sql == null) {
            return null;
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
            statement = connection.prepareStatement(sql);

            int i = 1;
            if (arguments != null) {
                for (String argValue : arguments) {
                    statement.setString(i++, argValue);
                }
                if (limitBySource != null) {
                    for (String argSource : limitBySource) {
                        statement.setString(i++, argSource);
                    }
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
        Logger.getGlobal().severe(e.toString());
    }
}
