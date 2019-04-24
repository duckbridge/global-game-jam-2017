using UnityEngine;
using System.Collections;

public class ScreenshotLoader : MonoBehaviour {

    private Renderer rendererToUse;

    private Texture2D texture2D;
    private WWW www;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadImage(Renderer renderer, ScreenshotSummary screenshotSummary) {
        this.rendererToUse = renderer;
        StartCoroutine(LoadImageFromUrl(screenshotSummary));
    }

    IEnumerator LoadImageFromUrl(ScreenshotSummary screenshotSummary) {

        rendererToUse.enabled = false;

        if(rendererToUse.material.mainTexture) {
            Destroy (rendererToUse.material.mainTexture);    
        }

        texture2D = new Texture2D(4, 4, TextureFormat.DXT1, false);
        
        string fullUrl = "file://" + Application.dataPath;
        fullUrl = fullUrl.Substring(0, fullUrl.Length - 6);

        fullUrl += GameSettings.MAP_SAVE_FOLDER + System.IO.Path.DirectorySeparatorChar + 
                   GameSettings.SCREENSHOTS + System.IO.Path.DirectorySeparatorChar + 
                   GameSettings.CHOSEN_SAVE_SLOT + System.IO.Path.DirectorySeparatorChar +
                   screenshotSummary.GetName() + ".png";
            
        Logger.Log(fullUrl);

        while (true) {
            www = new WWW(fullUrl);
            yield return www;

            rendererToUse.material.mainTexture = www.texture;
            rendererToUse.enabled = true;
        }
    }

    public void CleanUp() {
        texture2D = null;

        if(www != null) {
            www.Dispose();
        }

        if(rendererToUse) {
            rendererToUse.enabled = false;
        }
    }

}
