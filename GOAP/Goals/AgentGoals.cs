using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FacepunchDemo.GOAP
{
    public class AgentGoals : MonoBehaviour
    {
        [SerializeField, InlineEditor, ReadOnly] private Goal currentPriorityGoal; // Read Only as this is only serialized for easier visual understanding of the current priority in editor
        [Space]
        [SerializeField] private Goal[] allGoals;
        [Space]
        public UnityEvent<Goal> OnCurrentPriorityGoalChanged; // Used to tell the Goal Action Planner when a new goal has been set

        private void Awake()
        {
            // Get all of this agent's goals & ensure they are set up correctly
            allGoals = GetComponentsInChildren<Goal>();

            // Check to make sure all goals have goal data
            foreach (Goal goal in allGoals)
            {
                if (goal.GetGoalData() == null) { Debug.LogError("Couldn't find goal data on goal!", goal); }
            }
        }

        // NOTE: In a production setting I would remove the Update loop from this and the agent brain. Instead the functions within them would be called via a 'Agent Manager' class.
            // This class would store all the current active agents and then loop through and update one (or a few of them max) each frame.
            // This would allow for the game to have a lot more active agents without slowing down the CPU
            // If the game requires a very large number of agents to be active at any one time then moving to DOTS would be more apropiate
        private void Update()
        {
            UpdateAllGoals();
        }

        private void UpdateAllGoals()
        {
            // Find the goal with the highest current priority
            float highestPriorityValue = float.MinValue;
            Goal potentialNewPriority = null;
            foreach (Goal characterGoal in allGoals)
            {
                float priorityValue = characterGoal.UpdateGoalPriority();
                if (priorityValue > highestPriorityValue) { 
                    potentialNewPriority = characterGoal;
                    highestPriorityValue = priorityValue;
                }
            }

            // Check for unexpected error and stop if so
            if (potentialNewPriority == null) { Debug.LogWarning("Couldn't find any potential goals!", this); return; }

            // Check if the highest priority goal is not the current goal. If so, set it to the current goal
            if (potentialNewPriority != currentPriorityGoal) {
                SetNewGoalPriority(potentialNewPriority);
            }
        }

        private void SetNewGoalPriority(Goal _newPriorityGoal)
        {
            // Set the new goal
            currentPriorityGoal = _newPriorityGoal;

            // Send out an event (subscribed to by this Agent's Goal Action Planner).
            OnCurrentPriorityGoalChanged.Invoke(currentPriorityGoal);
        }
    }
}
