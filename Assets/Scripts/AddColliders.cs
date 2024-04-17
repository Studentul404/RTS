using UnityEngine;
using UnityEngine.SceneManagement;

public class AddColliders : MonoBehaviour
{

    private void Start()
    {
        GetChildRecursive(gameObject);
    }

    private void GetChildRecursive(GameObject obj)
    {
        if (null == obj)
            return;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
                continue;

            if (child.gameObject.GetComponent<MeshCollider>() == null)
            {
                child.gameObject.AddComponent<MeshCollider>();
                if (SceneManager.GetActiveScene().name == "Demo_Exterior")
                    child.gameObject.GetComponent<MeshCollider>().convex = true;
            }
            GetChildRecursive(child.gameObject);
        }
    }
}
