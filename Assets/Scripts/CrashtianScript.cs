using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CrashtianScript : MonoBehaviour, IDanable
{
    public GameObject estalactitaPrefab;
    public Transform[] puntosEstalactitas;

    public GameObject disparoGrandePrefab;
    public Transform puntoDisparo;

    [SerializeField] private GameObject DisparoCodigoPrefab;
    [SerializeField] private Transform[] puntosChorro;
    [SerializeField] private GameObject indicadorPrefab;
    [SerializeField] private GameObject _portalSiguienteNivel;

    public float intervaloEntreAtaques = 3f;
    private float _proximoAtaque;
    private float _salud = 30f;

    private void Update()
    {
        GameObject cristianObj = GameObject.FindGameObjectWithTag("Player");
        if (cristianObj != null)
        {
            Vector3 direccion = (cristianObj.transform.position - transform.position).normalized;
            if (direccion.x >= 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            else transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }

        if (Time.time >= _proximoAtaque)
        {
            ElegirYRealizarAtaque();
            _proximoAtaque = Time.time + intervaloEntreAtaques;
        }
    }

    private void ElegirYRealizarAtaque()
    {
        int ataque = Random.Range(0, 3); // 0 = estalactitas, 1 = disparo grande, 2 = chorros

        switch (ataque)
        {
            case 0:
                StartCoroutine(AtaqueEstalactitas()); // ✅ ahora sí es una corutina
                break;
            case 1:
                AtaqueDisparoGrande();
                break;
            case 2:
                StartCoroutine(LanzarChorroDesdeSuelo());
                break;
        }
    }

    // ✅ ATAQUE 1: ESTALACTITAS (cambiado de void a IEnumerator)
    private IEnumerator AtaqueEstalactitas()
    {
        List<int> indices = new List<int> { 0, 1, 2, 3, 4 };
        indices = indices.OrderBy(x => Random.value).ToList();

        yield return StartCoroutine(LanzarEstalactitasEnOrden(indices));
    }

   private IEnumerator LanzarEstalactitasEnOrden(List<int> orden)
{
    foreach (int i in orden)
    {
        Transform punto = puntosEstalactitas[i];

        //  Instancia con rotación de 180 grados en Z
        GameObject estalactita = Instantiate(estalactitaPrefab, punto.position, Quaternion.Euler(0, 0, 180f));

        //  Activar caída después de 1.2 segundos
        estalactita.GetComponent<EstalactitaScript>()?.CaerLuegoDe(1.2f);

        yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
    }
}


    // ATAQUE 2: DISPARO GRANDE
    private void AtaqueDisparoGrande()
    {
        GameObject disparo = Instantiate(disparoGrandePrefab, puntoDisparo.position, Quaternion.identity);
        Rigidbody2D rb = disparo.GetComponent<Rigidbody2D>();
    }

    // ATAQUE 3: CHORRO DE CÓDIGO DESDE EL SUELO
    private IEnumerator LanzarChorroDesdeSuelo()
{
    foreach (Transform punto in puntosChorro)
    {
        // 👇 Muestra alerta antes de iniciar los 5 chorros
        yield return StartCoroutine(MostrarIndicadorYEjecutar(punto));
    }
}
    private IEnumerator MostrarIndicadorYEjecutar(Transform punto)
    {
        GameObject indicador = Instantiate(indicadorPrefab, punto.position, Quaternion.identity);

        yield return new WaitForSeconds(1f); // Tiempo para que el jugador vea la alerta

        Destroy(indicador);

        for (int i = 1; i < 6; i++) // 5 disparos por punto
        {
            GameObject chorro = Instantiate(DisparoCodigoPrefab, punto.position, Quaternion.identity);

            float angulo = i * 30f;
            float fuerza = 1.3f;

            float rad = angulo * Mathf.Deg2Rad;
            Vector2 direccion = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            chorro.GetComponent<ChorroDisparoScript>()?.Inicializar(direccion, fuerza);

            yield return new WaitForSeconds(0.2f); // Espera entre disparos
        }
    }
  public void Golpe()
    {
        _salud --;
        if (_salud <= 0) Destroy(gameObject);
    }

}
