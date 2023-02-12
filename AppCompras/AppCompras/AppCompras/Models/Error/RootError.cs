using System;
using System.Collections.Generic;
using System.Text;

namespace AppCompras.Models.Error
{
    public class Errors
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<Details> errors { get; set; }
    }

    public class Details
    {
        public string message { get; set; }
        public string domain { get; set; }
        public string reason { get; set; }
    }

    public class RootError
    {
        public Errors error { get; set; }
    }
}
