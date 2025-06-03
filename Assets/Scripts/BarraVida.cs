using UnityEngine;
using UnityEngine.UI;

public class BarraVidaScript : MonoBehaviour
{
    private const int CeroReferencia = 0;
    [SerializeField] private Image rellenoBarraVida; 
    private CristianMovimiento cristianMovimiento;
    private float vidaMaxima;
    private bool corazonesOcultos = false;

    void Start()
    {
        try
        {
            // 1. Verificar y asignar la referencia al Image
            if (rellenoBarraVida == null)
                throw new MissingReferenceException("No se asignó la imagen de relleno en el Inspector");

            // 2. Buscar al jugador
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj == null)
                throw new MissingReferenceException("No se encontró objeto con tag 'Player'");

            // 3. Obtener el componente
            cristianMovimiento = playerObj.GetComponent<CristianMovimiento>();
            if (cristianMovimiento == null)
                throw new MissingComponentException("El objeto 'Player' no tiene el componente CristianMovimiento");

            // 4. Guardar vida máxima
            vidaMaxima = cristianMovimiento.Salud;

            ActualizarBarraVida(); // Inicialización inmediata
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[BarraVidaScript] Error en Start: {e.Message}", this);
        }
    }

    void Update()
    {
        try
        {
            ActualizarBarraVida();

            if (cristianMovimiento == null && !corazonesOcultos)
            {
                rellenoBarraVida.gameObject.SetActive(false);
                corazonesOcultos = true;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[BarraVidaScript] Error en Update: {e.Message}", this);
        }
    }

    void ActualizarBarraVida()
    {
        try
        {
            if (rellenoBarraVida == null || cristianMovimiento == null) return;

            float saludActual = cristianMovimiento.Salud;

            if (saludActual <= CeroReferencia && !corazonesOcultos)
            {
                rellenoBarraVida.gameObject.SetActive(false);
                corazonesOcultos = true;
            }
            else if (saludActual > CeroReferencia && corazonesOcultos)
            {
                rellenoBarraVida.gameObject.SetActive(true);
                corazonesOcultos = false;
            }

            if (saludActual > CeroReferencia)
            {
                rellenoBarraVida.fillAmount = Mathf.Clamp01(saludActual / vidaMaxima);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[BarraVidaScript] Error en ActualizarBarraVida: {e.Message}", this);
        }
    }
}
