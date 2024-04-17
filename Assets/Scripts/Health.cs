using NTC.MonoCache;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoCache
{
   WinControllerByTime winControllerByTime;
   WinControllerByFlags winControllerByFlags;
   public float health = 100;
   public GameObject destroyedPrefab;
   public HealthBar healthBar;
   public GameObject explosion;
   // Start is called before the first frame update
   void Start()
   {
      if (GameObject.Find("WinController") != null)
         winControllerByTime = GameObject.Find("WinController").GetComponent<WinControllerByTime>();
      if (GameObject.Find("WinControllerByFlags") != null)
         winControllerByFlags = GameObject.Find("WinControllerByFlags").GetComponent<WinControllerByFlags>();
      healthBar = GetComponentInChildren<HealthBar>();
      healthBar.SetMaxHealth(Convert.ToInt32(health));
   }
   public void TakeDamage(float damage)
   {
      health -= damage;
      healthBar.SetHealth(Convert.ToInt32(health));
   }

   // Update is called once per frame 
   protected override void Run()
   {
      if (health <= 6)
      {

         if (gameObject.GetComponent<SelectableUnit>() != null)
         {
            gameObject.GetComponent<SelectableUnit>().selector.AvailableUnits.Remove(gameObject.GetComponent<SelectableUnit>());
            gameObject.GetComponent<SelectableUnit>().selector.SelectedUnits.Remove(gameObject.GetComponent<SelectableUnit>());
            gameObject.GetComponent<SelectableUnit>().OnDeselected();
         }
         if (gameObject.GetComponent<SoldierControllerPlayer>() != null && !(gameObject.name.Contains("Tank")  || gameObject.name.Contains("Heavy") || gameObject.name.Contains("First Soldier") || gameObject.name.Contains("Savic") || gameObject.name.Contains("Carton") || gameObject.name.Contains("Francis") || gameObject.name.Contains("Henry")))
         {
            if (gameObject.tag == "Enemy")
            {
               if (winControllerByTime != null)
                  winControllerByTime.enemyDeads++;
               else if (winControllerByFlags != null)
                  winControllerByFlags.enemyDeads++;
            }
            else if (gameObject.tag == "UnitPlayer")
               if (winControllerByTime != null)
                  winControllerByTime.playerDeads++;
               else if (winControllerByFlags != null)
                  winControllerByFlags.playerDeads++;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            if (gameObject.GetComponent<NavMeshAgent>() != null)
               gameObject.GetComponent<NavMeshAgent>().enabled = false;
            gameObject.GetComponent<SoldierControllerPlayer>().enabled = false;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            gameObject.GetComponent<Animator>().SetTrigger("Dead");
            gameObject.GetComponentInChildren<Canvas>().enabled = false;
            gameObject.tag = "Untagged";
            Destroy(gameObject, 50f);
            //Debug.Log(gameObject.name + "is dead.");
         }
         else
         {
            if (destroyedPrefab != null)
            {
               Instantiate(explosion, gameObject.transform.position, Quaternion.Euler(-90, 0, 0));
               Instantiate(destroyedPrefab, gameObject.transform.position, gameObject.transform.rotation);
            }

            Destroy(gameObject);
         }
      }
   }
}
