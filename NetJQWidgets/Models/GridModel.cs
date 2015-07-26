using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace NetJQWidgets.Models
{
    public class Data
    {
        public string name { get; set; }
        public string productname { get; set; }
        public bool available { get; set; }
        public DateTime date { get; set; }
        public int quantity { get; set; }
    }
}
