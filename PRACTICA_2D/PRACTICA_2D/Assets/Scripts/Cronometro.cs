using UnityEngine;
using TMPro;

public class Cronometro : MonoBehaviour
{
    public float tiempoActual = 0f;
    private bool cronometroActivo = true;

    [SerializeField] private TextMeshProUGUI textoTiempo;

    void Update()
    {
        if (cronometroActivo)
        {
            tiempoActual += Time.deltaTime * Time.timeScale;
            ActualizarUI();
        }
    }

    public void PausarTiempo() { cronometroActivo = false; }
    public void ReanudarTiempo() { cronometroActivo = true; }
    public void EstablecerTiempo(float tiempoCargado) { tiempoActual = tiempoCargado; }

    private void ActualizarUI()
    {
        if (textoTiempo != null)
        {
            int minutos = Mathf.FloorToInt(tiempoActual / 60);
            int segundos = Mathf.FloorToInt(tiempoActual % 60);
            textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        }
    }
}