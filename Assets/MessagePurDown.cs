using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MessagePurDown : MonoBehaviour
{
   public float speed = 0.01f;
   float time = 0.0f;
   public TextMeshProUGUI text;
   public Image image;
   // Start is called before the first frame update
   void Start()
   {
      Destroy(gameObject, 25f);
   }

   // Update is called once per frame
   void FixedUpdate()
   {
      time += Time.fixedDeltaTime;
      GetComponent<RectTransform>().localPosition = new Vector3(GetComponent<RectTransform>().localPosition.x, GetComponent<RectTransform>().localPosition.y - speed*time * Time.fixedDeltaTime);
      GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, GetComponent<Image>().color.a - 0.03f * time * Time.fixedDeltaTime);
      image.color = GetComponent<Image>().color;
      text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.03f * time * Time.fixedDeltaTime);
   }
}
