using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmailsMenu : ScrollingMenu {

    public EmailDisplay emailDisplay;
    public Transform emailsStartPosition;
    public EmailButton emailButtonPrefab;

    private bool isShowingEmailDisplay = false;

    private float offsetY = 0;
    
    public void Initialize() {
        LoadEmailButtons();
    }

    private void LoadEmailButtons() {

        DestroyAllEmailButtons();

        List<SerializableEmail> emails = SceneUtils.FindObject<PlayerSaveComponent>().GetEmails();
        for(int i = emails.Count - 1; i >= 0 ; i--) { //last email should be shown first!
             
            EmailButton emailButton = (EmailButton) GameObject.Instantiate(emailButtonPrefab, emailsStartPosition.position, Quaternion.identity);

            emailButton.transform.parent = this.transform;
            emailButton.transform.localPosition = Vector3.zero;
            emailButton.transform.localPosition += new Vector3(0f, offsetY, 0f);
            emailButton.transform.localEulerAngles = Vector3.zero;
            
            emailButton.emailID = emails[i].emailID;
            emailButton.subject = emails[i].subject;
            emailButton.emailText = emails[i].text;
            emailButton.SetText(emails[i].subject + " - " + (emails[i].isRead ? "Read" : "Unread"));

            offsetY -= buttonOffsetY;

            menuButtons.Add(emailButton);
            emailButton.AddEventListener(this.gameObject);
        }

        SelectFirstButton();
        RefreshButtonsToDisplay();
        SetActive();
    }

    public void CloseEmailDisplay() {
        emailDisplay.GetComponent<ScreenshotLoader>().CleanUp();
        DoneReadingEmail(emailDisplay);
    }

    private bool EmailButtonAlreadyExists(SerializableEmail email) {
        foreach(EmailButton emailButton in menuButtons) {
            if(emailButton.emailID == email.emailID) {
                return true;
            }
        }

        return false;
    }
    
    public void ShowEmailOfButton(EmailButton emailButton) {
        
        emailDisplay.gameObject.SetActive(true);
        emailDisplay.AddEventListener(this.gameObject);
        emailDisplay.Show(emailButton.subject, emailButton.emailText);

        SceneUtils.FindObject<PlayerSaveComponent>().MarkEmailAsRead(emailButton.emailID);
        
        //temporary
        string emailButtonText = emailButton.GetText();
        string newText = emailButtonText.Split('-')[0] + "- Read";
        emailButton.SetText(newText);
    
        isShowingEmailDisplay = true;
    }

    public void DoneReadingEmail(EmailDisplay emailDisplay) {
        emailDisplay.gameObject.SetActive(false);
        SetActive();
        isShowingEmailDisplay = false;
    }

    public bool IsShowingEmailDisplay() {
        return isShowingEmailDisplay;
    }

    private void DestroyAllEmailButtons() {
        for(int i = 0 ; i < menuButtons.Count ; i++) {
            MenuButton button = menuButtons[i];
            menuButtons.RemoveAt(i);
            Destroy(button.gameObject);
            i--;
        }
    }
}
