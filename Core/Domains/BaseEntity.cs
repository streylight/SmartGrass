using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domains {
    /// <summary>
    /// The base entity class used for the unit of work
    /// </summary>
    public abstract partial class BaseEntity {
        /// <summary>
        /// The base id used in all domains
        /// </summary>
        public virtual int Id { get; set; }
    } // class
} // namespace
