using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlaySceneManager : MonoBehaviour {

    public static string languageName;
    public static bool usingContacts;
    public static List<ContactUser> contactNumbersList;

    private bool isAudio;
    private AudioSource[] audioNumberArray;
    private PlaySceneHelper playSceneHelper;

    private int[] phoneNumberArray;
    private string phoneNumberString;

    private int minDigits = 5;
    private int maxDigits = 20;
    private int currentNumberLength;
    private static int levelsPassed;

    private GameObject audioModePanel;
    private GameObject eyeModePanel;
    private GameObject inputPanel;

    private float eyeModeShowDuration;

    public ShowTimerManager showTimerManager;

    private TouchScreenKeyboard keyboard;
    private bool endedTyping;

    private float typeTime;
    private int repeatCount;

    private GameObject resultsPanel; //delete

    private void GetChildren () {
        playSceneHelper = transform.Find("ScriptContainer").Find("PlaySceneHelper").GetComponent<PlaySceneHelper>();
        audioModePanel = transform.Find("AudioModePanel").gameObject;
        eyeModePanel = transform.Find("EyeModePanel").gameObject;
        resultsPanel = transform.Find("ResultsPanel").gameObject;
    }

    void Awake () {
        GetChildren();
        
        if (languageName == "eye") {
            isAudio = false;
        } else {
            isAudio = true;
            audioModePanel.transform.Find("LanguageNamePanel").Find("Text").GetComponent<Text>().text = languageName;
            SetAudios();
        }

        if (usingContacts) {
            phoneNumberArray = contactNumbersList[levelsPassed].number;
            phoneNumberString = IntArrayToString(phoneNumberArray);
        } else {
            CreatePhoneNumber();
        }
        StartNewRound();
    }

    private void StartNewRound () {
        currentNumberLength = phoneNumberArray.Length;
        repeatCount = RepeatCountFromArray(phoneNumberArray);
        if (isAudio) {
            audioModePanel.SetActive(true);
            Invoke("StartAudio", 1);
        } else {
            eyeModeShowDuration = 5;
            eyeModePanel.transform.Find("NumberPanel").Find("Text").GetComponent<Text>().text = phoneNumberString;
            eyeModePanel.SetActive(true);
            StartTimer();
        }
    }

    void Update () {
        if (!endedTyping && keyboard != null) {
            if (keyboard.active) {
                typeTime += Time.deltaTime;
            } else if (keyboard.done) {
                endedTyping = true;
                if (keyboard.text == phoneNumberString) {
                    ShowResults();
                    Invoke("StartNextRound", 3);
                } else {
                    ShowResults();
                }
                keyboard = null;
            }
        }
    }

    private void StartNextRound () {
        levelsPassed++;
        SceneManager.LoadScene((int) Scene.PlayScene);
    }

    private void ShowResults () {
        Text resultText = resultsPanel.transform.Find("Text").GetComponent<Text>();
        string typeTimeString = "Typing Time: " + typeTime + "\n";
        string repeatsCountString = "Repeats Count: " + repeatCount + "\n";
        string numberLengthString = "Number Length: " + phoneNumberArray.Length + "\n";
        string levelsPassedString = "Levels Passed: " + levelsPassed + "\n";
        resultText.text = typeTimeString + repeatsCountString + numberLengthString + levelsPassedString;
        resultsPanel.SetActive(true);
    }

    private void TimeEnded () {
        OpenKeyboard();
    }

    private void OpenKeyboard () {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad, false, false, false, false, "Enter Number");
        endedTyping = false;
    }

    private void StartAudio () {
        StartCoroutine(playEngineSound());
    }

    IEnumerator playEngineSound (int currentIndex = 0) {
        audioNumberArray[phoneNumberArray[currentIndex]].Play();
        yield return new WaitForSeconds(audioNumberArray[phoneNumberArray[currentIndex]].clip.length);
        currentIndex++;
        if (currentIndex >= phoneNumberArray.Length) {
            Debug.Log("END");
            audioModePanel.SetActive(false);
            TimeEnded();
        } else {
            StartCoroutine(playEngineSound(currentIndex));
        }
    }

    private void StartTimer () {
        showTimerManager.duration = eyeModeShowDuration;
        showTimerManager.TimeEndedFunction = TimeEndedDelegateFunction;
        showTimerManager.gameObject.SetActive(true);
    }

    private void TimeEndedDelegateFunction () {
        Debug.Log("Timer ENDED");
        eyeModePanel.SetActive(false);
        TimeEnded();
    }

    private void CreatePhoneNumber () {
        int numberLength = Mathf.Min(minDigits + levelsPassed, maxDigits);
        phoneNumberArray = new int[numberLength];
        for (int i = 0 ; i < phoneNumberArray.Length ; i++) {
            phoneNumberArray[i] = Random.Range(0, 10);
            phoneNumberString += phoneNumberArray[i];
        }
    }

    private int RepeatCountFromList (List<int> list) {
        return list.Count - list.Distinct().ToList().Count;
    }

    private int RepeatCountFromArray (int[] arr) {
        return RepeatCountFromList(arr.ToList());
    }

    private string IntArrayToString (int[] arr) {
        string result = "";
        for (int i = 0 ; i < arr.Length ; i++) {
            result += arr[i];
        }
        return result;
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

    public void BackPressed () {
        levelsPassed = 0;
        SceneManager.LoadScene((int) Scene.MenuScene);
    }
}
