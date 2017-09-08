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

    public List<Requirement> getRequirements(SearchBy searchBy, String keywords, 
            String onlyFromSources) {
        SqlBuilder sqlb = new SqlBuilder(searchBy);
        sqlb.addCommaSeparatedParameters(keywords);
        sqlb.addCommaSeparatedSourcesToFilter(onlyFromSources);
        String selectSqlStatement = sqlb.getSql();
        String[] whereArguments = sqlb.getParametersList();
        return getRequirementsFromDB(selectSqlStatement, whereArguments);
    }
    
    private List<Requirement> getRequirementsFromDB(String selectSqlStatement, 
            String[] whereArguments) {
        if (selectSqlStatement == null) {
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
            statement = connection.prepareStatement(selectSqlStatement);

            int i = 1;
            if (whereArguments != null) {
                for (String argValue : whereArguments) {
                    statement.setString(i++, argValue);
                }
            }

            set = statement.executeQuery();

            if (set.isBeforeFirst()) {
                requirements = new ArrayList<>();
            }
            while (set.next()) {
                requirements.add(new Requirement(
                        set.getLong(RequirementsTableMapping.getRowIdColumnName()),
                        set.getString(RequirementsTableMapping.getRequirementNumberColumnName()),
                        set.getString(RequirementsTableMapping.getTmsTaskColumnName()),
                        set.getString(RequirementsTableMapping.getObjectNumberColumnName()),
                        set.getString(RequirementsTableMapping.getTextColumnName()),
                        set.getString(RequirementsTableMapping.getCcpColumnName()),
                        set.getString(RequirementsTableMapping.getCreatedColumnName()),
                        set.getString(RequirementsTableMapping.getModifiedColumnName()),
                        set.getString(RequirementsTableMapping.getStatusColumnName()),
                        set.getString(RequirementsTableMapping.getBoundaryColumnName()),
                        set.getString(RequirementsTableMapping.getSourceColumnName())
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
