using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domains {
    /// <summary>
    /// The user data model class
    /// </summary>
    public class UserDataModel {
        /// <summary>
        /// The id of the user
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The role of the user
        /// </summary>
        public Role Role { get; set; }
    } // class
} // namespace
