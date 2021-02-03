using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodenavirusSim.Models
{
    /* Runs simulation by the given data
     */
    public class Simulation
    {
        public int SimId { get; set; }
        // string array of symbols for the scale of the simulation
        public string[,] InputScale { get; set; }
        // matrix of Human objects based on the input scale
        public Human[,] Scale { get; set; }
        // coordinates of first infected human within the scale
        public int[] FirstInfected { get; set; }
        // list for storing the statistics for each day of the simulation
        public List<Stat> Stats { get; set; }

        public Simulation() { }

        /* Constructor. Initializes the id, scale, first infected, stats in the sim
         */
        public Simulation(int id, string[,] scale, int[] firstI)
        {
            this.SimId = id;
            Scale = new Human[scale.GetLength(0), scale.GetLength(1)];
            fillMatrix(scale);
            FirstInfected = firstI;
            Stats = new List<Stat>();
            runStatistics(0);
            setPatientZero();
        }

        /* Prepares a matrix of Human obejcts based on the input data
         * # for human; . for empty space
         */
        void fillMatrix(string[,] matrix)
        {
            Human nonHuman = new Human();
            nonHuman.Status = Status.N;
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    if (matrix[row, col] == "#")
                    {
                        Scale[row, col] = new Human();
                    }
                    else if (matrix[row, col] == ".")
                    {
                        Scale[row, col] = nonHuman;
                    }
                }
            }
        }

        void printMatrix(Human[,] matrix)
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    Console.Write(matrix[row, col].ToString() + " ");
                }
                Console.Write(Environment.NewLine);
            }
        }
        void printMatrix()
        {
            for (int row = 0; row < Scale.GetLength(0); row++)
            {
                for (int col = 0; col < Scale.GetLength(1); col++)
                {
                    if (Scale[row, col] != null)
                    {
                        Console.Write(Scale[row, col].ToString() + " ");
                    }
                    else
                    {
                        Console.Write(". ");
                    }
                }
                Console.Write(Environment.NewLine);
            }
        }

        /* Sets the object sitting at the FirstInfected property coordinates as infected
         */
        void setPatientZero()
        {
            Scale[FirstInfected[0], FirstInfected[1]].Infect();
        }

        /* Starts simulation
         */
        public void startSim()
        {
            bool infectionFlag = true; // loop stops when false;
            int dayCount = 0;

            /* loops until infectionFlag is false, meaning that no infecting has occurred in that day (condition to stop sim)
             */
            while (infectionFlag || dayCount == 1)
            {
                infectionFlag = false;
                dayCount++;

                // searching through matrix of Human obj starting from [0,0]
                for (int row = 0; row < Scale.GetLength(0); row++)
                {
                    for (int col = 0; col < Scale.GetLength(1); col++)
                    {
                        /* Checks if the next 'cell' in matrix is a human, able to infect
                         * Human object cannot becomes Healthy on Day 3 and cannot infect anymore
                         */
                        // edit && to ||
                        if (Scale[row, col].Status == Status.N || Scale[row, col].InfectedDays == 3)
                        {
                            continue;
                        }

                        // checks if current position's Human can infect
                        if (isInfected(Scale[row, col]) && Scale[row, col].InfectedDays != 0)
                        {
                            /* checks if the next possible direction to check is in bounds of the array
                             * looks if the 'neighbour' can be infected by checking if it's a Human and if it's status is Healthy
                             */
                            // right check
                            if (col + 1 < Scale.GetLength(1) && Scale[row, col + 1].Status != Status.N && Scale[row, col + 1].Status == Status.H)
                            {
                                Scale[row, col + 1].Infect();
                                infectionFlag = true;
                                continue;
                            }
                            // top check
                            if (row - 1 >= 0 && Scale[row - 1, col].Status != Status.N && Scale[row - 1, col].Status == Status.H)
                            {
                                Scale[row - 1, col].Infect();
                                infectionFlag = true;
                                continue;
                            }
                            // left check
                            if (col - 1 >= 0 && Scale[row, col - 1].Status != Status.N && Scale[row, col - 1].Status == Status.H)
                            {
                                Scale[row, col - 1].Infect();
                                infectionFlag = true;
                                continue;
                            }
                            // bottom check
                            if (row + 1 < Scale.GetLength(0) && Scale[row + 1, col].Status != Status.N && Scale[row + 1, col].Status == Status.H)
                            {
                                Scale[row + 1, col].Infect();
                                infectionFlag = true;
                                continue;
                            }

                        }

                    }
                }

                // call method for updating statistics
                runStatistics(dayCount);
            }

        }

        /* Checks matrix to fill list with statistics on how many
         * infected, healthy, recovered were there in the given day
         */
        private void runStatistics(int dayCount)
        {
            // stats after last day!!!!!!
            int inf = 0;
            int rec = 0;
            int hlt = 0;

            foreach (Human h in Scale)
            {
                if (h != null)
                {
                    h.check();
                    switch (h.Status)
                    {
                        case Status.H:
                            hlt += 1;
                            break;
                        case Status.I:
                            inf += 1;
                            break;
                        case Status.R:
                            rec += 1;
                            break;
                    }
                }


            }

            this.Stats.Add(new Stat(dayCount, inf, rec, hlt));
        }

        /* Checks status of given Human object
         * true if status is Infected;
         * false if status isn't Infected
         */
        bool isInfected(Human h)
        {
            return h.Status == Status.I;
        }
    }
}
