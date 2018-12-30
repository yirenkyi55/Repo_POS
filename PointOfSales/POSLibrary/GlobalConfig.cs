using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Drawing;
using System.IO;

namespace POSLibrary
{
    public static class GlobalConfig
    {
        private static string cnStringName = "POSConStr";

        /// <summary>
        /// Represents the connectionstring for the database
        /// </summary>
        /// <param name="name">
        /// The name of the connectionstring as specified
        /// in app.config file.
        /// </param>
        /// <returns>
        /// The connection string for the connection.
        /// </returns>
        public static string ConString()
        {
            return ConfigurationManager.ConnectionStrings[cnStringName].ConnectionString;
        }

        /// <summary>
        /// Converts an image into a byte array
        /// </summary>
        /// <param name="img">
        /// The image to be convert
        /// </param>
        /// <returns>
        /// A byte array of the image
        /// </returns>
        public static byte[] ConvertImageToBinary(this Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

    }
}
