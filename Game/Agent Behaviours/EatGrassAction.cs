using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FacepunchDemo.GOAP;
using UnityEngine.AI;

namespace FacepunchDemo.Game
{
    public class EatGrassAction : Action // Inherits & overrides base action class
    {
        GameObject targetFoodSource;

        public override void StartPerformingAction(AgentBrain performingAgent)
        {
            // Run all required base logic
            base.StartPerformingAction(performingAgent);

            // Get a food source from the World Manager and get the agent to move to it
            targetFoodSource = GameWorldManager.Instance.GetFoodSource(); // save the food source so it can be deleted later
            performingAgent.SetNavMeshDestination(targetFoodSource.transform.position);

            // NOTE: I have chosen to make the 'world manager' responsible for finding food sources instead of the agent searching for it themselves
                // This prevents situations where you have lots of agents all running their own checks looking for specific objects in large worlds
        }

        public override void UpdatePerformingAction(AgentBrain performingAgent)
        {
            // Do not call base.Update by default as timer should not tick down unless the agent is at the food source

            // Get the distance to my food source and then update the timer ONLY if I have reached it
            float distanceToFoodSource = performingAgent.GetDistanceRemaining();
            if (distanceToFoodSource <= performingAgent.GetStoppingDistance()) {
                base.UpdatePerformingAction(performingAgent);
            } else {
                // I have reached the food source so stop pathfinding
                performingAgent.ClearNavMeshDesination();
            }

            // NOTE: If this was production code some additonal check should be added to make sure the agent hasn't become stuck on its way to the food source.
        }

        public override void StopPerformingAction(AgentBrain performingAgent)
        {
            // Run all required base logic 
            base.StopPerformingAction(performingAgent);

            // Remove the food source from the world and spawn another (via the world manager)
            GameWorldManager.Instance.ConsumeFoodSource(targetFoodSource);
        }
    }
}
