using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VillagerTextBoxSwitcher : MonoBehaviour {

    public TextBoxManager[] textboxManagers;

    private Villager villager;    
    private Dictionary<string, TextBoxManager> textboxManagerByName = new Dictionary<string, TextBoxManager>();

    void Awake() {
        villager = GetComponent<Villager>();
        
        foreach(TextBoxManager tbManager in textboxManagers) {
            textboxManagerByName.Add(tbManager.name, tbManager);
        }
    }

    void Update() {

    }

    
    public void SwitchTextBoxManager(string tbManagerName) {

        TextBoxManager tbManagerFound;
        textboxManagerByName.TryGetValue(tbManagerName, out tbManagerFound);

        villager.textManager = tbManagerFound;
    }
}
