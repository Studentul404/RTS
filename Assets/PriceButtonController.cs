using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PriceButtonController : MonoBehaviour
{
   TextMeshProUGUI price;
   public int id;
   // Start is called before the first frame update
   void Start()
    {
      price = GetComponent<TextMeshProUGUI>();
      price.text = Convert.ToString(SpawnSoldierMenu.prices[id]) + "$";
   }

    // Update is called once per frame
    void Update()
    {
        
    }


}
