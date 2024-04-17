using NTC.MonoCache;
using System.Collections.Generic;
using UnityEngine;

public class DefendPoint : MonoCache
{
   public float defendPointRadius = 25;
   public SpriteRenderer flag;
   public SpriteRenderer miniMapFlag;
   public Sprite enemy;
   public Sprite user;
   public Sprite empty;
   // Start is called before the first frame update
   void Start()
   {

   }

   // Update is called once per frame
   protected override void LateRun()
   {
      if (isControlledByPlayer())
      {
         flag.sprite = user;
         
      }
      else if (isControlledByEnemy())
      {
         flag.sprite = enemy;
      }
      else
      {
         flag.sprite = empty;
      }
      miniMapFlag.sprite = flag.sprite;
   }



   public List<GameObject> GetObjectsInRadius(float radius)
   {

      // Получение всех объектов в сцене
      List<GameObject> allObjects = new List<GameObject>(FindObjectsOfType<GameObject>());

      // Список объектов в радиусе
      List<GameObject> objectsInRadius = new List<GameObject>();

      // Позиция точки защиты
      Vector3 defendPointPosition = transform.position;

      // Перебор всех объектов
      foreach (GameObject obj in allObjects)
      {
         // Расстояние между объектом и точкой защиты
         float distance = Vector3.Distance(obj.transform.position, defendPointPosition);

         // Добавление объекта в список, если он находится в радиусе
         if (distance <= radius)
         {
            objectsInRadius.Add(obj);
         }
      }

      return objectsInRadius;
   }


   public bool isControlledByPlayer()
   {
      List<GameObject> objects = GetObjectsInRadius(defendPointRadius);

      // Подсчет объектов с тегами UnitPlayer и enemy
      int unitPlayerCount = 0;
      int enemyCount = 0;
      foreach (GameObject obj in objects)
      {
         if (obj.CompareTag("UnitPlayer"))
         {
            unitPlayerCount++;
         }
         else if (obj.CompareTag("Enemy"))
         {
            enemyCount++;
         }
      }

      // Сравнение количества объектов
      int[] soldiers = GetSoldiers();
      // Сравнение количества объектов
      return soldiers[1] < soldiers[0];
   }

   public bool isControlledByEnemy()
   {

      int[] soldiers = GetSoldiers();
      // Сравнение количества объектов
      return soldiers[1] > soldiers[0];
   }

   public int[] GetSoldiers()
   {
      List<GameObject> objects = GetObjectsInRadius(defendPointRadius);

      // Подсчет объектов с тегами UnitPlayer и enemy
      int unitPlayerCount = 0;
      int enemyCount = 0;
      foreach (GameObject obj in objects)
      {
         if (obj.CompareTag("UnitPlayer"))
         {
            unitPlayerCount++;
         }
         else if (obj.CompareTag("Enemy"))
         {
            enemyCount++;
         }
      }
      int[] soldiers = { unitPlayerCount, enemyCount };
      return soldiers;
   }
}
