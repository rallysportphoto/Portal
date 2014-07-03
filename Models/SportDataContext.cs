/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Areas.Shop.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class SportDataContext : IdentityDbContext<User>
    {
        public SportDataContext()
            : base("SportDataContext")
        {            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>()
                .ToTable("Users");
            modelBuilder.Entity<User>()
                .ToTable("Users");
        }

        public IDbSet<News> News { get; set; }
        public IDbSet<Tag> Tags { get; set; }
        public IDbSet<File> Files { get; set; }
        public IDbSet<Event> Events { get; set; }
        public IDbSet<Series> Series { get; set; }
        public IDbSet<Group> Groups { get; set; }
        public IDbSet<Team> Teams { get; set; }
        public IDbSet<Partner> Partners { get; set; }
        public IDbSet<License> Licenses { get; set; }
        public IDbSet<Site> Sites { get; set; }

        public System.Data.Entity.DbSet<Portal.Models.SubEvent> SubEvents { get; set; }

        public System.Data.Entity.DbSet<Portal.Models.Accreditation> Accreditations { get; set; }

        public System.Data.Entity.DbSet<Portal.Models.Order> Orders { get; set; }

        public IDbSet<Gallery> Galleries { get; set; }

        public IDbSet<Product> Products {get;set;}
        public IDbSet<Category> Categories { get;set;}
        public IDbSet<PaymentProvider> PaymentProviders { get; set;}
        public IDbSet<DeliveryProvider> DeliveryProviders { get; set; }
        public IDbSet<ShopOrder> ShopOrders { get; set; }
    }
}