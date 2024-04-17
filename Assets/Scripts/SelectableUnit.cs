using NTC.MonoCache;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SelectableUnit : MonoCache
{
   public NavMeshAgent Agent;
   [SerializeField]
   private SpriteRenderer SelectionSprite;
   [SerializeField]
   public SelectionManager selector;
   Ray ray;
   float ArtilleryReloadTime = 3.5f;
   float timeSinceLastShot = 0f;
   string[] names = {
    "Liam",
    "Noah",
    "Oliver",
    "William",
    "Elijah",
    "James",
    "Benjamin",
    "Lucas",
    "Henry",
    "Alexander",
    "Mason",
    "Michael",
    "Ethan",
    "Daniel",
    "Jacob",
    "Aiden",
    "Jackson",
    "Logan",
    "David",
    "Sebastian",
    "Bill",
    "Matthew",
    "Evan",
    "Caleb",
    "Ryan"
};

   string[] surnames = {
    "Smith",
    "Johnson",
    "Williams",
    "Brown",
    "Jones",
    "Garcia",
    "Miller",
    "Davis",
    "Martin",
    "Anderson",
    "Taylor",
    "Thomas",
    "Moore",
    "Jackson",
    "White",
    "Harris",
    "Lewis",
    "Robinson",
    "Walker",
    "Young",
    "Allen",
    "King",
    "Wright",
    "Scott",
    "Green"
};


   RaycastHit hit;
   public Animator animator;
   public Vector3 target;
   public GameObject ShellPrefab;
   public float speed = 2.0f; // Скорость перемещения
   public float rotationSpeed = 5.0f; // Скорость поворота
   [SerializeField] MessageController messanger;
   public string unitName;
   PlayerInput inputHandler;
   public MeshRenderer[] tracks;
   public ParticleSystem tankParticle;

   private void Awake()
   {
      messanger = GameObject.Find("Message").GetComponent<MessageController>();
      selector = GameObject.Find("Main Camera").GetComponent<SelectionManager>();
      selector.AvailableUnits.Add(this);
      Agent = GetComponent<NavMeshAgent>();
      if (GetComponent<Animator>() != null)
         animator = GetComponent<Animator>();
      if (unitName == "")
         unitName = names[Random.Range(0, names.Length)] + " " + surnames[Random.Range(0, surnames.Length)];
      if (tag == "Artillery")
      {
         inputHandler = selector.gameObject.GetComponent<PlayerInput>();
         unitName = "Artillery";
      }
   }

   public static bool isObjectIsInLayer(Vector3 pos, LayerMask layer)
   {
      // Use Physics.OverlapSphere for wider area check (adjust radius if needed)
      Collider[] colliders = Physics.OverlapSphere(pos, 0.1f, layer);

      // Check if any colliders were found on the specified layer
      return colliders.Length > 0;
   }

   public void ArtilleryShot()
   {

      // Проверка пересечения луча с объектом


      float xrandom = UnityEngine.Random.Range(-7, 8f);
      float zrandom = UnityEngine.Random.Range(-8, 8);
      float yrandom = UnityEngine.Random.Range(0f, 25f);
      Vector3 spawnPosition = new Vector3(hit.point.x + xrandom, hit.point.y + 100f, hit.point.z + zrandom);
      // Спавн объекта ShellPrefab точке пересечения луча с объектом
      //UnityEngine.Debug.Log("One shell drop down");
      GameObject Shell = Instantiate(ShellPrefab, spawnPosition, Quaternion.identity);
      Shell.GetComponent<ArtilleryShellController>().damage = 30;
      Shell.GetComponent<ArtilleryShellController>().maxDistance = 40f;
      return;

      //UnityEngine.Debug.Log("One shell drop down");
   }

   void GunParticles()
   {
      GetComponentInChildren<ParticleSystem>().Play();
      Invoke("ArtilleryShot", 2.8f);
   }

   IEnumerator GunRotate()
   {
      int i = 0;
      while (i < 1000)
      {
         yield return new WaitForSeconds(0.001f);
         SoldierControllerPlayer.SlowLookAt(hit.point, transform, 5f, 10f);
         i++;
      }
   }

   public void MoveTo(Vector3 Position)
   {

      if (gameObject.tag == "Artillery")
      {
         if (inputHandler.timeSinceLastClick > ArtilleryReloadTime)
         {
            Transform[] soldersNearMG = SoldierControllerPlayer.GetSoldiers(gameObject.transform, LayerMask.GetMask("PlayerUnits"), 10f);
            if (soldersNearMG.Length > 2)
               foreach (Transform soldier in soldersNearMG)
               {
                  if (soldier.GetComponent<Health>() != null)
                  {
                     if (soldier.GetComponent<Health>().health > 5 && !soldier.name.Contains("Heavy"))
                     {
                        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit))
                        {
                           StartCoroutine(GunRotate());
                           GetComponent<Animation>().Play("GunShut");
                           GetComponent<AudioSource>().Play();
                           Invoke("GunParticles", 1.2f);
                           break;

                        }
                     }
                  }
               }
         }
         else
            messanger.Error("The artillery isn't ready to strike or the soldiers aren't around.");
         return;
      }
      if (selector.SelectedUnits.Count < 2)
      {
         Agent.SetDestination(Position);

      }
      else
      {
         Position.x += Random.Range(-10f, 10f);
         if (!isObjectIsInLayer(Position, LayerMask.GetMask("Trench")))
            Position.z += Random.Range(-10f, 10f);
         Agent.SetDestination(Position);
      }

   }

   protected override void FixedRun()
   {
      if (Agent == null)
         return;
      bool isMove = Agent.velocity.sqrMagnitude >= 0.001f;
      float sqrMagnitude = Agent.velocity.sqrMagnitude;
      if (tankParticle != null)
      {
         if (tankParticle.isPlaying && !isMove)
         {
            tankParticle.Stop();
         }
         else if (!tankParticle.isPlaying && isMove)
         {
            tankParticle.Play();
         }
      }
      if (!isMove)
         return;

      if (gameObject.name.Contains("Tank"))
      {
         if (!tankParticle.isPlaying)
            tankParticle.Play();
         //Debug.Log("Tank rotate");
         foreach (MeshRenderer track in tracks)
         {
            track.material.mainTextureOffset += new Vector2(0f, sqrMagnitude * 0.2f * Time.fixedDeltaTime);
         }
      }
   }
   public void OnSelected()
   {
      SelectionSprite.gameObject.SetActive(true);
      //Debug.Log(gameObject.name + "Selected");
   }

   public void OnDeselected()
   {
      SelectionSprite.gameObject.SetActive(false);
   }
}
