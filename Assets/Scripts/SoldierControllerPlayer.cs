using NTC.MonoCache;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SoldierControllerPlayer : MonoCache
{
   public float maxDistance = 50f;
   public float minDamage = 10f;
   public float maxDamage = 20f;
   public GameObject rifle;
   public GameObject granadeInHand;
   public GameObject granadeTossingPoint;
   public GameObject bulletCube;
   public int maxAmmoCount;
   int ammoCount;
   public float reloadTime = 3f;
   public int granadeCount = 2;
   public LayerMask enemyLayer;
   public AudioSource gunSound;
   public Transform gunPoint;
   public GameObject Granade;
   private float timeSinceLastShot = 0f;
   [SerializeField]
   private float breakTime = 2f;
   public float kickDamage = 20f;
   private NavMeshAgent agent;
   private Animator animator;
   [SerializeField]
   public Transform[] enemies;
   public float grenadeBreakTime = 4f;
   [SerializeField] private float timeSinceLastGrenade = 0f;
   public string enemyTag;
   public bool IsMachineGunner = false;
   public bool isShootingInTarget;
   float timeSinceLastCheck;
   bool isReloading = false;
   private void Awake()
   {
      if (gameObject.tag != "Machinegun")
         breakTime += UnityEngine.Random.Range(-0.3f, 0.4f);
      agent = GetComponent<NavMeshAgent>();
      animator = GetComponent<Animator>();
      if (animator != null)
      {
         if (gameObject.name.Contains("Henry"))
         {
            Debug.Log("Henry");
            animator.SetBool("isHenry", true);
         } 
         animator.SetBool("IsMachineGunner", IsMachineGunner);
      }
      SetEnemyTag();
      //Debug.Log(gameObject.name + " " + enemyTag);
   }

   public static void RemoveAt<T>(ref T[] arr, int index)
   {
      for (int a = index; a < arr.Length - 1; a++)
      {
         // moving elements downwards, to fill the gap at [index]
         arr[a] = arr[a + 1];
      }
      // finally, let's decrement Array's size by one
      Array.Resize(ref arr, arr.Length - 1);
   }

   public void SetEnemyTag()
   {
      if (gameObject.layer == 8)
      {
         enemyTag = "UnitPlayer";
      }
      else if (gameObject.layer == 6)
      {
         enemyTag = "Enemy";
      }
      else if (gameObject.layer == 0)
      {
         enemyTag = "";
      }
   }

   public static Transform[] GetSoldiers(Transform objTransform, LayerMask layer, float radius)
   {
      Transform[] enemies = Physics.OverlapSphere(objTransform.position, radius, layer).Select(x => x.transform).ToArray();

      for (int i = 0; i < enemies.Length; i++)
      {
         if (enemies[i] == null)
         {
            RemoveAt(ref enemies, i);
            continue;
         }
      }
      return enemies;
   }

   public static void SlowLookAt(Vector3 target, Transform follower, float smoothSpeed, float maxTurnSpeed)
   {
      if (target == null || follower == null)
         return;

      // Вычисление направления к цели
      Vector3 direction = target - follower.position;
      direction.y = 0f; // Фиксация по Y

      // Ограничение скорости поворота
      Quaternion targetRotation = Quaternion.LookRotation(direction);
      float turnAngle = Quaternion.Angle(follower.rotation, targetRotation);
      float turnSpeed = Mathf.Min(turnAngle * smoothSpeed, maxTurnSpeed);

      // Плавное вращение
      follower.rotation = Quaternion.Slerp(follower.rotation, targetRotation, turnSpeed * Time.deltaTime);
   }


   public void TossGranade()
   {
      if (timeSinceLastGrenade >= grenadeBreakTime && granadeCount > 0)
      {
         if (rifle != null)
            rifle.SetActive(false);
         granadeInHand.SetActive(true);
         timeSinceLastGrenade = 0f;
         granadeCount--;
         if (animator != null)
            animator.SetTrigger("Throw");
         Invoke("ThrowGranade", 0.3f);
         Invoke("DisableGranade", 1f);
      }
      // Вычисляем высоту цели
   }

   void ThrowGranade()
   {
      GameObject throwgranade = Instantiate(Granade, granadeTossingPoint.transform.position, Quaternion.identity);
      Rigidbody rb = throwgranade.GetComponent<Rigidbody>();
      rb.velocity = granadeTossingPoint.transform.forward * 60;


      rb.AddTorque(new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f)));
   }

   void DisableGranade()
   {
      granadeInHand.SetActive(false);
      rifle.SetActive(true);
      animator.SetTrigger("Shoot");
   }

   protected override void Run()
   {


      if (animator != null)
      {
         if (agent.velocity.sqrMagnitude >= 0.0001f)
         {
            animator.SetBool("isRun", true);
         }
         else
         {
            animator.SetBool("isRun", false);
         }
      }
      timeSinceLastShot += Time.deltaTime;
      timeSinceLastGrenade += Time.deltaTime;
      timeSinceLastCheck += Time.deltaTime;


      if (gameObject.tag == "Machinegun")
      {
         //Debug.Log("Machinegun: " + GetSoldiers(LayerMask.GetMask("PlayerUnits"), 5f).Length);
         Transform[] soldersNearMG = GetSoldiers(gameObject.transform, LayerMask.GetMask("PlayerUnits", "Enemy"), 10f);
         if (soldersNearMG.Length > 2)
            foreach (Transform soldier in soldersNearMG)
            {
               if (soldier.GetComponent<Health>() != null)
               {
                  if (soldier.GetComponent<Health>().health > 5 && !soldier.name.Contains("Heavy"))
                  {
                     gameObject.layer = soldier.gameObject.layer;
                     gameObject.tag = soldier.gameObject.tag;
                     SetEnemyTag();
                  }
               }
            }
         else
         {
            gameObject.layer = LayerMask.NameToLayer("Default");
            SetEnemyTag();
            return;
         }
      }
      enemies = GetSoldiers(gameObject.transform, enemyLayer, maxDistance);


      Transform closestEnemy = GetClosestEnemy(enemies);


      timeSinceLastGrenade += Time.deltaTime;
      timeSinceLastShot += Time.deltaTime;


      if (closestEnemy != null)
      {

         SlowLookAt(closestEnemy.position, gameObject.transform, 5f, 10f);
         transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);

         if (Vector3.Distance(transform.position, closestEnemy.position) <= maxDistance && timeSinceLastShot >= breakTime && ammoCount > 0)
         {
            barrelShoot(closestEnemy);
         }
      }
      else
      {
         // Остановка
      }

      if (ammoCount <= 0 && !isReloading)
      {
         if (animator != null)
            animator.SetTrigger("Reload");

         Invoke("Reload", reloadTime);
      }
   }

   void barrelShoot(Transform closestEnemy)
   {
      timeSinceLastShot = 0f;

      Vector3 targetPosition = closestEnemy.position;
      targetPosition.y += 1f;
      ammoCount--;
      Shoot(closestEnemy);
      if (animator != null)
         animator.SetTrigger("Shoot");
   }

   void Reload()
   {
      ammoCount = maxAmmoCount;
      isReloading = false;
   }

   private Transform GetClosestEnemy(Transform[] enemies)
   {
      Transform closestEnemy = null;
      float minDistance = float.MaxValue;

      foreach (Transform enemy in enemies)
      {
         float distance = Vector3.Distance(transform.position, enemy.position);
         if (enemy == null)
            continue;

         if (distance < minDistance && enemy.gameObject.tag == enemyTag && IsEnemyVisible(enemy))
         {
            minDistance = distance;
            closestEnemy = enemy;
         }
      }
      if (closestEnemy == null)
      {
         foreach (Transform enemy in enemies)
         {
            if (Vector3.Distance(transform.position, enemy.position) < maxDistance && IsEnemyVisible(enemy))
            {
               closestEnemy = enemy;
               break;
            }
         }
      }
      return closestEnemy;
   }

   private bool IsEnemyVisible(Transform enemy)
   {
      RaycastHit hit;
      Vector3 target = new Vector3(enemy.position.x, enemy.position.y + 1, enemy.position.z);
      Vector3 direction = target - transform.position;
      return Physics.Raycast(transform.position, direction, out hit, maxDistance) && hit.collider.gameObject == enemy.gameObject;
   }

   void gunShoot(Transform enemy)
   {
      gunSound.volume = 0.1f;
      gunSound.Play();
      SlowLookAt(enemy.position, gameObject.transform, 5f, 10f);
      //rifle.transform.rotation = new Quaternion(rifle.transform.rotation.x, rifle.transform.rotation.y, rifle.transform.rotation.z, rifle.transform.rotation.w);
      GameObject Bullet = Instantiate(bulletCube, gunPoint.position, gunPoint.rotation);
      Vector3 enemy_body = new Vector3(enemy.position.x, enemy.position.y + 1, enemy.position.z);
      Bullet.transform.LookAt(enemy_body);
      BulletController bullet = Bullet.GetComponent<BulletController>();
      bullet.damage = UnityEngine.Random.Range(minDamage, maxDamage);

      if (IsMachineGunner || name.Contains("MachineGun"))
      {
         Bullet.transform.localScale = new Vector3(0.1f, 0.1f, 0.99f);
         Bullet.GetComponent<BulletController>().speed += 28f;
      }
   }

   void Kick()
   {
      if (animator != null)
         animator.SetTrigger("Kick");
      Transform[] enemies = Physics.OverlapSphere(transform.position, 5f, enemyLayer).Select(x => x.transform).ToArray();
      foreach (Transform enemy in enemies)
      {
         if (enemy.GetComponent<Health>() != null)
         {
            enemy.GetComponent<Health>().TakeDamage(kickDamage);
         }
      }
   }

   private void Shoot(Transform enemy)
   {
      if (Vector3.Distance(transform.position, enemy.position) < 2f)
      {
         Kick();

      }
      else if (Vector3.Distance(transform.position, enemy.position) < 30f)
      {
         //Debug.Log("Granade: " + granadeCount);
         if (granadeCount > 0)
            TossGranade();
         //else
         gunShoot(enemy);
      }
      else
      {
         gunShoot(enemy);
      }

   }

}