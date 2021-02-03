using CodenavirusSim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodenavirusSim.Data
{
    /* Interface providing methods for database operations
     */
    public interface IDbRepo
    {
        IEnumerable<Stat> GetSimStats(int simId);
        void InsertInputData(Simulation sim);
        void InsertStats(Simulation sim);
    }
}
