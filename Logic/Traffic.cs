/*   
 *  WebQA
 *  WebQA Server
 *  Copyright (C) Fuks Alexander 2013
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

using System;

namespace WebQA.Logic
{
    internal class Traffic
    {
        private enum TrafficEnumeration
        {
            B = 1,
            KB = 2,
            MB = 4,
            GB = 8,
            TB = 16
        }

        /// <summary>
        /// return traffic size and enum representation
        /// </summary>
        private long[] convertBytes(long bytes)
        {
            return convertBytes(bytes, 1, 8);
        }

        private long[] convertBytes(long bytes, int minEnum, int maxEnum)
        {
            int value = 1024;
            //if (!ClientParams.Parameters.TrafficRoundUp) value = 10240; // 10 KB
            int enumeration = 1;
            double traffic = bytes;
            while ((traffic > value && enumeration < maxEnum) || enumeration < minEnum)
            {
                traffic = traffic / 1024;
                enumeration = enumeration * 2;
            }
            return new long[] { (long)Math.Round(traffic, 0), enumeration };
        }

        /// <summary>
        /// return traffic with size representation
        /// </summary>
        public string getConvertedBytes(long bytes)
        {
            long[] result = convertBytes(bytes);
            return 
                string.Format("{0} {1}", 
                result[0].ToString(), 
                ((TrafficEnumeration)result[1]).ToString());
        }
    }
}
