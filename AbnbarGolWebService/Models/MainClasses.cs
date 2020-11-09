using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbnbarGolWebService.Models
{
    public class Controls
    {
        public string Name { get; set; }
        public List<Item> Items { get; set; }

        public Controls()
        {
            Items = new List<Item>();
        }
    }

    public class Item
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
}