using NTC.MonoCache;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAtackUnit : MonoCache
{
   public float stopTime = 2.0f;
   public float minSpeed = 1.0f;
   public float activationRadius = 25f;
   public float maxSpeed = 5.0f;

   private NavMeshAgent agent;
   public Animator animator;
   public List<Transform> pointsList = new List<Transform>();
   private int currentPointIndex = 0;
   private bool isPaused = false;

   void Start()
   {
      agent = GetComponent<NavMeshAgent>();
      animator = GetComponent<Animator>();

      // Find all points and add them to the list
      //GameObject[] pointObjects = GameObject.FindGameObjectsWithTag("Point");
      //foreach (GameObject pointObject in pointObjects)
      //{
      //   pointsList.Add(pointObject.transform);
      //}
      Debug.Log(pointsList.Count);

      // Choose initial speed
      agent.speed = Random.Range(minSpeed, maxSpeed);

      // Start moving towards the first point
      GoToNextPoint();
   }

   protected override void LateRun()
   {
      if (gameObject.GetComponent<Health>().health < 35 && GetComponent<SoldierControllerPlayer>().enemies.Length > 1)
      {

         agent.speed = agent.speed * 0.6f;
      }
      if (animator != null)
      {
         if (agent.velocity.sqrMagnitude > 0.001f)
         {
            if (animator != null)
               if (!animator.GetBool("isRun"))
                  animator.SetBool("isRun", true);
         }
         else
         {
            animator.SetBool("isRun", false);
         }
      }

      if (pointsList.Count > 0 && currentPointIndex < pointsList.Count)
      {
         if (Vector3.Distance(transform.position, pointsList[currentPointIndex].position) <= activationRadius)
         {
            // Remove current point
            pointsList.RemoveAt(currentPointIndex);

            // Go to the next point if it exists
            if (currentPointIndex < pointsList.Count)
            {
               Debug.Log("Next point");
               GoToNextPoint();
            }
         }
      }
   }

   IEnumerator PauseAndGoToNextPoint()
   {
      isPaused = true;
      agent.isStopped = true;

      // Pause
      yield return new WaitForSeconds(stopTime);

      // Choose new speed
      agent.speed = Random.Range(minSpeed, maxSpeed);

      // Go to the next point
      currentPointIndex = (currentPointIndex + 1) % pointsList.Count;
      GoToNextPoint();

      isPaused = false;
   }

   void GoToNextPoint()
   {
      if (pointsList.Count > 0 && currentPointIndex < pointsList.Count)
      {
         //it was edited
         //replace transform.position with pointsList[currentPointIndex].position
         /* if (Camera.main.GetComponent<SelectionManager>().levelType == "Defend")
         {
            Vector3 nextPoint = new Vector3(transform.position.x + Random.RandomRange(-activationRadius, activationRadius), pointsList[currentPointIndex].position.y, pointsList[currentPointIndex].position.z + Random.RandomRange(-activationRadius, activationRadius));
            agent.SetDestination(nextPoint);
         } else if (Camera.main.GetComponent<SelectionManager>().levelType == "Attack")
         { */
         Vector3 nextPoint = new Vector3(pointsList[currentPointIndex].position.x + Random.RandomRange(-activationRadius, activationRadius), pointsList[currentPointIndex].position.y, pointsList[currentPointIndex].position.z + Random.RandomRange(-activationRadius, activationRadius));
         agent.SetDestination(nextPoint);

      }
   }
}
