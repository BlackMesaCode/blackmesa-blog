using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlackMesa.Website.Main.Models.Learning
{
    [Serializable]
    public class BrowseList
    {
        public List<Card> Cards { get; set; }
        public int CardsCount { get; set; }
        public int Position { get; set; }
    }
}