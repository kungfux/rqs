/*   
 *  WebQA
 *  WebQA Server
 *  Copyright (C) Fuks Alexander 2013-2015
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
 *  Fuks Alexander, hereby disclaims all copyright
 *  interest in the program "WebQA"
 *  (which makes passes at compilers)
 *  written by Alexander Fuks.
 * 
 *  Alexander Fuks, 06 November 2013.
 */

using System.Text;
using System.Data;
using System;
using System.Text.RegularExpressions;

namespace WebQA.Logic
{
    internal class RQS
    {
        WebPageSource webSource = new WebPageSource();
        StringBuilder html = new StringBuilder();

        string by = "";
        string value = "";
        string file_filter = "";

        public string ProcessRequest(string request)
        {
            html.Clear();

            // Display help
            if (request.ToLower().Equals("?help"))
            {
                return webSource.LoadSource("help");
            }

            // Clear previous request
            by = "";
            value = "";
            file_filter = "";

            if (request.Contains("?by=") && request.Contains("&value="))
            {
                by =
                    request.Substring(request.IndexOf("?by=") + "?by=".Length,
                    request.IndexOf("&value=") - request.IndexOf("?by=") - "?by=".Length).ToLower();

                if (request.Contains("&filter="))
                {
                    value =
                        request.Substring(request.IndexOf("&value=") + "&value=".Length,
                        request.IndexOf("&filter=") - request.IndexOf("&value=") - "&value=".Length);

                    file_filter =
                        request.Substring(request.IndexOf("&filter=") + "&filter=".Length,
                        request.Length - request.IndexOf("&filter=") - "&filter=".Length);
                }
                else
                {
                    value =
                        request.Substring(request.IndexOf("&value=") + "&value=".Length,
                        request.Length - request.IndexOf("&value=") - "&value=".Length);
                }
            }

            // Fix non-readable symbols
            //value = Uri.UnescapeDataString(value);
            value = value.Replace("%3B", ";").Replace("%2C", ",").Replace("+", " ");

            Trace.Instance.Add(
                    string.Format("RQS URL: {0}", value), Trace.Color.Yellow);

            string style = 
                string.Concat(
                "<style type='text/css'>",
                "body { font-family: Tahoma, Verdana, Arial, sans-serif; }",
                "body, div, p, table  { margin: 0px auto; }",
                "a { text-decoration: none; color: #000000;}",
                "input[type=text] { border:solid 1px #000000; color: #000000; height: 19px; padding-left:10px; width: 350px; box-shadow: 2px 2px 0 #000000 inset; }",
                "input[type=button] { color: #0080ff; height: 21px; width: 100px; color: #000000; text-transform:uppercase; box-shadow:-1px 2px #000000 inset; }",
                "input[type=button], input[type=text] { border: 1px; border-radius:5px; font: 14px 'atrament-web-1', 'atrament-web-2', Georgia, Serif; }",
                "select { border: solid 1px #000000; color: #000000; height: 21px; box-shadow: 2px 2px 0 #000000 inset; }",
                "table.results { border-collapse: collapse; empty-cells: show; font-size: 80%; }",
                "table.results th { border: 1px solid #5f5f5f; padding: 2px 3px; }",
                "table.results td { border: 1px solid #5f5f5f; padding: 2px 3px; }",
                "</style>"
		        );

            string js = 
                string.Concat(
                "<script type='text/javascript'>",
                "function SwitchBy(e) { var k = e.keyCode ? e.keyCode : e.charCode; sel = document.getElementById('by'); ",
                "if (k == 40 && sel.selectedIndex < 2) { sel.selectedIndex++; } if (k == 38 && sel.selectedIndex > 0) { sel.selectedIndex--; } }",
                "</script>"
                );


            // Print header
            html.Append(
                string.Concat(
                    "<!DOCTYPE HTML>",
                    "<html>",
                    "<head>",
                    "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>",
                    "<title>WebQA</title>",
                    style,
                    js,
                    "</head>",
                    "<body>",
                    "<div style='text-align: center; font-family: Tahoma, Verdana, Arial, sans-serif;'>",
                    "<table>",
                    "<form action='' method='get'>",
                    "<tr>",
                    "<th>Search by:</th><th>Value to Search:</th><th></th>",
                    "</tr>",
                    "<tr>",
                    "<td>",
                    "<select id='by' name='by'>",
                    by.Equals("id") ? 
                        "<option selected value='fr'>FR ID</option>" : "<option value='fr'>FR ID</option>",
                    by.Equals("tms") ? 
                        "<option selected value='tms'>TMS Task</option>" : "<option value='tms'>TMS Task</option>",
                    by.Equals("text") ? 
                        "<option selected value='text'>FR Text</option>" : "<option value='text'>FR Text</option>",
                    "</select>",
                    "</td>",
                    "<td>",
                    !by.Equals("id") && value.Length > 0 ?
                        "<input id='by' type='text' size='40' name='value' value='" + value + "' onkeyup='SwitchBy(event);'  />" :
                        "<input id='by' type='text' size='40' name='value' onkeyup='SwitchBy(event);'  />",
                    "</td>",
                    "<td>",
                    "<input type='submit' value=' Search ' />",
                    "</td>",
                    "</tr>",
                    "</form>",
                    "</table>",
                    !by.Equals("id") && !by.Equals("fr") && !by.Equals("tms") && !by.Equals("text") ? 
                        string.Format("<p style='font-size: 80%;'><font color='green'>{0} requirements in the database</font></p>", Program.ReqsCountInDB) : "",
                    "</div><br>"
                    ));

            GetRequirements(by, GetCriteria(by, value), file_filter);

            return html.ToString();
        }

        private void GetRequirements(string by, string[] criteria, string file_filter)
        {
            string query = "SELECT id, fr_id, fr_tms_task, fr_object, fr_text, ccp, created, modified, created = modified, status, source FROM REQUIREMENTS WHERE ";

            if (by.Equals("id") || by.Equals("fr") || by.Equals("tms") || by.Equals("text"))
            {
                if (by.Equals("id") && !criteria[0].Equals(""))
                {
                    query += "id = " + criteria[0];
                }

                if (by.Equals("fr") && !criteria[0].Equals(""))
                {
                    query += "lower(fr_id) in (";
                    for (int a = 0; a < criteria.Length; a++)
                    {
                        if (criteria[a].Length == 0)
                        {
                            // Skip empty criteria
                            continue;
                        }
                        query += "'" + criteria[a].ToLower() + "',";
                        //if (a != criteria.Length - 1)
                        //{
                        //    query += ",";
                        //}
                    }
                    if (query.EndsWith(","))
                    {
                        query = query.Remove(query.Length - 1, 1);
                    }
                    query += ")";
                }

                if (by.Equals("tms") && !criteria[0].Equals(""))
                {
                    for (int a = 0; a < criteria.Length; a++)
                    {
                        if (criteria[a].Length == 0)
                        {
                            // Skip empty criteria
                            continue;
                        }
                        query += "lower(fr_tms_task) like '%" + criteria[a].ToLower() + "%' or ";
                        //if (a != criteria.Length - 1)
                        //{
                        //    query += " or ";
                        //}
                    }
                    if (query.EndsWith(" or "))
                    {
                        query = query.Remove(query.Length - 4, 4);
                    }
                }

                if (by.Equals("text") && !criteria[0].Equals(""))
                {
                    for (int a = 0; a < criteria.Length; a++)
                    {
                        if (criteria[a].Length == 0)
                        {
                            // Skip empty criteria
                            continue;
                        }
                        query += "lower(fr_text) like '%" + criteria[a].ToLower() + "%' and ";
                        //if (a != criteria.Length - 1)
                        //{
                        //    query += " and ";
                        //}
                    }
                    if (query.EndsWith(" and "))
                    {
                        query = query.Remove(query.Length - 5, 5);
                    }
                    query += "";
                }

                if (file_filter != null && file_filter.Length > 0)
                {
                    query += string.Format(" and source = '{0}' ", file_filter);
                }

                query += " limit 100;";

                DataTable result = criteria.Length > 0 && !criteria[0].Equals("") ? SQLiteIteractionLite.SelectTable(query) : new DataTable();

                if (result != null && result.Rows.Count > 0)
                {
                    Trace.Instance.Add(
                       string.Format("Requirements found: {0}",
                       result != null ? result.Rows.Count : 0));

                    html.Append("<div style='text-align: center;'>");

                    if (result.Rows.Count > 99)
                    {
                        html.Append("<p style='font-size: 80%;'><font color='maroon'>Only first 100 results are displayed!</font></p>");
                    }

                    html.Append(
                        string.Concat(
                        "<table class='results'>",
                        "<tr>",
                        "<th><a id='a-hiden' href='/?help#ds' title='Display help'>FR ID</a></th>",
                        "<th><a id='a-hiden' href='/?help#ds' title='Display help'>TMS Task</a></th>",
                        "<th><a id='a-hiden' href='/?help#ds' title='Display help'>Object #</a></th>",
                        "<th><a id='a-hiden' href='/?help#ds' title='Display help'>Text</a></th>",
                        "<th><a id='a-hiden' href='/?help#ds' title='Display help'>CCP</a></th>",
                        "<th><a id='a-hiden' href='/?help#ds' title='Display help'>Created / Modified</a></th>",
                        "<th><a id='a-hiden' href='/?help#ds' title='Display help'>Is changed?</a></th>",
                        "<th><a id='a-hiden' href='/?help#ds' title='Display help'>Status</a></th>",
                        "<th><a id='a-hiden' href='/?help#ds' title='Display help'>Found in file</a></th>", 
                        "</tr>"));

                    for (int a = 0; a < result.Rows.Count; a++)
                    {
                        html.Append(
                            string.Format(
                            "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td></tr>",
                            // internal id and fr id
                            result.Rows[a].ItemArray[1] != null ?
                                string.Format("<a href='/?by=id&value={0}' title='Search by FR ID: {1}'>{1}</a>", 
                                result.Rows[a].ItemArray[0], result.Rows[a].ItemArray[1]) : 
                                "&nbsp;",
                            // fr tms task number
                            result.Rows[a].ItemArray[2] != null && !result.Rows[a].ItemArray[2].ToString().Equals("") ? 
                                string.Format("<a href='/?by=tms&value={0}' title='Search by TMS Task: {0}'>{0}</a>", 
                                result.Rows[a].ItemArray[2]) : 
                                "&nbsp;",
                            // fr object number
                            result.Rows[a].ItemArray[3] != null ? result.Rows[a].ItemArray[3] : "&nbsp;",
                            // fr text
                            by.Equals("text") ?
                            (result.Rows[a].ItemArray[4] != null ? Highlight(result.Rows[a].ItemArray[4].ToString(), criteria) : "&nbsp;") :
                            (result.Rows[a].ItemArray[4] != null ? result.Rows[a].ItemArray[4].ToString() : "&nbsp;"),
                            // ccp
                            Convert.ToInt16(result.Rows[a].ItemArray[5]) != 0 ? result.Rows[a].ItemArray[5] : "&nbsp;",
                            // created / modified dates
                            string.Concat(
                                result.Rows[a].ItemArray[6] != null && !result.Rows[a].ItemArray[6].ToString().Equals("") ? 
                                result.Rows[a].ItemArray[6].ToString() :
                                "-",
                                " / ",
                                result.Rows[a].ItemArray[7] != null && !result.Rows[a].ItemArray[7].ToString().Equals("") ? 
                                result.Rows[a].ItemArray[7].ToString() :
                                "-"),                            
                            // is changed
                            result.Rows[a].ItemArray[8] != null && !string.IsNullOrEmpty(result.Rows[a].ItemArray[8].ToString()) && 
                            result.Rows[a].ItemArray[7] != null && !string.IsNullOrEmpty(result.Rows[a].ItemArray[7].ToString()) ?
                            !Convert.ToBoolean(result.Rows[a].ItemArray[8]) ? "Yes" : "No" : "No",
                            // fr status
                            result.Rows[a].ItemArray[9] != null ? result.Rows[a].ItemArray[9] : "&nbsp;",
                            // source/file
                            string.Format("<a href='/?by={0}&value={1}&filter={2}' title='Repeat search using this source only'>{2}</a>", 
                                by,
                                value,
                                result.Rows[a].ItemArray[10])));

                        //a % 2 == 0 ? "bgcolor='#ffffff';" : "bgcolor='#cccccc';")
                    }
                    html.Append("</table></div>");
                }
                else
                {
                    if (result != null)
                    {
                        Trace.Instance.Add("Requirements NOT found", Trace.Color.Yellow);
                    }
                    html.Append("<div><p align='center'><font color='maroon'>No results to display. </font><a href='/?help'> Want to read help?</a></p></div>");
                }
            }

            html.Append(
                string.Format(
                string.Concat(
                //"<br><a href='#' id='toTop' onclick='scrollTopAnimated(1000)'>back to top</a>",
                "<div>",
                "<br>",
                "<table border=0 width=100% style='text-align: right; padding: 1em; font-size: 80%; white-space: nowrap; color: #cccccc;'>",
                "<tr>",
                "<td style='text-align: center;'>Powered by <a href='http://github.com/kungfux/rqs' title='View project on GitHub'>WebQA</a><br>Generated on {0}</td>",
                //"<td style='text-align: left;'></td>",
                "</tr></table></div>"),
                DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss:fff")));

            html.Append("</body></html>");
        }

        private string[] GetCriteria(string by, string value)
        {
            // Prepare search keywords
            string[] criteria = new string[] { value };
            // Detect several keywords splitted by ';' or ',' or '-'
            // Priorities for splitting will be the following: ',;-'
            if (value.Contains(";"))
            {
                criteria = value.Split(';');
            }
            if (value.Contains(","))
            {
                criteria = value.Split(',');
            }
            if (by.Equals("fr"))
            {
                // Navigate over initial search array
                for (int a = 0; a < criteria.Length; a++)
                {
                    if (criteria[a].Contains("-"))
                    {
                        // Explode the diapason
                        string[] criterias = ExplodeCriterias(criteria[a]);
                        if (criterias != null)
                        {
                            string[] oldArray = criteria;
                            // -1 because one element of the initial array should be
                            //  deleted and replaced by exploded array
                            criteria = new string[criteria.Length + criterias.Length - 1];
                            // Navigate over new search array
                            for (int b = 0; b < criteria.Length; b++)
                            {
                                switch (b.CompareTo(a))
                                {
                                    // if b < a
                                    case -1:
                                        criteria[b] = oldArray[b];
                                        continue;
                                    // if b == a
                                    case 0:
                                        // Navigate over exploded array
                                        for (int c = 0; c < criterias.Length; c++)
                                        {
                                            criteria[b + c] = criterias[c];
                                        }
                                        // -1 because 'for' statement will do ++
                                        b = b + criterias.Length - 1;
                                        continue;
                                    // if b > a
                                    case 1:
                                        criteria[b] = oldArray[a + 1];
                                        a++;
                                        continue;
                                }
                            }
                            a = 0;
                        }
                    }
                }
            }

            // Remove white spaces
            for (int a = 0; a < criteria.Length; a++)
            {
                criteria[a] = criteria[a].Trim();
            }

            return criteria;
        }

        private string Highlight(string str, string[] criteria)
        {
            for (int a = 0; a < criteria.Length; a++)
            {
                if (criteria[a].Length == 0)
                {
                    // Skip if criteria is empty
                    continue;
                }
                str = str.Replace(criteria[a], "<b>" + criteria[a] + "</b>");
            }
            return str;
        }

        // Converts keywords like FR001-005
        // to array with FR001, FR002, FR003, FR004, FR005
        private string[] ExplodeCriterias(string str)
        {
            // Check argument
            if (str == null || !str.Contains("-"))
            {
                return null;
            }
            // Get 1st and 2nd numbers
            string[] Input = str.Split('-');
            if (Input.Length != 2)
            {
                return null;
            }
            // Leave only numbers in Input string
            int _temp;
            string prefix = ""; // will store FR or NFR
            for (int a = 0; a < Input.Length; a++)
            {
                for (int b = 0; b < Input[a].Length; b++)
                {
                    if (Input[a].Length > b &&
                        !int.TryParse(Input[a].Substring(b, 1), out _temp))
                    {
                        if (a == 0)
                        {
                            // Store prefix only from 1st word
                            prefix += Input[a].Substring(b, 1);
                        }
                        Input[a] = Input[a].Remove(b, 1);
                        b--;
                    }
                }
            }
            // Get numbers
            int NumberSize = Input[0].Length;
            int Number1;
            int Number2;
            if (!int.TryParse(Input[0], out Number1) ||
                !int.TryParse(Input[1], out Number2))
            {
                return null;
            }
            // Check numbers
            if (Input[0].Length > Input[1].Length ||
                Number1 > Number2)
            {
                return null;
            }
            // Prevent working with big diapason
            int MaxDiapason = 5000;
            if (Number2 - Number1 > MaxDiapason)
            {
                //MessageBox.Show(string.Format("Diapason over {0} is prohibited and will be threated as a word!", MaxDiapason),
                //    "RQS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            // Prepare prefix
            prefix = prefix.Trim().ToLower(); // will store 'fr' or 'nfr'
            // Generate diapason
            string[] result = new string[Number2 - Number1 + 1];
            for (int a = 0; a <= Number2 - Number1; a++)
            {
                result[a] = (Number1 + a).ToString("D" + NumberSize.ToString());
                result[a] = prefix + result[a];
            }
            return result;
        }
    }
}
