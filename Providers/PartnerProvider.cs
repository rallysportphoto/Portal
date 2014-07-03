/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Portal.Providers
{
    public class PartnerProvider
    {
        internal static List<PartnerListViewModel> GetGlobal()
        {
            using (var ctx = new SportDataContext())
            {
                return ctx.Partners.Include(p=>p.Logo).Where(p => p.IsEnabled && p.Global).ToList().Select(p => new PartnerListViewModel(p)).ToList();
            }
        }
    }
}