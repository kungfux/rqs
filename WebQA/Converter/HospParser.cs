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
using System.Xml;
using System.IO;

namespace WebQA.Converter
{
    internal class HospParser
    {
        private string positionList;
        private bool semafor = true;
        private const string nodeMainName = "hosparams";
        private XmlDocument doc = new XmlDocument();
        private string currentLine;
        private TextReader tr;
        private Hosparam hosp;

        public HospParser(string path)
        {
            if (!File.Exists("output.xml"))
            {
                XmlTextWriter textWritter = new XmlTextWriter("output.xml", Encoding.UTF8);
                textWritter.WriteStartDocument();
                textWritter.WriteStartElement(nodeMainName);
                textWritter.WriteEndElement();
                textWritter.Close();
            }
        }

        public void Parse(string path)
        {
            tr = new StreamReader(path);
            doc.Load("output.xml");
            while (true)
            {
                if (semafor)
                {
                    if (ReadLine() == null)
                    {
                        break;
                    }
                }
                else
                {
                    AddToFile();
                }

                hosp = new Hosparam();

                ReadModuleName();
                ReadSectionName();
                ReadMainDescr();
                ReadShortDescr();
                ReadParameterName();
                ReadClassName();
                ReadPositionList();
            }

            tr.Close();
            doc.Save("output.xml");
        }

        void ReadMainDescr()
        {
            if (currentLine != null)
            {
                if (currentLine.Contains("Main Description:"))
                {
                    semafor = true;
                    hosp.mainDescription = currentLine;
                    while (ReadLine() != null)
                    {
                        if (currentLine.Contains("Short Description:"))
                        {

                            break;
                        }
                        else
                        {
                            hosp.mainDescription = hosp.mainDescription + "\n" + currentLine;
                        }
                    }
                }
            }
        }

        void ReadShortDescr()
        {
            if (currentLine != null)
            {
                if (currentLine.Contains("Short Description:"))
                {
                    hosp.shortDescription = currentLine;
                    while (ReadLine() != null)
                    {
                        if (currentLine.Contains("Parameter:"))
                        {
                            break;
                        }
                        else
                        {
                            hosp.shortDescription = hosp.shortDescription + currentLine;
                        }
                    }
                }
            }
        }

        bool ReadModuleName()
        {
            bool b = false;
            if (currentLine != null)
            {
                if (currentLine.Contains("Module:"))
                {
                    hosp.moduleName = currentLine;
                    b = true;
                }
            }
            return b;
        }

        bool ReadSectionName()
        {
            bool b = false;
            if (currentLine != null)
            {
                if (currentLine.Contains("Section:"))
                {
                    hosp.sectionName = currentLine;
                    b = true;
                }
            }
            return b;
        }

        void ReadParameterName()
        {
            string[] temp;
            if (currentLine != null)
            {
                if (currentLine.Contains("Parameter:"))
                {
                    hosp.parameterNameFull = currentLine;
                    temp = currentLine.Split(' ');
                    if (temp.Length >= 3)
                    {
                        hosp.parameterName = temp[2];
                    }
                }
            }
        }

        void ReadClassName()
        {
            if (currentLine != null)
            {
                if (currentLine.Contains("Class:"))
                {
                    hosp.className = currentLine;
                }
            }
        }

        void ReadPositionList()
        {
            if (currentLine != null)
            {
                if (currentLine.Contains("Position"))
                {
                    positionList = currentLine;
                    while (ReadLine() != null)
                    {
                        if (currentLine.Contains("Main Description:"))
                        {
                            semafor = false;
                            break;
                        }
                        else
                        {
                            positionList = positionList + currentLine + "\n";
                        }
                    }
                }
            }
        }

        string ReadLine()
        {
            while ((currentLine = tr.ReadLine()) != null)
            {
                if (!currentLine.Contains("5/17/2013") && !ReadModuleName() && !ReadSectionName())
                {
                    break;
                }
            }
            return currentLine;
        }

        public void AddToFile()
        {
            XmlNode element = null;
            element = doc.CreateElement("parameter");
            doc.DocumentElement.AppendChild(element);
            XmlAttribute attribute = doc.CreateAttribute("name");
            attribute.Value = hosp.parameterName;
            element.Attributes.Append(attribute);

            XmlNode subElement5 = doc.CreateElement("param");
            subElement5.InnerText = hosp.parameterNameFull;
            element.AppendChild(subElement5);

            XmlNode attribute1 = doc.CreateElement("module");
            attribute1.InnerText = hosp.moduleName;
            element.AppendChild(attribute1);

            XmlNode attribute2 = doc.CreateElement("section");
            attribute2.InnerText = hosp.sectionName;
            element.AppendChild(attribute2);

            XmlNode subElement4 = doc.CreateElement("class");
            subElement4.InnerText = hosp.className;
            element.AppendChild(subElement4);

            XmlNode subElement2 = doc.CreateElement("short_description");
            subElement2.InnerText = hosp.shortDescription;
            element.AppendChild(subElement2);

            XmlNode subElement = doc.CreateElement("main_description");
            subElement.InnerText = hosp.mainDescription;
            element.AppendChild(subElement);

            XmlNode subElement3 = doc.CreateElement("position");
            subElement3.InnerText = positionList;
            element.AppendChild(subElement3);
        }
    }
}
