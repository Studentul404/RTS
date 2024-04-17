using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
   public List<SelectableUnit> AvailableUnits = new List<SelectableUnit>();
   public List<SelectableUnit> SelectedUnits = new List<SelectableUnit>();
   public string levelType;
   public Camera pov_camera;
   public Vector3 pov;
   public RawImage image_from_camera;
   // Start is called before the first frame update
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {
      if (SelectedUnits.Count == 1)
      {
         string type = UnitsMenuController.GetSoldiersType(SelectedUnits[0].gameObject);
         if (type != "Tank" || type != "Light tank" || type != "Artillery")
         {
            Transform soldier = SelectedUnits[0].gameObject.transform;

            // Position the camera slightly behind and above the soldier's head
            Vector3 cameraOffset = new Vector3(0, 1.5f, -2.0f);
            pov_camera.gameObject.transform.position = soldier.position + cameraOffset;

            // Calculate the target position: slightly above and in front of the soldier's face
            Vector3 targetOffset = new Vector3(0, 0.5f, 0.5f); // Adjust offsets as needed
            Vector3 target = soldier.position + targetOffset;

            // Rotate the camera to look at the target while maintaining its Y rotation
            pov_camera.transform.LookAt(target, Vector3.up);

            image_from_camera.gameObject.SetActive(true);
            image_from_camera.texture = pov_camera.targetTexture;
         }
      }
      else
      {
         image_from_camera.gameObject.SetActive(false);
      }
   }



   public void SelectUnits(SelectableUnit unit)
   {
      if (!SelectedUnits.Contains(unit))
         SelectedUnits.Add(unit);
      unit.OnSelected();
   }
   public void DeselectUnits(SelectableUnit unit)
   {
      SelectedUnits.Remove(unit);
      unit.OnDeselected();
   }
   public void ClearSelectedUnits()
   {
      SelectedUnits.Clear();
      foreach (SelectableUnit unit in AvailableUnits)
      {
         unit.OnDeselected();
      }
   }
}
