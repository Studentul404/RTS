using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
   [SerializeField] private GameObject[] spawn_enemies;
   [SerializeField] private GameObject[] points;
   [SerializeField] private float _spawnInterval = 5f; // Интервал спавна в секундах
   [SerializeField] private bool _isSpawning = true; // Флаг, который активирует спавн

   private void Start()
   {
      StartCoroutine(SpawnEnemyRoutine());
      points = GameObject.Find("WinController").GetComponent<WinControllerByTime>().defeatObjects;
      int index = Random.Range(0, spawn_enemies.Length);
      GameObject prefabToSpawn = spawn_enemies[index];
      SpawnObject(prefabToSpawn);
   }

   private IEnumerator SpawnEnemyRoutine()
   {
      while (_isSpawning)
      {
         yield return new WaitForSeconds(_spawnInterval);
         int index = Random.Range(0, spawn_enemies.Length);
         GameObject prefabToSpawn = spawn_enemies[index];
         SpawnObject(prefabToSpawn);
      }
   }

   private void SpawnObject(GameObject prefabToSpawn)
   {
      GameObject obj = Instantiate(prefabToSpawn, transform.position, transform.rotation);
      obj.GetComponent<AIAtackUnit>().pointsList.Add(points[Random.Range(0, points.Length)].transform);
      obj.GetComponent<AIAtackUnit>().pointsList.Add(points[Random.Range(0, points.Length)].transform);
   }

   public void ToggleSpawning(bool isSpawning)
   {
      _isSpawning = isSpawning;
   }
}
