using UnityEngine;
using System.Collections;
public class Healer : MonoBehaviour
{
   [SerializeField] private float healInterval = 1f; // Adjust from Inspector for flexibility

   void Start()
   {
      StartCoroutine(Heal());
   }

   IEnumerator Heal()
   {
      while (true)
      {
         // Consider using Physics.OverlapSphere for performance optimization
         Collider[] nearbySoldiers = Physics.OverlapSphere(transform.position, 5f, LayerMask.GetMask("PlayerUnits"));
         if (nearbySoldiers.Length > 1)
         {
            foreach (Collider soldierCollider in nearbySoldiers)
            {
               Health soldierHealth = soldierCollider.GetComponent<Health>();
               if (soldierHealth != null)
               {
                  if (soldierHealth.health < soldierHealth.healthBar.slider.maxValue)
                     soldierHealth.health += 1;
               }
            }
         }

         yield return new WaitForSeconds(healInterval);
      }
   }
}
