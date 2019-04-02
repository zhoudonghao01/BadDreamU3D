using UnityEngine;

namespace CompleteProject
{
    public class GameOverManager : MonoBehaviour
    {
        public PlayerHealth playerHealth;       // Reference to the player's health.

        private bool once = false;
        Animation anim;                          // Reference to the animator component.


        void Awake ()
        {
            // Set up the reference.
            anim = GetComponent <Animation> ();
            once = true;
        }


        void Update ()
        {
            // If the player has run out of health...
            if(playerHealth.currentHealth <= 0 && once)
            {
                // ... tell the animator the game is over.
                //anim.SetTrigger ("GameOver");
                anim.Play();
                once = false;
            }
        }
    }
}