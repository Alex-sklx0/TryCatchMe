using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CrashtianScript : MonoBehaviour, IDanable, IVerificadorTrace
{
    private enum TipoAtaque { Estalactitas = 0, DisparoGrande = 1, Chorros = 2 }

    // --- CONSTANTES ---
    private const float TiempoEntreAtaques = 3f;
    private const float TiempoAviso = 1f;
    private const int UmbralVidaPuzzle = 3;
    private const int SaludMinima = 0;
    private const float RetrasoCaidaEstalactita = 1.2f;
    private const float MinEsperaEstalactita = 0.2f;
    private const float MaxEsperaEstalactita = 0.5f;
    private const int CantidadDisparosChorro = 5;
    private const float EsperaEntreDisparosChorro = 0.2f;
    private const float AnguloBaseChorro = 30f;
    private const float FuerzaDisparoChorro = 1.3f;
    private const int TraceCorrectoPorDefecto = 1;
    private const int AdicionSalud = 10;

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
    private int _salud = 20;
    private int _saludMaxima;
    private bool _modoPuzzleActivo = false;
    private string _traceCorrectoIndex = TraceCorrectoPorDefecto.ToString();
    private Coroutine _rutinaAtaque;

    private void Start()
    {
        try
        {
            _saludMaxima = _salud;
            _jugador = GameObject.FindGameObjectWithTag("Player");
            DesactivarTraces();
            IniciarAtaques();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[CrashtianScript] Error en Start: {e.Message}", this);
        }
    }

    private void Update()
    {
        try
        {
            if (_jugador != null)
            {
                Vector3 direccion = (_jugador.transform.position - transform.position).normalized;
                transform.localScale = new Vector3(direccion.x >= 0 ? 1f : -1f, 1f, 1f);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[CrashtianScript] Error en Update: {e.Message}", this);
        }
    }

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
        try
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
        catch (System.Exception e)
        {
            Debug.LogError($"[CrashtianScript] Error ejecutando ataque: {e.Message}", this);
        }
    }

    private IEnumerator AtaqueEstalactitas()
    {
        List<int> orden = Enumerable.Range(0, puntosEstalactitas.Length).OrderBy(_ => Random.value).ToList();

        foreach (int i in orden)
        {
            Transform punto = puntosEstalactitas[i];
            try
            {
                GameObject estalactita = Instantiate(estalactitaPrefab, punto.position, Quaternion.Euler(0, 0, 180f));
                Estalactita estalactitaScript = estalactita.GetComponent<Estalactita>();
                if (estalactitaScript != null)
                {
                    estalactitaScript.CaerLuegoDe(RetrasoCaidaEstalactita);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[CrashtianScript] Error instanciando estalactita: {e.Message}", this);
            }

            yield return new WaitForSeconds(Random.Range(MinEsperaEstalactita, MaxEsperaEstalactita));
        }
    }

    private void AtaqueDisparoGrande()
    {
        try
        {
            Instantiate(disparoGrandePrefab, puntoDisparo.position, Quaternion.identity);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[CrashtianScript] Error disparo grande: {e.Message}", this);
        }
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
        {
            try
            {
                GameObject indicador = Instantiate(indicadorPrefab, punto.position, Quaternion.identity);
                Destroy(indicador, TiempoAviso);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[CrashtianScript] Error mostrando indicador: {e.Message}", this);
            }
        }

        yield return new WaitForSeconds(TiempoAviso);

        for (int i = 1; i <= CantidadDisparosChorro; i++)
        {
            try
            {
                GameObject chorro = Instantiate(chorroPrefab, punto.position, Quaternion.identity);
                float angulo = i * AnguloBaseChorro;
                float rad = angulo * Mathf.Deg2Rad;
                Vector2 direccion = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                ChorroDisparo script = chorro.GetComponent<ChorroDisparo>();
                if (script != null)
                {
                    script.Inicializar(direccion, FuerzaDisparoChorro);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[CrashtianScript] Error lanzando chorro: {e.Message}", this);
            }
            yield return new WaitForSeconds(EsperaEntreDisparosChorro);
        }
    }

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
            AsignarTraceCorrecto(TraceCorrectoPorDefecto);
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

        foreach (GameObject trace in traceOpciones)
        {
            try { trace.SetActive(true); }
            catch { Debug.LogWarning("[CrashtianScript] Trace no válido"); }
        }
    }

    private void DesactivarTraces()
    {
        foreach (GameObject trace in traceOpciones)
        {
            try { trace.SetActive(false); }
            catch { Debug.LogWarning("[CrashtianScript] Trace inválido en desactivación"); }
        }

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
            _salud += AdicionSalud;
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
