using System.Collections;
using UnityEngine;

public class AutoSpawnByTime : MonoBehaviour
{
   public GameObject objectToSpawn;
   public float spawnTime;
   // Start is called before the first frame update
   void Start()
   {
      StartCoroutine(Spawn());
   }

   // Update is called once per frame
   void Update()
   {

   }
   IEnumerator Spawn()
   {
      while (true)
      {
         GameObject Bullet = Instantiate(objectToSpawn, transform.position, transform.rotation);
         yield return new WaitForSeconds(spawnTime);
      }
   }
}
