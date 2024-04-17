using TMPro;
using UnityEngine;

public class WinControllerByFlags : MonoBehaviour
{
   public DefendPoint[] flags;
   public GameObject victoryObject;
   public GameObject defeatObject;
   public int playerDeads = 0;
   public GameObject looseRatiosVictory;
   public GameObject looseRatiosDefeat;
   public SelectionManager selector;
   public int enemyDeads = 0;
   private bool isDefeated = false;
   public TopMenuContoller menuContoller;


   public static string GetReducedFraction(int numerator, int denominator)
   {
      int gcd = GetGCD(numerator, denominator);

      numerator /= gcd;
      denominator /= gcd;

      return $"{numerator} to {denominator}";
   }

   private static int GetGCD(int a, int b)
   {
      while (b != 0)
      {
         int temp = a % b;
         a = b;
         b = temp;
      }
      return a;
   }

   // Start is called before the first frame update
   void Start()
   {
         
   }

   void RemoveSoldierAndGuns()
   {
      foreach (GameObject obj in GameObject.FindGameObjectsWithTag("UnitPlayer"))
         obj.SetActive(false);
      foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
         obj.SetActive(false);
      foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Machinegun"))
         obj.SetActive(false);
   }

   private void DisplayVictoryMessage()
   {
      // Отображение сообщения о победе
      menuContoller.HideMenu();
      Camera.main.GetComponent<PlayerInput>().enabled = false;
      menuContoller.backgroundBlur.SetActive(true);
      victoryObject.SetActive(true);
      RemoveSoldierAndGuns();
      looseRatiosVictory.GetComponent<TextMeshProUGUI>().text = "Antanta/Germany loss ratios: " + GetReducedFraction(playerDeads, enemyDeads);
   }

   private void DisplayDefeatMessage()
   {
      menuContoller.HideMenu();
      menuContoller.backgroundBlur.SetActive(true);
      Camera.main.GetComponent<PlayerInput>().enabled = false;
      // Отображение сообщения о поражении
      defeatObject.SetActive(true);
      RemoveSoldierAndGuns();
      looseRatiosDefeat.GetComponent<TextMeshProUGUI>().text = "Antanta/Germany loss ratios: " + GetReducedFraction(playerDeads, enemyDeads);
   }

   // Update is called once per frame
   void Update()
   {
      int player_flags = 0;
      foreach (DefendPoint flag in flags)
         if (flag.isControlledByPlayer())
            player_flags++;
      if (player_flags == flags.Length)
         DisplayVictoryMessage();
      else if (selector.AvailableUnits.Count == 0)
         DisplayDefeatMessage();

   }
}
