using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private PauseAction action;
    public static bool GameIsPaused = false;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject menuCamera;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private GameObject tryAgainMenu;

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
        menuCanvas.SetActive(false);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;

        playerCanvas.SetActive(true);

        mainCamera.SetActive(true);
        menuCamera.SetActive(false);
    }

    private void PauseGame()
    {
        menuCanvas.SetActive(true);
        pauseMenuUI.SetActive(true);
        tryAgainMenu.SetActive(false);
        Time.timeScale = 0;
        GameIsPaused = true;

        playerCanvas.SetActive(false);

        mainCamera.SetActive(false);
        menuCamera.SetActive(true);

    }

    public void TryAgainMenu()
    {
        action.Pause.PauseGame.Disable();
        PauseGame();
        pauseMenuUI.SetActive(false);
        tryAgainMenu.SetActive(true);
    }
}
