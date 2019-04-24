using UnityEngine;
using System.Collections;

public class ScrollingMenu : Menu {

    public float buttonOffsetY = 10;
    public int maxButtonsAllowed = 5;
    
    protected int firstIndex = 0;
    protected int lastIndex = 0;

    protected override void OnMoveToNextButton() {
        base.OnMoveToNextButton();

        lastIndex = firstIndex + maxButtonsAllowed;

        if(currentIndex >= maxButtonsAllowed && lastIndex < menuButtons.Count) {
            firstIndex++;        
        }

        RefreshButtonsToDisplay();
    }

    protected override void OnMoveToPreviousButton() {
        base.OnMoveToPreviousButton();

        lastIndex = firstIndex + maxButtonsAllowed;

        if(currentIndex < firstIndex && firstIndex > 0) {
            firstIndex--;  
        }

        RefreshButtonsToDisplay();
    }

    protected virtual void RefreshButtonsToDisplay() {
        
        lastIndex = firstIndex + maxButtonsAllowed;
       
        float offsetY = 0f;

        for(int i = 0; i < menuButtons.Count; i++) {
     
            bool show = (i >= firstIndex && i < lastIndex);
            
            if(show) {
                offsetY -= buttonOffsetY;
                menuButtons[i].transform.localPosition = new Vector3(0f, offsetY, 0f);
            }

            menuButtons[i].gameObject.SetActive(show);
        }
    }
}
