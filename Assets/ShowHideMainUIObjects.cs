using UnityEngine;
using System.Collections;

public class ShowHideMainUIObjects : MonoBehaviour {


    public GameObject log;

    /// <summary>
    /// Shows the menu specified by a_menuToShow
    /// </summary>
    /// <param name="a_menuToShow"></param>
    public void ShowMenu(string a_menuToShow)
    {
        if (a_menuToShow == "log")
            log.SetActive(true);
    }


    /// <summary>
    /// Hides the menu specified by a_menuToShow
    /// </summary>
    /// <param name="a_menuToHide"></param>
    public void HideMenu(string a_menuToHide)
    {
        if (a_menuToHide == "log")
            log.SetActive(false);
    }

    /// <summary>
    /// Toggles the menu specified by a_menuToToggle to enable or disable
    /// </summary>
    /// <param name="a_menuToToggle"></param>
    public void ToggleMenu(string a_menuToToggle)
    {
        if (a_menuToToggle == "log")
            log.SetActive(!log.active);
    }
}
