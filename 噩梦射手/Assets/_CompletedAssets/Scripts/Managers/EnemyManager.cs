using UnityEngine;

namespace CompleteProject
{
    public class EnemyManager : MonoBehaviour
    {
        public PlayerHealth playerHealth;       // Reference to the player's heatlh.
        public GameObject enemy;                // The enemy prefab to be spawned.
        public GameObject[] bearenemy;
        public GameObject[] bunnyenemy;
        public GameObject[] hellenemy;
        public float spawnTime = 3f;            // How long between each spawn.
        public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
        public int total_enemy;
        //[HideInInspector]
        public int curr_enemy = 0;

        void Start ()
        {
            // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
            InvokeRepeating ("Spawn", spawnTime, spawnTime);
            
        }

        void Update()
        {
            if(enemy.tag == "bearmaster")
                bearenemy = GameObject.FindGameObjectsWithTag("bearmaster");
            else if(enemy.tag == "bunnymaster")
                bunnyenemy = GameObject.FindGameObjectsWithTag("bunnymaster");
            else if(enemy.tag == "hellemaster")
                hellenemy = GameObject.FindGameObjectsWithTag("hellemaster");
        }


        void Spawn ()
        {
            
            // If the player has no health left...
            if(playerHealth.currentHealth <= 0f)
            {
                // ... exit the function.
                return;
            }
            if(enemy.tag == "bearmaster")
            {
                if (curr_enemy >= total_enemy && bearenemy.Length >= total_enemy) return;
            } else if(enemy.tag == "bunnymaster")
            {
                if (curr_enemy >= total_enemy && bunnyenemy.Length >= total_enemy) return;
            } else if(enemy.tag == "hellemaster")
            {
                if (curr_enemy >= total_enemy && hellenemy.Length >= total_enemy) return;
            }
            

            // Find a random index between zero and one less than the number of spawn points.
            int spawnPointIndex = Random.Range (0, spawnPoints.Length);

            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
            Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            curr_enemy++;
        }
    }
}