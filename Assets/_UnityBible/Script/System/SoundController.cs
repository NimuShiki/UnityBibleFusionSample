using UnityEngine;
using UnityEngine.Audio;

namespace UnityBibleSample
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;

        public void SetBGM(float volume)
        {
            audioMixer.SetFloat("BGM", Mathf.Sqrt(volume) * 80f - 80f);
        }

        public void SetSE(float volume)
        {
            audioMixer.SetFloat("SE", Mathf.Sqrt(volume) * 80f - 80f);
        }
    }
}