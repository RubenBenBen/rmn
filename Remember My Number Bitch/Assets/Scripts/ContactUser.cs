using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactUser : MonoBehaviour {
    public int[] number;
    public string userName;

    public ContactUser (string userName1, int[] phoneNumber) {
        number = phoneNumber;
        userName = userName1;
    }
}
