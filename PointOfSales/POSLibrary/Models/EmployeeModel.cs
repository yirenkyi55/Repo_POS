using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSLibrary.Models
{
    public class EmployeeModel
    {
        /// <summary>
        /// Represents the unique Id of Employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Represents the firstname of an employee
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Represents employee last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Represents Employee Gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Represents Employee Phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Represents employee Email Address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Represents employee Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Represents the filename for the picture
        /// </summary>
        public string PictureName { get; set; }

        /// <summary>
        /// Represents employee profile picture
        /// </summary>
        public byte[] PictureData { get; set; }

        /// <summary>
        /// Represents employee position
        /// </summary>
        public PositionModel Position { get; set; }
    }
}
