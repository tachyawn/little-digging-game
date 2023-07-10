using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static bool _isPaused = false;

    List<GameObject> _menuList = new List<GameObject>();
    //List<Text> _textList = new List<Text>(); //Used to update all text across menus

    [SerializeField] GameObject _defaultMenu;
    [SerializeField] GameObject _defaultDigMenu;
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] bool _autoSetDefaultActive = false; //When changing menus, sets the default active if none else are
    GameObject _activeMenu;

    void Start()
    {
        //Finds and adds all menus to the list
        foreach (var menu in GameObject.FindGameObjectsWithTag("Menu"))//Change to FindObjectsOfType<Menu>(true)
        {
            _menuList.Add(menu.gameObject);
            //menu.gameObject.SetActive(false);
        }
    }
    
    public void Pause()
    {
        SetMenuActive(_pauseMenu.name);

        if (_activeMenu != null)
        {
            _isPaused = true;
            Time.timeScale = 0;
            PlayerController.inPlay = false;
        }
        else
        {
            _isPaused = false;
            Time.timeScale = 1;
            PlayerController.inPlay = true;
        }
    }

    private void SetMenuActive(string menuName)
    {
        //If there is an active menu other than the one being changed, deactivate it.
        if (_activeMenu != null && _activeMenu.name != menuName) _activeMenu.SetActive(false);

        //Finds and changes the active state of the requested menu
        foreach (var menu in _menuList)
        {
            if (menu.name == menuName) menu.SetActive(!menu.activeSelf);
        }
        GetActiveMenu(); //Updates _activeMenu

        //Situation could arise where no menus are active at all, so:
        if (_autoSetDefaultActive && _activeMenu == null) SetMenuActive(_defaultMenu.name);
    }

    private bool GetActiveMenu()
    {
        //Returns the index of the active menu
        //If there are multiple, will only return the last one checked
        int index = -1;
        _activeMenu = null;

        for (int i = 0; i < _menuList.Count; i++)
        {
            if (_menuList[i].activeSelf)
            {
                index = i;
            }
        }
        if (index != -1)
        {
            _activeMenu = _menuList[index];

            if (_activeMenu == _pauseMenu) _isPaused = true;
            else _isPaused = false;

            return true;
        }

        _isPaused = false; //Cannot be paused if no menu is active
        return false;
    }
}
