using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaghettiCodeBoss : MonoBehaviour
{
    [Header("Configuración")]
    public float tiempoEntreAtaques = 2f;
    public float radioDeteccion = 8f; // Nuevo: Radio donde detecta al jugador
    public Transform puntoAtaqueAlto;
    public Transform puntoAtaqueMedio;
    public Transform puntoAtaqueBajo;

    [Header("Prefabs de Ataques")]
    public GameObject albondigaPrefab;
    public GameObject espaguetiPrefab;
    public GameObject quesoPrefab;

    private int _ultimoAtaque = -1;
    private bool _estaAtacando = false;
    // private Animator _animator;
    private Transform _jugador;
    private SpriteRenderer _spriteRenderer;
    private int _salud = 5;

    private void Start()
    {
        // _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _jugador = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(CicloDeAtaque());
    }

    IEnumerator CicloDeAtaque()
    {
        while (true)
        {
            float distancia = Vector2.Distance(transform.position, _jugador.position);

            if (!_estaAtacando && distancia <= radioDeteccion)
            {
                _estaAtacando = true;
                _spriteRenderer.color = Color.red;

                int ataqueSeleccionado;
                do
                {
                    ataqueSeleccionado = Random.Range(0, 3);
                } while (ataqueSeleccionado == _ultimoAtaque);

                _ultimoAtaque = ataqueSeleccionado;

                switch (ataqueSeleccionado)
                {
                    case 0: // Albóndiga
                        AtaqueAlbondiga(puntoAtaqueAlto);
                        break;
                    case 1: // Espagueti
                        AtaqueEspagueti(puntoAtaqueBajo);
                        break;
                    case 2: // Queso (usa aleatorio si quieres)
                        AtaqueQueso(GetPuntoAtaqueAleatorio());
                        break;
                }

                yield return new WaitForSeconds(tiempoEntreAtaques);
                _estaAtacando = false;
                _spriteRenderer.color = Color.white;
            }

            yield return null;
        }
    }

    private Transform GetPuntoAtaqueAleatorio()
    {
        int altura = Random.Range(0, 3);
        switch (altura)
        {
            case 0: return puntoAtaqueAlto;
            case 1: return puntoAtaqueMedio;
            case 2: return puntoAtaqueBajo;
            default: return puntoAtaqueMedio;
        }
    }

    private void AtaqueAlbondiga(Transform puntoAtaque)
    {
        GameObject albondiga = Instantiate(albondigaPrefab, puntoAtaque.position, Quaternion.identity);

        // Dirección hacia el jugador
        Vector2 direccion = (_jugador.position - puntoAtaque.position).normalized;
        direccion.y = Mathf.Abs(direccion.y); // Siempre hacia arriba

        // Ajustar proporción: más vertical que horizontal
        direccion = new Vector2(direccion.x * 0.4f, direccion.y * 0.8f).normalized;

        // Calcular distancia al jugador
        float distancia = Vector2.Distance(_jugador.position, puntoAtaque.position);

        // Escalar la fuerza en base a la distancia (ajustar el factor según lo que se sienta bien)
        float fuerza = distancia * 1.5f; // Puedes ajustar el 1.5f para calibrar

        // Lanzar la albóndiga
        albondiga.GetComponent<AlbondigaScript>().Inicializar(direccion, fuerza);
    }




    private void AtaqueEspagueti(Transform puntoAtaque)
    {
        GameObject espagueti = Instantiate(espaguetiPrefab, puntoAtaque.position, Quaternion.identity);

        // Asegurar que crezca horizontalmente hacia la derecha
        espagueti.transform.right = Vector2.right;

        // No reasignes posición Y: ya viene del punto correcto
        espagueti.transform.position = puntoAtaque.position;
    }



    private void AtaqueQueso(Transform puntoAtaque)
    {
        // _animator.SetTrigger("Queso");
        GameObject queso = Instantiate(quesoPrefab, puntoAtaque.position, Quaternion.identity);
        queso.GetComponent<QuesoScript>().Inicializar(_jugador.position);
    }

    // Método para visualizar el radio en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }
    public void Golpe()
    {
        _salud -= 1;
        if (_salud <= 0) Destroy(gameObject);
    }
}