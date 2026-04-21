using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuCarga : MonoBehaviour
{
    [Header("Textos y Botones")]
    public TextMeshProUGUI textoInicio;
    public Button[] botonesPartida;

    void Start()
    {
        ConfigurarMenu();
    }

    void ConfigurarMenu()
    {
        string rutaBase = Application.persistentDataPath;

        for (int i = 0; i < botonesPartida.Length; i++)
        {
            int numeroPartida = i + 1;
            string rutaCompleta = Path.Combine(rutaBase, "partida_" + numeroPartida + ".json");

            TextMeshProUGUI textoBoton = botonesPartida[i].GetComponentInChildren<TextMeshProUGUI>();
            botonesPartida[i].onClick.RemoveAllListeners();

            if (File.Exists(rutaCompleta))
            {
                string contenido = File.ReadAllText(rutaCompleta);
                DatosJuego datos = JsonUtility.FromJson<DatosJuego>(contenido);
                textoBoton.text = "Partida " + numeroPartida + " - " + datos.fecha;
            }
            else
            {
                textoBoton.text = "Nueva Partida " + numeroPartida;
            }

            botonesPartida[i].onClick.AddListener(() => SeleccionarSlot(numeroPartida));
        }
    }

    void SeleccionarSlot(int slot)
    {
        PlayerPrefs.SetInt("SlotSeleccionado", slot);
        SceneManager.LoadScene("proyecto");
    }
}