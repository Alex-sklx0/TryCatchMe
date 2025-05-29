using UnityEngine;
using System.Collections;
using System.Collections.Generic;
enum TipoAtaque { Albondiga, Espagueti, Queso }
public class SpaghettiCodeBoss : MonoBehaviour
{
    [Header("Configuración")]
    private float _tiempoEntreAtaques = 2f;
    private float _radioDeteccion = 8f; 
    public Transform jugador;
    private SpriteRenderer _spriteRenderer;

    public Transform puntoAtaqueAlto;
    public Transform puntoAtaqueMedio;
    public Transform puntoAtaqueBajo;

    public GameObject albondigaPrefab;
    public GameObject espaguetiPrefab;
    public GameObject quesoPrefab;

    private int _ultimoAtaque = -1;
    private bool _estaAtacando = false;
    // private Animator _animator;
    private int _salud =20;
    

    private void Start()
    {
        // _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        if (jugador != null)
            StartCoroutine(CicloDeAtaque());    }
    private IEnumerator CicloDeAtaque()
    {
        while (true)
        {
            float distancia = Vector2.Distance(transform.position, jugador.position);

            if (!_estaAtacando && distancia <= _radioDeteccion)
            {
                _estaAtacando = true;

                TipoAtaque ataque = ObtenerAtaqueAleatorio();

                EjecutarAtaque(ataque);

                yield return new WaitForSeconds(_tiempoEntreAtaques);
                _estaAtacando = false;
            }

            yield return null;
        }
    }

    private TipoAtaque ObtenerAtaqueAleatorio()
    {
        int ataque;
        do
        {
            ataque = Random.Range(0, 3);
        } while (ataque == _ultimoAtaque);

        _ultimoAtaque = ataque;
        return (TipoAtaque)ataque;
    }

    private void EjecutarAtaque(TipoAtaque tipo)
    {
        switch (tipo)
        {
            case TipoAtaque.Albondiga:
                LanzarAlbondiga(puntoAtaqueAlto);
                break;
            case TipoAtaque.Espagueti:
                CrearEspagueti(puntoAtaqueBajo);
                break;
            case TipoAtaque.Queso:
                LanzarQueso(GetPuntoAtaqueAleatorio());
                break;
        }
    }

    private void LanzarAlbondiga(Transform punto)
    {
        GameObject albondiga = Instantiate(albondigaPrefab, punto.position, Quaternion.identity);

        Vector2 direccion = (jugador.position - punto.position).normalized;
        direccion = new Vector2(direccion.x * 0.4f, Mathf.Abs(direccion.y) * 0.8f).normalized;

        float distancia = Vector2.Distance(jugador.position, punto.position);
        float fuerza = distancia * 1.6f;

        albondiga.GetComponent<AlbondigaScript>()?.Inicializar(direccion, fuerza);
    }

    private void CrearEspagueti(Transform punto)
    {
        GameObject espagueti = Instantiate(espaguetiPrefab, punto.position, Quaternion.identity);
        espagueti.transform.right = Vector2.right;
    }

    private void LanzarQueso(Transform punto)
    {
        GameObject queso = Instantiate(quesoPrefab, punto.position, Quaternion.identity);
        queso.GetComponent<QuesoScript>()?.Inicializar(jugador.position);
    }

    private Transform GetPuntoAtaqueAleatorio()
    {
        return Random.Range(0, 3) switch
        {
            0 => puntoAtaqueAlto,
            1 => puntoAtaqueMedio,
            _ => puntoAtaqueBajo,
        };
    }

    public void Golpe()
    {
        _spriteRenderer.color = Color.white;
        _salud--;
        if (_salud <= 0)
            Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radioDeteccion);
    }
}