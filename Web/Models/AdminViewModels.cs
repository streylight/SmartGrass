using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Domains;

namespace Web.Models {
    public class UserAccountViewModel {
        public User User { get; set; }
        public Unit Unit { get; set; }
    }
}