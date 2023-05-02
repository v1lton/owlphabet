using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class StartManager : MonoBehaviour
{
    public GameObject StartMenu;
    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        SetVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Play()
    {
        SceneManager.LoadScene("Owlphabet", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void UpdateVolume() {
        float volume = volumeSlider.value;
        PlayerPrefs.SetFloat("volume", volume);
        AudioListener.volume = volume;
    }

    private void SetVolume() {
        float volume = PlayerPrefs.GetFloat("volume", 1f);
        volumeSlider.value = volume;
        UpdateVolume();
    }
}
