using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodenavirusSim.Models
{
    /* Class for statistics from the simulation
     * An object holds information for the day of the simulation
     * and the number of healthy, infected, recovered people on it
     */
    public class Stat
    {
        public int StatId { get; set; }
        public int Day { get; set; }
        public int HealthyCount { get; set; }
        public int InfectedCount { get; set; }
        public int RecoveredCount { get; set; }

        public Stat()
        {
            this.Day = 0;
            this.HealthyCount = 0;
            this.InfectedCount = 0;
            this.RecoveredCount = 0;
        }

        public Stat(int day, int iCount, int rCount, int hCount)
        {
            this.Day = day;
            this.HealthyCount = hCount;
            this.InfectedCount = iCount;
            this.RecoveredCount = rCount;
        }

        public Stat(Human[,] matrix)
        {
            this.Day = 0;
            this.InfectedCount = 0;
            this.RecoveredCount = 0;

            int count = 0;
            foreach (var m in matrix)
            {
                if (m is Human)
                {
                    count++;
                }
            }

            this.HealthyCount = count;
        }
    }
}
