using UnityEngine;
using System.Collections;

public class EmailSubmitComponent : MonoBehaviour {

    private SoundObject emailReceiveSound;
    
	// Use this for initialization
	void Awake () {
	   emailReceiveSound = this.transform.Find("Sounds/EmailReceive").GetComponent<SoundObject>();
	}
	
    void Start() {
    }

	// Update is called once per frame
	void Update () {
	
	}

    public void SubmitEmail(string subject, string text) {

        PlayerSaveComponent playerSaveComponent = SceneUtils.FindObject<PlayerSaveComponent>();

        SerializableEmail email = new SerializableEmail();

        email.subject = subject;
        email.text = text;
        email.isRead = false;

        bool emailExists = false;
        
        foreach(SerializableEmail oldEmail in playerSaveComponent.GetEmails()) {
            if(oldEmail.Equals(email)) emailExists = true;
        }
    
        if(!emailExists) {
            playerSaveComponent.AddEmail(email);
            
            NewEmailDisplay newEmailDisplay = SceneUtils.FindObject<NewEmailDisplay>();
            if(newEmailDisplay) {
                newEmailDisplay.Show();
            }
        
            emailReceiveSound.Play(true);
        } else  {
            Logger.Log("email already exists!");
        }
    }
}
