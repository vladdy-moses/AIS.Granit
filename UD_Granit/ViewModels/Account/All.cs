using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Account
{
    public struct AccountViev
    {
        public int Id;
        public string Name;
        public string Role;
        public string Email;
        public bool CanEdit;
        public bool CanRemove;
    }

    public class All
    {
        public IEnumerable<AccountViev> Accounts;
    }
}