using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Panels : MonoBehaviour
{
    [SerializeField]
    GameObject pausedMenu;

    [SerializeField]
    GameObject start;

    [SerializeField]
    GameObject gameOver;


    void Start()
    {

    }

    void Update()
    {
        // paused Menue (Escape)
        if (Input.GetKey(KeyCode.Escape))
        {
            Time.timeScale = 0;
            pausedMenu.SetActive(true);
        }
    }

    // resume button
    public void resume()
    {
        pausedMenu.SetActive(false);
        Time.timeScale = 1;
    }

    //Quite button
    public void quite()
    {
        pausedMenu.SetActive(false);
        Time.timeScale = 1;
        start.SetActive(true);
    }   

    // start menu
    public void startGame()
    {
        start.SetActive(false);
        SceneManager.LoadScene("SampleScene");
        start.SetActive(false);
    }

    public void closeStartPanel()
    {
        start.SetActive(false);
        gameOver.SetActive(false);
    }

    public void replay()
    {
        gameOver.SetActive(false);
        SceneManager.LoadScene("SampleScene");
        closeStartPanel();
    }

}
