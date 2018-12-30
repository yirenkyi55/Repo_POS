using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace POSLibrary.Infrastructure
{
    public class ProductSettings
    {
        private string filePath = AppSettings.GetFolderByName(Folders.Products) + "Scanner.txt";

        /// <summary>
        /// Write a value to a scanner file
        /// </summary>
        /// <param name="value"></param>
        public void WriteToScanner(string value)
        {
            AppSettings.CreateDirectories();

            if (!File.Exists(filePath))
            {
                //Create a Scanner file
                using (File.Create(filePath)) { }


            }

            File.WriteAllText(filePath, value);
        }

        /// <summary>
        /// Reads the content of the scanner text file
        /// </summary>
        /// <returns>
        /// The string content of the scanner file
        /// </returns>
        public bool ReadFromScanner()
        {
            AppSettings.CreateDirectories();
            if (File.Exists(filePath))
            {
                string fileContent = File.ReadAllText(filePath);
                return bool.Parse(fileContent);
            }
            else
            {
                return false;
            }
        }

    }
}
