using NTC.MonoCache;
using UnityEngine;
using UnityEngine.Rendering;

public class TopMenuContoller : MonoCache
{
   public GameObject wheelMenu;
   public GameObject localMenu;
   public GameObject playerSCore;
   public GameObject backgroundBlur;
   public GameObject unitMenu;
   public GameObject[] menus;
   public GameObject[] allMenus;
   public GameObject selectionCanvas;
   // Start is called before the first frame update
   void Start()
   {

   }

   public void HideMenu()
   {
      wheelMenu.SetActive(false);
      foreach (GameObject menu in allMenus)
      {
         menu.SetActive(false);
      }
   }

   public void Escape()
   {
      HideMenu();
      selectionCanvas.SetActive(!selectionCanvas.activeInHierarchy);
      backgroundBlur.SetActive(!backgroundBlur.activeInHierarchy);
      localMenu.SetActive(!localMenu.activeInHierarchy);
      if (!localMenu.activeInHierarchy)
      {
         wheelMenu.SetActive(true);
         playerSCore.SetActive(true);
         unitMenu.SetActive(true);
      }
   }


   // Update is called once per frame
   protected override void Run()
   {
      bool isMenuActive = false;
      foreach (GameObject menu in menus)
      {
         if (menu.activeSelf)
            isMenuActive = true;
      }
      /*
      if (Input.GetKeyUp(KeyCode.Space))
      {
         wheelMenu.SetActive(!wheelMenu.activeInHierarchy && !isMenuActive);
      }*/

      //selectionCanvas.SetActive(!wheelMenu.activeSelf && !isMenuActive);

      if (Input.GetKeyUp(KeyCode.Escape))
      {
         Escape();
      }
   }
}
