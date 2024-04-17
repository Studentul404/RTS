using NTC.MonoCache;
using System;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Cameras;

public class PlayerInput : MonoCache
{
   [SerializeField]
   private Camera Camera;
   [SerializeField]
   public float timeSinceLastClick = 0.0f;
   public RectTransform SelectionBox;
   [SerializeField]
   private LayerMask UnitLayers;
   [SerializeField]
   private LayerMask FloorLayers;
   private bool isMousePressed;
   [SerializeField]
   public SelectionManager selector;
   [SerializeField]
   private float DragDelay = 0.1f;
   [SerializeField]
   private float MouseDownTime;
   [SerializeField]
   GameObject[] menus;
   public float mouseSensitivity = 100f;
   public float moveSpeed = 5.0f;
   private Vector3 old_position;
   private Quaternion old_rotation;
   [SerializeField]
   private bool isSelected = false;

   public Vector3 offset; // Смещение камеры относительно цели
   public float sensitivity; // Чувствительность мыши

   private float _rotationX;
   private float _rotationY;

   private Vector2 StartMousePosition;

   protected override void Run()
   {
      HandleControllInput();
      HandleMenuOpen();
      if (SelectionBox.gameObject.activeSelf && !isSelected)
      {
         HandleSelectionInput();
         HandleMovementInput();

      }
   }


  /*  private void HandleCameraLook()
   {
      if (selector.SelectedUnits[selector.SelectedUnits.Count-1].gameObject != null)
      {
         if (selector.SelectedUnits[selector.SelectedUnits.Count-1].GetComponent<Health>().health > 5)
         {
            GameObject target = selector.SelectedUnits[selector.SelectedUnits.Count-1].gameObject;
            transform.position = target.GetComponent<SelectableUnit>().cameraPOV.transform.position;
            _rotationY += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

            // Вращение камеры по вертикали
            _rotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            _rotationX = Mathf.Clamp(_rotationX, -90.0f, 90.0f);

            // Применение вращения к камере
            target.GetComponent<SelectableUnit>().hands.transform.localRotation = Quaternion.Euler(_rotationX, _rotationY, 0.0f);
            transform.localRotation = target.GetComponent<SelectableUnit>().hands.transform.localRotation;
            // Перемещение игрока
            float forwardSpeed = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            float strafeSpeed = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            target.transform.position += transform.forward * forwardSpeed + transform.right * strafeSpeed;
         } else
         {
            Camera.GetComponentInParent<CameraController>().enabled = true;
            SelectionBox.gameObject.SetActive(true);
            selector.SelectedUnits[selector.SelectedUnits.Count-1].GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            isSelected = false;
            gameObject.transform.position = old_position;
            gameObject.transform.rotation = old_rotation;
         }
      }
      else
      {
         Camera.GetComponentInParent<CameraController>().enabled = true;
         SelectionBox.gameObject.SetActive(true);
         isSelected = false;
         gameObject.transform.position = old_position;
         gameObject.transform.rotation = old_rotation;
      }
   } */

   public void HandleControllInput()
   {
      
      if (selector.SelectedUnits.Count == 1 && Input.GetKeyUp(KeyCode.C))
      {
         isSelected = !isSelected;
         if (isSelected)
         {
            old_position = gameObject.transform.position;
            old_rotation = gameObject.transform.rotation;
            SelectionBox.gameObject.SetActive(false);
            if (selector.SelectedUnits[selector.SelectedUnits.Count-1])
               selector.SelectedUnits[selector.SelectedUnits.Count-1].GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            Camera.main.GetComponentInParent<CameraController>().enabled = false;
         }
         else
         {
            gameObject.transform.position = old_position;
            gameObject.transform.rotation = old_rotation;
            Camera.GetComponentInParent<CameraController>().enabled = true;
            SelectionBox.gameObject.SetActive(true);
            if (selector.SelectedUnits[selector.SelectedUnits.Count - 1].GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
               selector.SelectedUnits[selector.SelectedUnits.Count-1].GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
         }
      }
   }
   private void HandleMenuOpen()
   {
      bool ismMenuActive = false;
      foreach (GameObject menu in menus)
      {
         if (menu.activeSelf)
            ismMenuActive = true;
      }
      SelectionBox.gameObject.SetActive(!ismMenuActive);
   }

   private void HandleMovementInput()
   {
      timeSinceLastClick += Time.deltaTime;
      if (Input.GetKey(KeyCode.Mouse1) && selector.SelectedUnits.Count > 0)
      {
         if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, FloorLayers))
         {
            for (int i = 0; i < selector.SelectedUnits.Count; i++)
            {
               selector.SelectedUnits[i].MoveTo(hit.point);
            }
            timeSinceLastClick = 0;
         }
      }
   }

   private void HandleSelectionInput()
   {
      if (Input.GetKeyDown(KeyCode.Mouse0))
      {
         isMousePressed = true;
         SelectionBox.sizeDelta = Vector2.zero;
         SelectionBox.gameObject.SetActive(true);
         StartMousePosition = Input.mousePosition;
         MouseDownTime = Time.time;
      }
      else if (Input.GetKeyUp(KeyCode.Mouse0))
      {
         isMousePressed = false;
         SelectionBox.gameObject.SetActive(false);

         if ((Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, UnitLayers))
            && hit.collider.TryGetComponent<SelectableUnit>(out SelectableUnit unit))
         {
            if (Input.GetKey(KeyCode.LeftShift))
            {
               if (selector.SelectedUnits.Contains(unit))
               {
                  selector.DeselectUnits(unit);
               }
               else
               {
                  selector.SelectUnits(unit);
               }
            }
            else
            {
               selector.ClearSelectedUnits();
               selector.SelectUnits(unit);
            }
         }
         else if (MouseDownTime + DragDelay > Time.time)
         {
            selector.ClearSelectedUnits();
         }

         SelectionBox.sizeDelta = Vector2.zero;
         MouseDownTime = 0f;
      }
      else if (isMousePressed && MouseDownTime + DragDelay < Time.time)
      {
         ResizeSelectionBox();
      }
   }

   private void ResizeSelectionBox()
   {
      //Debug.Log(Input.mousePosition);
      float width = Input.mousePosition.x - StartMousePosition.x;
      float height = Input.mousePosition.y - StartMousePosition.y;

      SelectionBox.anchoredPosition = StartMousePosition + new Vector2(width / 2, height / 2);
      SelectionBox.sizeDelta = new Vector2(Math.Abs(width), Math.Abs(height));

      //Debug.Log(selector.AvailableUnits.Count);
      Bounds bounds = new Bounds(SelectionBox.anchoredPosition, SelectionBox.sizeDelta);
      for (int i = 0; i < selector.AvailableUnits.Count; i++)
      {
         if (UnitIsInSelectionBox(Camera.WorldToScreenPoint(selector.AvailableUnits[i].transform.position), bounds))
         {
            selector.SelectUnits(selector.AvailableUnits[i]);
            //Debug.Log(selector.AvailableUnits[i].gameObject.name + "Selected");
         }
         else
         {
            selector.DeselectUnits(selector.AvailableUnits[i]);
            //Debug.Log(selector.AvailableUnits[i].gameObject.name + "Deselected");
         }
      }
   }

   private bool UnitIsInSelectionBox(Vector2 Position, Bounds Bounds)
   {
      bool result = Position.x > Bounds.min.x && Position.x < Bounds.max.x && Position.y > Bounds.min.y && Position.y < Bounds.max.y; ;
      return result;
   }

}
