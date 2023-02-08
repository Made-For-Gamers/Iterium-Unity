using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Iterium
{
    /// <summary>
    /// Singleton manager to manage main game specific data
    /// </summary>

    public class GameManager : Singleton<GameManager>
    {
        [Header("Faction Upgrade Scenes")]
#if UNITY_EDITOR
        public UnityEditor.SceneAsset destinationSceneChn;
        public UnityEditor.SceneAsset destinationSceneUs;
        public UnityEditor.SceneAsset destinationSceneUssr;
        private void OnValidate()
        {
            if (destinationSceneChn != null)
            {
                upgradeChnScene = destinationSceneChn.name;
                upgradeUsScene = destinationSceneUs.name;
                UpgradeUssrScene = destinationSceneUssr.name;
            }
        }
#endif

        [HideInInspector] public string upgradeChnScene;
        [HideInInspector] public string upgradeUsScene;
        [HideInInspector] public string UpgradeUssrScene;

        [Header("Save Game")]
        [SerializeField] private string saveFile = "Player.save";
        [SerializeField] private string saveFileAi = "AI.save";
        [SerializeField] private string saveFileLeaderboard = "Leaderboard.save";
        [SerializeField] private int leaderboardSize = 25;

        [Header("Player Settings")]
        public SO_Player player;
        [HideInInspector] public GameObject targetPlayer;
        public SO_Factions factions;
        public float deathRespawnTime = 4f;
        public int xpLevelSteps = 2000;
        public int maxLevel = 50;
        public int freeShip = 100000;
        [HideInInspector]public bool isPlaying;
        private int curretAiFaction;

        [Header("AI Settings")]
        public SO_Player aiPlayer;
        public bool aiPermadeath;
        [HideInInspector] public GameObject targetAi;

        [Header("NPC Settings")]
        public SO_Player npcPlayer;
        [HideInInspector] public GameObject targetNpc;

        [Header("Iterium Requirements")]
        public SO_GameObjects iterium;
        public int iteriumChance = 20;
        public int speedLevel1 = 10;
        public int speedLevel2 = 30;
        public int shieldLevel1 = 10;
        public int shieldLevel2 = 30;
        public int firepowerLevel1 = 10;
        public int firepowerLevel2 = 30;

        [Space(10)]
        //Save data objects
        public List<LeaderboardItem> leaderboard = new List<LeaderboardItem>();
        [HideInInspector] public SaveData saveData = new SaveData();
        [HideInInspector] public SaveData saveDataAi = new SaveData();

        //Spawn points
        [HideInInspector] public Transform playerSpawner;
        [HideInInspector] public Transform aiSpawner;

        private FileSaveHandler fileSaveHandler;
        


        private void Start()
        {
            //Load Game Save
            fileSaveHandler = new FileSaveHandler(Application.persistentDataPath);
            LoadGame();

            //Events
            player.onChange_bulletLvl.AddListener(BulletLvlChanged);
            player.onChange_shieldLvl.AddListener(ShieldLvlChanged);
            player.onChange_speedLvl.AddListener(SpeedLvlChanged);
            aiPlayer.onChange_bulletLvl.AddListener(BulletLvlChangedAi);
            aiPlayer.onChange_shieldLvl.AddListener(ShieldLvlChangedAi);
            aiPlayer.onChange_speedLvl.AddListener(SpeedLvlChangedAi);
        }

        private void SelectAiPlayer()
        {
            int rnd = Random.Range(1, 4);
            aiPlayer.Character = factions.Factions[rnd];
            //Reset AI bullets pool if a different ship or different bullet level
            if (curretAiFaction != rnd && BulletPooling.bulletPoolAi != null)
            {
                BulletPooling.bulletPoolAi.Clear();
            }
            curretAiFaction = rnd;
        }

        public void SaveGame()
        {
            //Update data to be saved
            saveData.profileName = player.ProfileName;
            saveData.bio = player.Bio;
            saveData.email = player.Email;
            saveData.xp = player.Xp;
            saveData.level = player.Level;
            saveData.iterium = player.Iterium;
            saveData.bulletLvlUs = player.BulletLvlUs;
            saveData.speedLvlUs = player.SpeedLvlUs;
            saveData.shieldLvlUs = player.ShieldLvlUs;
            saveData.bulletLvlUssr = player.BulletLvlUssr;
            saveData.speedLvlUssr = player.SpeedLvlUssr;
            saveData.shieldLvlUssr = player.ShieldLvlUssr;
            saveData.bulletLvlChn = player.BulletLvlChn;
            saveData.speedLvlChn = player.SpeedLvlChn;
            saveData.shieldLvlChn = player.ShieldLvlChn;
            saveData.character = player.Character;
            saveData.effectVolume = player.EffectsVolume;
            saveData.musicVolume = player.MusicVolume;

            saveDataAi.xp = aiPlayer.Xp;
            saveDataAi.level = aiPlayer.Level;
            saveDataAi.iterium = aiPlayer.Iterium;
            saveDataAi.bulletLvlUs = aiPlayer.BulletLvlUs;
            saveDataAi.speedLvlUs = aiPlayer.SpeedLvlUs;
            saveDataAi.shieldLvlUs = aiPlayer.ShieldLvlUs;
            saveDataAi.bulletLvlUssr = aiPlayer.BulletLvlUssr;
            saveDataAi.speedLvlUssr = aiPlayer.SpeedLvlUssr;
            saveDataAi.shieldLvlUssr = aiPlayer.ShieldLvlUssr;
            saveDataAi.bulletLvlChn = aiPlayer.BulletLvlChn;
            saveDataAi.speedLvlChn = aiPlayer.SpeedLvlChn;
            saveDataAi.shieldLvlChn = aiPlayer.ShieldLvlChn;

            fileSaveHandler.Save(saveData, saveFile);
            fileSaveHandler.Save(saveDataAi, saveFileAi);
            print("Saved Game");
            SaveLeaderboard();

        }

        public void LoadGame()
        {
            saveData = fileSaveHandler.Load(saveFile);
            saveDataAi = fileSaveHandler.Load(saveFileAi);
            print("Loaded Game");
            LoadLeaderboard();

            //Update player data from load
            player.ProfileName = saveData.profileName;
            player.Bio = saveData.bio;
            player.Email = saveData.email;
            player.Xp = saveData.xp;
            player.Level = saveData.level;
            player.Iterium = saveData.iterium;
            player.BulletLvlUs = saveData.bulletLvlUs;
            player.SpeedLvlUs = saveData.speedLvlUs;
            player.ShieldLvlUs = saveData.shieldLvlUs;
            player.BulletLvlUssr = saveData.bulletLvlUssr;
            player.SpeedLvlUssr = saveData.speedLvlUssr;
            player.ShieldLvlUssr = saveData.shieldLvlUssr;
            player.BulletLvlChn = saveData.bulletLvlChn;
            player.SpeedLvlChn = saveData.speedLvlChn;
            player.ShieldLvlChn = saveData.shieldLvlChn;
            player.EffectsVolume = saveData.effectVolume;
            player.MusicVolume = saveData.musicVolume;

            //Update AI data from load
            aiPlayer.Xp = saveDataAi.xp;
            aiPlayer.Level = saveDataAi.level;
            aiPlayer.Iterium = saveDataAi.iterium;
            aiPlayer.BulletLvlUs = saveDataAi.bulletLvlUs;
            aiPlayer.SpeedLvlUs = saveDataAi.speedLvlUs;
            aiPlayer.ShieldLvlUs = saveDataAi.shieldLvlUs;
            aiPlayer.BulletLvlUssr = saveDataAi.bulletLvlUssr;
            aiPlayer.SpeedLvlUssr = saveDataAi.speedLvlUssr;
            aiPlayer.ShieldLvlUssr = saveDataAi.shieldLvlUssr;
            aiPlayer.BulletLvlChn = saveDataAi.bulletLvlChn;
            aiPlayer.SpeedLvlChn = saveDataAi.speedLvlChn;
            aiPlayer.ShieldLvlChn = saveDataAi.shieldLvlChn;

            //New game then set the default player ship
            if (saveData.character != null)
            {
                player.Character = saveData.character;
            }
            else
            {
                player.Character = factions.Factions[2];
            }

            UpgradeLevelSync();
        }

        //Reset data to a new game state
        public void ResetGame()
        {
            //Player data
            player.ProfileName = "Player 1";
            player.Health = 100;
            player.Score = 0;
            player.Xp = 0;
            player.Level = 1;
            player.Iterium = 0;
            player.IteriumCollected = 0;
            player.SpeedLvl = 1;
            player.ShieldLvl = 1;
            player.BulletLvl = 1;
            player.SpeedLvlUs = 1;
            player.ShieldLvlUs = 1;
            player.BulletLvlUs = 1;
            player.SpeedLvlUssr = 1;
            player.ShieldLvlUssr = 1;
            player.BulletLvlUssr = 1;
            player.SpeedLvlChn = 1;
            player.ShieldLvlChn = 1;
            player.BulletLvlChn = 1;
            player.Lives = 3;

            //AI data
            aiPlayer.ProfileName = "AI Player";
            aiPlayer.Health = 100;
            aiPlayer.Score = 0;
            aiPlayer.Xp = 0;
            aiPlayer.Level = 1;
            aiPlayer.Iterium = 0;
            aiPlayer.IteriumCollected = 0;
            aiPlayer.Lives = 3;
            aiPlayer.SpeedLvl = 1;
            aiPlayer.ShieldLvl = 1;
            aiPlayer.BulletLvl = 1;
            aiPlayer.SpeedLvlUs = 1;
            aiPlayer.ShieldLvlUs = 1;
            aiPlayer.BulletLvlUs = 1;
            aiPlayer.SpeedLvlUssr = 1;
            aiPlayer.ShieldLvlUssr = 1;
            aiPlayer.BulletLvlUssr = 1;
            aiPlayer.SpeedLvlChn = 1;
            aiPlayer.ShieldLvlChn = 1;
            aiPlayer.BulletLvlChn = 1;
            aiPlayer.Lives = 3;

            SaveGame();
        }

        //Reset data for a new arena battle
        public void ResetArena()
        {
            //Player data
            player.Health = 100;
            player.Score = 0;
            player.IteriumCollected = 0;
            player.XpCollected = 0;
            player.Lives = 3;

            //AI data
            aiPlayer.Health = 100;
            aiPlayer.Score = 0;
            aiPlayer.IteriumCollected = 0;
            aiPlayer.XpCollected = 0;
            aiPlayer.Lives = 3;

            isPlaying = true;
        }

        public void OnApplicationQuit()
        {
            SaveGame();
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                Application.ExternalEval("window.open('https://mfg.gg','_self')");
            }
            else
            {
                Application.Quit();
            }
        }

        public void SceneMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void SceneUpgrade()
        {
            switch (player.Character.Id)
            {
                case "chn":
                    SceneManager.LoadScene(upgradeChnScene);
                    break;
                case "us":
                    SceneManager.LoadScene(upgradeUsScene);
                    break;
                case "ussr":
                    SceneManager.LoadScene(UpgradeUssrScene);
                    break;
            }
        }

        public void SpawnPlayer(float time)
        {
            StartCoroutine(SpawnPlayerOverTime(time));
        }

        public void SpawnAi(float time)
        {
            SelectAiPlayer();
            StartCoroutine(SpawnAiOverTime(time));
        }

        IEnumerator SpawnPlayerOverTime(float time)
        {
            yield return new WaitForSeconds(time);
            if (isPlaying)
            {
                targetPlayer = Instantiate(Instance.player.Character.Ship.ShipPrefab);
                targetPlayer.transform.position = RandomScreenPosition(playerSpawner);
                targetPlayer.transform.rotation = playerSpawner.rotation;
                targetPlayer.transform.name = "Player";
                targetPlayer.transform.tag = "Player";
                player.Health = 100;
            }
        }

        //Get a random position near a spawn point for a ship to re-spawn
        public Vector3 RandomScreenPosition(Transform spawnPoint)
        {
            float x = Random.Range(-4f, 4f);
            float z = Random.Range(-6f, 6f);
            Vector3 position = new Vector3(spawnPoint.position.x + x, 0, spawnPoint.position.z + z);
            return position;
        }

        IEnumerator SpawnAiOverTime(float time)
        {
            yield return new WaitForSeconds(time);
            if (isPlaying)
            {
                targetAi = Instantiate(Instance.aiPlayer.Character.Ship.ShipPrefab);
                Destroy(targetAi.GetComponent<PlayerController>());
                Destroy(targetAi.GetComponent<InputManager>());
                targetAi.AddComponent<AIController>();
                targetAi.transform.position = RandomScreenPosition(aiSpawner);
                targetAi.transform.rotation = aiSpawner.rotation;
                targetAi.transform.name = "AI";
                targetAi.transform.tag = "AI";
                aiPlayer.Health = 100;
            }
        }


        public void GameOver()
        {
            SceneManager.LoadScene("GameOver");
        }

        //Save leaderboard to file
        public void SaveLeaderboard()
        {
            fileSaveHandler.SaveLeaderboard(leaderboard, saveFileLeaderboard);
        }

        //Load leaderboard from file
        public void LoadLeaderboard()
        {
            leaderboard = fileSaveHandler.LoadLeaderboard<LeaderboardItem>(saveFileLeaderboard);

            // If the loaded leaderboard is empty then create a new default board and save
            if (leaderboard.Count == 0)
            {
                InitLeaderboard();
                SaveLeaderboard();
            }
        }

        //Add a new row to the leaderboard
        public void AddLeaderboardItem(bool isPlayer)
        {
            LeaderboardItem item = new LeaderboardItem();
            if (isPlayer)
            {
                item.score = player.Score;
                item.playerName = player.ProfileName;
            }
            else
            {
                item.score = aiPlayer.Score;
                item.playerName = aiPlayer.ProfileName;
            }
            item.date = System.DateTime.Now.Date.ToShortDateString();
            leaderboard.Add(item);
            SortLeaderboard();

            //Max leaderboard size
            if (leaderboard.Count > leaderboardSize)
            {
                leaderboard.Remove(leaderboard[leaderboard.Count - 1]);
            }
        }

        public void InitLeaderboard()
        {
            print("Initialising a new leaderboard");
            for (int i = 0; i < 10; i++)
            {
                LeaderboardItem item = new LeaderboardItem();
                item.score = (i + 1) * 10000;
                item.date = System.DateTime.Now.Date.ToShortDateString();
                item.playerName = "Imperial Xoid";
                leaderboard.Add(item);
            }
            SortLeaderboard();
        }

        public void SortLeaderboard()
        {
            leaderboard = leaderboard.OrderByDescending(x => x.score).ToList();
        }

        public void CameraShake(float time, float magnitude)
        {
            StartCoroutine(Shake(time, magnitude));
        }

        //Shake camera when player dies
        IEnumerator Shake(float time, float magnitude)
        {
            Vector3 pos = Camera.main.transform.position;
            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                float x = Random.Range(-0.3f, 0.3f) * magnitude;
                float y = Random.Range(-0.3f, 0.3f) * magnitude;
                Camera.main.transform.position = new Vector3(x, pos.y, y);
                yield return null;
            }
            Camera.main.transform.position = pos;
        }

        public void CalculateXP()
        {
            player.Xp += player.XpCollected;
            if (player.Xp > xpLevelSteps * player.Level)
            {
                if (player.Level < maxLevel)
                {
                    player.Level++;
                }
            }
            aiPlayer.Xp += aiPlayer.XpCollected;
            if (aiPlayer.Xp > xpLevelSteps * aiPlayer.Level)
            {
                if (aiPlayer.Level < maxLevel)
                {
                    aiPlayer.Level++;
                }
            }
        }

        public void CalculateIterium()
        {
            player.Iterium += player.IteriumCollected;
            aiPlayer.Iterium += aiPlayer.IteriumCollected;
        }

        public int CalculatePlayerBonus()
        {
            int bonus;
            bonus = player.IteriumCollected * 100 * player.Level;
            return bonus;
        }

        public void CalculateAiBonus()
        {
            aiPlayer.Score += aiPlayer.IteriumCollected * 100 * aiPlayer.Level;
        }

        //Keep player bullet upgrade value in sync with current upgrade value
        private void BulletLvlChanged()
        {
            switch (Instance.player.Character.Id)
            {
                case "chn":
                    player.BulletLvlChn = player.BulletLvl;
                    break;
                case "us":
                    player.BulletLvlUs = player.BulletLvl;
                    break;
                case "ussr":
                    player.BulletLvlUssr = player.BulletLvl;
                    break;
            }
        }

        //Keep player shield upgrade value in sync with current upgrade value
        private void ShieldLvlChanged()
        {
            switch (Instance.player.Character.Id)
            {
                case "chn":
                    player.ShieldLvlChn = player.ShieldLvl;
                    break;
                case "us":
                    player.ShieldLvlUs = player.ShieldLvl;
                    break;
                case "ussr":
                    player.ShieldLvlUssr = player.ShieldLvl;
                    break;
            }
        }

        //Keep player speed upgrade value in sync with current upgrade value
        private void SpeedLvlChanged()
        {
            switch (Instance.player.Character.Id)
            {
                case "chn":
                    player.SpeedLvlChn = player.SpeedLvl;
                    break;
                case "us":
                    player.SpeedLvlUs = player.SpeedLvl;
                    break;
                case "ussr":
                    player.SpeedLvlUssr = player.SpeedLvl;
                    break;
            }
        }

        //Keep AI bullet upgrade value in sync with current upgrade value
        private void BulletLvlChangedAi()
        {
            switch (Instance.aiPlayer.Character.Id)
            {
                case "chn":
                    aiPlayer.BulletLvlChn = aiPlayer.BulletLvl;
                    break;
                case "us":
                    aiPlayer.BulletLvlUs = aiPlayer.BulletLvl;
                    break;
                case "ussr":
                    aiPlayer.BulletLvlUssr = aiPlayer.BulletLvl;
                    break;
            }
        }

        //Keep AI shield upgrade value in sync with current upgrade value
        private void ShieldLvlChangedAi()
        {
            switch (Instance.aiPlayer.Character.Id)
            {
                case "chn":
                    aiPlayer.ShieldLvlChn = aiPlayer.ShieldLvl;
                    break;
                case "us":
                    aiPlayer.ShieldLvlUs = aiPlayer.ShieldLvl;
                    break;
                case "ussr":
                    aiPlayer.ShieldLvlUssr = aiPlayer.ShieldLvl;
                    break;
            }
        }

        //Keep AI speed upgrade value in sync with current upgrade value
        private void SpeedLvlChangedAi()
        {
            switch (Instance.aiPlayer.Character.Id)
            {
                case "chn":
                    aiPlayer.SpeedLvlChn = aiPlayer.SpeedLvl;
                    break;
                case "us":
                    aiPlayer.SpeedLvlUs = aiPlayer.SpeedLvl;
                    break;
                case "ussr":
                    aiPlayer.SpeedLvlUssr = aiPlayer.SpeedLvl;
                    break;
            }
        }

        //Sync saved upgrade level data with current upgrade variables
        public void UpgradeLevelSync()
        {
            //Player upgrade sync
            switch (Instance.player.Character.Id)
            {
                case "chn":
                    player.BulletLvl = player.BulletLvlChn;
                    player.ShieldLvl = player.ShieldLvlChn;
                    player.SpeedLvl = player.SpeedLvlChn;
                    break;
                case "us":
                    player.BulletLvl = player.BulletLvlUs;
                    player.ShieldLvl = player.ShieldLvlUs;
                    player.SpeedLvl = player.SpeedLvlUs;
                    break;
                case "ussr":
                    player.BulletLvl = player.BulletLvlUssr;
                    player.ShieldLvl = player.ShieldLvlUssr;
                    player.SpeedLvl = player.SpeedLvlUssr;
                    break;
            }

            //Ai upgrade sync
            switch (Instance.aiPlayer.Character.Id)
            {
                case "chn":
                    aiPlayer.BulletLvl = aiPlayer.BulletLvlChn;
                    aiPlayer.ShieldLvl = aiPlayer.ShieldLvlChn;
                    aiPlayer.SpeedLvl = aiPlayer.SpeedLvlChn;
                    break;
                case "us":
                    aiPlayer.BulletLvl = aiPlayer.BulletLvlUs;
                    aiPlayer.ShieldLvl = aiPlayer.ShieldLvlUs;
                    aiPlayer.SpeedLvl = aiPlayer.SpeedLvlUs;
                    break;
                case "ussr":
                    aiPlayer.BulletLvl = aiPlayer.BulletLvlUssr;
                    aiPlayer.ShieldLvl = aiPlayer.ShieldLvlUssr;
                    aiPlayer.SpeedLvl = aiPlayer.SpeedLvlUssr;
                    break;
            }
        }
    }
}