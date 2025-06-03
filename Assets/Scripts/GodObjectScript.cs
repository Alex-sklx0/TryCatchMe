using System.Collections;
using UnityEngine;

public class GodObjectScript : MonoBehaviour, IDanable, IVerificadorTrace
{
    // --- ENUM DE ATAQUES ---
    private enum TipoAtaque
    {
        Perseguidor = 1,
        Suelo = 2,
        Techo = 3
    }

    // --- CONSTANTES ---
    private const float TiempoEntreAtaques = 4f;
    private const float TiempoAviso = 1f;
    private const int UmbralVidaPuzzle = 3;
    private const int SaludMinima = 0;

    // --- SERIALIZE ---
    [SerializeField] private Transform _cristianPosicion;
    [SerializeField] private GameObject _disparoPerseguidorPrefab;
    [SerializeField] private GameObject _explosionSueloPrefab;
    [SerializeField] private GameObject _ataqueTechoPrefab;
    [SerializeField] private GameObject _indicadorPrefab;
    [SerializeField] private Transform[] _puntosSuelo;
    [SerializeField] private Transform[] _puntosTecho;
    [SerializeField] private GameObject[] _traceOpciones;
    [SerializeField] private GameObject _portalSiguienteNivel;

    // --- PRIVADAS ---
    private float _proximoAtaque;
    private int _salud = 10;
    private int _saludMaxima = 10;
    private bool _modoPuzzleActivo = false;
    private string _traceCorrectoIndex = "1";
    private Coroutine _rutinaAtaque;

    // --- UNITY EVENTS ---
    private void Start()
    {
        DesactivarTraces();
        IniciarAtaques();
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
        TipoAtaque ataque = (TipoAtaque)Random.Range(1, 4);

        switch (ataque)
        {
            case TipoAtaque.Perseguidor:
                LanzarPerseguidor();
                break;
            case TipoAtaque.Suelo:
                EjecutarExplosionesDesdePuntos(_puntosSuelo, _explosionSueloPrefab);
                break;
            case TipoAtaque.Techo:
                EjecutarExplosionesDesdePuntos(_puntosTecho, _ataqueTechoPrefab);
                break;
        }
    }

    // --- ATAQUES ---
    private void LanzarPerseguidor()
    {
        if (_disparoPerseguidorPrefab == null || _cristianPosicion == null) return;

        GameObject disparo = Instantiate(_disparoPerseguidorPrefab, transform.position, Quaternion.identity);
        disparo.GetComponent<DisparoPerseguidorScript>()?.Iniciar(_cristianPosicion);
    }

    private void EjecutarExplosionesDesdePuntos(Transform[] puntos, GameObject prefabAtaque)
    {
        foreach (Transform punto in puntos)
        {
            StartCoroutine(MostrarIndicadorYEjecutar(punto, prefabAtaque));
        }
    }

    private IEnumerator MostrarIndicadorYEjecutar(Transform punto, GameObject prefabAtaque)
    {
        if (_indicadorPrefab != null)
        {
            GameObject indicador = Instantiate(_indicadorPrefab, punto.position, Quaternion.identity);
            yield return new WaitForSeconds(TiempoAviso);
            Destroy(indicador);
        }

        if (prefabAtaque != null)
        {
            Instantiate(prefabAtaque, punto.position, Quaternion.identity);
        }
    }

    // --- TRACES ---
    private void DesactivarTraces()
    {
        foreach (GameObject trace in _traceOpciones)
            trace.SetActive(false);
        _portalSiguienteNivel.SetActive(false); // Mostrar portal
    }

    private void IniciarModoPuzzle()
    {
        _modoPuzzleActivo = true;
        DetenerAtaques();

        foreach (GameObject trace in _traceOpciones)
            trace.SetActive(true);
    }

    public void VerificarTrace(string idSeleccionado)
    {
        if (!_modoPuzzleActivo) return;

        if (idSeleccionado == _traceCorrectoIndex)
        {
            Debug.Log("¡Correcto! El jefe ha sido destruido.");
            foreach (GameObject trace in _traceOpciones)
            trace.SetActive(false);
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

    // --- VIDA / MUERTE ---
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
            AsignarTraceCorrecto(1); // Configura cuál trace es el correcto
            return;
        }

        if (_salud <= SaludMinima)
        {
            MatarJefe();
        }
    }

    private void MatarJefe()
    {
        Debug.Log("El jefe ha muerto.");
        if (_portalSiguienteNivel != null)
        _portalSiguienteNivel.SetActive(true); // Mostrar portal
        Destroy(gameObject);
    }
}
