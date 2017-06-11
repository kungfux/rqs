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

    private static final String SQL = "select id, fr_id, fr_tms_task, fr_object, fr_text, ccp, created, modified, status, source from requirements %s limit 100;";
    
//    public List<Requirement> getAllRequirements() {
//        return getRequirements(String.format(SQL, ""));
//    }
    
    public List<Requirement> getRequirementsByRequirementNumbers(String RequirementNumbers) {
        return getRequirements(String.format(SQL, "where fr_id = ?"), new String[] {RequirementNumbers});
    }
    
    public List<Requirement> getRequirementsByTmsTaskNumbers(String TmsTaskNumbers) {
        return getRequirements(String.format(SQL, "where fr_tms_task = ?"), new String[] {TmsTaskNumbers});
    }
    
    public List<Requirement> getRequirementsByTextPhrases(String TextPhrases) {
        return getRequirements(String.format(SQL, "where lower(fr_text) like lower(?)"), new String[] {("%" + TextPhrases + "%")});
    }

//    private List<Requirement> getRequirements(String sql) {
//        return getRequirements(sql, null);
//    }
    
    private List<Requirement> getRequirements(String sql, String[] arguments) {
        List<Requirement> requirements = new ArrayList<>();

        Connection connection = null;
        PreparedStatement statement = null;

        try {
            Context initContext = new InitialContext();
            Context context = (Context) initContext.lookup("java:/comp/env");
            DataSource dataSource = (DataSource) context.lookup("jdbc/sqlite");
            connection = dataSource.getConnection();
            statement = connection.prepareStatement(sql);
            if (arguments != null) {
                for (int i=0; i<arguments.length; i++) {
                    statement.setString(i+1, arguments[i]);
                }
            }
            ResultSet set = statement.executeQuery();

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
            statement.close();
            connection.close();
        } catch (NamingException | SQLException e) {
            System.out.println(e.getMessage());
        }
        return requirements;
    }

//    private void printResultSet(ResultSet results) {
//        try {
//            ResultSetMetaData rsmd = results.getMetaData();
//            int columnsNumber = rsmd.getColumnCount();
//            for (int i = 1; i <= columnsNumber; i++) {
//                System.out.print(rsmd.getColumnName(i) + " ");
//            }
//            System.out.println();
//            while (results.next()) {
//                for (int i = 1; i <= columnsNumber; i++) {
//                    System.out.print(results.getString(i) + "\" ");
//                }
//            }
//            System.out.println();
//        } catch (SQLException e) {
//            System.out.println(e.getMessage());
//        }
//    }
}
