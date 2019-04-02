using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace CompleteProject
{
    public class PlayerShooting : MonoBehaviour
    {
        public int damagePerShot = 20;                  // The damage inflicted by each bullet.
        public float timeBetweenBullets = 0.15f;        // The time between each shot.
        public float range = 100f;                      // The distance the gun can fire.
        public bool strongbullet = false;

        float timer;                                    // A timer to determine when to fire.
        [HideInInspector]
        public float bulletTimer = 0;                   //子弹持续时间
        public float bullet_Time = 0;
        Ray shootRay;                                   // A ray from the gun end forwards.
        Ray shootRayRight;
        Ray shootRayLeft;
        RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
        RaycastHit shootHitright;
        RaycastHit shootHitleft;
        int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
        ParticleSystem gunParticles;                    // Reference to the particle system.
        LineRenderer gunLine;                           // Reference to the line renderer.
        LineRenderer gunLineLeft;
        LineRenderer gunLineRight;
        AudioSource gunAudio;                           // Reference to the audio source.
        Light gunLight;                                 // Reference to the light component.
		public Light faceLight;								// Duh
        float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.
        Transform GunBarrelEndleft;
        Transform GunBarrelEndright;

        void Awake ()
        {
            // Create a layer mask for the Shootable layer.
            shootableMask = LayerMask.GetMask ("Shootable");

            // Set up the references.
            gunParticles = GetComponent<ParticleSystem> ();
            gunLine = GetComponent <LineRenderer> ();
            gunAudio = GetComponent<AudioSource> ();
            gunLight = GetComponent<Light> ();
			faceLight = GetComponentInChildren<Light> ();
            gunLineLeft = GameObject.Find("GunBarrelEndleft").GetComponent<LineRenderer>();
            gunLineRight = GameObject.Find("GunBarrelEndright").GetComponent<LineRenderer>();
            gunLineLeft.enabled = false;
            gunLineRight.enabled = false;
            GunBarrelEndleft = GameObject.Find("GunBarrelEndleft").GetComponent<Transform>();
            GunBarrelEndright = GameObject.Find("GunBarrelEndright").GetComponent<Transform>();
        }


        void Update ()
        {
            // Add the time since Update was last called to the timer.
            timer += Time.deltaTime;
            bulletTimer += Time.deltaTime;
            if(bulletTimer >= bullet_Time && strongbullet)
            {
                    strongbullet = false;
            }
#if !MOBILE_INPUT
            // If the Fire1 button is being press and it's time to fire...
            //if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
            if(Input.GetKeyDown(KeyCode.F) && timer >= timeBetweenBullets && Time.timeScale != 0)
            {
                // ... shoot the gun.
                if (strongbullet)
                    Strongshoot();
                else
                    Shoot ();
            }
#else
            // If there is input on the shoot direction stick and it's time to fire...
            if ((CrossPlatformInputManager.GetAxisRaw("Mouse X") != 0 || CrossPlatformInputManager.GetAxisRaw("Mouse Y") != 0) && timer >= timeBetweenBullets)
            {
                // ... shoot the gun
                Shoot();
            }
#endif
            // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
            if(timer >= timeBetweenBullets * effectsDisplayTime)
            {
                // ... disable the effects.
                DisableEffects ();
            }
        }


        public void DisableEffects ()
        {
            // Disable the line renderer and the light.
            gunLine.enabled = false;
            gunLineLeft.enabled = false;
            gunLineRight.enabled = false;
			faceLight.enabled = false;
            gunLight.enabled = false;
        }


        void Shoot ()
        {
            // Reset the timer.
            timer = 0f;

            // Play the gun shot audioclip.
            gunAudio.Play ();

            // Enable the lights.
            gunLight.enabled = true;
			faceLight.enabled = true;

            // Stop the particles from playing if they were, then start the particles.
            gunParticles.Stop ();
            gunParticles.Play ();

            // Enable the line renderer and set it's first position to be the end of the gun.
            gunLine.enabled = true;
            gunLine.SetPosition (0, transform.position);

            // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
            shootRay.origin = transform.position;
            shootRay.direction = transform.forward;

            // Perform the raycast against gameobjects on the shootable layer and if it hits something...
            if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
                // If the EnemyHealth component exist...
                if (enemyHealth != null)
                {
                    // ... the enemy should take damage.
                    enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                }

                // Set the second position of the line renderer to the point the raycast hit.
                gunLine.SetPosition(1, shootHit.point);
            }
            // If the raycast didn't hit anything on the shootable layer...
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
            }
        }

        void Strongshoot()
        {
            // Reset the timer.
            timer = 0f;

            // Play the gun shot audioclip.
            gunAudio.Play();

            // Enable the lights.
            gunLight.enabled = true;
            faceLight.enabled = true;

            // Stop the particles from playing if they were, then start the particles.
            gunParticles.Stop();
            gunParticles.Play();

            // Enable the line renderer and set it's first position to be the end of the gun.
            gunLine.enabled = true;
            gunLineLeft.enabled = true;
            gunLineRight.enabled = true;
            gunLine.SetPosition(0, transform.position);
            gunLineLeft.SetPosition(0, GunBarrelEndleft.position);
            gunLineRight.SetPosition(0, GunBarrelEndright.position);

            // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
            shootRay.origin = transform.position;
            shootRayLeft.origin = GunBarrelEndleft.position;
            shootRay.direction = transform.forward;
            shootRayLeft.direction = GunBarrelEndleft.forward;
            shootRayRight.origin = GunBarrelEndright.position;
            shootRayRight.direction = GunBarrelEndright.forward;
            // Perform the raycast against gameobjects on the shootable layer and if it hits something...
            if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
                // If the EnemyHealth component exist...
                if (enemyHealth != null)
                {
                    // ... the enemy should take damage.
                    enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                }

                // Set the second position of the line renderer to the point the raycast hit.
                gunLine.SetPosition(1, shootHit.point);
            } 
            // If the raycast didn't hit anything on the shootable layer...
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
            }
            if (Physics.Raycast(shootRayLeft, out shootHitleft, range, shootableMask))
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                EnemyHealth enemyHealth = shootHitleft.collider.GetComponent<EnemyHealth>();
                // If the EnemyHealth component exist...
                if (enemyHealth != null)
                {
                    // ... the enemy should take damage.
                    enemyHealth.TakeDamage(damagePerShot, shootHitleft.point);
                }

                // Set the second position of the line renderer to the point the raycast hit.
                gunLineLeft.SetPosition(1, shootHitleft.point);
            }
            // If the raycast didn't hit anything on the shootable layer...
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                gunLineLeft.SetPosition(1, shootRayLeft.origin + shootRayLeft.direction * range);
            }
            if (Physics.Raycast(shootRayRight, out shootHitright, range, shootableMask))
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                EnemyHealth enemyHealth = shootHitleft.collider.GetComponent<EnemyHealth>();
                // If the EnemyHealth component exist...
                if (enemyHealth != null)
                {
                    // ... the enemy should take damage.
                    enemyHealth.TakeDamage(damagePerShot, shootHitright.point);
                }

                // Set the second position of the line renderer to the point the raycast hit.
                gunLineRight.SetPosition(1, shootHitright.point);
            }
            // If the raycast didn't hit anything on the shootable layer...
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                gunLineRight.SetPosition(1, shootRayRight.origin + shootRayRight.direction * range);
            }
        }
    }
}