using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    public static UIScript instance;
    
    public GameObject panelInicio;
    public GameObject panelPausa;
    public GameObject panelGanar;
    public GameObject panelPerder;

    private bool isPaused = true;
    //public bool canvasBool = true;

    [SerializeField] private GameObject enemigosParent;
    private int enemigosActivos;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        Time.timeScale = 0;
        panelPausa.SetActive(false);
        enemigosActivos = enemigosParent.transform.childCount;
    }
    // Update is called once per frame
    void Update()
    {
        if (isPaused == false)
        {
            Time.timeScale = 1;
            /*Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;*/
        }
        if (isPaused == true)
        {
            Time.timeScale = 0;
            /*Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;*/
        }
        // Detectar si se presiona la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        {
            panelPausa.SetActive(true); // Mostrar el Panel
            isPaused = true; // Alternar estado (activar)
        }
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
        {
            panelPausa.SetActive(false);
            isPaused = false;
        }
    }

    public void SeClickaBotonDeJugar()
        {
            panelInicio.SetActive(false);
            isPaused = false;
            //canvasBool = false;
        }

    public void PartidaPerdida()
    {
        Cursor.lockState = CursorLockMode.None;
        panelPerder.SetActive(true);
    }

    public void PartidaGanada()
    {
        Cursor.lockState = CursorLockMode.None;
        panelGanar.SetActive(true);
    }

    public void ReiniciarJuego()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void SalirDeJuego()
    {
        Application.Quit();
    }

    public void RestarEnemigo()
    {
        enemigosActivos--;

        if (enemigosActivos <= 0)
        {
            PartidaGanada();
        }
    }
}
