using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class AudioManager : MonoBehaviour
    {
        private void Start()
        {
            var vol = PlayerPrefs.GetFloat(SpaceTruckerConstants.VolumeKey, 1);
            var sources = FindObjectsOfType<AudioSource>();
            foreach (var audioSource in sources)
            {
                audioSource.volume = vol;
            }
        }
    }
}