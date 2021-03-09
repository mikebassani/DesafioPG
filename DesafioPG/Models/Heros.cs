using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesafioPG.Models
{
    public class Heros
    {
        public Data data { get; set; }

        public class Data
        {
           public List<Result> results { get; set; }
        }



        public class Result
        {
            public int      id           { get; set; }
            public string   name         { get; set; }
            public string   description  { get; set; } 
            public Comics   comics       { get; set; }
            public Series   series       { get; set; }
            public Stories  stories      { get; set; }
            public Events   events       { get; set; }
          
        }



        public class Comics
        {
           
            public List<Item> items { get; set; }
            
        }

        public class Series
        {
           
            public List<Item> items { get; set; }
          
        }

        public class Stories
        {
           
            public List<Item> items { get; set; }
           
        }

        public class Events
        {
           
            public List<Item> items { get; set; }
           
        }

      

        public class Item
        {
           
            public string name { get; set; }
            
        }







    }
}