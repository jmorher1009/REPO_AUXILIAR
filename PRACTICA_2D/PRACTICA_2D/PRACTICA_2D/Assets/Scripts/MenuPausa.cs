using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject BotonPausa;
    [SerializeField] private GameObject MPausa;
    private GameObject[] monedas;
    private Cronometro cronometro;

    void Start()
    {
        monedas = GameObject.FindGameObjectsWithTag("Moneda");
        cronometro = FindAnyObjectByType<Cronometro>();
    }

    public void Pausa()
    {
        Time.timeScale = 0f;
        BotonPausa.SetActive(false);
        MPausa.SetActive(true);
        ActivarMonedas(false);
        ActivarVidas(false);
        if (cronometro != null)
        {
            cronometro.PausarTiempo();
        }
    }

    public void Reanudar()
    {
        Time.timeScale = 1f;
        BotonPausa.SetActive(true);
        MPausa.SetActive(false);
        ActivarMonedas(true);
        ActivarVidas(true);
        if (cronometro != null)
        {
            cronometro.ReanudarTiempo();
        }
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !RanaMovement.Instancia.muerto)
        {
            if (Time.timeScale == 0f)
            {
                Reanudar();
            }
            else
            {
                Pausa();
            }
        }
    }

    public void Guardar()
    {
        ControladorDatosJuego controlador = FindAnyObjectByType<ControladorDatosJuego>();

        if (controlador != null)
        {
            controlador.GuardarDatos();
            Debug.Log("Partida guardada desde menú pausa");
        }
        else
        {
            Debug.LogWarning("No se encontró ControladorDatosJuego");
        }
        Reanudar();
    }

    public void Salir()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    void ActivarMonedas(bool estado)
    {
        GameObject[] monedas = GameObject.FindGameObjectsWithTag("Moneda");

        foreach (GameObject moneda in monedas)
        {
            SpriteRenderer[] renderers = moneda.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer sr in renderers)
            {
                sr.enabled = estado;
            }
        }
    }

    void ActivarVidas(bool estado)
    {
        GameObject[] vidas = GameObject.FindGameObjectsWithTag("Vidas");

        foreach (GameObject vida in vidas)
        {
            SpriteRenderer[] renderers = vida.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in renderers)
            {
                sr.enabled = estado;
            }

            UnityEngine.UI.Image[] imagenes = vida.GetComponentsInChildren<UnityEngine.UI.Image>();
            foreach (UnityEngine.UI.Image img in imagenes)
            {
                img.enabled = estado;
            }
        }
    }
}