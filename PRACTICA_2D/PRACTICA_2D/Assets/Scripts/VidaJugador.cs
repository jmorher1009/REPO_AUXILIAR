using UnityEngine;
using System.Collections;

public class VidaJugador : MonoBehaviour
{
    private const int NUMERO_VIDAS = 6;
    public int cantidadVida;

    public float fuerzaRebote = 3f;
    public float fuerzaEmpujeLateral = 5f;

    public float tiempoInvulnerabilidad = 1f;
    public float tiempoPerdidaControl = 0.3f;

    public GameObject[] corazones;

    private Rigidbody2D rb;
    private RanaMovement movement;
    private bool puedeRecibirDano = true;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoGolpe;

    void Awake()
    {
        cantidadVida = NUMERO_VIDAS;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<RanaMovement>();

        InicializarCorazones();
        ActualizarCorazonesUI();
    }

    public void RecibirDanioDesdeAbeja(Vector2 posicionAbeja)
    {
        if (!puedeRecibirDano) return;

        float diferenciaY = transform.position.y - posicionAbeja.y;

        //empuje
        if (diferenciaY > 0.8f)
        {
            float direccion = transform.position.x < posicionAbeja.x ? -1f : 1f;

            rb.linearVelocity = new Vector2(direccion * fuerzaEmpujeLateral, 2f);
            return;
        }

        AplicarDanio(2, posicionAbeja);
    }

    private void AplicarDanio(int dano, Vector2 posicionEnemigo)
    {
        cantidadVida -= dano;

        audioSource?.PlayOneShot(sonidoGolpe);
        ActualizarCorazonesUI();

        Vector2 direccion = ((Vector2)transform.position - posicionEnemigo).normalized;

        rb.linearVelocity = new Vector2(
            direccion.x * fuerzaRebote,
            fuerzaRebote * 0.9f
        );

        if (cantidadVida <= 0)
        {
            movement?.Morir();
        }
        else
        {
            StartCoroutine(ActivarInvulnerabilidad());
        }
    }

    private IEnumerator ActivarInvulnerabilidad()
    {
        puedeRecibirDano = false;

        if (movement != null)
            movement.enabled = false;

        yield return new WaitForSeconds(tiempoPerdidaControl);

        if (movement != null)
            movement.enabled = true;

        yield return new WaitForSeconds(tiempoInvulnerabilidad - tiempoPerdidaControl);

        puedeRecibirDano = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!puedeRecibirDano) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            AplicarDanio(1, collision.transform.position);
        }
    }

    private void InicializarCorazones()
    {
        GameObject contenedorVidas = GameObject.Find("VIDAS");

        if (contenedorVidas != null)
        {
            corazones = new GameObject[contenedorVidas.transform.childCount];

            for (int i = 0; i < corazones.Length; i++)
            {
                corazones[i] = contenedorVidas.transform.GetChild(i).gameObject;
            }
        }
    }

    public void ActualizarCorazonesUI()
    {
        if (corazones == null || corazones.Length == 0)
            InicializarCorazones();

        for (int i = 0; i < corazones.Length; i++)
        {
            corazones[i].SetActive(i < cantidadVida);
        }
    }
}