using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public GameObject jugador;
    // Nombre de la escena de carga


    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CargarEscenaCarga();
        }
    }
    private void CargarEscenaCarga()
    {
        gameObject.SetActive(true);
        Time.timeScale = 1;
        SceneManager.LoadScene(1); // Carga la escena de carga
                                   // Reinicia el jugador
        if (Controller_Player._Player != null)
        {
            Controller_Player._Player.ResetPlayer();
        }
    }
}