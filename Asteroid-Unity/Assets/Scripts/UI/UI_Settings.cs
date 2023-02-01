using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class UI_Settings : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private string sliderMusic = "musicSlider";
    [SerializeField] private string sliderSound = "soundSlider";
    [SerializeField] private string musicIcon = "musicIcon";
    [SerializeField] private string soundIcon = "soundIcon";

    //Slider planes (shader graph materials)
    [Header("Music/Sound Planes")]
    [SerializeField] private GameObject musicPlane;
    [SerializeField] private GameObject soundPlane;

    [Header("Audio Nixer Groups")]
    [SerializeField] private AudioMixer audioMixer;

    private Renderer renderer;
    private Material matMusic;
    private Material matSound;
    private VisualElement iconMusic;
    private VisualElement iconSound;

    private void OnEnable()
    {
        VisualElement uiRoot = GetComponent<UIDocument>().rootVisualElement;
        Slider musicSlider = uiRoot.Q<Slider>(sliderMusic);
        Slider soundSlider = uiRoot.Q<Slider>(sliderSound);
        iconMusic = uiRoot.Q<VisualElement>(musicIcon);
        iconSound = uiRoot.Q<VisualElement>(soundIcon);
        musicSlider.highValue = -0;
        musicSlider.lowValue = -50;
        soundSlider.highValue = -0;
        soundSlider.lowValue = -50;

        //Events
        musicSlider.RegisterCallback<ChangeEvent<float>>(MusicChanged);
        soundSlider.RegisterCallback<ChangeEvent<float>>(SoundChanged);
    }

    private void Start()
    {
        //Init music material
        renderer = musicPlane.GetComponent<Renderer>();
        matMusic = renderer.material;

        //Init sound material
        renderer = soundPlane.GetComponent<Renderer>();
        matSound = renderer.material;
    }

    private void MusicChanged(ChangeEvent<float> slider)
    {
        float volume = slider.newValue;
        matMusic.SetFloat("_RemovedSeg", -0 - volume);
        iconMusic.transform.rotation = Quaternion.Euler(0f, 0f, (-0 -volume) * 3.33f);
        if (volume <= -50)
        {
            volume = -80f;
        }
        audioMixer.SetFloat("Music", volume);
     
        print(-0 - volume);
    }

    private void SoundChanged(ChangeEvent<float> slider)
    {
        float volume = slider.newValue;
        matSound.SetFloat("_RemovedSeg", -0 - volume);
        iconSound.transform.rotation = Quaternion.Euler(0f, 0f, (-0 - volume) * 3.33f);
        if (volume <= -50)
        {
            volume = -80f;
        }
        audioMixer.SetFloat("Sound", volume);
    }
}
