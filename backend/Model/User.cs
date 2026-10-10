using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class User //: IdentityUser
    {
        public virtual ICollection<Borrowing> Borrowiengs { get; set; } = new List<Borrowing>();
    }
}
