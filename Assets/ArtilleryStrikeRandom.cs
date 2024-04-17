using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryStrikeRandom : MonoBehaviour
{
   // Публичная переменная для объекта спавна
   public GameObject objectToSpawn;

   // Диапазон спавна по осям x и z
   public float minX = -500f, maxX = 500f;
   public float minZ = -500f, maxZ = 500f;

   // Диапазон спавна по оси y
   public float minY = 60f, maxY = 99f;

   // Задержка перед первым спавном
   public float initialDelay = 1f;

   // Интервал между спавнами
   [SerializeField]
   public float spawnInterval = 2f;

   private void Start()
   {
      // Запуск спавна с задержкой
      InvokeRepeating("SpawnObject", initialDelay, spawnInterval);
   }

   private void SpawnObject()
   {
      // Генерация случайных координат
      float x = Random.Range(minX, maxX);
      float y = Random.Range(minY, maxY);
      float z = Random.Range(minZ, maxZ);

      // Спавн объекта в случайной позиции
      Instantiate(objectToSpawn, new Vector3(x, y, z), Quaternion.identity);
   }
}
