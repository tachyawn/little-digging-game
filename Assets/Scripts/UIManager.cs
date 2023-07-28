using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
//using UnityEngine.UI;
//using TMPro;

public class UIManager : MonoBehaviour
{
    public static bool _isPaused = false;

    public Menu _activeMenu;
    List<Menu> _menuList = new List<Menu>();
    //List<Text> _textList = new List<Text>(); //Used to update all text across menus
    Player _player;

    [SerializeField] bool _autoSetDefaultActive = false; //When changing menus, sets the default active if none else are

    private void Awake() {
        _player = ReInput.players.GetPlayer(PlayerController.playerId);
        _player.AddInputEventDelegate(Pause, UpdateLoopType.Update, "Pause");
    }

    void Start()
    {
        //Finds and adds all menus to the list
        foreach (var menu in GameObject.FindObjectsOfType<Menu>(true))//Change to FindObjectsOfType<Menu>(true)
        {
            _menuList.Add(menu);
        }

        if (_autoSetDefaultActive)
        {
            SetMenuActive(Menu.MenuType.Default);
        }
    }
    
    public void Pause(InputActionEventData data)
    {
        if (data.GetButtonDown())
        {
            SetMenuActive(Menu.MenuType.Pause);

            if (_activeMenu != null && _activeMenu._currentMenuType == Menu.MenuType.Pause)
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
    }

    public void SetMenuActive(Menu.MenuType setType)
    {
        //If there is an active menu other than the one being changed, deactivate it.
        if (_activeMenu != null && _activeMenu._currentMenuType != setType) _activeMenu.gameObject.SetActive(false);

        //Finds and changes the active state of the requested menu
        foreach(Menu menu in _menuList)
        {
            if (menu._currentMenuType == setType) menu.gameObject.SetActive(!menu.gameObject.activeSelf);
            else menu.gameObject.SetActive(false);
        }
        GetActiveMenu(); //Updates _activeMenu

        //Situation could arise where no menus are active at all, so:
        if (_autoSetDefaultActive && _activeMenu == null) SetMenuActive(Menu.MenuType.Default);
    }

    public bool GetActiveMenu()
    {
        //Returns the index of the active menu
        //If there are multiple, will only return the last one checked
        int index = -1;
        _activeMenu = null;

        for (int i = 0; i < _menuList.Count; i++)
        {
            if (_menuList[i].gameObject.activeSelf)
            {
                index = i;
            }
        }
        if (index != -1)
        {
            _activeMenu = _menuList[index];

            if (_activeMenu._currentMenuType == Menu.MenuType.Pause) _isPaused = true;
            else _isPaused = false;

            return true;
        }

        _isPaused = false; //Cannot be paused if no menu is active
        return false;
    }
}
