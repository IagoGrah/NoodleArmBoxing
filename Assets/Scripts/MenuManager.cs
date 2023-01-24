using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject activeMenu;

    public void GoToMenu(GameObject go)
    {
        activeMenu.SetActive(false);
        go.SetActive(true);
        activeMenu = go;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
