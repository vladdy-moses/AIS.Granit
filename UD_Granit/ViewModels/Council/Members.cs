using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Council
{
    public struct MemberView
    {
        public string Name;
        public string Position;
        public string Speciality;
        public string Degree;
    }

    public class Members
    {
        public IEnumerable<MemberView> CouncilMembers;
    }
}