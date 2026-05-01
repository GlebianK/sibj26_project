using UnityEngine;
using UnityEngine.Audio;

    public class PrefsVolumeSettingHandler : PrefsSettingHandlerBase
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private string _parameterName;

        protected override void SetValue(float value)
        {
            var volume = ConvertToVolume(value);
            _audioMixer.SetFloat(_parameterName, volume);
        }

        /// <summary>
        /// Convert clamped01 value to volume in -db
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private float ConvertToVolume(float value)
        {
            var t = Mathf.Log(value * 100f, 100f);
            return Mathf.Lerp(-80f, 0f, t);
        }
    }
