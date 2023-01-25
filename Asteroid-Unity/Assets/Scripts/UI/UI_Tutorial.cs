using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Tutorial : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private string factions;
    [SerializeField] private string upgrades;
    [SerializeField] private string iterium;
    [SerializeField] private string gameplay;

    private void OnEnable()
    {
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
        Button factionsButton = uiRoot.Q<Button>(factions);
        Button upgradesButton = uiRoot.Q<Button>(factions);
        Button iteriumButton = uiRoot.Q<Button>(factions);
        Button gameplayButton = uiRoot.Q<Button>(factions);

        //Click events
        factionsButton.clicked += factions_clicked;
        upgradesButton.clicked += upgrades_clicked;
        iteriumButton.clicked += iterium_clicked;
        gameplayButton.clicked += gameplay_clicked;
    }

   private void factions_clicked()
    {
        print("clicked");
    }

    private void upgrades_clicked()
    {
        print("clicked");
    }
   private void iterium_clicked()
    {
        print("clicked");
    }

    private void gameplay_clicked()
    {
        print("clicked");
    }
}
