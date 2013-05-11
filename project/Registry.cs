/*   
  *  .NET Assemblies Collection
  *  Assemblies collection for .NET technology projects
  *  Copyright (C) IT WORKS TEAM 2010-2012
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
  *  IT WORKS TEAM, hereby disclaims all copyright
  *  interest in the program ".NET Assemblies Collection"
  *  (which makes passes at compilers)
  *  written by Alexander Fux.
  * 
  *  Alexander Fux, 01 July 2010
  *  IT WORKS TEAM, Founder of the team.
  */

/* 
 * =========================== [CHANGES HISTORY] ================================
 * ==============================================================================
 * | Revision |    Date         |      Author      |            Reason          |
 * ------------------------------------------------------------------------------
 * | #1       | August 11, 2011 | Andrey Malitskyy | Issue #2 [code.google.com] |
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using Microsoft.Win32;


namespace ItWorksTeam.IO
{
    /// <summary>
    /// Provides method for working with OS Windows Registry
    /// </summary>
    public class Registry
    {
        /// <summary>
        /// BaseKeys describe partition of register, which can use for work. 
        /// </summary>
        public enum BaseKeys
        {
            /// <summary>
            /// HKEY_LOCAL_MACHINE branch
            /// </summary>
            HKEY_LOCAL_MACHINE,
            /// <summary>
            /// HKEY_CLASSES_ROOT branch
            /// </summary>
            HKEY_CLASSES_ROOT,
            /// <summary>
            /// HKEY_CURRENT_CONFIG branch
            /// </summary>
            HKEY_CURRENT_CONFIG,
            /// <summary>
            /// HKEY_CURRENT_USER branch
            /// </summary>
            HKEY_CURRENT_USER,
            /// <summary>
            /// HKEY_USERS branch
            /// </summary>
            HKEY_USERS
        }

        /// <summary>
        /// Contains last error message (default - empty)
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Read value of key and return it.
        /// </summary>
        /// <typeparam name="T">Type of method (string, int, bool, ets..)</typeparam>
        /// <param name="BaseKey">Select name of parent partition of register.</param>
        /// <param name="BranchPath">Name of branch(tree).</param>
        /// <param name="KeyName">Name of register's key.</param>
        /// <param name="DefReturnValue">Default value that was return by method</param>
        /// <returns>Return value of register's key or default value that was declared in method</returns>
        public T ReadKey<T>(BaseKeys BaseKey, string BranchPath, string KeyName, T DefReturnValue)
        {
            RegistryKey key = null;
            try
            {
                key = SwitchKey(BaseKey).OpenSubKey(BranchPath);

                if (typeof(T) == typeof(bool))
                {
                    return key != null ? (T)(bool.Parse((key.GetValue(KeyName, DefReturnValue).ToString())) as object) : DefReturnValue;
                }
                else
                {
                    return key != null ? (T)key.GetValue(KeyName, DefReturnValue) : DefReturnValue;
                }
            }

            catch (Exception err)
            {
                ErrorMessage = err.Message;
                return DefReturnValue;
            }

            finally
            {
                if (key != null)
                    key.Close();
            }
        }

        /// <summary>
        /// Save key with value to register by using described path of branch.
        /// </summary>
        /// <param name="BaseKey">Select name of parent partition of register.</param>
        /// <param name="BranchPath">Name of branch(tree).</param>
        /// <param name="KeyName">Name of register's key.</param>
        /// <param name="KeyValue">Value of register's key.</param>
        /// <returns>True - if key was saved. False - if there is exception.</returns>
        public bool SaveKey(BaseKeys BaseKey, string BranchPath, string KeyName, object KeyValue)
        {
            RegistryKey key = null;
            try
            {
                key = SwitchKey(BaseKey).CreateSubKey(BranchPath);
                key.SetValue(KeyName, KeyValue);
                return true;
            }
            catch (Exception err)
            {
                ErrorMessage = err.Message;
                return false;
            }
            finally
            {
                if (key != null)
                    key.Close();
            }

        }

        /// <summary>
        /// Delete tree with all subtrees.
        /// </summary>
        /// <param name="BaseKey">Select name of parent partition of register.</param>
        /// <param name="BranchPath">Name of branch(tree).</param>
        public bool DeleteBranch(BaseKeys BaseKey, string BranchPath)
        {
            RegistryKey key = null;
            try
            {
                key = SwitchKey(BaseKey);
                key.DeleteSubKeyTree(BranchPath);
                return true;
            }
            catch (Exception err)
            {
                ErrorMessage = err.Message;
                return false;
            }
        }

        /// <summary>
        /// Delete key.
        /// </summary>
        /// <param name="BaseKey">Select name of parent partition of register.</param>
        /// <param name="BranchPath">Name of branch(tree).</param>
        /// <param name="KeyName">Name of register's key.</param>
        public bool DeleteKey(BaseKeys BaseKey, string BranchPath, string KeyName)
        {
            RegistryKey key = null;
            try
            {
                key = SwitchKey(BaseKey).OpenSubKey(BranchPath, true);
                key.DeleteValue(KeyName);
                return true;
            }
            catch (Exception err)
            {
                ErrorMessage = err.Message;
                return false;
            }
        }

        /// <summary>
        /// Check that described branch(tree) is exist.
        /// </summary>
        /// <param name="BaseKey">Select name of parent partition of register.</param>
        /// <param name="BranchPath">Name of branch(tree).</param>
        /// <returns></returns>
        public bool IsBranchExist(BaseKeys BaseKey, string BranchPath)
        {
            try
            {

                return SwitchKey(BaseKey).OpenSubKey(BranchPath) != null;

            }
            catch (Exception err)
            {
                ErrorMessage = err.Message;
                return false;
            }

        }

        /// <summary>
        /// Check that described key is exist.
        /// </summary>
        /// <param name="BaseKey">Select name of parent partition of register.</param>
        /// <param name="BranchPath">Name of branch(tree).</param>
        /// <param name="KeyName">Name of register's key.</param>
        /// <returns></returns>
        public bool IsKeyExist(BaseKeys BaseKey, string BranchPath, string KeyName)
        {
            RegistryKey key = null;
            try
            {
                key = SwitchKey(BaseKey).OpenSubKey(BranchPath);
                return key.GetValue(KeyName) != null;

            }
            catch (Exception err)
            {
                ErrorMessage = err.Message;
                return false;
            }

        }

        /// <summary>
        /// Get SubKey name of branch(tree).
        /// </summary>
        /// <param name="BaseKey">Select name of parent partition of register.</param>
        /// <param name="BranchPath">Name of branch(tree).</param>
        /// <returns>Return an array with subkeys'es name.</returns>
        public string[] GetSubKeyNames(BaseKeys BaseKey, string BranchPath)
        {
            RegistryKey key = null;
            try
            {
                key = SwitchKey(BaseKey).OpenSubKey(BranchPath);
                return key != null ? key.GetSubKeyNames() : null;
            }
            catch (Exception err)
            {
                ErrorMessage = err.Message;
                return null;
            }

        }

        /// <summary>
        /// Get values'es name of key.
        /// </summary>
        /// <param name="BaseKey">Select name of parent partition of register.</param>
        /// <param name="KeyPath">Path of key.</param>
        /// <returns>Return an array with values'es name of key.</returns>
        public string[] GetValuesNames(BaseKeys BaseKey, string KeyPath)
        {

            RegistryKey key = null;
            try
            {

                key = SwitchKey(BaseKey).OpenSubKey(KeyPath);
                return key != null ? key.GetValueNames() : null;
            }
            catch (Exception err)
            {
                ErrorMessage = err.Message;
                return null;
            }

        }

        /// <summary>
        /// This privat method. It select name of parent partition of register.
        /// </summary>
        /// <param name="NameBaseKey"></param>
        /// <returns></returns>
        private RegistryKey SwitchKey(BaseKeys NameBaseKey)
        {
            switch (NameBaseKey)
            {
                case BaseKeys.HKEY_LOCAL_MACHINE:
                    return Microsoft.Win32.Registry.LocalMachine;


                case BaseKeys.HKEY_CURRENT_USER:
                    return Microsoft.Win32.Registry.CurrentUser;


                case BaseKeys.HKEY_CURRENT_CONFIG:
                    return Microsoft.Win32.Registry.CurrentConfig;


                case BaseKeys.HKEY_CLASSES_ROOT:
                    return Microsoft.Win32.Registry.ClassesRoot;


                case BaseKeys.HKEY_USERS:
                    return Microsoft.Win32.Registry.Users;
            }
            return null;
        }

    }
}