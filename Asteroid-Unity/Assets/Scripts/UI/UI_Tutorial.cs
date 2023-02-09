using UnityEngine;
using UnityEngine.UIElements;

namespace Iterium
{
    // Tutorial screen UI

    public class UI_Tutorial : MonoBehaviour
    {
        [Header("Button Elements")]
        [SerializeField] private string story = "buttonStory";
        [SerializeField] private string controls = "controls";
        [SerializeField] private string gameplay = "gameplay";
        [SerializeField] private string scoring = "scoring";
        [SerializeField] private string upgrades = "upgrades";
        [SerializeField] private string faction = "faction";

        [Header("Visual Elements")]
        [SerializeField] private string storyPanel = "storyPanel";
        [SerializeField] private string controlsPanel = "controlsPanel";
        [SerializeField] private string gameplayPanel = "gameplayPanel";
        [SerializeField] private string scoringPanel = "scoringPanel";
        [SerializeField] private string upgradesPanel = "upgradesPanel";
        [SerializeField] private string factionPanel = "factionPanel";

        //Buttons
        private Button buttonStory;
        private Button buttonControls;
        private Button buttonGameplay;
        private Button buttonScoring;
        private Button buttonUpgrades;
        private Button buttonFaction;

        //Visual Elements
        private VisualElement panelStory;
        private VisualElement panelControls;
        private VisualElement panelGameplay;
        private VisualElement panelScoring;
        private VisualElement panelUpgrades;
        private VisualElement panelFaction;

        private void OnEnable()
        {
            VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;

            //Init buttons
            buttonStory = uiRoot.Q<Button>(story);
            buttonControls = uiRoot.Q<Button>(controls);
            buttonGameplay = uiRoot.Q<Button>(gameplay);
            buttonScoring = uiRoot.Q<Button>(scoring);
            buttonUpgrades = uiRoot.Q<Button>(upgrades);
            buttonFaction = uiRoot.Q<Button>(faction);

            //Init visual elements
            panelStory = uiRoot.Q<VisualElement>(storyPanel);
            panelControls = uiRoot.Q<VisualElement>(controlsPanel);
            panelGameplay = uiRoot.Q<VisualElement>(gameplayPanel);
            panelScoring = uiRoot.Q<VisualElement>(scoringPanel);
            panelUpgrades = uiRoot.Q<VisualElement>(upgradesPanel);
            panelFaction = uiRoot.Q<VisualElement>(factionPanel);

            //Button click events
            buttonStory.clicked += StoryClicked;
            buttonControls.clicked += ControlsClicked;
            buttonGameplay.clicked += GameplayClicked;
            buttonScoring.clicked += ScoringClicked;
            buttonUpgrades.clicked += UpgradesClicked;
            buttonFaction.clicked += FactionClicked;

            //Default panel focus
            buttonStory.Focus();
            HideAllPanels();
            panelStory.style.display = DisplayStyle.Flex;
        }

        private void OnDisable()
        {
            //Clean-up events
            buttonStory.clicked -= StoryClicked;
            buttonControls.clicked -= ControlsClicked;
            buttonGameplay.clicked -= GameplayClicked;
            buttonScoring.clicked -= ScoringClicked;
            buttonUpgrades.clicked -= UpgradesClicked;
            buttonFaction.clicked -= FactionClicked;
        }

        private void UpgradesClicked()
        {
            SoundManager.Instance.PlayEffect(2);
            HideAllPanels();
            panelUpgrades.style.display = DisplayStyle.Flex;
        }

        private void ScoringClicked()
        {
            SoundManager.Instance.PlayEffect(2);
            HideAllPanels();
            panelScoring.style.display = DisplayStyle.Flex;
        }

        private void GameplayClicked()
        {
            SoundManager.Instance.PlayEffect(2);
            HideAllPanels();
            panelGameplay.style.display = DisplayStyle.Flex;
        }

        private void ControlsClicked()
        {
            SoundManager.Instance.PlayEffect(2);
            HideAllPanels();
            panelControls.style.display = DisplayStyle.Flex;
        }

        private void StoryClicked()
        {
            SoundManager.Instance.PlayEffect(2);
            HideAllPanels();
            panelStory.style.display = DisplayStyle.Flex;
        }

        private void FactionClicked()
        {
            SoundManager.Instance.PlayEffect(2);
            HideAllPanels();
            panelFaction.style.display = DisplayStyle.Flex;
        }

        private void HideAllPanels()
        {
            panelStory.style.display = DisplayStyle.None;
            panelControls.style.display = DisplayStyle.None;
            panelGameplay.style.display = DisplayStyle.None;
            panelScoring.style.display = DisplayStyle.None;
            panelUpgrades.style.display = DisplayStyle.None;
            panelFaction.style.display = DisplayStyle.None;
        }
    }
}