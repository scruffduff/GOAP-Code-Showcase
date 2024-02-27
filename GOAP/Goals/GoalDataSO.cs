using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FacepunchDemo.GOAP
{
    // This Scriptable Objects acts a single place for designers to set all of the data associated with any insance of an goal.
    // It could be expanded further to include things like having a list of conditions that change how the goal priority changes (for example, getting tired at a faster rate during night time than during day time)

    [CreateAssetMenu(fileName = "New Goal Data", menuName = "Facepunch Demo/Goal Data")]
    public class GoalDataSO : ScriptableObject
    {
        public string goalName;
        public float minPriorityVaule;
        public float maxPriorityVaule;
        [Space]
        public float defaultPriorityChangePerTick;
    }
}
