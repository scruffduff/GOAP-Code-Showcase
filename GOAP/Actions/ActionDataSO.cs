using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FacepunchDemo.GOAP
{
    // This Scriptable Objects acts as a single place for designers to set all of the data associated with any instance of an action.
    // It could be expanded further to include things like success and failure chances as well as storing more data for the agent to use (such as animations or sound effects)

    [CreateAssetMenu(fileName = "New Action Data", menuName = "Facepunch Demo/Action Data")]
    public class ActionDataSO : ScriptableObject
    {
        public string actionName;

        [Tooltip("The goal that this action will aim to statisfy")] 
        public GoalDataSO targetGoalData;

        [Header("Action Cost")]
        [Range(1, 100)] public int defaultActionCost = 1;

        [Header("Action Duration")]
        [Tooltip("The shortest an action will last for in seconds")] public float minActionDuration = 1f;
        [Tooltip("The longest an action will last for in seconds")] public float maxActionDuration = 3f;

        [Header("Goal Satisfying")]
        [Tooltip("The amount this action will change the current goal once it has been preformed")] public float goalChangeAmountOnCompletion = -100f;
    }
}
