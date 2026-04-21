using TMPro;
using UnityEngine;

public class VisualizadorRankingsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoRankings;

    private void OnEnable()
    {
        ActualizarUI();
    }

    public void ActualizarUI()
    {
        if (GestorRankings.Instancia != null && textoRankings != null)
        {
            textoRankings.text = GestorRankings.Instancia.ObtenerTextoFormateado();
            Debug.Log("Ranking actualizado desde el Gestor.");
        }
    }
}