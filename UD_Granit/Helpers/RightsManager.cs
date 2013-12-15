using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UD_Granit.Models;

namespace UD_Granit.Helpers
{
    // Задаёт правила доступа к определённым действиям
    public static class RightsManager
    {
        // Задаёт права доступа по работе с учётными записями
        public static class Account
        {
            public static bool RegisterApplicant(User user) { return ((user == null)  || ((user is Member) && ((user as Member).Position == MemberPosition.Secretary))); }
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
                return ((user is Administrator) || (user.Id == removedUser.Id) || ((user is Member) && ((user as Member).Position == MemberPosition.Chairman) && !(removedUser is Administrator)));
            }

            public static bool ShowAdditionalInfo(User user) { return ((user is Member) || (user is Administrator)); }
            public static bool ShowAdditionalInfo(User user, User showedUser)
            {
                if ((user == null) || (showedUser == null)) return false;
                return ((user.Id == showedUser.Id) || (user is Member) || (user is Administrator));
            }
        }

        // Задаёт права доступа по работе с диссертациями
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
                        if ((user is Applicant) && (user.Id != dissertation.Id))
                            if (!dissertation.Defensed || dissertation.Administrative_Use)
                                return false;
                    }
                    return true;
                }
                return false;
            }

            public static bool Edit(User user, Models.Dissertation dissertation) { return ((user is Applicant) && (user.Id == dissertation.Applicant.Id)); }
        }

        // Задаёт права доступа по работе со специальностями
        public static class Speciality
        {
            public static bool Control(User user) { return ((user is Administrator) || ((user is Member) && ((user as Member).Position == MemberPosition.Chairman))); }
        }

        // Задаёт права доступа по работе с отзывами
        public static class Reply
        {
            public static bool Control(User user, Models.Dissertation dissertation) { return ((user is Applicant) && (dissertation != null) && ((user as Applicant).Id == dissertation.Id)); }
        }

        // Задаёт права доступа по работе с заседаниями
        public static class Session
        {
            public static bool Create(User user) { return ((user is Administrator) || ((user is Member) && ((user as Member).Position == MemberPosition.Secretary))); }
            public static bool Edit(User user) { return ((user is Administrator) || ((user is Member) && (((user as Member).Position == MemberPosition.Chairman) || ((user as Member).Position == MemberPosition.Secretary)))); }
        }
    }
}