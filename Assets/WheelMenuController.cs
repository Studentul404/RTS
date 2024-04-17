using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Diagnostics;

public class WheelMenuController : MonoBehaviour
{
   string[] names = { "Wire", "Medicine help", "Soldiers", "Legends", "HeavyGuns", "Cancel" };
   public GameObject camera;
   public GameObject shellPrefab;
   public SpawnSoldierMenu soldierMenu;
   public GameObject gunsMenu;
   public GameObject machineGun;
   public GameObject medBoxPrefab;
   public TacticalScore score;
   public MessageController messager;
   public ObjectSpawner spawner;
   public GameObject legendsMenu;
   public GameObject barbedWirePrefab;
   public TextMeshProUGUI tmp_text;
   // Start is called before the first frame update
   void Start()
   {
      spawner = camera.GetComponent<ObjectSpawner>();
   }

   // Update is called once per frame
   void Update()
   {

   }



   public string GetName(int index)
   {
      string name = "";
      name = names[index-1];
      return name;
   }

   public void OnButtonClick(int index)
   {
      string action = GetName(index);
      //UnityEngine.Debug.Log("action: " + action);
      switch (action)
      {
         case "Wire":
            if (score.score < 15)
            {
               messager.Error("Not enough points!");
               return;
            }
            score.score -= 15;
            spawner.price = 15;
            spawner.enabled = true;
            spawner.objectToSpawn = barbedWirePrefab;
            spawner.spawnType = "MG";
            messager.Warning(15 + " points withdrawn to buy barbed wire. Click right mouse button to place it.");
            camera.GetComponent<SelectionManager>().ClearSelectedUnits();
            
            break;

         case "Medicine help":
            if (score.score < 30)
            {
               messager.Error("Not enough points!");
               return;
            }
            score.score -= 30;
            
            spawner.price = 30;
            spawner.enabled = true;
            spawner.objectToSpawn = medBoxPrefab;
            spawner.spawnType = "MG";
            messager.Warning(30 + " points withdrawn to buy medicine box. Click right mouse button to place it.");
            camera.GetComponent<SelectionManager>().ClearSelectedUnits();
            
            break;
         case "Soldiers":
            soldierMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
            break;
         case "Legends":
            /* if (score.score < 70)
             {
                messager.Error("Not enough points!");
                return;
             }
             score.score -= 70;
             spawner = camera.GetComponent<ObjectSpawner>();
             spawner.price = 70;
             messager.Warning(70 + " points withdrawn to buy machinegun. Click right mouse button to place it.");
             spawner.enabled = true;
             spawner.objectToSpawn = machineGun;
             spawner.spawnType = "MG";
             camera.GetComponent<SelectionManager>().ClearSelectedUnits();
             gameObject.SetActive(false);
             break; */
            legendsMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
            break;
         case "HeavyGuns":
            gunsMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
            break;
         case "Cancel":
            gameObject.SetActive(false);
            break;
         default:
            break;
      }

   }

   public void OnButtonHover(int index)
   {
      tmp_text.text = GetName(index);
   }
   public void OnButtonHoverExit()
   {
      tmp_text.text = "";
   }
}
