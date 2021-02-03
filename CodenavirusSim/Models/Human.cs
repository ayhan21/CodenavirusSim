using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodenavirusSim.Models
{
    /* Obects hold data for one human, who's default status is H (Healthy)
     */
    public class Human
    {
        public Status Status { get; set; }
        public int InfectedDays { get; set; }

        public Human()
        {
            this.Status = Status.H;
        }

        /* Changes status of Human to Infected and 
         */
        public void Infect()
        {
            this.Status = Status.I;
            this.InfectedDays = 0;
        }

        public override string ToString()
        {
            return this.Status.ToString();
        }

        /* Checks the infection of a human for each passing day in the simulation
         * If the infected days aren't at 3, the day increments by 1
         * If it's been infected for 3 days, status becomes Healthy
         */
        public void check()
        {
            if (this.InfectedDays < 3 && this.Status == Status.I)
            {
                this.InfectedDays += 1;
            }
            else if (this.InfectedDays == 3)
            {
                this.Status = Status.R;
            }
        }

        public void nextDay()
        {
            if (this.InfectedDays < 3)
            {
                this.InfectedDays += 1;
            }
        }
    }
}
