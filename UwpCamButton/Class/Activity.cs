using System;

namespace UwpCamButton
{
    public class Activity
    {
        //přesný čas aktivity
        public DateTime DateAction { get; set; }
        //název aktivity
        public string Name { get; set; }
        //stručný popis aktivity
        public string Description { get; set; }
        //založení nové aktivity
        public Activity(string name, string description)
        {
            DateAction = DateTime.Now;
            Name = name;
            Description = description;
        }

        public string ListText
        {
            get
            {
                return DateAction.ToString() + "/" + Name;
            }
        }

        public override string ToString()
        {
            return DateAction.ToString();
        }
    }
}
