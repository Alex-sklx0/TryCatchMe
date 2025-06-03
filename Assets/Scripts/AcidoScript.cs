using System.Collections;
using UnityEngine;

public class AcidoScript : MonoBehaviour
{
    // constantes
    private const string TagJugador = "Player";
    private const int TiempoDesvanecimiento = 1;
    private const int Dano = 1;
    private const int IntervaloDano = 1;
    private const int Duracion = 4;
    private const int TiempoMinimo = 0;


    // privada
    private float _tiempoUltimoDano;
    private SpriteRenderer _spriteRenderer;
    private CristianMovimiento _cristianScript;

    private void Start()
    {
        try
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
                throw new MissingComponentException("SpriteRenderer no se encontró en el objeto.");

            StartCoroutine(DesvanecerGradualmente());
            Autodestruir(Duracion);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[AcidoScript] Error en Start: {ex.Message}");
        }
    }

    private void Autodestruir(float tiempo)
    {
        try
        {
            if (tiempo <= TiempoMinimo)
                throw new System.ArgumentOutOfRangeException(nameof(tiempo), "La duración debe ser mayor a 0.");
            Destroy(gameObject, tiempo);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[AcidoScript] Error al destruir: {ex.Message}");
        }
    }

    private bool PuedeAplicarDano()
    {
        return Time.time >= _tiempoUltimoDano + IntervaloDano;
    }

    private void AplicarDano(Collider2D objetivo)
    {
        try
        {
            var cristian = objetivo.GetComponent<CristianMovimiento>();
            if (cristian == null)
                throw new MissingComponentException("CristianMovimiento no se encontró en el jugador.");

            cristian.Golpe(Dano);
            _tiempoUltimoDano = Time.time;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[AcidoScript] Error al aplicar daño: {ex.Message}");
        }
    }

    private IEnumerator DesvanecerGradualmente()
    {
        yield return new WaitForSeconds(Duracion - TiempoDesvanecimiento);

        float tiempoRestante = TiempoDesvanecimiento;
        Color colorInicial = _spriteRenderer.color;

        while (tiempoRestante > TiempoMinimo)
        {
            try
            {
                float alpha = tiempoRestante / TiempoDesvanecimiento;
                _spriteRenderer.color = new Color(colorInicial.r, colorInicial.g, colorInicial.b, alpha);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[AcidoScript] Error durante el desvanecimiento: {ex.Message}");
                break;
            }

            tiempoRestante -= Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out _cristianScript) && PuedeAplicarDano())
            AplicarDano(collision);

    }
}