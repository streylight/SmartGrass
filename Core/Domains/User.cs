using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domains {
    /// <summary>
    /// The user class
    /// </summary>
    public class User : BaseEntity {
        /// <summary>
        /// Unique username for login
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The encrypted password
        /// </summary>
        public string PasswordHash { get; set; }
        /// <summary>
        /// The password salt used for decrypting the password
        /// </summary>
        public string PasswordSalt { get; set; }
        /// <summary>
        /// The role of the user
        /// </summary>
        public Role Role { get; set; }
        /// <summary>
        /// Foreign key for the HICS unit
        /// </summary>
        public int UnitId { get; set; }
        /// <summary>
        /// Virtual reference to the HICS unit domain
        /// </summary>
        public virtual Unit Unit { get; set; }
    }
}
