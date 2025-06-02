using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum TipoAtaque
{
    Albondiga,
    Espagueti,
    Queso
}
public class SpaghettiCode : MonoBehaviour, IDanable
{
    //constantes
    private const float TiempoEntreAtaques = 1.8f;
    private const int RadioDeteccion = 8;
    private const string TagJugador = "Player";




    //seriazile y variables publicas
    [SerializeField] private Transform _jugador;

    [SerializeField] private Transform _puntoAtaqueAlto;
    [SerializeField] private Transform _puntoAtaqueMedio;
    [SerializeField] private Transform _puntoAtaqueBajo;

    [SerializeField] private GameObject _albondigaPrefab;
    [SerializeField] private GameObject _espaguetiPrefab;
    [SerializeField] private GameObject _quesoPrefab;
    [SerializeField] private GameObject[] _traceOpciones; // los 3 traces en la escena

    //variabels privadas
    private SpriteRenderer _spriteRenderer;
    private Coroutine _rutinaAtaque;
    private int _ultimoAtaque = -1;
    private bool _estaAtacando = false;
    // private Animator _animator;
    private bool _modoPuzzleActivo = false;
    private int _salud;
    private int _saludMaxima = 20;
    private string _traceCorrectoIndex = "1";

    private void Start()
    {
        Incializar();

        if (_jugador != null)
            IniciarAtaques();
    }
    private void Incializar()
    {
        // _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _salud = _saludMaxima;
        DesactivarTraces();
    }
    private void DesactivarTraces()
    {
        foreach (GameObject trace in _traceOpciones)
            trace.SetActive(false);

    }
      private void IniciarAtaques()
    {
        if (_rutinaAtaque == null)
            _rutinaAtaque = StartCoroutine(CicloDeAtaque());
    }

    private IEnumerator CicloDeAtaque()
    {
        while (!_modoPuzzleActivo)
        {
            float distancia = Vector2.Distance(transform.position, _jugador.position);

            if (!_estaAtacando && distancia <= RadioDeteccion)
            {
                _estaAtacando = true;

                TipoAtaque ataque = ObtenerAtaqueAleatorio();

                EjecutarAtaque(ataque);

                yield return new WaitForSeconds(TiempoEntreAtaques);
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
            StartCoroutine(AtaqueAlbondiga());
            break;
        case TipoAtaque.Espagueti:
            StartCoroutine(AtaqueEspagueti());
            break;
        case TipoAtaque.Queso:
            StartCoroutine(AtaqueQueso());
            break;
    }
}

    private void LanzarAlbondiga(Transform punto)
    {
        GameObject albondiga = Instantiate(_albondigaPrefab, punto.position, Quaternion.identity);

        Vector2 direccion = (_jugador.position - punto.position).normalized;
        direccion = new Vector2(direccion.x * 0.4f, Mathf.Abs(direccion.y) * 0.8f).normalized;

        float distancia = Vector2.Distance(_jugador.position, punto.position);
        float fuerza = distancia * 1.6f;

        albondiga.GetComponent<DisparoAlbondiga>()?.LanzarDisparo(direccion, fuerza);
    }

    private void CrearEspagueti(Transform punto)
    {
        GameObject espagueti = Instantiate(_espaguetiPrefab, punto.position, Quaternion.identity);
        espagueti.transform.right = Vector2.right;
    }

    private void LanzarQueso(Transform punto)
    {
        GameObject queso = Instantiate(_quesoPrefab, punto.position, Quaternion.identity);
        queso.GetComponent<DisparoQueso>()?.LanzarDisparo(_jugador.position);
    }
private IEnumerator AtaqueAlbondiga()
{
    LanzarAlbondiga(_puntoAtaqueAlto);
    yield return new WaitForSeconds(TiempoEntreAtaques);
    _estaAtacando = false;
}

private IEnumerator AtaqueEspagueti()
{
    CrearEspagueti(_puntoAtaqueBajo);
    yield return new WaitForSeconds(TiempoEntreAtaques);
    _estaAtacando = false;
}

private IEnumerator AtaqueQueso()
{
    LanzarQueso(GetPuntoAtaqueAleatorio());
    yield return new WaitForSeconds(TiempoEntreAtaques);
    _estaAtacando = false;
}

    private Transform GetPuntoAtaqueAleatorio()
    {
        return Random.Range(0, 3) switch
        {
            0 => _puntoAtaqueAlto,
            1 => _puntoAtaqueMedio,
            _ => _puntoAtaqueBajo,
        };
    }
    public void IniciarModoPuzzle()
    {
        _modoPuzzleActivo = true;
        DetenerAtaques();

        foreach (GameObject trace in _traceOpciones)
            trace.SetActive(true);
    }
       private void DetenerAtaques()
    {
        if (_rutinaAtaque != null)
        {
            StopCoroutine(_rutinaAtaque);
            _rutinaAtaque = null;
        }
    }



    public void VerificarTrace(string idSeleccionado)
    {
        if (!_modoPuzzleActivo) return;

        if (idSeleccionado.Equals(_traceCorrectoIndex))
        {
            Debug.Log("¡Correcto! El jefe ha sido derrotado.");
            MatarJefe();
        }
        else
        {
            Debug.Log("Incorrecto. El jefe recupera fuerza...");
            _salud = Mathf.Min(_salud + 3, _saludMaxima); // recupera vida
            _modoPuzzleActivo = false;
            DesactivarTraces();
            IniciarAtaques();
        }
    }

    public void Golpe()
    {
        RecibirDano();
    }
    private void RecibirDano()
    {

        if (_modoPuzzleActivo) return; // no se puede dañar

        _salud--;

        if (_salud <= 3 && !_modoPuzzleActivo) // umbral para entrar en modo puzzle
        {
            AsignarTraceCorrecto(1); // defines tú cuál es el correcto
            IniciarModoPuzzle();     // ← corregido aquí el nombre del método
            return;
        }


        if (_salud <= 0)
        {
            MatarJefe();
        }
    }
    private void MatarJefe()
    {
        Debug.Log("El jefe ha sido destruido.");
        Destroy(gameObject);
    }

    public void AsignarTraceCorrecto(int index)
    {
        _traceCorrectoIndex = index.ToString();

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, RadioDeteccion);
    }
}