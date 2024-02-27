using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FacepunchDemo.Game
{
    public class SpawnAgentsOnStart : MonoBehaviour
    {
        [SerializeField] private int numberToSpawn;
        [SerializeField] private GameObject agentPrefab;

        private void Start()
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                GameObject agentPrefab = Instantiate(this.agentPrefab, this.transform);
                agentPrefab.transform.position = GameWorldManager.Instance.GetRandomPointInWorldBounds();
                agentPrefab.transform.eulerAngles = new Vector3(0f, Random.Range(-180, 180), 0f);
            }
        }
    }
}
