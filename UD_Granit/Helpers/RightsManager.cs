using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UD_Granit.Models;

namespace UD_Granit.Helpers
{
    public static class RightsManager
    {
        public static class Account
        {
            public static bool RegisterApplicant(User user) { return (user == null); }
            public static bool RegisterMember(User user) { return ((user is Administrator) || ((user is Member) && ((user as Member).Position == MemberPosition.Chairman))); }
            public static bool RegisterAdministrator(User user) { return (user is Administrator); }

            public static bool Edit(User user) { return ((user is Administrator) || (((user is Member) && ((user as Member).Position == MemberPosition.Chairman)))); }
            public static bool Edit(User user, User editedUser)
            {
                if ((user == null) || (editedUser == null)) return false;
                return ((user.Id == editedUser.Id) || (user is Administrator) || (((user is Member) && ((user as Member).Position == MemberPosition.Chairman) && !(editedUser is Administrator))));
            }

            public static bool Remove(User user) { return (user is Administrator); }
            public static bool Remove(User user, User removedUser)
            {
                if ((user == null) || (removedUser == null)) return false;
                return ((user is Administrator) || (user.Id == removedUser.Id) || ((user is Member) && ((user as Member).Position == MemberPosition.Chairman)));
            }

            public static bool ShowAdditionalInfo(User user) { return ((user is Member) || (user is Administrator)); }
            public static bool ShowAdditionalInfo(User user, User showedUser)
            {
                if ((user == null) || (showedUser == null)) return false;
                return ((user.Id == showedUser.Id) || (user is Member) || (user is Administrator));
            }
        }

        public static class Council
        {

        }

        public static class Dissertation
        {
            public static bool Show(User user, Models.Dissertation dissertation)
            {
                if (dissertation != null)
                {
                    if (user == null)
                    {
                        if (!dissertation.Defensed || dissertation.Administrative_Use)
                            return false;
                    }
                    else
                    {
                        if ((user is Applicant) && (user.Id != dissertation.Applicant.Id))
                            if (!dissertation.Defensed || dissertation.Administrative_Use)
                                return false;
                    }
                    return true;
                }
                return false;
            }

            public static bool Edit(User user, Models.Dissertation dissertation) { return ((user is Applicant) && (user.Id == dissertation.Applicant.Id)); }
        }

        public static class Speciality
        {

        }

        public static class Reply
        {
            public static bool AddReply(User user, Models.Dissertation dissertation) { return ((user is Applicant) && (dissertation != null) && ((user as Applicant).Id == dissertation.Id)); }
        }

        public static class Session
        {
            public static bool Create(User user) { return ((user is Administrator) || ((user is Member) && ((user as Member).Position == MemberPosition.Chairman))); }
            public static bool Result(User user) { return ((user is Administrator) || ((user is Member) && (((user as Member).Position == MemberPosition.Chairman) || ((user as Member).Position == MemberPosition.Secretary)))); }
        }
    }
}