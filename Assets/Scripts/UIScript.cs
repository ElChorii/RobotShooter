using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    public GameObject panelInicio;
    public GameObject panelPausa;
    private bool isPaused = true;
    //public bool canvasBool = true;

    private void Start()
    {
        Time.timeScale = 0;
        panelPausa.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (isPaused == false)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (isPaused == true)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
}
