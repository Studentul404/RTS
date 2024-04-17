using TMPro;
using UnityEngine;

public class FullScreenDetection : MonoBehaviour
{
   TextMeshProUGUI text;
   public Color Showed;
   public Color Hiden;
   // Start is called before the first frame update
   void Start()
   {
      text = GetComponent<TextMeshProUGUI>();
   }

   // Update is called once per frame
   void Update()
   {
      text.color = Screen.fullScreen ? Hiden : Showed;
   }
}
