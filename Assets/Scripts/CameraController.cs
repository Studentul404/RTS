using UnityEngine;

public class CameraController : MonoBehaviour
{
   public Transform cameraTransform;
   public LayerMask collisionLayer; // Layer, с которым не должна сталкиваться камера

   public float movementSpeed;
   public float normalSpeed;
   public float fastSpeed;
   public float mouseSensitivity = 200f;
   public float movementTime;
   public float rotationAmount;
   public Vector3 zoomAmount;

   private Vector3 newPosition;
   private Quaternion newRotation;
   private Vector3 newZoom;

   void Start()
   {
      newPosition = transform.position;
      newRotation = transform.rotation;
      newZoom = cameraTransform.localPosition;
   }

   void Update()
   {
      HandleMovementInput();
      CheckCollision();
   }

   void HandleMovementInput()
   {
      if (Input.GetKey(KeyCode.LeftShift))
      {
         movementSpeed = fastSpeed;
      }
      else
      {
         movementSpeed = normalSpeed;
      }

      if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
      {
         newPosition += transform.forward * movementSpeed;
      }
      if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
      {
         newPosition -= transform.forward * movementSpeed;
      }
      if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
      {
         newPosition += transform.right * movementSpeed;
      }
      if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
      {
         newPosition -= transform.right * movementSpeed;
      }

      if (Input.GetKey(KeyCode.Q))
      {
         newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
      }
      if (Input.GetKey(KeyCode.E))
      {
         newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
      }

      if (Input.GetKey(KeyCode.R))
      {
         newZoom += zoomAmount;
      }
      if (Input.GetKey(KeyCode.F))
      {
         newZoom -= zoomAmount;
      }

      transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, movementTime * Time.deltaTime);
      transform.position = Vector3.Lerp(transform.position, newPosition, movementTime * Time.deltaTime);
      if (newPosition.x > 140)
         newPosition = new Vector3(140, newPosition.y, newPosition.z);
      if (newPosition.x < -150)
         newPosition = new Vector3(-150, newPosition.y, newPosition.z);
      if (newPosition.z > 165)
         newPosition = new Vector3(newPosition.x, newPosition.y, 165);
      if (newPosition.z < -75)
         newPosition = new Vector3(newPosition.x, newPosition.y, -75);
      if (newZoom.y < 6)
         newZoom = new Vector3(newZoom.x, 6, newZoom.z);

      cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, movementTime * Time.deltaTime);

      /* if (Input.GetMouseButtonDown(2))
      {
         //Debug.Log("Middle mouse down");
         float rotY = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

         // Вращение камеры по вертикали
         float rotX = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
         cameraTransform.Rotate(-rotX, rotY, 0);
      } */
   }

   void CheckCollision()
   {
      RaycastHit hit;
      if (Physics.Raycast(transform.position, cameraTransform.position - transform.position, out hit, Vector3.Distance(transform.position, cameraTransform.position), collisionLayer))
      {
         if (hit.collider == null)
            newPosition = hit.point;
      }
   }
}
