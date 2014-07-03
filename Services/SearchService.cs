/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using Portal.Helpers;

namespace Portal.Services
{
    public class SearchService
    {

        internal static Team FindTeam(string name, Models.SportDataContext ctx)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            var upperName = name.ToUpper();
            var teams = ctx.Teams.Where(t => t.Name.StartsWith(upperName));
            if (teams.Count() != 1) return null;
            return teams.First();
        }

        internal static async Task<User> FindUser(string license, SportDataContext ctx)
        {
            if (license == null) return null;
            var licenseFormat = license.ToUpper();
            return await ctx.Users.Include(u=>u.Licenses).FirstOrDefaultAsync(u => u.Licenses.Any(l => l.Number == licenseFormat));
        }

        internal static async Task<User> FindUserById(string userId, SportDataContext ctx)
        {
            if (userId == null) return null;
            return await ctx.Users.Include(u => u.Licenses).FirstOrDefaultAsync(u => u.Id == userId);
        }

        internal static async Task<User> FindUserName(string firstName, string lastName, SportDataContext ctx)
        {
            if (firstName == null && lastName==null) return null;
            lastName = lastName.OrDefault().ToUpper();
            var user =  await ctx.Users.Include(u => u.Licenses).FirstOrDefaultAsync(u => (u.LastName == lastName && u.FirstName==firstName));
            //if (user == null)
            //    user = await ctx.Users.Include(u => u.Licenses).FirstOrDefaultAsync(u => u.LastName + " "+ u.FirstName == lastName + " " + firstName);
            return user;
        }
    }

    
}