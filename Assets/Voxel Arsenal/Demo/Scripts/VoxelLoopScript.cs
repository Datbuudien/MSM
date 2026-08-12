using UnityEngine;
using System.Collections;

namespace VoxelArsenal
{
    public class VoxelLoopScript : MonoBehaviour
    {
        [Header("Attach effect here")]
        public GameObject chosenEffect;
        public float loopDuration = 2.0f;

        void Start()
        {
            PlayEffect();
        }

        public void PlayEffect()
        {
            StartCoroutine("EffectLoop");
        }

        IEnumerator EffectLoop()
        {
            GameObject effectPlayer = (GameObject)Instantiate(chosenEffect); //Spawns the attached effect
            effectPlayer.transform.position = transform.position; //Sets the position of the effect
            yield return new WaitForSeconds(loopDuration); //Waits for the set time
            Destroy(effectPlayer); //Removes the effect
            PlayEffect(); //Restarts the effect
        }
    }
}