using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristianMovimiento : MonoBehaviour
{
    // Constantes
    private const float CooldownDisparo = 0.5f;
    private const float DistanciaRaycast = 0.12f;
    private const float EscalaEjesSprite = 1.0f;
    private const float VelocidadInicial = 0.8f;
    private const float FuerzaSalto = 130f;
    private const float OffsetDisparo = 0.12f;
    private const float PosicionCaidaMuerte = -0.5f;
    private const float TiempoDestruccion = 4f;
    private const int SaludInicial = 5;
    private const int SaludMin = 0;
    private const int SaltosParaLiberarse = 3;
    private const float ConstanteDeteccionSuelo = 0.035f;
    private const int ReferenciaCero = 0;
    private const int CeroSaltos = 0;
    

    // Serialized y públicas
    [SerializeField] private GameObject _disparoPrefab;
    [SerializeField] private Animator _animator;

    // Privadas
    private float _horizontal;
    private float _velocidad = VelocidadInicial;
    private float _velocidadBase;

    private bool _saltoBloqueado = false;
    private int _saltosNecesarios = 0;
    private int _saltosRealizados = 0;

    private SpriteRenderer _spriteRenderer;
    private bool _disparosBloqueados = false;
    private float _tiempoUltimoDisparo;
    private bool _atorado = false;
    private Rigidbody2D _rigidbody2D;

    private bool _tocaSuelo;
    private float _ultimoDisparo;
    private float _salud = SaludInicial;

    public float Salud => _salud;

    private void Start()
    {
        _velocidadBase = _velocidad;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        ActualizarOrientacion();
        DetectarSuelo();
        ProcesarSalto();
        ProcesarDisparo();
        VerificarCaida();
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = _atorado
            ? new Vector2(ReferenciaCero, _rigidbody2D.linearVelocity.y)
            : new Vector2(_horizontal * _velocidad, _rigidbody2D.linearVelocity.y);
    }

    private void ActualizarOrientacion()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");

        if (_horizontal < ReferenciaCero)
            transform.localScale = new Vector3(-EscalaEjesSprite, EscalaEjesSprite, EscalaEjesSprite);
        else if (_horizontal > ReferenciaCero)
            transform.localScale = new Vector3(EscalaEjesSprite, EscalaEjesSprite, EscalaEjesSprite);

        _animator.SetBool("corriendo", _horizontal != ReferenciaCero);
    }

    private void DetectarSuelo()
    {
        Vector3 origenCentro = transform.position;
        Vector3 origenIzquierda = transform.position + Vector3.left * ConstanteDeteccionSuelo;
        Vector3 origenDerecha = transform.position + Vector3.right * ConstanteDeteccionSuelo;

        Debug.DrawRay(origenCentro, Vector3.down * DistanciaRaycast, Color.red);
        Debug.DrawRay(origenIzquierda, Vector3.down * DistanciaRaycast, Color.red);
        Debug.DrawRay(origenDerecha, Vector3.down * DistanciaRaycast, Color.red);

        _tocaSuelo =
            Physics2D.Raycast(origenCentro, Vector3.down, DistanciaRaycast) ||
            Physics2D.Raycast(origenIzquierda, Vector3.down, DistanciaRaycast) ||
            Physics2D.Raycast(origenDerecha, Vector3.down, DistanciaRaycast);
    }

    private void ProcesarSalto()
    {
        if (Input.GetKeyDown(KeyCode.W) && PuedeSaltar())
        {
            Saltar();
            ContarSaltosSiAtorado();
        }
    }

    private bool PuedeSaltar()
    {
        return _tocaSuelo && !_saltoBloqueado;
    }

    private void Saltar()
    {
        _rigidbody2D.AddForce(Vector2.up * FuerzaSalto);
        _animator.SetTrigger("saltar");
    }

    private void ContarSaltosSiAtorado()
    {
        if (_atorado)
        {
            _saltosRealizados++;
            Debug.Log($"Saltos realizados: {_saltosRealizados}/{_saltosNecesarios}");
        }
    }

    public void BloquearDisparos(bool estado)
    {
        _disparosBloqueados = estado;
    }

    private void ProcesarDisparo()
    {
        if (Input.GetKey(KeyCode.Space) && PuedeDisparar())
        {
            Disparar();
        }
    }

    private bool PuedeDisparar()
    {
        return Time.time > _ultimoDisparo + CooldownDisparo && !_disparosBloqueados;
    }

    private void Disparar()
    {
        Vector3 direccion = transform.localScale.x > ReferenciaCero ? Vector3.right : Vector3.left;
        GameObject disparo = Instantiate(_disparoPrefab, transform.position + direccion * OffsetDisparo, Quaternion.identity);
        DisparoCristian script = disparo.GetComponent<DisparoCristian>();
        if (script != null) script.Direccion = direccion;

        _ultimoDisparo = Time.time;
        _animator.SetTrigger("disparar");
    }

    private void VerificarCaida()
    {
        if (transform.position.y < PosicionCaidaMuerte && _salud > 0)
        {
            Golpe(_salud);
        }
    }

    public void RecibirDano(float dano)
    {
        _salud -= dano;

        if (_salud <= SaludMin)
        {
            Morir();
        }
        else
        {
            _animator.SetTrigger("golpe");
        }
    }

    private void Morir()
    {
        _animator.SetTrigger("morir");
        Destroy(gameObject, TiempoDestruccion);
    }

    public void Golpe(float dano)
    {
        RecibirDano(dano);
    }

    public void AplicarAtoramiento()
    {
        if (!_atorado)
        {
            _atorado = true;
            _saltosNecesarios = SaltosParaLiberarse;
            _saltosRealizados = CeroSaltos;
            StartCoroutine(AtoradoCoroutine());
            Debug.Log("¡Estás aturdido! Salta 3 veces para liberarte");
        }
    }

    private IEnumerator AtoradoCoroutine()
    {
        Color originalColor = _spriteRenderer.color;
        _spriteRenderer.color = Color.yellow;

        while (_saltosRealizados < _saltosNecesarios)
        {
            yield return null;
        }

        _atorado = false;
        _spriteRenderer.color = originalColor;
        ResetearContadorSaltos();
    }

    public void AplicarRalentizacion(float factor, float duracion)
    {
        StopAllCoroutines();
        StartCoroutine(EfectoRalentizacionCoroutine(factor, duracion));
    }

    private IEnumerator EfectoRalentizacionCoroutine(float factor, float duracion)
    {
        _velocidad = _velocidadBase * factor;
        _spriteRenderer.color = Color.blue;

        yield return new WaitForSeconds(duracion);

        _velocidad = _velocidadBase;
        _spriteRenderer.color = Color.white;
    }

    public void BloquearSalto(float duracion)
    {
        if (!_saltoBloqueado)
        {
            StartCoroutine(BloqueoSaltoCoroutine(duracion));
        }
    }

    private IEnumerator BloqueoSaltoCoroutine(float duracion)
    {
        _saltoBloqueado = true;
        Color originalColor = _spriteRenderer.color;
        _spriteRenderer.color = Color.magenta;

        yield return new WaitForSeconds(duracion);

        _saltoBloqueado = false;
        _spriteRenderer.color = originalColor;
    }

    private void ResetearContadorSaltos()
    {
        _saltosRealizados = CeroSaltos;
        _saltosNecesarios = CeroSaltos;
    }
}
