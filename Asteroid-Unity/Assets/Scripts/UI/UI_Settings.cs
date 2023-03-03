using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Iterium
{
    //Game Settings UI
    //Effects/music volume progress bars are cameras projecting on RenderTextures in the UI, capturing planes with shader graph materials

    public class UI_Settings : MonoBehaviour
    {
        [Header("Drag scene to load")]
#if UNITY_EDITOR
        public UnityEditor.SceneAsset destinationScene;
        private void OnValidate()
        {
            if (destinationScene != null)
            {
                mainMenu = destinationScene.name;
            }
        }
#endif
        [HideInInspector] public string mainMenu;

        [Header("UI Elements")]
        [SerializeField] private string sliderMusic = "musicSlider";
        [SerializeField] private string sliderSound = "soundSlider";
        [SerializeField] private string musicIcon = "musicIcon";
        [SerializeField] private string soundIcon = "soundIcon";
        [SerializeField] private string homeButton = "mainMenu";

        //Slider planes (shader graph materials)
        [Header("Progress Bar Planes")]
        [SerializeField] private GameObject progressMusic;
        [SerializeField] private GameObject progressSound;
        [SerializeField] private GameObject waveformMusic;
        [SerializeField] private GameObject waveformSound;

        [Header("Audio Mixer Groups")]
        [SerializeField] private AudioMixer audioMixer;

        private Renderer matRenderer;
        private Material matMusic;
        private Material matSound;
        private Material waveformMatMusic;
        private Material waveformMatSound;
        private VisualElement iconMusic;
        private VisualElement iconSound;
        private Slider musicSlider;
        private Slider soundSlider;
        private Button buttonHome;

        private void OnEnable()
        {
            //Init UI elements
            VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
            musicSlider = uiRoot.Q<Slider>(sliderMusic);
            soundSlider = uiRoot.Q<Slider>(sliderSound);
            iconMusic = uiRoot.Q<VisualElement>(musicIcon);
            iconSound = uiRoot.Q<VisualElement>(soundIcon);
            buttonHome = uiRoot.Q<Button>(homeButton);
            musicSlider.highValue = -0;
            musicSlider.lowValue = -50;
            soundSlider.highValue = -0;
            soundSlider.lowValue = -50;

            //Events
            musicSlider.RegisterCallback<ChangeEvent<float>>(MusicChanged);
            soundSlider.RegisterCallback<ChangeEvent<float>>(SoundChanged);
            buttonHome.clicked += MainMenu;
        }

        private void Start()
        {
            //Init music progress bar material (shader graph)
            matRenderer = progressMusic.GetComponent<Renderer>();
            matMusic = matRenderer.material;

            //Init sound progress bar material (shader graph)
            matRenderer = progressSound.GetComponent<Renderer>();
            matSound = matRenderer.material;

            //Init music waveform material (shader graph)
            matRenderer = waveformMusic.GetComponent<Renderer>();
            waveformMatMusic = matRenderer.material;

            //Init sound waveform material (shader graph)
            matRenderer = waveformSound.GetComponent<Renderer>();
            waveformMatSound = matRenderer.material;

            InitSliders();
        }

        //Set sliders/waveforms/progress bars to game save loaded values
        private void InitSliders()
        {
            //Music slider
            matMusic.SetFloat("_RemovedSeg", -0 - GameManager.Instance.player.MusicVolume);
            iconMusic.transform.rotation = Quaternion.Euler(0f, 0f, (-0 - GameManager.Instance.player.MusicVolume) * 3.33f);
            musicSlider.value = GameManager.Instance.player.MusicVolume;

            //Sound slider
            matSound.SetFloat("_RemovedSeg", -0 - GameManager.Instance.player.MusicVolume);
            iconSound.transform.rotation = Quaternion.Euler(0f, 0f, (-0 - GameManager.Instance.player.MusicVolume) * 3.33f);
            soundSlider.value = GameManager.Instance.player.EffectsVolume;

            //Music waveform
            waveformMatMusic.SetFloat("_HeightAdjustmentMusic", 1 + musicSlider.value / 50);

            //Sound waveform
            waveformMatSound.SetFloat("_HeightAdjustmentSound", 1 + soundSlider.value / 50);
        }

        //Update music from slider
        private void MusicChanged(ChangeEvent<float> slider)
        {
            float volume = slider.newValue;
            matMusic.SetFloat("_RemovedSeg", -0 - volume);
            waveformMatMusic.SetFloat("_HeightAdjustmentMusic", 1 + volume / 50);
            iconMusic.transform.rotation = Quaternion.Euler(0f, 0f, (-0 - volume) * 3.33f);
            if (volume <= -50)
            {
                volume = -80f;
            }
            audioMixer.SetFloat("Music", volume);
            GameManager.Instance.player.MusicVolume = volume;
            SoundManager.Instance.PlayEffect(2);
        }

       
        //Update sound effects from slider
        private void SoundChanged(ChangeEvent<float> slider)
        {
            float volume = slider.newValue;
            matSound.SetFloat("_RemovedSeg", -0 - volume);
            waveformMatSound.SetFloat("_HeightAdjustmentSound", 1 + volume / 50);
            iconSound.transform.rotation = Quaternion.Euler(0f, 0f, (-0 - volume) * 3.33f);
            if (volume <= -50)
            {
                volume = -80f;
            }
            audioMixer.SetFloat("Sound", volume);
            GameManager.Instance.player.EffectsVolume = volume;
            SoundManager.Instance.PlayEffect(2);
        }

        private void MainMenu()
        {
            GameManager.Instance.SaveGame();
            SceneManager.LoadScene(mainMenu);
        }
    }
}