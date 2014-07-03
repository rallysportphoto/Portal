/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class DocumentsTagViewModel
    {
        public DocumentsTagViewModel(DocumentGroup group)
        {
            Name = group.Name;
            Files = group.Files.Select(f => new FileViewModel(f)).ToList();            
        }

        
        public string Name { get; set; }
        public List<FileViewModel> Files { get; set; }
    }
}