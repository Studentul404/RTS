using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyRotation : MonoBehaviour
{
   public ParticleSystem[] particles;
   // Start is called before the first frame update
   void Start()
    {
      StartCoroutine(RandomParticle());
   }

   // Update is called once per frame
   void Update()
   {
      RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.01f);
   }

   IEnumerator RandomParticle()
   {
      while (true)
      {
         yield return new WaitForSeconds(10f);
         int index = Random.Range(0, particles.Length);
         particles[index].Play();
      }
   }
}
