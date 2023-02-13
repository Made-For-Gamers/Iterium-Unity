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
            HideAllPanels();
            buttonStory.Focus();
            panelStory.RemoveFromClassList("fadeOutPanel");
            panelStory.AddToClassList("fadeInPanel");
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
            panelUpgrades.RemoveFromClassList("fadeOutPanel");
            panelUpgrades.AddToClassList("fadeInPanel");
        }

        private void ScoringClicked()
        {
            SoundManager.Instance.PlayEffect(2);
            HideAllPanels();
            panelScoring.RemoveFromClassList("fadeOutPanel");
            panelScoring.AddToClassList("fadeInPanel");
        }

        private void GameplayClicked()
        {
            SoundManager.Instance.PlayEffect(2);
            HideAllPanels();
            panelGameplay.RemoveFromClassList("fadeOutPanel");
            panelGameplay.AddToClassList("fadeInPanel");

        }

        private void ControlsClicked()
        {
            SoundManager.Instance.PlayEffect(2);
            HideAllPanels();
            panelControls.RemoveFromClassList("fadeOutPanel");
            panelControls.AddToClassList("fadeInPanel");
        }

        private void StoryClicked()
        {
            SoundManager.Instance.PlayEffect(2);
            HideAllPanels();
            panelStory.RemoveFromClassList("fadeOutPanel");
            panelStory.AddToClassList("fadeInPanel");
        }

        private void FactionClicked()
        {
            SoundManager.Instance.PlayEffect(2);
            HideAllPanels();
            panelFaction.RemoveFromClassList("fadeOutPanel");
            panelFaction.AddToClassList("fadeInPanel");
        }

        private void HideAllPanels()
        {
            panelStory.AddToClassList("fadeOutPanel");
            panelStory.RemoveFromClassList("fadeInPanel");
            panelControls.AddToClassList("fadeOutPanel");
            panelControls.RemoveFromClassList("fadeInPanel");
            panelGameplay.AddToClassList("fadeOutPanel");
            panelGameplay.RemoveFromClassList("fadeInPanel");
            panelScoring.AddToClassList("fadeOutPanel");
            panelScoring.RemoveFromClassList("fadeInPanel");
            panelUpgrades.AddToClassList("fadeOutPanel");
            panelUpgrades.RemoveFromClassList("fadeInPanel");
            panelFaction.AddToClassList("fadeOutPanel");
            panelFaction.RemoveFromClassList("fadeInPanel");
        }
    }
}