using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class CollectionItemMenu : DispatchBehaviour {

    public CollectionItemDisplay collectionItemDisplay;

    public Vector2 buttonOffset;
    public Vector2 maxButtonsAllowed;
    
    protected int firstIndex = 0;
    protected int lastIndex = 0;

    private List<List<MenuButton>> menuButtons = new List<List<MenuButton>>();
    
    public List<ButtonRow> buttonRows = new List<ButtonRow>();
    
    private CollectionManager collectionManager;
    private PlayerInputActions playerInputActions;

    private bool canPressNavigationButton = true; 
    private bool isActive = false;
    private int currentX, currentY;

    private MenuButton currentButton;
    private CollectionItemMenuUpdater menuUpdater;
    
    void Awake() {
        menuUpdater = GetComponent<CollectionItemMenuUpdater>();
        playerInputActions = PlayerInputHelper.LoadData();
        LoadCassetteButtons();
    }

    public void OnMenuButtonPressed(MenuButton menuButton) {
        if(menuButton.GetComponent<AnimalItemButton>()) {

            collectionItemDisplay.gameObject.SetActive(true);
            collectionItemDisplay.Show(menuButton.GetComponent<AnimalItemButton>());
        } else if(menuButton.GetComponent<CollectionItemButton>()) {
            //make sub button type for cassettes!
        }
    
    }

    private void LoadCassetteButtons() {

        DestroyAllButtons();

        for(int i = 0; i < buttonRows.Count ; i++) {
            menuButtons.Add(buttonRows[i].buttons);
            for(int j = 0 ; j < buttonRows[i].buttons.Count; j++) {
                buttonRows[i].buttons[j].AddEventListener(this.gameObject);    
            }
        }

        collectionManager = SceneUtils.FindObject<CollectionManager>();

        UpdateVisibleButtons();
        RefreshButtonsToDisplay();
    }

    public void Start() {
        SelectFirstButton();
    }

    public void UpdateVisibleButtons() {
        if(menuUpdater) {
            menuUpdater = GetComponent<CollectionItemMenuUpdater>();
        }

        if(!collectionManager) {
            collectionManager = SceneUtils.FindObject<CollectionManager>();
        }

        menuUpdater.UpdateVisibleButtons(collectionManager, buttonRows);
    }

    public void Update () {
            
        if(!isActive) {
            return;        
        }

        if(canPressNavigationButton && playerInputActions.up.IsPressed && playerInputActions.up.Value > 0.4f) {
            canPressNavigationButton = false;
            OnMoveUp();
        
        }
        
        if(canPressNavigationButton && playerInputActions.down.IsPressed && playerInputActions.down.Value > 0.4f) {
            canPressNavigationButton = false;
            OnMoveDown();

        }

        if(canPressNavigationButton && playerInputActions.right.IsPressed && playerInputActions.right.Value > 0.4f) {
            canPressNavigationButton = false;
            OnMoveRight();

        }
        
        if(canPressNavigationButton && playerInputActions.left.IsPressed && playerInputActions.left.Value > 0.4f) {
            canPressNavigationButton = false;
            OnMoveLeft();

        }
        
		if(playerInputActions.menuSelect.LastValue == 0 && playerInputActions.menuSelect.IsPressed) {
            if(currentButton != null) {
                currentButton.OnPressed();
            }
        }

        if(!canPressNavigationButton) {
            if(playerInputActions.up.Value == 0 && playerInputActions.down.Value == 0 && playerInputActions.left.Value == 0 && playerInputActions.right.Value == 0) {
                canPressNavigationButton = true;  
            }
        }
    }

    private void DoMove(int x, int y) {

        if(currentButton) {    
            currentButton.OnUnSelected();
        }
    
        //PLAY SOUND
    
        if(y >= menuButtons.Count) {      
           y = menuButtons.Count - 1;
        }

        if(x >= menuButtons[y].Count) {      
            x = menuButtons[y].Count - 1;
        }
 
        currentButton = menuButtons[y][x];
        currentButton.OnSelected();
    }

    private void OnMoveUp() {

        --currentY;

        if(currentY < 0) {
            currentY = 0;
        }
        
        DoMove(currentX, currentY);

       lastIndex = firstIndex + ((int)maxButtonsAllowed.y);

        if(currentY < firstIndex && firstIndex > 0) {
            firstIndex--;  
        }

        RefreshButtonsToDisplay();
        
    }

    private void OnMoveDown() {

        ++currentY;
        
        if(currentY > menuButtons.Count - 1) {
            currentY = menuButtons.Count - 1;
        }

        DoMove(currentX, currentY);

        lastIndex = firstIndex + ((int)maxButtonsAllowed.y);

        if(currentY >= maxButtonsAllowed.y && lastIndex < menuButtons.Count) {
            firstIndex++;        
        }

         RefreshButtonsToDisplay();
    }

    private void OnMoveLeft() {
        
        --currentX;

        if(currentX < 0) {
            currentX = menuButtons[currentY].Count - 1;
        }
        
        DoMove(currentX, currentY);
    }

    private void OnMoveRight() {

        ++currentX;

        if(currentX > menuButtons[currentY].Count - 1) {
            currentX = 0;
        }

        DoMove(currentX, currentY);
    }

    public void SelectFirstButton() {
        currentX = 0;
        currentY = 0;

        DoMove(currentY, currentX);
    }
    
    private void RefreshButtonsToDisplay() {

        lastIndex = firstIndex + ((int)maxButtonsAllowed.y);
       
        float offsetX = 0f, offsetY = 0f;
        
        for(int y = 0; y < menuButtons.Count; y++) {
            for(int x = 0; x < menuButtons[y].Count; x++) {

                bool show = (y >= firstIndex && y < lastIndex);
                
                if(show) {
                    menuButtons[y][x].transform.localPosition = new Vector3(offsetX, -offsetY, 0f);
                    offsetX += buttonOffset.x;
                }

                menuButtons[y][x].gameObject.SetActive(show);
            }

            if(y >= firstIndex) {
                offsetY += buttonOffset.y;
            }

            offsetX = 0f;
        }
    }

    private void DestroyAllButtons() {
        for(int i = 0 ; i < menuButtons.Count ; i++) {
            for(int j = 0 ; j < menuButtons.Count ; j++) {
                MenuButton button = menuButtons[i][j];
                menuButtons[i].RemoveAt(j);
                Destroy(button.gameObject);
                j--;
            }
        }
    }

    public void SetActive() {
        this.isActive = true;
    }

    public void SetInactive() {
        this.isActive = false;

        for(int i = 0 ; i < menuButtons.Count ; i++) {
            for(int j = 0 ; j < menuButtons.Count ; j++) { 
                menuButtons[i][j].OnUnSelected();
            }
        }
    }

    public List<List<MenuButton>> GetMenuButtons() {
        return menuButtons;
    }

	public bool IsShowingDisplay() {
		return collectionItemDisplay.IsShown ();
	}

	public void CloseDisplay() {
		collectionItemDisplay.Hide ();
	}
}