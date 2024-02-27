using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FacepunchDemo.Game
{
    // This is an example of a game world manager that can store the state of the world and provide the nessary information to agents and other game objects.
        // This approach ensures that there is one, easy to access place for finding resources within the world and prevents agents needing to find target obejct's themselves on a per action bias.

    public class GameWorldManager : MonoBehaviour
    {
        public static GameWorldManager Instance;

        [SerializeField] private Vector2 worldDimensions;

        [Header("Food Source Spawning")]
        [SerializeField] private float maxNumberOfFoodSources;
        [SerializeField] private GameObject foodSourcePrefab;
        [SerializeField] private List<GameObject> foodSources;

        // Set Up Singleton Pattern & Ensure I am set up correctly
        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            else { Debug.LogError("One or more instances of 'Sheep Pen Manager' found!", this.gameObject); }

            if (foodSourcePrefab == null) { Debug.LogError("No food source prefab assigned!", this.gameObject); }
        }

        private void Start()
        {
            // Spawn in food around the game world
            for (int i = 0; i < maxNumberOfFoodSources; i++)
            {
                SpawnNewFoodSource();
            }
        }

        #region Food Source
        // Returns a random food source but in a production setting should be changed to return the closest food source to a input position
        public GameObject GetFoodSource()
        {
            return foodSources[Random.Range(0, foodSources.Count - 1)];
        }


        // Can be called GOAP actions when ever a agent eats a piece of food.
        public void ConsumeFoodSource(GameObject foodSource)
        {
            // Check for unacounted for object
            if (foodSources.Contains(foodSource) == false) { Debug.Log("Could find that food source in list.", foodSource); return; }

            // Destroy food source and spawn a new one (in a production seting a object pooling system should be used instead)
            foodSources.Remove(foodSource);
            Destroy(foodSource);
            SpawnNewFoodSource();
        }

        // Spawns new food source at a random position and adds it to the list
        private void SpawnNewFoodSource()
        {
            GameObject newFoodSource = Instantiate(foodSourcePrefab, this.transform);
            foodSources.Add(newFoodSource);

            Vector3 grassPosition = GetRandomPointInWorldBounds();
            newFoodSource.transform.position = grassPosition;
            newFoodSource.transform.eulerAngles = new Vector3(0f, Random.Range(-180f, 180f), 0f);
        }
        #endregion

        // Can be used by any object to get a point that is within the world.
            // This ensures agents never try to walk off the nav mesh and food always spawns in a place agents can get to
        public Vector3 GetRandomPointInWorldBounds()
        {
            Vector3 randomPoint = new Vector3(Random.Range(-worldDimensions.x / 2f, worldDimensions.x / 2f), 0f, Random.Range(-worldDimensions.y / 2f, worldDimensions.y / 2f));
            return randomPoint;
        }

#if UNITY_EDITOR // Draw some gizmos for the level designers
        private void OnDrawGizmosSelected()
        {
            // Draw the world dimensions in editor
            Gizmos.color = Color.yellow;
            Vector3 centre = (transform.position + ((Vector3.up * 3f) / 2f));
            Gizmos.DrawWireCube(centre, new Vector3(worldDimensions.x, 3f, worldDimensions.y));
        }
#endif
    }
}
