using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MyLittleMergeTool
{
    public static class Settings
    {
        public static String Break
        {
            get
            {
                return
                ConfigurationManager.AppSettings["BreakString"];
            }
        }

        public static String UseMatchRegex
        {
            get
            {
                return ConfigurationManager.AppSettings["UseMatchRegex"];
            }
        }

       

    }
}
