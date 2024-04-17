using NTC.MonoCache;
using UnityEngine;

public class ArtilleryShellController : MonoCache
{
   public float damage = 25f;
   [SerializeField]
   public float size = 3f;
   public float maxDistance = 20f;
   public float lifeTime = 5f;
   public LayerMask soldierLayer;
   public GameObject explosionPrefab;
   public GameObject sharpnelPrefab;
   public GameObject explosionSound;
   public GameObject craterPrefab;
   public float rotationSpeed = 100f; // Speed of rotation
   public float movementSpeed = 1f; // Speed of movement
   public Vector3 initialVelocity = new Vector3(0f, -1f, 0f); // Initial velocity of the shell

   private Rigidbody rb;
   private bool exploded = false; // Flag to track if explosion has occurred

   // Start is called before the first frame update
   void Awake()
   {
      rb = GetComponent<Rigidbody>();
      lifeTime *= Random.Range(0.9f, 1.1f); // Multiply by random factor for variation
      //Destroy(gameObject, lifeTime); // Destroy after lifetime
   }

   // Update is called once per frame
   protected override void Run()
   {
      if (!exploded) // Only check for explosion if not already exploded
      {
         lifeTime -= Time.deltaTime;
         if (name.Contains("Shell") && transform.position.y < -0.2)
         {
            //Debug.Log("Boom!");

           
            Explode();
         }
         if (lifeTime <= 0) // Check if lifetime has expired
         {
            Collider[] objects = Physics.OverlapSphere(transform.position, maxDistance);
            if (objects.Length > 0) // If there are soldiers nearby
            {
               Explode();
            }
            exploded = true; // Set exploded flag to true
         }
      }

      //transform.Rotate(new Vector3(0f, rotationSpeed * Time.deltaTime, 0f));
      //transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
      if (rb != null)
         rb.AddForce(Physics.gravity, ForceMode.Acceleration);
   }

   private void OnCollisionEnter(Collision collision)
   {
      if (!exploded) // Check for collision with soldier and not already exploded
      {
         Explode();
      }
   }

   private void Explode()
   {
      Vector3 explosion_pos = new Vector3(transform.position.x, transform.position.y+4, transform.position.z);
      GameObject exp = Instantiate(explosionPrefab, explosion_pos, Quaternion.Euler(-90,0,0));
      //GameObject sharp = Instantiate(sharpnelPrefab, explosion_pos, Quaternion.identity);

      
      //sharp.transform.localScale = new Vector3(size, size, size);
      exp.transform.localScale = new Vector3(size, size, size);


      Collider[] objects = Physics.OverlapSphere(transform.position, maxDistance, soldierLayer);
      foreach (Collider obj in objects)
      {
         if (obj.GetComponent<Health>() != null)
         {
            Instantiate(explosionSound, transform.position, Quaternion.identity);
            obj.GetComponent<Health>().TakeDamage(damage);
         }
         if (obj.gameObject.GetComponent<Animator>() != null)
         {
            obj.gameObject.GetComponent<Animator>().SetTrigger("hitArt");
         }
      }


      Destroy(gameObject); // Destroy shell after explosion
   }
}
