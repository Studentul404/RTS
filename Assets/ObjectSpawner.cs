using NTC.MonoCache;
using UnityEngine;

public class ObjectSpawner : MonoCache
{
   public GameObject objectToSpawn;
   public TacticalScore score;
   public string spawnType;
   public GameObject[] flags;
   public int price;
   public MessageController messager;

   private void Start()
   {
      flags = GameObject.Find("WinController").GetComponent<WinControllerByTime>().defeatObjects;
   }


   public float GetFurtherFlagZ()
   {
      Debug.Log("Far flag !");
      float z = -500;
      foreach (GameObject flag in flags)
      {
         if (flag.transform.position.z > z && flag.GetComponent<DefendPoint>().isControlledByPlayer())
            z = flag.transform.position.z;
      }
      //Debug.Log("Far flag z: " + z);
      return z + 25f;
   }


   public void Spawn(GameObject objectToSpawn)
   {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Создание луча из камеры в точку курсора
      RaycastHit hit; // Переменная для хранения информации о столкновении

      if (Physics.Raycast(ray, out hit)) // Проверка пересечения луча с объектом
      {
         // Спавн объекта в точке пересечения луча с объектом
         Instantiate(objectToSpawn, hit.point, Quaternion.identity);
      }
      else
      {
         messager.Error("Not enough place for object!");
      }
   }

   public void ArtilleryStrikeSpawn(int shells)
   {
      for (int i = 0; i < shells; i++)
      {
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Создание луча из камеры в точку курсора
         RaycastHit hit; // Переменная для хранения информации о столкновении

         if (Physics.Raycast(ray, out hit)) // Проверка пересечения луча с объектом
         {
            float xrandom = UnityEngine.Random.Range(-25f, 25f);
            float zrandom = UnityEngine.Random.Range(-25f, 25f);
            float yrandom = UnityEngine.Random.Range(0f, 25f);
            Vector3 spawnPosition = new Vector3(hit.point.x + xrandom, hit.point.y + 100f, hit.point.z + zrandom);
            // Спавн объекта в точке пересечения луча с объектом
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
         }
         else
         {
            messager.Error("Error in artilerry strike.");
            score.score += price;
         }
      }
   }

   public void SquadSpawner(GameObject objectToSpawn)
   {

      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Создание луча из камеры в точку курсора
      RaycastHit hit; // Переменная для хранения информации о столкновении

      if (Physics.Raycast(ray, out hit)) // Проверка пересечения луча с объектом
      {
         Vector3 spawnPosition = new Vector3(hit.point.x, hit.point.y + 10f, hit.point.z);
         // Спавн объекта в точке пересечения луча с объектом
         if (spawnPosition.z < GetFurtherFlagZ())
            Instantiate(objectToSpawn, hit.point, Quaternion.identity);
         else
         {
            messager.Error("This zone not control by player! Place drop obejct behind your flags.");
            score.score += price;
         }
      }
   }

   public void MachineGunSpawn(GameObject objectToSpawn)
   {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Создание луча из камеры в точку курсора
      RaycastHit hit; // Переменная для хранения информации о столкновении
      if (Physics.Raycast(ray, out hit)) // Проверка пересечения луча с объектом
      {
         Vector3 spawnPosition = new Vector3(hit.point.x, hit.point.y + 10f, hit.point.z);
         // Спавн объекта в точке пересечения луча с объектом
         if (spawnPosition.z < GetFurtherFlagZ())
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
         else
         {
            messager.Error("This zone not control by player! Place drop obejct behind your flags.");
            score.score += price;

         }
      }
      else
      {
         messager.Error("Not enough place for object!");
         score.score += price;
      }
   }


   protected override void Run()
   {
      if (Input.GetMouseButtonUp(1)) // Проверка нажатия правой кнопки мыши
      {
         switch (spawnType)
         {
            case "Spawn":
               Spawn(objectToSpawn);
               break;
            case "ArtilleryStrike":
               ArtilleryStrikeSpawn(5);
               gameObject.GetComponent<ObjectSpawner>().enabled = false;
               break;
            case "Soldiers":
               SquadSpawner(objectToSpawn);
               gameObject.GetComponent<ObjectSpawner>().enabled = false;
               break;
            case "MG":
               MachineGunSpawn(objectToSpawn);
               gameObject.GetComponent<ObjectSpawner>().enabled = false;
               break;

         }
      }
   }
}
