using System.Collections;
using UnityEngine;

public class AcidoScript : MonoBehaviour
{
    public int dano = 1;
    public float intervaloDano = 1f;
    public float duracion = 4f;

    private float _tiempoUltimoDano;
    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, duracion);
        StartCoroutine(Desvanecer());
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time >= _tiempoUltimoDano + intervaloDano)
        {
            other.GetComponent<CristianMovimiento>()?.Golpe(dano);
            _tiempoUltimoDano = Time.time;
        }
    }

    private IEnumerator Desvanecer()
    {
        yield return new WaitForSeconds(duracion - 1f);
        
        float tiempo = 1f;
        Color colorInicial = _renderer.color;
        
        while (tiempo > 0)
        {
            _renderer.color = new Color(colorInicial.r, colorInicial.g, colorInicial.b, tiempo);
            tiempo -= Time.deltaTime;
            yield return null;
        }
    }
}