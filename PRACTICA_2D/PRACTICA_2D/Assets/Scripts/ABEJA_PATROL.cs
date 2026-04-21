using UnityEngine;

public class ENEMIGO_VOLADOR : MonoBehaviour
{
    public enum EstadoAbeja
    {
        Patrullando,
        Persiguiendo,
        Atacando,
        Retirandose
    }

    [Header("Movimiento")]
    [SerializeField] private float velocidadMovimiento = 1.5f;
    [SerializeField] private float suavizado = 3f;
    private Vector2 velocidadActual;

    [Header("Patrulla")]
    [SerializeField] private Transform[] puntosMovimiento;
    [SerializeField] private float distanciaMinima = 0.2f;
    private int indicePatrulla = 0;
    private int direccionPatrulla = 1;

    [Header("Jugador")]
    [SerializeField] private Transform jugador;
    [SerializeField] private float distanciaMinimaJugador = 1.2f;

    [Header("Radios")]
    [SerializeField] private float radioPersecucionEntrada = 6f;
    [SerializeField] private float radioPersecucionSalida = 8f;
    [SerializeField] private float radioAtaqueEntrada = 2f;
    [SerializeField] private float radioAtaqueSalida = 3f;

    [Header("Ataque")]
    [SerializeField] private float cooldownAtaque = 1.5f;
    private float tiempoUltimoAtaque;

    [Header("Retirada")]
    [SerializeField] private float distanciaRetirada = 3f;
    [SerializeField] private float tiempoRetirada = 1.2f;
    private float timerRetirada;

    [Header("Evitar obstáculos")]
    [SerializeField] private LayerMask capaTerreno;
    [SerializeField] private float distanciaDeteccion = 2.5f;
    [SerializeField] private float fuerzaEvasion = 2f;

    [Header("Decisión")]
    [SerializeField] private float tiempoDecision = 0.3f;
    private float timerDecision;
    private Vector2 direccionGuardada;

    [Header("Referencias")]
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private EstadoAbeja estadoActual;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (jugador == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null) jugador = obj.transform;
        }

        CambiarEstado(EstadoAbeja.Patrullando);
    }

    // ESTADOS

    void FixedUpdate()
    {
        if (jugador == null)
        {
            Patrullar();
            return;
        }

        float distancia = Vector2.Distance(rb.position, jugador.position);
        bool jugadorPorEncima = jugador.position.y > rb.position.y + 0.5f;

        switch (estadoActual)
        {
            case EstadoAbeja.Patrullando:

                Patrullar();

                if (distancia < radioPersecucionEntrada)
                    CambiarEstado(EstadoAbeja.Persiguiendo);

                break;

            case EstadoAbeja.Persiguiendo:

                Perseguir();

                if (distancia < radioAtaqueEntrada && !jugadorPorEncima)
                    CambiarEstado(EstadoAbeja.Atacando);

                else if (distancia > radioPersecucionSalida)
                    CambiarEstado(EstadoAbeja.Patrullando);

                break;

            case EstadoAbeja.Atacando:

                Perseguir();

                if (Time.time > tiempoUltimoAtaque + cooldownAtaque && !jugadorPorEncima)
                {
                    Atacar();
                }

                if (distancia > radioAtaqueSalida || jugadorPorEncima)
                    CambiarEstado(EstadoAbeja.Persiguiendo);

                break;

            case EstadoAbeja.Retirandose:

                Retirarse();

                break;
        }
    }

    //DAÑO A JUGADOR

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            VidaJugador vida = collision.GetComponent<VidaJugador>();

            if (vida != null)
            {
                vida.RecibirDanioDesdeAbeja(transform.position);
            }
        }
    }

    void Atacar()
    {
        if (Random.value > 0.5f)
            animator.SetTrigger("attack1");
        else
            animator.SetTrigger("attack2");

        tiempoUltimoAtaque = Time.time;

        CambiarEstado(EstadoAbeja.Retirandose);
        timerRetirada = tiempoRetirada;
    }

    void Retirarse()
    {
        Vector2 direccion = (rb.position - (Vector2)jugador.position).normalized;

        Vector2 destino = rb.position + direccion * distanciaRetirada;

        MoverConEvasion(destino);

        timerRetirada -= Time.fixedDeltaTime;

        if (timerRetirada <= 0)
        {
            CambiarEstado(EstadoAbeja.Persiguiendo);
        }

        Girar(jugador.position.x);
    }

    void Patrullar()
    {
        if (puntosMovimiento.Length == 0) return;

        Vector2 destino = puntosMovimiento[indicePatrulla].position;

        MoverConEvasion(destino);

        if (Vector2.Distance(rb.position, destino) < distanciaMinima)
        {
            indicePatrulla += direccionPatrulla;

            if (indicePatrulla >= puntosMovimiento.Length)
            {
                indicePatrulla = puntosMovimiento.Length - 2;
                direccionPatrulla = -1;
            }
            else if (indicePatrulla < 0)
            {
                indicePatrulla = 1;
                direccionPatrulla = 1;
            }
        }

        Girar(destino.x);
    }

    void Perseguir()
    {
        timerDecision -= Time.fixedDeltaTime;

        if (timerDecision <= 0)
        {
            Vector2 dir = jugador.position - transform.position;

            if (Mathf.Abs(dir.y) > 2f)
                dir.y *= 0.5f;

            direccionGuardada = dir.normalized;
            timerDecision = tiempoDecision;
        }

        if (Vector2.Distance(rb.position, jugador.position) > distanciaMinimaJugador)
        {
            MoverConEvasion(rb.position + direccionGuardada);
        }

        Girar(jugador.position.x);
    }

    void MoverConEvasion(Vector2 destino)
    {
        Vector2 direccion = (destino - rb.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(rb.position, direccion, distanciaDeteccion, capaTerreno);

        if (hit.collider != null)
        {
            Vector2 normal = hit.normal;

            Vector2 tangente1 = new Vector2(-normal.y, normal.x);
            Vector2 tangente2 = new Vector2(normal.y, -normal.x);

            float dot1 = Vector2.Dot(tangente1, direccion);
            float dot2 = Vector2.Dot(tangente2, direccion);

            Vector2 direccionEvasion = (dot1 > dot2) ? tangente1 : tangente2;

            direccion = (direccionEvasion * fuerzaEvasion + normal).normalized;
        }

        velocidadActual = Vector2.Lerp(
            velocidadActual,
            direccion * velocidadMovimiento,
            suavizado * Time.fixedDeltaTime
        );

        rb.MovePosition(rb.position + velocidadActual * Time.fixedDeltaTime);
    }

    void Girar(float objetivoX)
    {
        if (rb.position.x < objetivoX)
            spriteRenderer.flipX = false;
        else if (rb.position.x > objetivoX)
            spriteRenderer.flipX = true;
    }

    void CambiarEstado(EstadoAbeja nuevoEstado)
    {
        if (estadoActual == nuevoEstado) return;

        estadoActual = nuevoEstado;

        switch (estadoActual)
        {
            case EstadoAbeja.Patrullando:
                animator.SetBool("isMoving", false);
                break;

            case EstadoAbeja.Persiguiendo:
                animator.SetBool("isMoving", true);
                break;
        }
    }

    // RADIOS
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioPersecucionEntrada);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radioPersecucionSalida);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioAtaqueEntrada);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radioAtaqueSalida);
    }
}