using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TacticalScore : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI scoreText;
   [SerializeField] private List<GameObject> pointsList; // Consider using a more descriptive name if applicable
   [SerializeField] private TextMeshProUGUI flagsText;

   public int score;
   public string levelType;
   public int flags;

   private void Start()
   {
      pointsList = GameObject.Find("WinController").GetComponent<WinControllerByTime>().defeatObjects.ToList();
      StartCoroutine(AddScore());
   }

   private IEnumerator AddScore()
   {
      while (true)
      {
         flags = GetTacticalPoint();
         flagsText.text = "Flags: " + flags.ToString();
         score += GetTacticalPointBasedOnLevelType();
         scoreText.text = $"Tactical score: {score}"; // Using f-string for cleaner string formatting
         yield return new WaitForSeconds(1f); // Using 'f' suffix for clarity
      }
   }

   private int GetTacticalPointBasedOnLevelType()
   {
      return levelType switch
      {
         "Defend" => 2,
         "Attack" => 1+flags,
         _ => throw new System.ArgumentException(nameof(levelType)), // Handle unexpected level types
      };
   }

   private int GetTacticalPoint()
   {
      int dots = 0;
      foreach (GameObject point in pointsList)
      {
         if (point.GetComponent<DefendPoint>().isControlledByPlayer())
         {
            dots++;
         }
      }
      return dots;
   }
}
