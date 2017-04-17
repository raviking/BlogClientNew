using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogClientNew.Models
{
    public class JqViewModel
    {
        //no of records to fetch
        public int rows { get; set; }
        
        //page number (page index)
        public int page { get; set; }

        //sort column name
        public string sidx { get; set; }

        //sorting order "asc" or "desc"
        public string sord { get; set; }
    }
}