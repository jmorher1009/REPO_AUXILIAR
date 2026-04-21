using UnityEngine;

public class FondoManagement : MonoBehaviour
{
    [Header("Configuración Parallax")]
    [Range(0f, 1f)]
    public float parallaxMultiplierX = 0.1f;

    private Transform cam;
    private Vector2 startpos;
    private float length;

    void Start()
    {
        cam = Camera.main.transform;

        // Guardamos la posición inicial
        startpos = new Vector2(transform.position.x, transform.position.y);
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Movimiento relativo al efecto parallax (Solo X)
        float temp = (cam.position.x * (1 - parallaxMultiplierX));

        // Distancia que debe moverse en X
        float distX = (cam.position.x * parallaxMultiplierX);

        // Aplicamos el movimiento (startpos.y se queda fijo, no se le suma nada)
        transform.position = new Vector3(startpos.x + distX, startpos.y, transform.position.z);

        // Lógica de repetición infinita en el eje X
        if (temp > startpos.x + length)
        {
            startpos.x += length;
        }
        else if (temp < startpos.x - length)
        {
            startpos.x -= length;
        }
    }
}