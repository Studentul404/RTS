using UnityEngine;

public class BulletController : MonoBehaviour
{
   public float speed = 10f;
   public float damage = 10;
   public GameObject father;
   private Rigidbody rb;
   GameObject target;

   private void Start()
   {
      rb = GetComponent<Rigidbody>();
      rb.velocity = transform.forward * speed;
      Destroy(gameObject, 3f);
   }

   private void OnTriggerEnter(Collider other)
   {
      target = other.gameObject;
      Health health = other.GetComponent<Health>();
      if (health != null)
      {
         float coeficient = 2;
         if (target.name.Contains("Tank") || target.name.Contains("Heavy") && !(father.name.Contains("Tank") || father.name.Contains("Heavy")))
            coeficient = 0.5f;
         health.TakeDamage(damage * coeficient);
      }
      Destroy(gameObject);
   }
}
