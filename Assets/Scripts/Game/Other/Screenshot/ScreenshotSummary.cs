using UnityEngine;
using System.Collections;

public class ScreenshotSummary {

    private string name, description, url;

    public ScreenshotSummary SetName(string name){ 
        this.name = name;
        return this;
    }

    public ScreenshotSummary SetDescription(string description){ 
        this.description = description;
        return this;
    }

    public ScreenshotSummary SetUrl(string url){ 
        this.url = url;
        return this;
    }

    public string GetName(){ 
        return this.name;
    }

    public string GetDescription(){ 
        return this.description;
    }

    public string GetUrl(){ 
        return this.url;
    }

    public ScreenshotSummary Build() {
        return this;
    }
}
