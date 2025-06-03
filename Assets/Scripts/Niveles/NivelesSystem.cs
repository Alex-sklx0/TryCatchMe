using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NivelesSystem : MonoBehaviour
{
    public void BtnN1()
    {
        try
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al intentar ir a al Nivel 1: {e.Message}");

        }
    }

    public void BtnN2()
    {
        try
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al intentar ir a al Nivel 2: {e.Message}");

        }
    }

    public void BtnN3()
    {
        try
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al intentar ir a al Nivel 3: {e.Message}");

        }
    }

    public void BtnVolver()
    {

        try
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al intentar ir al Menú principal: {e.Message}");

        }

    }
}
