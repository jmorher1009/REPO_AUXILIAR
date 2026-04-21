using UnityEngine;
using UnityEngine.Audio;

public class AplicarVolumen : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    void Start()
    {
        float valorM = PlayerPrefs.GetFloat("VOLUMEN", 1f);
        float valorE = PlayerPrefs.GetFloat("SFX", 1f);

        float dbM = valorM > 0.0001f ? Mathf.Log10(valorM) * 20 : -80f;
        float dbE = valorE > 0.0001f ? Mathf.Log10(valorE) * 20 : -80f;

        audioMixer.SetFloat("VOLUMEN", dbM);
        audioMixer.SetFloat("SFX", dbE);
    }
}