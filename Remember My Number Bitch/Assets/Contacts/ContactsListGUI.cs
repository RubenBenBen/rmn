using UnityEngine;
using System.Collections;

//this is test class, it use Contacts class to access your mobile contacts and draw it on screen 
//it is just viewer , you may create your own GUI for that
public class ContactsListGUI : MonoBehaviour {
	
	public Font font ;
	public Texture2D even_contactTexture;
	public Texture2D odd_contactTexture;
	public Texture2D contact_faceTexture;
	GUIStyle style ;
	GUIStyle   	evenContactStyle ;
	GUIStyle   	oddContactStyle ;
	GUIStyle   	contactFaceStyle ;
	GUIStyle   	nonStyle2 ;
	Vector2 size ;
	float   dragTime ;
	float   dragSpeed ;

	string failString;
	// Use this for initialization
	void Start () {	
		Contacts.LoadContactList( onDone, onLoadFailed );
	}


	void onLoadFailed( string reason ) {

	}

	void onDone() {
        if (Contacts.ContactsList.Count > 0) {
            for (int i = 0 ; i < Contacts.ContactsList.Count ; i++) {
                Contact user = Contacts.ContactsList[i];
                for (int k = 0 ; i < user.Phones.Count ; k++) {
                    PhoneContact phone = user.Phones[k];
                }
            }
        }
	}
}
