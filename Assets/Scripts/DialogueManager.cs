using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using Rewired;
using System;

public class DialogueManager : MonoBehaviour, IDataPersistence
{
    UIManager _uiManager;
    Player _player;

    [Header("Dialogue UI")]
    [SerializeField] TextMeshProUGUI _dialogueText;
    
    [Header("Choices UI")]
    [SerializeField] GameObject[] _choiceObjs;
    TextMeshProUGUI[] _choicesText;

    private Story _currentStory;

    public static DialogueManager Instance { get; private set; }
    public bool _dialogueIsPlaying { get; private set; }

    bool _visitedCurator = false;
    bool _visitedShop = false;


    //Singleton pattern
    private void Awake() 
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start() 
    {
        _player = ReInput.players.GetPlayer(PlayerController.playerId);
        _uiManager = GameObject.FindObjectOfType<UIManager>();

        _dialogueIsPlaying = false;

        _choicesText = new TextMeshProUGUI[_choiceObjs.Length];
        for (int i = 0; i < _choiceObjs.Length; i++)
        {
            _choicesText[i] = _choiceObjs[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    private void Update() 
    {
        if (!_dialogueIsPlaying) return;

        //Continue story with interact button
        if (_currentStory.currentChoices.Count == 0 && _player.GetButtonDown("Interact"))
        { 
            ContinueDialogueMode();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        PlayerController.inPlay = false;

        _currentStory = new Story(inkJSON.text);
        LoadStoryVariables();
        
        _dialogueIsPlaying = true;
        _uiManager.SetMenuActive(Menu.MenuType.Dialogue);
        ContinueDialogueMode();
    }

    private void ContinueDialogueMode()
    {
        if (_currentStory.canContinue)
        {
            _dialogueText.text = _currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            SaveStoryVariables();
            ExitDialogueMode();
        }
    }

    private void ExitDialogueMode()
    {
        _uiManager.SetMenuActive(Menu.MenuType.Default);
        _dialogueIsPlaying = false;
        _dialogueText.text = "";

        PlayerController.inPlay = true;
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = _currentStory.currentChoices;
        if (currentChoices.Count > _choiceObjs.Length) Debug.LogError("Dialogue choice count is greater than can be supported by the UI.");

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            _choiceObjs[index].gameObject.SetActive(true);
            _choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < _choiceObjs.Length; i++)
        {
            _choiceObjs[i].gameObject.SetActive(false);
        }
        StartCoroutine(SelectFirstChoice());
    }

    IEnumerator SelectFirstChoice()
    {
        //Event system requires we clear it first, the wait
        // for at least one frame before setting the current selected object
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(_choiceObjs[0]);
    }

    public void MakeChoice(int choiceIndex)
    {
        _currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueDialogueMode();
    }

    //---Story Variable Methods---

    private void SaveStoryVariables()
    {
        _visitedCurator = (Ink.Runtime.BoolValue)TryGetStoryVariable("visitedCurator");
        _visitedShop = (Ink.Runtime.BoolValue)TryGetStoryVariable("visitedShop");
        DataPersistenceManager.instance.SaveGame();
    }

    private void LoadStoryVariables()
    {
        TrySetStoryVariable("visitedCurator", _visitedCurator);
        TrySetStoryVariable("visitedShop", _visitedShop);
    }

    public Ink.Runtime.Object TryGetStoryVariable(string variable)
    {
        Ink.Runtime.Object value = null;
        if (_currentStory.variablesState.GlobalVariableExistsWithName(variable))
        {
            return _currentStory.variablesState.GetVariableWithName(variable);
        }

        return value;
    }

    public bool TrySetStoryVariable(string variable, object value)
    {
        if(_currentStory.variablesState.GlobalVariableExistsWithName( variable ) )
        {
            _currentStory.variablesState[variable] = value;
 
            return true;
        }
 
        return false;
    }

    //---Data Persistence Methods---

    public void SaveData(GameData data)
    {
        data.visitedCurator = _visitedCurator;
        data.visitedShop = _visitedShop;
    }

    public void LoadData(GameData data)
    {
        _visitedCurator = data.visitedCurator;
        _visitedShop = data.visitedShop;
    }
}
