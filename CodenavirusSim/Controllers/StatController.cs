using CodenavirusSim.Data;
using CodenavirusSim.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodenavirusSim.Controllers
{
    /*
     * Controller class for handling HTTP responses
     */
    [ApiController]
    [Route("api/stats")]
    public class StatController : ControllerBase
    {
        private readonly IDbRepo repository;

        public StatController(IDbRepo repo)
        {
            repository = repo;
        }

        /*
         * Get response for retrieving statistics of a simulation by its ID
         * returns a list of the stats
         */
        [HttpGet("{simId}")]
        public ActionResult <IEnumerable<Stat>> GetStatsById (int simId)
        {
            var list = repository.GetSimStats(simId);

            return Ok(list);
        }

        /*
         * Post response for getting input data from client.
         * Stores inputd data in DB
         * Begins simulations
         * Responds back with statistics
         */
        [HttpPost]
        public ActionResult PostInputData(Simulation simulationInput)
        {
            repository.InsertInputData(simulationInput);

            Simulation simulation = new Simulation(simulationInput.SimId, simulationInput.InputScale, simulationInput.FirstInfected);
            simulation.startSim();

            repository.InsertStats(simulation);

            return Ok(simulation);
        }
    }
}

