using System;
using System.Collections.Generic;
using System.Text;

namespace StartCoach.Models
{
    public enum MenuItemType
    {
        Stranka,
        Browse,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
