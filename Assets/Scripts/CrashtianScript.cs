using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CrashtianScript : MonoBehaviour, IDanable, IVerificadorTrace
{
    // --- ENUM DE ATAQUES ---
    private enum TipoAtaque
    {
        Estalactitas = 0,
        DisparoGrande = 1,
        Chorros = 2
    }

    // --- CONSTANTES ---
    private const float TiempoEntreAtaques = 3f;
    private const float TiempoAviso = 1f;
    private const int UmbralVidaPuzzle = 3;
    private const int SaludMinima = 0;

    // --- SERIALIZED ---
    [Header("Ataques")]
    [SerializeField] private GameObject estalactitaPrefab;
    [SerializeField] private Transform[] puntosEstalactitas;

    [SerializeField] private GameObject disparoGrandePrefab;
    [SerializeField] private Transform puntoDisparo;

    [SerializeField] private GameObject chorroPrefab;
    [SerializeField] private Transform[] puntosChorro;
    [SerializeField] private GameObject indicadorPrefab;

    [Header("Traces y Portal")]
    [SerializeField] private GameObject[] traceOpciones;
    [SerializeField] private GameObject portalSiguienteNivel;

    // --- PRIVADAS ---
    private GameObject _jugador;
    private float _proximoAtaque;
    private int _salud = 10;
    private int _saludMaxima ;
    private bool _modoPuzzleActivo = false;
    private string _traceCorrectoIndex = "1";
    private Coroutine _rutinaAtaque;

    // --- UNITY EVENTS ---
    private void Start()
    {
        _saludMaxima = _salud;
        _jugador = GameObject.FindGameObjectWithTag("Player");
        DesactivarTraces();
        IniciarAtaques();
    }

    private void Update()
    {
        if (_jugador != null)
        {
            Vector3 direccion = (_jugador.transform.position - transform.position).normalized;
            transform.localScale = new Vector3(direccion.x >= 0 ? 1f : -1f, 1f, 1f);
        }
    }

    // --- ATAQUES ---
    private void IniciarAtaques()
    {
        if (_rutinaAtaque == null)
            _rutinaAtaque = StartCoroutine(CicloDeAtaque());
    }

    private void DetenerAtaques()
    {
        if (_rutinaAtaque != null)
        {
            StopCoroutine(_rutinaAtaque);
            _rutinaAtaque = null;
        }
    }

    private IEnumerator CicloDeAtaque()
    {
        while (!_modoPuzzleActivo)
        {
            if (Time.time >= _proximoAtaque)
            {
                EjecutarAtaqueAleatorio();
                _proximoAtaque = Time.time + TiempoEntreAtaques;
            }
            yield return null;
        }
    }

    private void EjecutarAtaqueAleatorio()
    {
        TipoAtaque ataque = (TipoAtaque)Random.Range(0, 3);
        switch (ataque)
        {
            case TipoAtaque.Estalactitas:
                StartCoroutine(AtaqueEstalactitas());
                break;
            case TipoAtaque.DisparoGrande:
                AtaqueDisparoGrande();
                break;
            case TipoAtaque.Chorros:
                StartCoroutine(LanzarChorroDesdeSuelo());
                break;
        }
    }

    private IEnumerator AtaqueEstalactitas()
    {
        List<int> orden = Enumerable.Range(0, puntosEstalactitas.Length)
                                    .OrderBy(_ => Random.value)
                                    .ToList();

        foreach (int i in orden)
        {
            var punto = puntosEstalactitas[i];
            GameObject estalactita = Instantiate(estalactitaPrefab, punto.position, Quaternion.Euler(0, 0, 180f));
            estalactita.GetComponent<Estalactita>()?.CaerLuegoDe(1.2f);
            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
        }
    }

    private void AtaqueDisparoGrande()
    {
        Instantiate(disparoGrandePrefab, puntoDisparo.position, Quaternion.identity);
    }

    private IEnumerator LanzarChorroDesdeSuelo()
    {
        foreach (Transform punto in puntosChorro)
        {
            yield return StartCoroutine(MostrarIndicadorYEjecutarChorros(punto));
        }
    }

    private IEnumerator MostrarIndicadorYEjecutarChorros(Transform punto)
    {
        if (indicadorPrefab != null)
            Destroy(Instantiate(indicadorPrefab, punto.position, Quaternion.identity), TiempoAviso);

        yield return new WaitForSeconds(TiempoAviso);

        for (int i = 1; i <= 5; i++)
        {
            GameObject chorro = Instantiate(chorroPrefab, punto.position, Quaternion.identity);
            float angulo = i * 30f;
            float rad = angulo * Mathf.Deg2Rad;
            Vector2 direccion = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            chorro.GetComponent<ChorroDisparo>()?.Inicializar(direccion, 1.3f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    // --- VIDA / TRACES ---
    public void Golpe()
    {
        RecibirDano();
    }

    private void RecibirDano()
    {
        if (_modoPuzzleActivo) return;

        _salud--;

        if (_salud <= UmbralVidaPuzzle && !_modoPuzzleActivo)
        {
            IniciarModoPuzzle();
            AsignarTraceCorrecto(1); // config predeterminada
            return;
        }

        if (_salud <= SaludMinima)
        {
            MatarJefe();
        }
    }

    private void IniciarModoPuzzle()
    {
        _modoPuzzleActivo = true;
        DetenerAtaques();

        foreach (var trace in traceOpciones)
            trace.SetActive(true);
    }

    private void DesactivarTraces()
    {
        foreach (var trace in traceOpciones)
            trace.SetActive(false);

        if (portalSiguienteNivel != null)
            portalSiguienteNivel.SetActive(false);
    }

    public void VerificarTrace(string idSeleccionado)
    {
        if (!_modoPuzzleActivo) return;

        if (idSeleccionado == _traceCorrectoIndex)
        {
            Debug.Log("¡Correcto! Jefe derrotado.");
            MatarJefe();
        }
        else
        {
            Debug.Log("Incorrecto. El jefe recupera vida.");
            _salud = Mathf.Min(_salud + 3, _saludMaxima);
            _modoPuzzleActivo = false;
            DesactivarTraces();
            IniciarAtaques();
        }
    }

    public void AsignarTraceCorrecto(int index)
    {
        _traceCorrectoIndex = index.ToString();
    }

    private void MatarJefe()
    {
        Debug.Log("Jefe eliminado.");
        if (portalSiguienteNivel != null)
            portalSiguienteNivel.SetActive(true);

        Destroy(gameObject);
    }
}
