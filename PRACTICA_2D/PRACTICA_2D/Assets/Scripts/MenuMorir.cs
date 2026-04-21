using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuMorir : MonoBehaviour
{
    [SerializeField] private GameObject panelGameOver;
    private TextMeshProUGUI texto;

    private Puntuaje puntuacion;
    private Cronometro cronometro;

    private void Start()
    {
        puntuacion = FindAnyObjectByType<Puntuaje>();
        cronometro = FindAnyObjectByType<Cronometro>();
    }

    public void MostrarGameOver()
    {
        panelGameOver.SetActive(true);
        texto = panelGameOver.GetComponentInChildren<TextMeshProUGUI>(true);

        if (cronometro != null)
        {
            cronometro.PausarTiempo();
        }

        string textoPuntos = puntuacion != null ? puntuacion.ObtenerPuntos().ToString() : "0";

        if (texto != null)
        {
            texto.text = "Game Over\n" + "Puntos: " + textoPuntos;
        }

        Time.timeScale = 0f;
    }

    public void Reintentar()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Salir()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}