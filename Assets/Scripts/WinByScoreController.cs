using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinByScoreController : MonoBehaviour
{
   public int scorePlayer;
   public int scoreEnemy;
   public List<GameObject> objectsWithHealthAndSoldierControlPlayer;
   public List<GameObject> PlayersObjects;
   public List<GameObject> EnemyObjects;
   public TMPro.TextMeshProUGUI PlayerScoreText, EnemyScoreText;
   public int healthPlayer, healthEnemy;
   void Start()
   {
      StartCoroutine(ExecuteEverySecond(5f));
   }

   IEnumerator ExecuteEverySecond(float time)
   {
      while (true)
      {
         yield return new WaitForSeconds(time);

      }
   }
}