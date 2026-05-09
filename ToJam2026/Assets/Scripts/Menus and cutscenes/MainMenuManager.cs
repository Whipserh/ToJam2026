using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainMenu, CreditMenu;

    void Start()
    {
        backBtn();
    }

    //TO BE TESTED
    public void startBtn()
    {
        SceneManager.LoadScene(1);
    }

    //TO BE TESTED
    public void quitBtn()
    {
        Application.Quit();
    }


    public void creditsBtn()
    {
        MainMenu.SetActive(false);
        CreditMenu.SetActive(true);
    }

    public void backBtn()
    {
        MainMenu.SetActive(true);
        CreditMenu.SetActive(false);
    }
}
