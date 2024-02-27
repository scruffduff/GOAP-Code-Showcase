using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FacepunchDemo.GOAP
{
    public class AgentActions : MonoBehaviour
    {
        [SerializeField] private Action[] allActions;

        private void Awake()
        {
            // Get all my actions
            allActions = GetComponentsInChildren<Action>();

            // Check to make sure all my actions have a target goal
            foreach (Action action in allActions)
            {
                if (action.GetActionData() == null) { Debug.LogError("Couldn't find action data!", action); }
                if (action.GetActionData().targetGoalData == null) { Debug.LogError("Couldn't find a target goal on Action Data: " + action.GetActionData().actionName); }
            }
        }

        public Action GetLowestCostActionToStatisfyGoal(Goal targetGoal)
        {
            // Loop through all the actions and return the one with the lowest closest but also that satisfys the target
            Action lowestCostAction = null;
            int lowestCost = 999;
            foreach (Action action in allActions)
            {
                if (action == null) { Debug.Log("No action found", this.gameObject); continue; }
                if (action.GetActionData() == null) { Debug.Log("No targetGoalData", this.gameObject); continue; }
                if (action.GetActionData().targetGoalData == null) { Debug.Log("No action.getactiondata.targetgoaldata", this.gameObject); continue; }
                if (targetGoal == null) { Debug.Log("No target goal found", this.gameObject); continue; }
                if (targetGoal.GetGoalData() == null) { Debug.Log("target goal.getgoaldata", this.gameObject); continue; }

                // Check if this action does not meet the target goal
                if (action.GetActionData().targetGoalData != targetGoal.GetGoalData()) { continue; }

                // Get the cost of this action & then save it if needed
                int actionCost = action.GetActionCost();
                if (actionCost <= lowestCost) {
                    lowestCostAction = action;
                    lowestCost = actionCost;
                }
            }

            return lowestCostAction;
        }
    }
}
