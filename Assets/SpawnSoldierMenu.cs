using UnityEngine;

public class SpawnSoldierMenu : MonoBehaviour
{
   public GameObject[] Squads;
   public GameObject circleMenu;
   public MessageController messager;
   public static int[] prices = { 55, 60, 55, 75, 60, 60, 135, 125, 80, 80, 80, 80, 80, 80};
   public ObjectSpawner spawner;
   public GameObject[] buttons = { null, null, null, null, null, null, null, null, null, null, null};
   public TacticalScore tacticalScore;
   public void OnButtonClick(int id)
   {
      bool isAvailable = false;
      if (tacticalScore.score > prices[id])
      {
         tacticalScore.score -= prices[id];
         messager.Warning(prices[id] + " points withdrawn to buy " + Squads[id].name + ". Click right mouse button to place it.");
         isAvailable = true;
      } else
      {
         messager.Error("Not enough points to buy " + Squads[id].name);
      }
      if (isAvailable)
      {
         GameObject button = buttons[id];
         if (button != null) 
            button.SetActive(false);

         gameObject.gameObject.SetActive(false);
         spawner.enabled = true;
         spawner.objectToSpawn = Squads[id];
         spawner.spawnType = (false) ? "MG" : "Soldiers";
         spawner.price = prices[id];
         Camera.main.gameObject.GetComponent<SelectionManager>().ClearSelectedUnits();
         gameObject.SetActive(false);
         circleMenu.SetActive(true);
      }
   }
}

