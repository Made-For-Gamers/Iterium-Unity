using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

namespace Iterium
{
    // Singleton manager to manage main game data

    public class GameManager : Singleton<GameManager>
    {
        #region Variables

        //Plugin function - open a webpage in the current browser
        [DllImport("__Internal")]
        private static extern void OpenURL(string url);

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

        [Header("Save Game Settings")]
        [SerializeField] private string saveFile = "Player.save";
        [SerializeField] private string saveFileAi = "AI.save";
        [SerializeField] private string saveFileLeaderboard = "Leaderboard.save";
        [SerializeField] private int leaderboardSize = 25;

        [Header("Player Settings")]
        public SO_Player player;
        public SO_FactionList factions;
        public float deathRespawnTime = 4f;
        public int xpLevelSteps = 2000;
        public int maxLevel = 50;
        public int freeShip = 100000;
        [HideInInspector] public bool isPlaying;
        [HideInInspector] public GameObject targetPlayer;

        [Header("AI Settings")]
        public SO_Player aiPlayer;
        public bool aiPermadeath;
        [HideInInspector] public GameObject targetAi;

        [Header("NPC Settings")]
        public SO_Player npcPlayer;
        [HideInInspector] public GameObject targetNpc;
        [SerializeField] int minSpeedNpc = 3;
        [SerializeField] int maxSpeedNpc = 7;

        [Header("Boss Settings")]
        public SO_Player bossPlayer;
        [HideInInspector] public GameObject targetBoss;

        [Header("Iterium Settings")]
        public SO_GameObjects iterium;
        public int iteriumScore = 250;
        public int iteriumXp = 25;
        public int iteriumSfx;
        public int speedLevel1 = 15;
        public int speedLevel2 = 40;
        public int shieldLevel1 = 10;
        public int shieldLevel2 = 30;
        public int firepowerLevel1 = 20;
        public int firepowerLevel2 = 50;

        [Space(10)]
        //Save data objects
        public SO_Leaderboard leaderboard;
        [HideInInspector] public SaveData saveData = new SaveData();
        [HideInInspector] public SaveData saveDataAi = new SaveData();

        //Spawn points
        [HideInInspector] public Vector3 playerSpawner;
        [HideInInspector] public Vector3 aiSpawner;

        //Private
        private FileSaveHandler fileSaveHandler;
        private int curretAiFaction; 
        private int npcSpeed;

        #endregion

        #region Event Subscriptions

        private void OnEnable()
        {
            //Events
            PlayerController.PlayerDamage += PlayerDamage;
            AIController.AiDamage += AiDamage;
            NPCController.NpcDamage += NpcDamage;
            Asteroid.AsteroidDamage += AsteroidDamage;
            Asteroid.DropIterium += DropIterium;
            Iterium.CollectIterium += CollectIterium;
            PlayerSpawner.SpawnPlayer += PlayerSpawning;
            AISpawner.SpawnAi += AiSpawning;
            NPCSpawner.SpawnNpc += NpcSpawning;
            BossSpawner.SpawnBoss += BossSpawning;
            BossController.BossDestroy += BossDestroy;
            BossController.BossDamage += BossDamage;
        }

        private void OnDisable()
        {
            //Disable events
            PlayerController.PlayerDamage -= PlayerDamage;
            AIController.AiDamage -= AiDamage;
            NPCController.NpcDamage -= NpcDamage;
            Asteroid.AsteroidDamage -= AsteroidDamage;
            Asteroid.DropIterium -= DropIterium;
            Iterium.CollectIterium -= CollectIterium;
            PlayerSpawner.SpawnPlayer -= PlayerSpawning;
            AISpawner.SpawnAi -= AiSpawning;
            NPCSpawner.SpawnNpc -= NpcSpawning;
            BossSpawner.SpawnBoss -= BossSpawning;
            BossController.BossDestroy -= BossDestroy;
            BossController.BossDamage -= BossDamage;
        }

        private void BossSpawning(Vector3 position)
        {
            targetBoss = Instantiate(bossPlayer.Faction.Ship.ShipPrefab);
            targetBoss.transform.position = position;
            SoundManager.Instance.MusicPitch(1.3f);
        }

        private void BossDestroy()
        {
            SoundManager.Instance.MusicPitch(1);
        }

        private void BossDamage(string attacker)
        {
            SoundManager.Instance.PlayEffect(1);
            switch (attacker)
            {
                case "player":
                    player.Score += 800;
                    player.XpCollected += 25;
                    break;
                case "ai":
                    aiPlayer.Score += 800;
                    aiPlayer.XpCollected += 25;
                    break;
            }
        }

        private void NpcSpawning(Vector3 position)
        {
            GameObject ship = Instantiate(npcPlayer.Faction.Ship.ShipPrefab);
            ship.transform.position = position;

            //Target player first, or then AI, or then center of screen
            if (targetPlayer.gameObject)
            {
                ship.transform.LookAt(targetPlayer.transform);
            }
            else if (targetAi.gameObject)
            {
                ship.transform.LookAt(targetAi.transform);
            }
            else
            {
                ship.transform.Rotate(Vector3.zero);
            }

            npcSpeed = Random.Range(minSpeedNpc, maxSpeedNpc);
            ship.GetComponent<Rigidbody>().velocity = ship.transform.forward * npcSpeed;
            targetNpc = ship;
        }


        private void PlayerSpawning(Vector2 pos, float delay)
        {
            playerSpawner = pos;
            SpawnPlayer(delay);
        }

        private void AiSpawning(Vector2 pos, float delay)
        {
            aiSpawner = pos;
            SpawnAi(delay);
        }

        private void CollectIterium(string collector)
        {
            switch (collector)
            {
                case "player":
                    player.Score += iteriumScore;
                    player.XpCollected += iteriumXp;
                    player.IteriumCollected++;
                    break;
                case "ai":
                    aiPlayer.Score += iteriumScore;
                    aiPlayer.XpCollected += iteriumXp;
                    aiPlayer.IteriumCollected++;
                    break;
            }
            SoundManager.Instance.PlayEffect(iteriumSfx);
        }


        //Called by invoke of bulletHit event on Bullet
        private void PlayerDamage()
        {
            SoundManager.Instance.PlayEffect(1);
            aiPlayer.Score += 500;
            aiPlayer.XpCollected += 50;
        }

        //Called by invoke of bulletHit event on BulletAI
        private void AiDamage()
        {
            SoundManager.Instance.PlayEffect(1);
            player.Score += 500;
            player.XpCollected += 50;
        }

        private void DropIterium(Vector3 position)
        {
            Instantiate(iterium.GetRandomGameObject(), position, Random.rotation);
        }

        //Called by invoke of bulletHit event on BulletNPC
        private void NpcDamage(string attacker)
        {
            SoundManager.Instance.PlayEffect(1);
            SoundManager.Instance.PlayShipExplosion();
            switch (attacker)
            {
                case "player":
                    player.Score += 1500;
                    player.XpCollected += 100;
                    break;
                case "ai":
                    aiPlayer.Score += 1500;
                    aiPlayer.XpCollected += 100;
                    break;
            }
        }

        public void AsteroidDamage(string attacker)
        {
            SoundManager.Instance.PlayEffect(1);
            SoundManager.Instance.PlayAsteroidExplosion();
            switch (attacker)
            {
                case "player":
                    player.Score += 50;
                    player.XpCollected += 10;
                    break;
                case "ai":
                    aiPlayer.Score += 50;
                    aiPlayer.XpCollected += 10;
                    break;
            }
        }

        #endregion

        #region General Methods
      
        private void Start()
        {
            //Load Game Save
            fileSaveHandler = new FileSaveHandler(Application.persistentDataPath);
            LoadGame();
            //Event subscriptions
            player.onChange_bulletLvl.AddListener(BulletLvlChanged);
            player.onChange_shieldLvl.AddListener(ShieldLvlChanged);
            player.onChange_speedLvl.AddListener(SpeedLvlChanged);
            aiPlayer.onChange_bulletLvl.AddListener(BulletLvlChangedAi);
            aiPlayer.onChange_shieldLvl.AddListener(ShieldLvlChangedAi);
            aiPlayer.onChange_speedLvl.AddListener(SpeedLvlChangedAi);
        }

        //Select a new random AI faction
        private void SelectAiPlayer()
        {
            int rnd = Random.Range(1, 4);
            aiPlayer.Faction = factions.Factions[rnd];

            //Clear AI bullet pool if a different ship is spawned
            if (curretAiFaction != rnd && BulletPooling.bulletPoolAi != null)
            {
                BulletPooling.bulletPoolAi.Clear();
            }
            curretAiFaction = rnd;
        }

        //Reset data to a complete new game state
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
            //Player
            player.Health = 100;
            player.Score = 0;
            player.IteriumCollected = 0;
            player.XpCollected = 0;
            player.Lives = 3;

            //AI
            aiPlayer.Health = 100;
            aiPlayer.Score = 0;
            aiPlayer.IteriumCollected = 0;
            aiPlayer.XpCollected = 0;
            aiPlayer.Lives = 3;

            isPlaying = true;
        }

        //Spawn player
        public void SpawnPlayer(float time)
        {
            StartCoroutine(SpawnPlayerOverTime(time));
        }
        IEnumerator SpawnPlayerOverTime(float time)
        {
            yield return new WaitForSeconds(time);
            if (isPlaying)
            {
                targetPlayer = Instantiate(Instance.player.Faction.Ship.ShipPrefab);
                targetPlayer.transform.position = RandomScreenPosition(playerSpawner);
                targetPlayer.transform.RotateAround(targetPlayer.transform.position, Vector3.up, 90);
                targetPlayer.transform.name = "Player";
                targetPlayer.transform.tag = "Player";
                player.Health = 100;
            }
        }

        //Spawn AI
        public void SpawnAi(float time)
        {
            SelectAiPlayer();
            StartCoroutine(SpawnAiOverTime(time));
        }

        IEnumerator SpawnAiOverTime(float time)
        {
            yield return new WaitForSeconds(time);
            if (isPlaying)
            {
                targetAi = Instantiate(Instance.aiPlayer.Faction.Ship.ShipPrefab);
                Destroy(targetAi.GetComponent<PlayerController>());
                Destroy(targetAi.GetComponent<InputManager>());
                targetAi.AddComponent<AIController>();
                targetAi.transform.position = RandomScreenPosition(aiSpawner);
                targetAi.transform.name = "AI";
                targetAi.transform.tag = "AI";
                aiPlayer.Health = 100;
            }
        }

        //Get a random position near a spawn point for a ship to re-spawn
        public Vector3 RandomScreenPosition(Vector3 spawnPoint)
        {
            float x = Random.Range(-4f, 4f);
            float z = Random.Range(-6f, 6f);
            Vector3 position = new Vector3(spawnPoint.x + x, 0, spawnPoint.z + z);
            return position;
        }


        //Shake camera on player death
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

        #endregion

        #region Save/Load

        //Save game data
        public void SaveGame()
        {
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
            saveData.character = player.Faction;
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

        //Load game data
        public void LoadGame()
        {
            saveData = fileSaveHandler.Load(saveFile);
            saveDataAi = fileSaveHandler.Load(saveFileAi);
            print("Loaded Game");
            LoadLeaderboard();

            //Update player ScriptableObject with loaded data
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

            //Update AI ScriptableObject with loaded data
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

            //If a new game then set the default player ship
            if (saveData.character != null)
            {
                player.Faction = saveData.character;
            }
            else
            {
                player.Faction = factions.Factions[2];
            }

            //Set upgrade levels
            UpgradeLevelSync();
        }

        //Save leaderboard
        public void SaveLeaderboard()
        {
            fileSaveHandler.SaveLeaderboard(leaderboard.Leaderboard, saveFileLeaderboard);
        }

        //Load leaderboard
        public void LoadLeaderboard()
        {
            leaderboard.Leaderboard = fileSaveHandler.LoadLeaderboard<LeaderboardItem>(saveFileLeaderboard);

            // If the loaded leaderboard is empty then create a new default board and save
            if (leaderboard.Leaderboard.Count == 0)
            {
                InitLeaderboard();
                SaveLeaderboard();
            }
        }

        #endregion

        #region Scenes/Menu

        //Quit the application
        public void OnApplicationQuit()
        {
            SaveGame();
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {             
                OpenURL("https://mfg.gg");
            }
            else
            {
                Application.Quit();
            }
        }

        //Load main menu scene
        public void SceneMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        //Load faction upgrade scene
        public void SceneUpgrade()
        {
            switch (player.Faction.Id)
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

        //Load game over scene
        public void GameOver()
        {
            SceneManager.LoadScene("GameOver");
        }

        #endregion

        #region Leaderboard

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
            leaderboard.Leaderboard.Add(item);
            SortLeaderboard();

            //Max leaderboard size
            if (leaderboard.Leaderboard.Count > leaderboardSize)
            {
                leaderboard.Leaderboard.Remove(leaderboard.Leaderboard[leaderboard.Leaderboard.Count - 1]);
            }
        }

        //Populate a new leaderboard with entries
        public void InitLeaderboard()
        {
            print("Initialising a new leaderboard");
            for (int i = 0; i < leaderboardSize; i++)
            {
                LeaderboardItem item = new LeaderboardItem();
                item.score = (i + 1) * 10000;
                item.date = System.DateTime.Now.Date.ToShortDateString();
                item.playerName = "Imperial Xoid";
                leaderboard.Leaderboard.Add(item);
            }
            SortLeaderboard();
        }

        //Sort all leaderboard rows
        public void SortLeaderboard()
        {
            leaderboard.Leaderboard = leaderboard.Leaderboard.OrderByDescending(x => x.score).ToList();
        }

        #endregion

        #region XP/Levels/Iterium/Bonus

        //Add XP gained from a battle and calculate leveling
        public void CalculateXP()
        {
            //Player leveling
            player.Xp += player.XpCollected;
            if (player.Xp > xpLevelSteps * player.Level)
            {
                if (player.Level < maxLevel)
                {
                    player.Level++;
                }
            }

            //AI leveling
            aiPlayer.Xp += aiPlayer.XpCollected;
            if (aiPlayer.Xp > xpLevelSteps * aiPlayer.Level)
            {
                if (aiPlayer.Level < maxLevel)
                {
                    aiPlayer.Level++;
                }
            }
        }

        //Calculate how much Iterium was collected from a battle
        public void CalculateIterium()
        {
            player.Iterium += player.IteriumCollected;
            aiPlayer.Iterium += aiPlayer.IteriumCollected;
        }

        //Calculate player points bonus at end of battle
        public int CalculatePlayerBonus()
        {
            int bonus;
            bonus = player.IteriumCollected * 100 * player.Level;
            player.Score += bonus;
            return bonus;
        }

        //Calculate AI points bonus at end of battle
        public void CalculateAiBonus()
        {
            aiPlayer.Score += (aiPlayer.IteriumCollected * 100) * aiPlayer.Level;
        }

        #endregion

        #region Upgrades

        //Keep player bullet upgrade data in sync with current game value
        private void BulletLvlChanged()
        {
            switch (Instance.player.Faction.Id)
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

        //Keep player shield upgrade data in sync with current game value
        private void ShieldLvlChanged()
        {
            switch (Instance.player.Faction.Id)
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

        //Keep player speed upgrade data in sync with current game value
        private void SpeedLvlChanged()
        {
            switch (Instance.player.Faction.Id)
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

        //Keep AI bullet upgrade data in sync with current game value
        private void BulletLvlChangedAi()
        {
            switch (Instance.aiPlayer.Faction.Id)
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

        //Keep AI shield upgrade data in sync with current game value
        private void ShieldLvlChangedAi()
        {
            switch (Instance.aiPlayer.Faction.Id)
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

        //Keep AI speed upgrade data in sync with current game value
        private void SpeedLvlChangedAi()
        {
            switch (Instance.aiPlayer.Faction.Id)
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

        //Update game upgrade variables with loaded upgrade data
        public void UpgradeLevelSync()
        {
            //Player upgrade sync
            switch (Instance.player.Faction.Id)
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
            switch (Instance.aiPlayer.Faction.Id)
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

        #endregion 
    }
}