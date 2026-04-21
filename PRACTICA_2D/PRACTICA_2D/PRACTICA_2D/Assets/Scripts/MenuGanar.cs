using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MenuGanar : MonoBehaviour
{
    [SerializeField] private GameObject panelGanar;
    private TextMeshProUGUI texto;

    private Puntuaje puntuacion;
    private Cronometro cronometro;

    private void Start()
    {
        puntuacion = FindAnyObjectByType<Puntuaje>();
        cronometro = FindAnyObjectByType<Cronometro>();
    }

    public void MostrarGanar()
    {
        float tiempoFinal = 0f;
        float puntosFinales = 0f;
        int vidasFinales = 0;
        string textoTiempo = "00:00";

        if (cronometro != null)
        {
            cronometro.PausarTiempo();
            tiempoFinal = cronometro.tiempoActual;
            int minutos = Mathf.FloorToInt(tiempoFinal / 60);
            int segundos = Mathf.FloorToInt(tiempoFinal % 60);
            textoTiempo = string.Format("{0:00}:{1:00}", minutos, segundos);
        }

        if (puntuacion != null) puntosFinales = puntuacion.ObtenerPuntos();

        VidaJugador scriptVida = FindAnyObjectByType<VidaJugador>();
        if (scriptVida != null) vidasFinales = scriptVida.cantidadVida;

        panelGanar.SetActive(true);
        texto = panelGanar.GetComponentInChildren<TextMeshProUGUI>(true);

        if (texto != null)
        {
            texto.text = "¡Nivel Completado!\n" +
                         "Puntos: " + puntosFinales + "\n" +
                         "Tiempo: " + textoTiempo + "\n" +
                         "Vidas: " + vidasFinales;
        }

        try
        {
            if (GestorRankings.Instancia != null)
            {
                GestorRankings.Instancia.GuardarNuevaPuntuacion(tiempoFinal, puntosFinales, vidasFinales);
                Debug.Log("Ranking guardado correctamente.");
            }
        }
        catch (Exception e) { Debug.LogError("Error en Ranking: " + e.Message); }

        try
        {
            if (ControladorDatosJuego.Instancia != null)
            {
                ControladorDatosJuego.Instancia.BorrarPartidaActual();
                Debug.Log("Partida borrada tras guardar el ranking.");
            }
        }
        catch (Exception e) { Debug.LogError("Error al borrar: " + e.Message); }

        Time.timeScale = 0f;
    }

    public void VolverMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}