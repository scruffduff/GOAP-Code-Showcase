using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FacepunchDemo.GOAP;

namespace FacepunchDemo.Game
{
    public class WanderAction : Action
    {
        // This would be a 'fall back action' that is run to ensure the agent is always doing something.
                // This would be set up in engine by having a 'wander goal' that never changes it's priority.  The priority should be set at something not too high to ensure that other goals often have a higher priority.
                // On finishing running this action, the priority of the goal should not change to allow the agent to run it repeatidly until another action has priority

        public override void StartPerformingAction(AgentBrain performingAgent)
        {
            // Perform the base action
            base.StartPerformingAction(performingAgent);

            // Get a random destination from the world manager and start moving toward it
            Vector3 randomPositionOnNavmesh = GameWorldManager.Instance.GetRandomPointInWorldBounds();
            performingAgent.SetNavMeshDestination(randomPositionOnNavmesh);
        }

        public override void UpdatePerformingAction(AgentBrain performingAgent)
        {
            // Perform the base logic to allow the timer to tick down
               // If the timer reaches zero before I reach the destination then stop the action anyway
            base.UpdatePerformingAction(performingAgent);

            // Check if I have reached my destination then get a new one and start wandering over there
                // The completion of this action is tied to the action's duration not reaching the destination
            if (performingAgent.GetDistanceRemaining() <= performingAgent.GetStoppingDistance())
            {
                Vector3 randomDirectionVector = GameWorldManager.Instance.GetRandomPointInWorldBounds(); ;
                performingAgent.SetNavMeshDestination(randomDirectionVector);
            }
        }

        public override void StopPerformingAction(AgentBrain performingAgent)
        {
            // Perform the base logic
            base.StopPerformingAction(performingAgent);

            // Ensure the agent does not have a path before picking a new behaviour
            performingAgent.ClearNavMeshDesination();
        }
    }
}
