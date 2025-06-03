using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristianMovimiento : MonoBehaviour
{
    //constantes
    private const float CooldownDisparo = 0.5f;
    private const float DistanciaRaycast = 0.12f;
    private const float EscalaEjesSprite = 1.0f; //constante para cuando se gira el srite en direccion y,z
    private const int SaludMin = 0;

    //serialized y variables publicas
    [SerializeField] private GameObject _disparoPrefab;
    [SerializeField] private Animator _animator;

    //variables privadas
    private float _horizontal;
    private float _velocidad = 0.8f;
    private float _velocidadBase;

    private float _fuerzaSalto = 130f;

    private bool _saltoBloqueado = false;
    private int _saltosNecesarios = 0;
    private int _saltosRealizados = 0;

    private SpriteRenderer _spriteRenderer;
    private bool _disparosBloqueados = false;
    private float tiempoUltimoDisparo;
    private bool _atorado = false;
    private Rigidbody2D _rigidbody2D;

    private bool _tocaSuelo;
    private float _ultimoDisparo;
    private float _cooldownDisparo = 0.5f;
    private float _offsetDisparo = 0.12f;
    private float _salud = 5;
    public float Salud
    {
        get
        {
            return _salud;
        }
    }

    private void Start()
    {
        _velocidadBase = _velocidad; // Guarda la velocidad original
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
        if (_atorado)
        {
            // El jugador no puede moverse lateralmente mientras este atorado
            _rigidbody2D.linearVelocity = new Vector2(0f, _rigidbody2D.linearVelocity.y);
        }
        else
        {
            _rigidbody2D.linearVelocity = new Vector2(_horizontal * _velocidad, _rigidbody2D.linearVelocity.y);
        }
    }
    private void ActualizarOrientacion()
    {
        // Movimiento
        _horizontal = Input.GetAxisRaw("Horizontal");

        // Rotación del sprite
        if (_horizontal < 0.0f) transform.localScale = new Vector3(-EscalaEjesSprite, EscalaEjesSprite, EscalaEjesSprite);
        else if (_horizontal > 0.0f) transform.localScale = new Vector3(EscalaEjesSprite, EscalaEjesSprite, EscalaEjesSprite);

        // Animación de correr
        _animator.SetBool("corriendo", _horizontal != 0.0f);

    }
    private void DetectarSuelo()
    {
        // Detección de suelo
        Vector3 origenCentro = transform.position;
        Vector3 origenIzquierda = transform.position + Vector3.left * 0.035f;
        Vector3 origenDerecha = transform.position + Vector3.right * 0.035f;

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
    return _tocaSuelo && !_saltoBloqueado; // <-- se elimina el !_atorado
}


    private void Saltar()
    {
        _rigidbody2D.AddForce(Vector2.up * _fuerzaSalto);
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
        return Time.time > _ultimoDisparo + _cooldownDisparo && !_disparosBloqueados;
    }

    private void Disparar()
    {
        Vector3 direccion = transform.localScale.x > 0 ? Vector3.right : Vector3.left;
        Instantiate(_disparoPrefab, transform.position + direccion * _offsetDisparo, Quaternion.identity)
            .GetComponent<DisparoCristian>().Direccion = direccion;

        _ultimoDisparo = Time.time;
        _animator.SetTrigger("disparar");
    }

    private void VerificarCaida()
    {
        if (transform.position.y < -0.5f && _salud > 0)
        {
            Golpe(_salud);
        }
    }




    public void RecibirDano(float dano)
    {
        _salud -= dano;

        if (_salud <= 0)
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
        Destroy(gameObject, 4f);
        //FindAnyObjectByType<ControlGameover>().MostrarGameOver();
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
            _saltosNecesarios = 3;
            _saltosRealizados = 0;
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
        _saltosRealizados = 0;
        _saltosNecesarios = 0;
    }
}