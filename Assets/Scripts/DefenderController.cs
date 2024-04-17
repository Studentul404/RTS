using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefenderController : MonoBehaviour
{
   public GameObject pointPrefab;
   public int countPoints = 10;
   public float spawnRadius = 10f;
   public Animator animator;
   public NavMeshAgent agent;

   private void Start()
   {
      agent = GetComponent<NavMeshAgent>();
      animator = GetComponent<Animator>();
      // Спавним точки вокруг игрока
      SpawnPoints();

      // Запускаем процесс посещения точек
      StartCoroutine(VisitPoints());
   }

   private void SpawnPoints()
   {
      for (int i = 0; i < countPoints; i++)
      {
         Vector3 randomPos = Random.insideUnitSphere * spawnRadius;
         randomPos.y = 30f; // Чтобы точки не спавнились в воздухе
         Instantiate(pointPrefab, transform.position + randomPos, Quaternion.identity);
      }
   }

   private IEnumerator VisitPoints()
   {
      while (true)
      {
         GameObject[] points = GameObject.FindGameObjectsWithTag("Point");

         if (points.Length > 0)
         {
            GameObject randomPoint = points[Random.Range(0, points.Length)];
            agent.SetDestination(randomPoint.transform.position);
            if (agent.velocity.sqrMagnitude > 0f)
            {
               animator.SetBool("isRun", true);
            }
            else
            {
               animator.SetBool("isRun", false);
            }
         }

         yield return new WaitForSeconds(5f); // Интервал между посещениями точек
      }
   }

}
