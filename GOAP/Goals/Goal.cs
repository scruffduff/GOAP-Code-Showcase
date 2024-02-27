using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FacepunchDemo.GOAP
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private GoalDataSO goalData;

        [SerializeField] private float goalPriority; // The current priority of this goal instance

        public GoalDataSO GetGoalData() { return goalData; }

        public float GetGoalPriority() { return goalPriority; }


        // Used by actions to change the goals priority once they have completed their behaviour
            // This function ensures that goals are not performed over and over (making more more realistic and varried AI)
        public void OverrideGoalPriority(float newVaule)
        {
            goalPriority = newVaule;
        }


        // This is called by the Agent Goals class every time the agent is updated.
        // If needed a child class could override this function to create more custom behaviours
            // For example, having a 'fall back' or 'heal' goal only increase when the agent is on low health
        public virtual float UpdateGoalPriority()
        {
            goalPriority += goalData.defaultPriorityChangePerTick * Time.deltaTime;
            goalPriority = Mathf.Clamp(goalPriority, goalData.minPriorityVaule, goalData.maxPriorityVaule);

            return goalPriority;
        }
    }
}
