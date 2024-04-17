using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
   public int sceneId;
   public float elapsedTime;
   public float timeoutTime = 5f; // Time to wait before switching scenes (in seconds)
   public UnityEngine.UI.Slider progressBar;
   public AudioSource audio;
   public TextMeshProUGUI text;

   private AsyncOperation sceneLoadOperation; // Reference to the loading operation

   private void Start()
   {
      audio = GameObject.Find("Audio").GetComponent<AudioSource>();
      timeoutTime = audio.clip.length;
      int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
      sceneId = currentSceneIndex + 1;
      PlayerPrefs.SetInt("Level", sceneId);
      StartCoroutine(LoadSceneAsync(sceneId));
   }

   IEnumerator LoadSceneAsync(int sceneId)
   {
      sceneLoadOperation = SceneManager.LoadSceneAsync(sceneId);
      sceneLoadOperation.allowSceneActivation = false; // Prevent immediate scene activation

      // Optionally activate a loading screen UI element here

      float startTime = Time.time;
      elapsedTime = 0f;

      while (!sceneLoadOperation.isDone)
      {
         elapsedTime = Time.time - startTime;

         // Update progress bar if available
         if (progressBar != null)
         {
            float progressValue = Mathf.Clamp01(sceneLoadOperation.progress / 0.9f) * 100;
            if (progressValue > 50)
            {
               progressBar.maxValue = timeoutTime;
               progressBar.value = audio.time;
            }
            else
            {
               progressBar.value = progressValue;
            }
         }

         // Check if timeout has been reached
         text.text = "Press spacebar to continue...";
         if (Input.GetKeyUp(KeyCode.Space))
         {
            sceneLoadOperation.allowSceneActivation = true; // Allow scene activation now

            break;
         }

         yield return null;
      }

      // Scene is fully loaded at this point
      PlayerPrefs.SetInt("Level", sceneId);
      PlayerPrefs.Save();
   }
}
