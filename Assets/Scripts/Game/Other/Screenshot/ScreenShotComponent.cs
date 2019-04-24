using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenShotComponent : MonoBehaviour {

    public float flashDelay = .5f;
    public Animation2D pixAnimation;
    public string ssName, ssDescription;
    
   	// Use this for initialization
	void Start () {
   	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void SaveScreenShot() {
        ScreenshotSaveComponent ssSaveComponent = SceneUtils.FindObject<ScreenshotSaveComponent>();
        List<ScreenshotSummary> screenshotSummaries = ssSaveComponent.GetScreenshotSummaries();

        bool screenshotFound = false;
    
        foreach(ScreenshotSummary screenshotSummary in screenshotSummaries) {
            if(screenshotSummary.GetName().Equals(ssName) && screenshotSummary.GetDescription().Equals(ssDescription)) {
                screenshotFound = true;
                Logger.Log("Screenshot exists!");
                break;
            }
        }

        if(!screenshotFound) {
            ssSaveComponent.SaveScreenShot(ssName, ssDescription);

            Invoke("ShowPix", .1f);
            
            float fullFlashDelay =  .1f + flashDelay;
            Invoke("ShowFlashDelayed", fullFlashDelay);
        }
    }

    private void ShowPix() {
        if(pixAnimation) {
            pixAnimation.Play(true);    
        }
    }
   
    private void ShowFlashDelayed() {
        SceneUtils.FindObject<ScreenshotFlash>().ShowFlash();
    }
}
