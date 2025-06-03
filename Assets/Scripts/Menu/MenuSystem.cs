using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void BtnJugar()
    {
        try
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Pasa a la siguiente escena donde estan los niveles
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al intentar ir a ala siguiente escena: {e.Message}");

        }

    }
    public void BtnSalir()
    {
        try
        {
            Debug.Log("Saliendo del juego...");
            Application.Quit();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al intentar cerrar la aplicación: {e.Message}");
        }
    }


}
