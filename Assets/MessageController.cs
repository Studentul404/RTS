using NTC.MonoCache;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoCache
{
   [SerializeField] GameObject errorWindow;
   [SerializeField] GameObject messageWindowPrefab;
   [SerializeField] SelectionManager selector;
   [SerializeField] Image image;
   [SerializeField] Sprite errorImage;
   [SerializeField] Sprite warningImage;
   [SerializeField] Sprite messageImage;
   [SerializeField] TextMeshProUGUI text;
   public float messageDuration = 3f;
   public float timeSinceLastMessage = 0f;
   public float tipsDelay = 8f;
   public string[] Tips = new string[]
{
   
    // Tactics:
    "Utilize a diverse army of units for a balanced approach.",
    "Combine units to maximize their strengths and create synergies.",
    "Utilize the terrain to your advantage for concealment and protection.",
    "Flank your enemies to deliver devastating blows.",
    "Maintain mobility on the battlefield and avoid staying stagnant.",
    "Utilize machine guns, tanks and artillery to suppress enemy forces.",
    "Protect your units with smoke grenades and other defensive measures.",
    "Maintain reserve forces to counter enemy counterattacks.",
    "Coordinate effectively with your allies for a cohesive strategy.",

    // Gameplay:
    "Manage your resources wisely and avoid overspending on unnecessary units.",
    "Utilize micromanagement to maximize the effectiveness of your units.",
    "Complete tutorials to familiarize yourself with the game's mechanics.",
    "Study each map's features and use them to your advantage.",
    "Adapt your tactics based on the evolving situation of the battle.",
    "Analyze your performance after each battle to identify areas for improvement.",
    "Most importantly, have fun and enjoy the experience!",

    // Additional Tips:
    "Utilize medkits to restore unit health.",
    "Repair damaged machine guns to maintain their effectiveness.",
    "Capture flags to gain strategic advantages.",
    "Utilize artillery to bombard enemy fortifications.",
    "Complete objectives to earn additional points.",
    "Don't be afraid to experiment with different tactics.",
    "Learn from your mistakes and strive for continuous improvement.",
    "Remember, the ultimate goal is to enjoy the game and have fun!",
};

   private void Start()
   {
      Tips[0] = "Use WASD to move, Q and E to rotate, R and F to zoom. Select units with the left button. Use the right button to spawn and give commands.";
      ShowMessage(Tips[0], messageImage);
   }

   protected override void Run()
   {
      timeSinceLastMessage += Time.deltaTime;
      if (timeSinceLastMessage > tipsDelay)
      {
         ShowMessage(Tips[Random.Range(0, Tips.Length - 1)], messageImage);
      }
   }

   public void ShowMessage(string message, Sprite messageImage = null)
   {
      errorWindow = Instantiate(messageWindowPrefab);
      errorWindow.SetActive(true);
      errorWindow.GetComponent<RectTransform>().SetParent(transform, false);
      text = errorWindow.GetComponentInChildren<TextMeshProUGUI>();
      text.text = message;
      image = errorWindow.transform.Find("Image").GetComponent<Image>(); // true = include inactive навсякий случай
      image.sprite = messageImage ?? (errorImage ? errorImage : warningImage); // Default to warning if no image provided
      StartCoroutine(HideWindowAfterDelay(messageDuration));
      timeSinceLastMessage = 0f;
   }

   public void Error(string message)
   {
      ShowMessage(message, errorImage);
   }

   public void Warning(string message)
   {
      ShowMessage(message, warningImage);
   }

   IEnumerator HideWindowAfterDelay(float delay)
   {
      yield return new WaitForSeconds(delay);
      errorWindow.SetActive(false);
   }
}
