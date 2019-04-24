using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using InControl;

public class EmailDisplay : DispatchBehaviour {

    private TextMesh subjectOutput, textOutput;
    private Renderer imageOutput;
    private PlayerInputActions playerInputActions;
  
	// Use this for initialization
	void Awake () {

        subjectOutput = this.transform.Find("Container/Subject").GetComponent<TextMesh>();
        textOutput = this.transform.Find("Container/Text").GetComponent<TextMesh>();
        imageOutput = this.transform.Find("Container/Image").GetComponent<Renderer>();

        playerInputActions = PlayerInputHelper.LoadData();

 
	}

    public void Show(string subject, string text) { 
        string fullText = text;

        Regex regex = new Regex(@"\[\w+\]");
        Match match = regex.Match(text);
        if(match.Success) {

            string realValue = match.Value.Substring(1, match.Value.Length - 2);
            
           
            GetComponent<ScreenshotLoader>().LoadImage(
                imageOutput,
                new ScreenshotSummary()
                    .SetName(realValue)
                    .Build()
            );

            fullText = Regex.Replace(text, @"\[\w+\]", "");        
        }
        
        textOutput.text = fullText;
        subjectOutput.text = subject;

    }
}
