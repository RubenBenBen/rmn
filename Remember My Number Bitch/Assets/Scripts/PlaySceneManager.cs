using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneManager : MonoBehaviour {

    public static string languageName;
    public static bool usingContacts;
    public static List<int[]> contactNumbersList;
    private bool isAudio;
    private AudioSource[] audioNumberArray;
    private PlaySceneHelper playSceneHelper;

    private int[] phoneNumberArray;
    private string phoneNumberString;

    void Awake () {
        GetChildren();

        if (languageName == "eye") {
            isAudio = false;
        } else {
            isAudio = true;
            SetAudios();
        }
    }

    void Start () {
        if (usingContacts) {
            phoneNumberArray = contactNumbersList[contactNumbersList.Count - 1];
            Debug.Log(phoneNumberArray.Length);
        } else {
            CreatePhoneNumber();
        }
        StartNewRound(isAudio);
    }

    private void StartNewRound (bool isAudio) {
        if (isAudio) {
            StartCoroutine(playEngineSound());
        }
    }

    private void CreatePhoneNumber () {
        phoneNumberArray = new int[8];
        for (int i = 0 ; i < phoneNumberArray.Length ; i++) {
            phoneNumberArray[i] = Random.Range(0, 10);
            phoneNumberString += phoneNumberArray[i];
        }
        Debug.Log(phoneNumberString);
    }

    IEnumerator playEngineSound (int currentIndex = 0) {
        audioNumberArray[phoneNumberArray[currentIndex]].Play();
        yield return new WaitForSeconds(audioNumberArray[phoneNumberArray[currentIndex]].clip.length);
        currentIndex++;
        if (currentIndex >= phoneNumberArray.Length) {
            Debug.Log("END");
            //end
        } else {
            StartCoroutine(playEngineSound(currentIndex));
        }
    }

    private void GetChildren () {
        playSceneHelper = transform.Find("ScriptContainer").Find("PlaySceneHelper").GetComponent<PlaySceneHelper>();
    }

    private void SetAudios () {
        audioNumberArray = new AudioSource[10];
        Transform audioContainer = transform.Find("ScriptContainer").Find("AudioContainer");
        for (int i = 0 ; i <= 9 ; i++) {
            audioNumberArray[i] = audioContainer.Find(i + "").GetComponent<AudioSource>();
            AudioClip clip = Resources.Load<AudioClip>("Audio/Sound Numbers/" + languageName + "/" + i);
            audioNumberArray[i].clip = clip;
        }
        audioContainer.gameObject.SetActive(true);
    }
}
