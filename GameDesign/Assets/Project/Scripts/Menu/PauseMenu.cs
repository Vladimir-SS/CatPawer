using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private PauseAction action;
    public static bool GameIsPaused = false;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject menuCamera;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject playerCanvas;

    private void Awake()
    {
        action = new PauseAction();
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    private void Start()
    {
        action.Pause.PauseGame.performed += _ => DeterminePause();
    }

    public void DeterminePause()
    {
        if (GameIsPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;

        playerCanvas.SetActive(true);

        mainCamera.SetActive(true);
        menuCamera.SetActive(false);
    }

    private void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;

        playerCanvas.SetActive(false);

        mainCamera.SetActive(false);
        menuCamera.SetActive(true);

    }
}
