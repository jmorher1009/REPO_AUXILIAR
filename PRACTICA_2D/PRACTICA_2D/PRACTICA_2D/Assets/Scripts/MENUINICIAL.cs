using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
public class MENUINICIAL : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sliderVolumenMusica;
    [SerializeField] private Slider sliderVolumenSFX;
    private GameObject panelRankings;

    void Start()
    {
        float valorM = PlayerPrefs.GetFloat("VOLUMEN", 1f);
        float valorE = PlayerPrefs.GetFloat("SFX", 1f);

        sliderVolumenMusica.value = valorM;
        sliderVolumenSFX.value = valorE;

        CambiarVolumenMusica(valorM);
        CambiarVolumenSFX(valorE);
    }

    public void Jugar()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void CambiarVolumenMusica(float valor)
    {
        float db = valor > 0.0001f ? Mathf.Log10(valor) * 20 : -80f;
        audioMixer.SetFloat("VOLUMEN", db);
        PlayerPrefs.SetFloat("VOLUMEN", valor);
    }

    public void CambiarVolumenSFX(float valor)
    {
        float db = valor > 0.0001f ? Mathf.Log10(valor) * 20 : -80f;
        audioMixer.SetFloat("SFX", db);
        PlayerPrefs.SetFloat("SFX", valor);
    }


}