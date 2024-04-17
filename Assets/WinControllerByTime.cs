using System.Collections;
using TMPro;
using UnityEngine;

public class WinControllerByTime : MonoBehaviour
{
   public float winTime = 60f;
   public GameObject[] defeatObjects;
   [SerializeField] TextMeshProUGUI timerText;
   public GameObject victoryObject;
   public GameObject defeatObject;
   public int playerDeads = 0;
   public GameObject looseRatiosVictory;
   public GameObject looseRatiosDefeat;
   public int enemyDeads = 0;
   private bool isDefeated = false;
   int particleIndex = 0;
   public ParticleSystem[] particleSystems;
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


   private void Start()
   {
      winTime = 180;
      // Запуск таймера для победы
      Invoke("CheckForVictory", winTime);
      StartCoroutine(DisplayTime());
   }

   private void Update()
   {
      // Проверка поражения
      if (!isDefeated)
      {
         foreach (GameObject obj in defeatObjects)
         {
            if (obj.GetComponent<DefendPoint>().isControlledByPlayer())
            {
               // Поражение
               isDefeated = false;
               break;
            }
            isDefeated = true;
         }
      }
      else
      {
         DisplayDefeatMessage();
      }
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

   private void CheckForVictory()
   {
      // Проверка, не проиграл ли игрок до этого
      if (!isDefeated)
      {
         DisplayVictoryMessage();
      }
   }

   private void DisplayVictoryMessage()
   {
      Debug.Log("Vecitory!");
      // Отображение сообщения о победе
      menuContoller.HideMenu();
      menuContoller.backgroundBlur.SetActive(true);
      victoryObject.SetActive(true);
      RemoveSoldierAndGuns();
      looseRatiosVictory.GetComponent<TextMeshProUGUI>().text = "Antanta/Germany loss ratios: " + GetReducedFraction(playerDeads, enemyDeads);
   }

   private void DisplayDefeatMessage()
   {
      menuContoller.HideMenu();
      menuContoller.backgroundBlur.SetActive(true);
      // Отображение сообщения о поражении
      defeatObject.SetActive(true);
      RemoveSoldierAndGuns();
      looseRatiosDefeat.GetComponent<TextMeshProUGUI>().text = "Antanta/Germany loss ratios: " +  GetReducedFraction(playerDeads, enemyDeads);
   }



   IEnumerator DisplayTime()
   {
      while (winTime > 0)
      {
         
         // Вычисление минут и секунд
         int minutes = Mathf.FloorToInt(winTime / 60);
         int seconds = Mathf.FloorToInt(winTime % 60);
         if (seconds == 0)
         {
            particleSystems[particleIndex].Play();
            particleIndex++;
         }
         // Форматирование и отображение времени
         string timeText = string.Format("{0:00}:{1:00}", minutes, seconds);
         timerText.text = timeText;

         // Ожидание 1 секунды
         yield return new WaitForSeconds(1.0f);
         // Уменьшение оставшегося времени
         winTime -= 1.0f;
      }

      // Выполнение действий после истечения времени
      Debug.Log("Время истекло!");
   }
}
