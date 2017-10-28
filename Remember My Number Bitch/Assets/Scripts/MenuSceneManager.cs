using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class MenuSceneManager : MonoBehaviour {

    public void OpenScene (Scene scene) {
        SceneManager.LoadScene((int)scene);
    }

    public void OpenPlaySceneWithCurrentMode () {
        if (EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite.name == "contacts") {
            GetContacts();
        } else {
            PlaySceneManager.usingContacts = false;
            PlaySceneManager.languageName = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite.name;
            OpenScene(Scene.PlayScene);
        }
    }

    private void GetContacts () {
        Contacts.LoadContactList(onDone, onLoadFailed);
    }

    private void onLoadFailed (string reason) {

    }

    private void onDone () {
        PlaySceneManager.contactNumbersList = new List<int[]>();
        if (Contacts.ContactsList.Count > 0) {
            Debug.Log("CONTACT COUNT " + Contacts.ContactsList.Count);
            for (int i = 0 ; i < Contacts.ContactsList.Count ; i++) {
                Debug.Log("WTF IS GOING ON");
                Contact user = Contacts.ContactsList[i];
                Debug.Log("PHONE COUNT " + user.Phones.Count);
                for (int k = 0 ; k < user.Phones.Count ; k++) {
                    Debug.Log("K IS WTF " + k);
                    PhoneContact phone = user.Phones[k];
                    string numbersOnly = Regex.Replace(phone.Number, "[^0-9]", "");
                    Debug.Log("NUMBER IS " + numbersOnly);
                    PlaySceneManager.contactNumbersList.Add(StringToIntArray(numbersOnly));
                }
            }
        }
        PlaySceneManager.languageName = "German";
        PlaySceneManager.usingContacts = true;
        OpenScene(Scene.PlayScene);
    }

    private int[] StringToIntArray (string numString) {
        int[] intArr = new int[numString.Length];
        for (int i = 0 ; i < intArr.Length ; i++) {
            intArr[i] = System.Convert.ToInt32(numString[i] - '0');
        }
        return intArr;
    }

    private int[] IntToIntArray (int num) {
        if (num == 0)
            return new int[1] { 0 };

        List<int> digits = new List<int>();

        for (; num != 0 ; num /= 10)
            digits.Add(num % 10);

        int[] array = digits.ToArray();
        System.Array.Reverse(array);

        return array;
    }
}
