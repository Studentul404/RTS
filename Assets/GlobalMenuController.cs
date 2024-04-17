using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalMenuController : MonoBehaviour
{
   public string location;
   public string typeoOfBattle;
   public Button continueButton;
   // Start is called before the first frame update
   void Start()
   {
      if (continueButton != null)
         continueButton.gameObject.SetActive(!(PlayerPrefs.GetInt("Level") == 0 || !PlayerPrefs.HasKey("Level")));
   }

   // Update is called once per frame
   void Update()
   {

   }

 public void ContinueGame()
   {
      if (PlayerPrefs.HasKey("Level"))
         MoveToScene(PlayerPrefs.GetInt("Level"));
   }

   public void Quit()
   {
      Application.Quit();
   }

   public static void MoveToScene(int sceneIndex)
   {

      SceneManager.LoadScene(sceneIndex);
      if (PlayerPrefs.HasKey("Level"))
         if (PlayerPrefs.GetInt("Level") < sceneIndex)
         {
            PlayerPrefs.SetInt("Level", sceneIndex);
            //SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
            PlayerPrefs.Save();
         }
   }

   public static void NextScene()
   {
      // Get the current scene index
      int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

      // Check if there's a next scene (avoid going out of bounds)
      if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
      {
         // Load the scene by its build index (next scene)
         SceneManager.LoadScene(currentSceneIndex + 1);
      }
      else
      {
         Debug.Log("There is no next scene in Build Settings");
      }
   }
}
