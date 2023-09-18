using UnityEngine;

namespace IsoRush.Utils
{
    public class SoundVariator : MonoBehaviour
    {
        [SerializeField]
        AudioSource _audioSource;

        [SerializeField]
        private float _pitch = 1;

        [SerializeField]
        private float _volume = 1;

        [SerializeField]
        private float _pitchVariation = 0.01f;

        [SerializeField]
        private float _volumeVariation = 0.08f;

        private void Update()
        {
            _audioSource.pitch = _pitch + Mathf.Sin(Time.time * 2f) * _pitchVariation;
            _audioSource.volume = _volume + Mathf.Sin(Time.time * 2f) * _volumeVariation;
        }
    }
}
