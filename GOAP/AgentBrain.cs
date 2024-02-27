using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FacepunchDemo.GOAP
{
    [RequireComponent(typeof(AgentGoals))] 
    [RequireComponent(typeof(AgentActions))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentBrain : MonoBehaviour
    {
        private AgentGoals agentGoals;
        private AgentActions agentActions;
        private NavMeshAgent myNavMeshAgent;

        [Header("Current Goal")]
        [SerializeField, InlineEditor, ReadOnly] private Goal currentGoal;
        [Space]
        [Header("Current Action")]
        [SerializeField] private Action myCurrentAction;
        public float currentActionDurationTimer; // public as is set via UpdateAction in action classes

        #region Get Components & Subscribe To Events

        private void Awake()
        {
            // Get components and check they are not null
            agentGoals = GetComponent<AgentGoals>();
            if (agentGoals == null) { Debug.LogError("No AgentGoals Component Found!", this.gameObject); return; }

            agentActions = GetComponent<AgentActions>();
            if (agentActions == null) { Debug.LogError("No AgentActions Component Found!", this.gameObject); return; }

            myNavMeshAgent = GetComponent<NavMeshAgent>();
            if (myNavMeshAgent == null) { Debug.LogError("No NavMeshAgent Componet Found!", this.gameObject); return; }
        }

        private void OnEnable()
        {
            // Subscribe to when this agents current goal changes
            agentGoals.OnCurrentPriorityGoalChanged.AddListener(OnCurrentGoalChanged);
        }

        private void OnDisable()
        {
            // Unsubscribe from new goals
            agentGoals.OnCurrentPriorityGoalChanged.RemoveListener(OnCurrentGoalChanged);
        }

        private void OnCurrentGoalChanged(Goal newPriorityGoal)
        {
            // Save the new current goal
            currentGoal = newPriorityGoal;
        }

        #endregion

        // NOTE: In a production setting I would remove the Update loop from this and the agent goals class. Instead the functions within them would be called via a 'Agent Manager' class.
        // This class would store all the current active agents and then loop through and update one (or a few of them max) each frame.
        // This would allow for the game to have a lot more active agents without slowing down the CPU
        // If the game requires a very large number of agents to be active at any one time then moving to DOTS would be more appropriate
        private void Update()
        {
            if (myCurrentAction != null)
            {
                if (currentActionDurationTimer >= 0)
                {
                    // Continue performing my current action
                    myCurrentAction.UpdatePerformingAction(this);
                }
                else
                {
                    // Handle all logic for stopping my current action
                    CurrentActionCompleted();
                }
            }
            else
            {
                // I am 'idle' so find a new action that meets my goal and start performing it
                Action newAction = agentActions.GetLowestCostActionToStatisfyGoal(currentGoal);
                if (newAction == null) { Debug.Log("No Action"); return; }
                StartNewAction(newAction);
            }
        }

        private void StartNewAction(Action newAction)
        {
            // Store the current action and call its start logic 
            myCurrentAction = newAction;
            myCurrentAction.StartPerformingAction(this);
        }

        private void CurrentActionCompleted()
        {
            // Remove the current action and call its stop logic
            myCurrentAction.StopPerformingAction(this);
            myCurrentAction = null;
        }

        // Return my current goal (ensures it cannot be overwritten as a public var)
        public Goal GetCurretGoal() { return currentGoal; }

        #region Nav Mesh & Pathfinding
        public void SetNavMeshDestination(Vector3 destination)
        {
            myNavMeshAgent.SetDestination(destination);
        }

        public void ClearNavMeshDesination()
        {
            myNavMeshAgent.SetDestination(transform.position);
        }

        public float GetDistanceRemaining()
        {
            // returns the length of the current path
            return myNavMeshAgent.remainingDistance;
        }

        public float GetStoppingDistance()
        {
            // returns the threshold that the agent will stop at
            return myNavMeshAgent.stoppingDistance;
        }

        #endregion
    }
}
