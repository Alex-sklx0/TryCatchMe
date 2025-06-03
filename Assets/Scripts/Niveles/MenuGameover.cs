using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameover : MonoBehaviour
{
    //Botones:
    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

   
}
