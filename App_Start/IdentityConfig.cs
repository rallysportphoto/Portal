/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Portal
{
    //public class MyDbInitializer : DropCreateDatabaseAlways<SportDataContext>
    //{
    //    protected override void Seed(SportDataContext context)
    //    {
    //        var UserManager = new UserManager<User>(new UserStore<User>(context));
    //        var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

    //        string name = "Admin";
    //        string password = "123456";
    //        string test = "test";

    //        //Create Role Test and User Test
    //        RoleManager.Create(new IdentityRole(test));
    //        UserManager.Create(new User() { UserName = test });

    //        //Create Role Admin if it does not exist
    //        if (!RoleManager.RoleExists(name))
    //        {
    //            var roleresult = RoleManager.Create(new IdentityRole(name));
    //        }

    //        //Create User=Admin with password=123456
    //        var user = new User();
    //        user.UserName = name;            
    //        var adminresult = UserManager.Create(user, password);

    //        //Add User Admin to Role Admin
    //        if (adminresult.Succeeded)
    //        {
    //            var result = UserManager.AddToRole(user.Id, name);
    //        }
    //        base.Seed(context);
    //    }
    //}
}