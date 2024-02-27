using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FacepunchDemo.GOAP;
using UnityEngine.Rendering;

namespace FacepunchDemo.Game
{
    public class SleepAction : Action
    {
        [SerializeField] private ParticleSystem myParticleSystem; // Should be set via prefab reference

        private void Awake()
        {
            // Ensure I have the needed components
            if (myParticleSystem == null) { Debug.LogError("I cannot find a particle system", this.gameObject); }
        }

        public override void StartPerformingAction(AgentBrain performingAgent)
        {
            // Run the base logic
            base.StartPerformingAction(performingAgent);
            
            // Play some floating 'z' particles above the agents head (like in cartoons)
            myParticleSystem.Play();
        }

        public override void StopPerformingAction(AgentBrain performingAgent)
        {
            // Run the base logic 
            base.StopPerformingAction(performingAgent);
            
            // Stop those 'z' particles
            myParticleSystem.Stop();
        }
    }
}
