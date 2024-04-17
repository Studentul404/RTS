using UnityEngine;

public class SoldierBullet : MonoBehaviour
{
   public float trailLength = 10f; // Длина траектории
   public float trailWidth = 0.5f; // Ширина траектории
   public float speed = 10f; // Скорость движения пули
   public Material trailMaterial; // Материал линии

   private LineRenderer lineRenderer;
   private Vector3 startPosition; // Начальная позиция
   private Vector3 endPosition; // Конечная позиция
   private float time; // Время

   void Start()
   {
      // Создание нового Line Renderer
      lineRenderer = gameObject.AddComponent<LineRenderer>();

      // Инициализация Line Renderer
      lineRenderer.material = trailMaterial;
      lineRenderer.startWidth = trailWidth;
      lineRenderer.endWidth = trailWidth;
      lineRenderer.enabled = false;
   }

   public void Shoot(Vector3 startPos, Vector3 targetPos)
   {
      startPosition = startPos;
      endPosition = targetPos;

      // Включение Line Renderer
      lineRenderer.enabled = true;

      // Сброс времени
      time = 0f;
   }

   void Update()
   {
      // Расчет позиции пули
      Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, time);

      // Обновление позиций Line Renderer
      lineRenderer.SetPosition(0, startPosition);
      lineRenderer.SetPosition(1, currentPosition);

      // Увеличение времени
      time += Time.deltaTime * speed;

      // Отключение Line Renderer, когда пуля достигла цели
      if (time >= 1f)
      {
         lineRenderer.enabled = false;
      }
   }

}
