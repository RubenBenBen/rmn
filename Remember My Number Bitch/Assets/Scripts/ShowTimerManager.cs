using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTimerManager : MonoBehaviour {

    public float duration;
    private Image myImage;
    private Text myText;
    private float rate;
    private bool timerIsOn;

    public delegate void TimerEndedFunction ();
    public TimerEndedFunction TimeEndedFunction;

    void Awake () {
        myImage = GetComponent<Image>();
        myText = transform.Find("Text").GetComponent<Text>();
        ShowTimer();
    }

    void Update () {
        if (timerIsOn) {
            if (myImage.fillAmount > 0) {
                myImage.fillAmount = myImage.fillAmount - rate * Time.deltaTime;
                myText.text = (myImage.fillAmount * duration).ToString("F1");
            } else {
                TurnOffTimer();
            }
        }
    }

    public void TurnOffTimer () {
        timerIsOn = false;
        gameObject.SetActive(false);
        myImage.fillAmount = 1;
        TimeEndedFunction();
    }

    public void ShowTimer () {
        rate = myImage.fillAmount / duration;
        timerIsOn = true;
    }
}
