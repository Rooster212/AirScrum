using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScrumTool.Database;
using WebGrease.Css.Extensions;

namespace ScrumTool.Objects
{
    public class UserManage
    {
        public List<string> AssignedRoles;
        public List<string> NotAssignedRoles;
        public string Username;

        public UserManage(string username)
        {
            this.Username = username;
            using (ScrumDB db = new ScrumDB())
            {
                var user = db.AspNetUsers.FirstOrDefault(u => u.UserName == username);

                var allRoleNames = db.AspNetRoles.Select(i => i.Name).ToList();

                if (user == null) throw new Exception("User not found with username \""+username+"\"");

                AssignedRoles = user.AspNetRoles.Select(i => i.Name).ToList();

                NotAssignedRoles = new List<string>();
                foreach (var role in allRoleNames.Where(role => !AssignedRoles.Contains(role)))
                {
                    NotAssignedRoles.Add(role);
                }
            }
        }
    }
}