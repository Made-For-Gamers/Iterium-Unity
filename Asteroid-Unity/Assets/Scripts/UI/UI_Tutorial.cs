using System;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public class UI_Tutorial : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private string story = "buttonStory";
    [SerializeField] private string controls = "controls";
    [SerializeField] private string gameplay = "gameplay";
    [SerializeField] private string scoring = "scoring";
    [SerializeField] private string upgrades = "upgrades";

    [SerializeField] private string storyPanel = "storyPanel";
    [SerializeField] private string controlsPanel = "controlsPanel";
    [SerializeField] private string gameplayPanel = "gameplayPanel";
    [SerializeField] private string scoringPanel = "scoringPanel";
    [SerializeField] private string upgradesPanel = "upgradesPanel";

    Button buttonStory;
    Button buttonControls;
    Button buttonGameplay;
    Button buttonScoring;
    Button buttonUpgrades;

    VisualElement panelStory;
    VisualElement panelControls;
    VisualElement panelGameplay;
    VisualElement panelScoring;
    VisualElement panelUpgrades;

    private void OnEnable()
    {
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
        
        //UI buttons
        buttonStory = uiRoot.Q<Button>(story);
        buttonControls = uiRoot.Q<Button>(controls);
        buttonGameplay = uiRoot.Q<Button>(gameplay);
        buttonScoring = uiRoot.Q<Button>(scoring);
        buttonUpgrades = uiRoot.Q<Button>(upgrades);

        //Visual elements
        panelStory = uiRoot.Q<VisualElement>(storyPanel);
        panelControls = uiRoot.Q<VisualElement>(controlsPanel);
        panelGameplay = uiRoot.Q<VisualElement>(gameplayPanel);
        panelScoring = uiRoot.Q<VisualElement>(scoringPanel);
        panelUpgrades = uiRoot.Q<VisualElement>(upgradesPanel);

        //Button click events
        buttonStory.clicked += StoryClicked;
        buttonControls.clicked += ControlsClicked;
        buttonGameplay.clicked += GameplayClicked;
        buttonScoring.clicked += ScoringClicked;
        buttonUpgrades.clicked += UpgradesClicked;

        buttonStory.Focus();
    }

    private void UpgradesClicked()
    {
        HideAllPanels();
        panelUpgrades.style.display = DisplayStyle.Flex;
    }

    private void ScoringClicked()
    {
        HideAllPanels();
        panelScoring.style.display = DisplayStyle.Flex;
    }

    private void GameplayClicked()
    {
        HideAllPanels();
        panelGameplay.style.display = DisplayStyle.Flex;
    }

    private void ControlsClicked()
    {
        HideAllPanels();
        panelControls.style.display = DisplayStyle.Flex;
    }

    private void StoryClicked()
    {
        HideAllPanels();
        panelStory.style.display = DisplayStyle.Flex;
    }

    private void HideAllPanels()
    { 
        panelStory.style.display = DisplayStyle.None;
        panelControls.style.display = DisplayStyle.None;
        panelGameplay.style.display = DisplayStyle.None;
        panelScoring.style.display = DisplayStyle.None;
        panelUpgrades.style.display = DisplayStyle.None;
    }
}
