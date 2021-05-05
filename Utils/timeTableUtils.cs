using OOP_CA_Macintosh.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OOP_CA_Macintosh.Utils
{
    public class timeTableUtils
    {
        public static List<Events> GetEvent(int studentId, List<Events> allEvents)
        {
            List<Events> events = new List<Events> { };

            foreach (Events ev in allEvents)
            {
                if (ev.Id.Equals(studentId))
                {
                    events.Add(ev);
                }
            }
            return events;
        }
    }

}