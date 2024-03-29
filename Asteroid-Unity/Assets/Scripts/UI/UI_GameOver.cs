using UnityEngine.UIElements;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using static UnityEngine.GUILayout;
using UnityEngine.SocialPlatforms.Impl;

namespace Iterium
{
    // Game Over UI
    // * Shows final score, iterium collected, total iterium, level
    // * Indicates if a high score is achieved
    // * Calculate XP/Leveling/points bonus
    // * Display specific message depending on game performance

    public class UI_GameOver : MonoBehaviour
    {
        [Header("Game Play Scene")]
#if UNITY_EDITOR
        public UnityEditor.SceneAsset destinationScene;
        private void OnValidate()
        {
            if (destinationScene != null)
            {
                gameScene = destinationScene.name;
            }
        }
#endif
        [HideInInspector] public string gameScene;

        [Header("UI Elements")]
        [SerializeField] private string playerScore = "playerScore";
        [SerializeField] private string playerIteriumCollected = "playerIterium";
        [SerializeField] private string playerIteriumTotal = "playerIteriumTotal";
        [SerializeField] private string playerMessage = "message";
        [SerializeField] private string rematchButton = "rematch";
        [SerializeField] private string playerXpTotal = "playerXPTotal";
        [SerializeField] private string playerXpEarned = "playerXPEarned";
        [SerializeField] private string playerLevel = "playerLevel";
        [SerializeField] private string playerBonus = "playerBonus";

        private Button rematch;
        private int arenaScore;
        private int roundBonus;
        Label score;
        Label iteriumCollected;
        Label iteriumTotal;
        Label message;
        Label xpTotal;
        Label xpEarned;
        Label level;
        Label bonus;

        private void OnEnable()
        {
            VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;

            //Init player UI elements
            score = uiRoot.Q<Label>(playerScore);
            iteriumCollected = uiRoot.Q<Label>(playerIteriumCollected);
            iteriumTotal = uiRoot.Q<Label>(playerIteriumTotal);
            message = uiRoot.Q<Label>(playerMessage);
            xpTotal = uiRoot.Q<Label>(playerXpTotal);
            xpEarned = uiRoot.Q<Label>(playerXpEarned);
            level = uiRoot.Q<Label>(playerLevel);
            bonus = uiRoot.Q<Label>(playerBonus);
            rematch = uiRoot.Q<Button>(rematchButton);

            //Events
            rematch.clicked += Rematch;
            
            //XP
            GameManager.Instance.CalculateXP();
            xpEarned.text = GameManager.Instance.player.XpCollected.ToString();
            xpTotal.text = GameManager.Instance.player.Xp.ToString() + " / " + GameManager.Instance.xpLevelSteps * GameManager.Instance.player.Level;

            //Bonus
            roundBonus = GameManager.Instance.CalculatePlayerBonus();
            GameManager.Instance.CalculateAiBonus();
            bonus.text = roundBonus.ToString() + " points";

            //Score
            StartCoroutine(ScoreTicker());
            arenaScore = GameManager.Instance.player.Score;          

            //Level
            level.text = GameManager.Instance.player.Level.ToString();

            //Iterium
            GameManager.Instance.CalculateIterium();
            iteriumCollected.text = GameManager.Instance.player.IteriumCollected.ToString();
            iteriumTotal.text = GameManager.Instance.player.Iterium.ToString();


            //AI firepower upgrade
            if (GameManager.Instance.aiPlayer.BulletLvl == 1 && GameManager.Instance.aiPlayer.Iterium >= GameManager.Instance.firepowerLevel1)
            {
                //Upgrade bullet to lvl 2
                GameManager.Instance.aiPlayer.Iterium -= GameManager.Instance.firepowerLevel1;
                GameManager.Instance.aiPlayer.BulletLvl = 2;
            }
            else if (GameManager.Instance.aiPlayer.BulletLvl == 2 && GameManager.Instance.aiPlayer.Iterium >= GameManager.Instance.firepowerLevel2)
            {
                //Upgrade bullet to lvl 3
                GameManager.Instance.aiPlayer.Iterium -= GameManager.Instance.firepowerLevel2;
                GameManager.Instance.aiPlayer.BulletLvl = 3;
            }

            //AI shield upgrade
            if (GameManager.Instance.aiPlayer.ShieldLvl == 1 && GameManager.Instance.aiPlayer.Iterium >= GameManager.Instance.shieldLevel1)
            {
                //Upgrade shield to lvl 2
                GameManager.Instance.aiPlayer.Iterium -= GameManager.Instance.shieldLevel1;
                GameManager.Instance.aiPlayer.ShieldLvl = 2;
            }
            else if (GameManager.Instance.aiPlayer.ShieldLvl == 2 && GameManager.Instance.aiPlayer.Iterium >= GameManager.Instance.shieldLevel2)
            {
                //Upgrade shield to lvl 3
                GameManager.Instance.aiPlayer.Iterium -= GameManager.Instance.shieldLevel2;
                GameManager.Instance.aiPlayer.ShieldLvl = 3;
            }

            //AI speed upgrade
            if (GameManager.Instance.aiPlayer.SpeedLvl == 1 && GameManager.Instance.aiPlayer.Iterium >= GameManager.Instance.speedLevel1)
            {
                //Upgrade speed to lvl 2
                GameManager.Instance.aiPlayer.Iterium -= GameManager.Instance.speedLevel1;
                GameManager.Instance.aiPlayer.SpeedLvl = 2;
            }
            else if (GameManager.Instance.aiPlayer.SpeedLvl == 2 && GameManager.Instance.aiPlayer.Iterium >= GameManager.Instance.speedLevel2)
            {
                //Upgrade speed to lvl 3
                GameManager.Instance.aiPlayer.Iterium -= GameManager.Instance.speedLevel2;
                GameManager.Instance.aiPlayer.SpeedLvl = 3;
            }

            //Player leaderboard entry if the score is higher than the last row
            if (GameManager.Instance.leaderboard.Leaderboard[GameManager.Instance.leaderboard.Leaderboard.Count - 1].score <= arenaScore)
            {
                //High Score greeting
                message.text = "Congratulations " + GameManager.Instance.player.ProfileName + "!, a new high score.";
                GameManager.Instance.AddLeaderboardItem(true);
                SoundManager.Instance.PlayMusic(2, false, true);
            }
            else
            {
                //Normal greeting
                SoundManager.Instance.PlayMusic(1, false, true);
                switch (arenaScore)
                {
                    case >= 200000:
                        message.text = "Super score " + GameManager.Instance.player.ProfileName + ", an arcade pro!";
                        break;
                    case >= 100000:
                        message.text = "Gosh brilliant score " + GameManager.Instance.player.ProfileName + ", impressed!";
                        break;
                    case >= 50000:
                        message.text = "Well done " + GameManager.Instance.player.ProfileName + ", a very good score!";
                        break;
                    case >= 25000:
                        message.text = "Descent score " + GameManager.Instance.player.ProfileName + ", well played!";
                        break;
                    case >= 6000:
                        message.text = "Nice score " + GameManager.Instance.player.ProfileName + ", keep it up.";
                        break;
                    case 0 :
                        message.text = "Really " + GameManager.Instance.player.ProfileName + "?, zero score!";
                        break;
                    case <6000:
                        message.text = "Average score " + GameManager.Instance.player.ProfileName + ", you need more practise.";
                        break;
                }
            }

            //AI leaderboard challenge
            arenaScore = GameManager.Instance.aiPlayer.Score;
            if (GameManager.Instance.leaderboard.Leaderboard[GameManager.Instance.leaderboard.Leaderboard.Count - 1].score <= arenaScore)
            {
                GameManager.Instance.AddLeaderboardItem(false);
            }

            //Save Game
            GameManager.Instance.SaveGame();
        }

        private void OnDisable()
        {
            //Clean-up events
            rematch.clicked -= Rematch;
        }

        private void Rematch()
        {
            SoundManager.Instance.PlayEffect(2);
            GameManager.Instance.ResetArena();
            SceneManager.LoadScene(gameScene);
        }


        IEnumerator ScoreTicker()
        {
            int increment = GameManager.Instance.player.Score / 100;
            for (int i = 0; i < GameManager.Instance.player.Score; i += increment)
            {
                score.text = i.ToString();
                SoundManager.Instance.PlayEffect(0);
                yield return new WaitForSeconds(0.05f);
            }
            score.text = GameManager.Instance.player.Score.ToString();
        }
    }
}