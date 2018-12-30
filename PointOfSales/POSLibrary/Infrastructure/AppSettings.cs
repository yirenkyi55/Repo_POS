using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace POSLibrary.Infrastructure
{
   public static class AppSettings
    {
        private static string[] folders =
        {
            @"Workspace\",
            @"Workspace\Products\"
        };

        /// <summary>
        /// Get a application folder
        /// </summary>
        /// <param name="folderName">
        /// The folder name you want to work with
        /// </param>
        /// <returns>
        /// The folder as indicated in the parameter.
        /// </returns>
        public static string GetFolderByName(Folders folderName)
        {
            return folders[(int)folderName];
        }

        /// <summary>
        /// Creates working directories.
        /// </summary>
        public static void CreateDirectories()
        {
            foreach (var directoryName in folders)
            {
                if (! Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
            }
        }


        /// <summary>
        /// Removes commas from textbox values
        /// </summary>
        /// <param name="valueToTrim">
        /// The value to trim.
        /// </param>
        public static string TrimValue(string valueToTrim)
        {
            int periodCount = 0;
            List<int> charIndex = new List<int>();
            for (int i = 0; i < valueToTrim.Length; i++)
            {
                if (valueToTrim[i] == ',')
                {
                    //Notice the location
                    charIndex.Add(i);
                }

                else if (valueToTrim[i] == '/')
                {
                    //Notice the location
                    charIndex.Add(i);
                }

                else if (valueToTrim[i] == ';')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '\'')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '[')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == ']')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '-')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '*')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '/')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '\\')
                {
                    //Notice the location
                    charIndex.Add(i);
                }

                else if (valueToTrim[i] == '(')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == ')')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '_')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '&')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '^')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '%')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '!')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '@')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '#')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == ':')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '"')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '}')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '{')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '?')
                {
                    //Notice the location
                    charIndex.Add(i);
                }
                else if (valueToTrim[i] == '.')
                {
                    periodCount += 1;
                }

            }

            if (periodCount > 1)
            {
                int count = 0;
                for (int i = 0; i < valueToTrim.Length; i++)
                {
                    if (valueToTrim[i] == '.')
                    {
                        count++;
                        if (count > 1)
                        {
                            charIndex.Add(i);
                        }
                    }


                }
            }

            if (charIndex.Count > 0)
            {
                foreach (var index in charIndex)
                {
                    valueToTrim = valueToTrim.Remove(index, 1);
                }
            }

            return valueToTrim;
        }
    }
}
