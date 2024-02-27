using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FacepunchDemo.GOAP
{
    public class Action : MonoBehaviour
    {
        [SerializeField] private ActionDataSO actionData;
        public ActionDataSO GetActionData() { return actionData; }

        public virtual int GetActionCost()
        {
            // Ensure that by default the action cost is not zero or out of bounds
                // Otherwise the agent's GetLowestCost() function might skip over an action with cost zero (if it is the first checked action)
            return Mathf.Clamp(actionData.defaultActionCost, 1, 100);
        }

        public virtual void StartPerformingAction(AgentBrain performingAgent)
        {
            Debug.Log(performingAgent.gameObject.name + " starting performing " + actionData.actionName);

            // Set the agents action timer to a random value with this actions range
            performingAgent.currentActionDurationTimer = Random.Range(actionData.minActionDuration, actionData.maxActionDuration);
        }

        public virtual void UpdatePerformingAction(AgentBrain performingAgent)
        {
            // Tick the agent's timer down
            performingAgent.currentActionDurationTimer -= Time.deltaTime;

            // When the timer is zero then stop this action
            if (performingAgent.currentActionDurationTimer <= 0) { StopPerformingAction(performingAgent); }
        }

        public virtual void StopPerformingAction(AgentBrain performingAgent)
        {
            Debug.Log(performingAgent.gameObject.name + " stopped performing " + actionData.actionName);

            // Get the current target goal and change it's priority by this action's goal change amount
                // NOTE: In a production setting, I would run a check just incase that for some reason the current goal is not this action's target goal
            Goal targetGoal = performingAgent.GetCurretGoal();
            float currentPriority = targetGoal.GetGoalPriority();
            targetGoal.OverrideGoalPriority(currentPriority + actionData.goalChangeAmountOnCompletion);
        }
    }
}