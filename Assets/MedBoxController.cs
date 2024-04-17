using UnityEngine;

public class MedBoxController : MonoBehaviour
{

   // Start is called before the first frame update
   void Start()
   {
      Invoke("GiveSupport", 3f);
      Destroy(gameObject, 4f);
   }

   void GiveSupport()
   {
      Transform[] soldeirs = SoldierControllerPlayer.GetSoldiers(gameObject.transform, LayerMask.GetMask("PlayerUnits", "Enemy"), 10f);
      foreach (Transform soldeir in soldeirs)
      {
         Health patient = soldeir.GetComponent<Health>();
         if (patient != null)
         {
            if (patient.health < patient.healthBar.slider.maxValue)
            {
               patient.TakeDamage(patient.health - patient.healthBar.slider.maxValue);
            }
         }
      }
   }

   // Update is called once per frame
   void Update()
   {

   }
}
