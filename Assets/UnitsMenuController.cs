using NTC.MonoCache;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitsMenuController : MonoCache
{
   public TextMeshProUGUI health, type, global;
   public SelectionManager selector;
   // Start is called before the first frame update
   void Start()
   {

   }

   // Update is called once per frame
   protected override void Run()
   {
      SetUnitsMenu(selector.SelectedUnits);
   }

   public static string GetSoldiersType(GameObject soldier)
   {
      if (soldier.name.Contains("Based"))
         return "Infantryman";
      else if (soldier.name.Contains("Shturm"))
         return "Stormtrooper";
      else if (soldier.name.Contains("Defend"))
         return "Defender";
      else if (soldier.name.Contains("gunner"))
         return "Machingunner";
      else if (soldier.name.Contains("Artillery"))
         return "Artillery";
      else if (soldier.name.Contains("Renault"))
         return "Light tank";
      else if (soldier.name.Contains("Mark"))
         return "Tank";
      else return "";
   }

   void SetUnitsMenu(List<SelectableUnit> SelectedUnits)
   {
      if (SelectedUnits.Count <= 0)
      {
         health.text = "";
         type.text = "";
         global.fontSize = 28;
         global.text = "No one units selected";
      }
      else if (SelectedUnits.Count == 1)
      {
         health.text = "Health:" +  Math.Round(SelectedUnits[0].gameObject.GetComponent<Health>().health / SelectedUnits[0].gameObject.GetComponent<Health>().healthBar.slider.maxValue * 100) + "%";
         type.text = "Type:" + GetSoldiersType(SelectedUnits[0].gameObject);
         global.text = SelectedUnits[0].unitName;

      } else if (SelectedUnits.Count > 1)
      {
         health.text = "";
         type.text = "";
         string squad = "In selected squad: ";
         int i = 0;
         int machinegunners = 0; 
         int stormtroopers = 0;
         int defenders = 0;
         int artillerys = 0;
         int infantrymans = 0;
         int light_tanks = 0;
         int tanks = 0;
         foreach (SelectableUnit unit in SelectedUnits)
         {
            switch (GetSoldiersType(unit.gameObject)) {
               case "Infantryman":
                  infantrymans++;
                  break;
               case "Stormtrooper":
                  stormtroopers++;
                  break;
               case "Defender":
                  defenders++;
                  break;
               case "Machingunner":
                  machinegunners++;
                  break;
               case "Artillery":
                  artillerys++;
                  break;
               case "Light tank":
                  light_tanks++;
                  break;   
               case "Tank":
                  tanks++;
                  break;
               default:
                  break;
            }
         }
         if (infantrymans > 0)
            squad += "\n Infantrymans: " + infantrymans + " ";
         if (stormtroopers > 0)
            squad += "\n Stormtroopers: " + stormtroopers + " ";
         if (defenders > 0)
            squad += "\n Defenders: " + defenders + " ";
         if (machinegunners > 0)
            squad += "\n Machinegunners: " + machinegunners;
         if (artillerys > 0)
            squad += "\n Artillerys cannons: " + artillerys;
         if (tanks > 0 || light_tanks > 0)
            squad += "\n Tanks:" + (light_tanks + tanks).ToString();
         global.fontSize = 20;
         global.text = squad;
      }

      
   }
}