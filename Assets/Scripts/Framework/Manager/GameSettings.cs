public class GameSettings {

	public static bool SKIP_START_CUTSCENE = true;
	public const bool ERASE_SAVED_DATA = false;

	public static int MAX_HEARTS = 3;
	public const int DEFAULT_HEARTS = 3;

	public const int ANIMALS_LAYER = 8;

	public const float DEFAULT_MUSIC_VOLUME = 0.4f;
	public const float DEFAULT_FX_VOLUME = 0.4f;

	public const string INPUT_SAVE_NAME = "SecondGameInputSaveData";
	public const string FX_SAVE_NAME = "GameFXVolume";
	public const string BG_SAVE_NAME = "GameBGVolume";
	public const string USINGCONTROLLER_SAVE_NAME = "GameUsingController";

	public const string GAME_SAVE_NAME = "GameLevelSave_";
	public const string KIDDO_SAVE_NAME = "SavedKiddos_";

	public const string ALLSAVES = "AllSaves";
	public const string MAP_SAVE_FOLDER = "Data";
	public const string MAP_SAVE_NAME = "map";
	public const string PD_SAVE_NAME = "tpdat";
    public const string SD_SAVE_NAME = "settings";
    public const string SCREENSHOTS = "Screenshots";

	public const int MAX_SAVE_SLOTS = 3;
	public static int CHOSEN_SAVE_SLOT = 1;

	public static string GetFullSaveName() {
		return GAME_SAVE_NAME + CHOSEN_SAVE_SLOT;
	}

	public static string GetKiddoSaveName() {
		return KIDDO_SAVE_NAME + CHOSEN_SAVE_SLOT;
	}

	public static string GetFullSaveNameAndAllSaves() {
		return GAME_SAVE_NAME + CHOSEN_SAVE_SLOT + ALLSAVES;
	}

	public static string GetMapSaveName() {
		return MAP_SAVE_FOLDER + "/" + MAP_SAVE_NAME + CHOSEN_SAVE_SLOT + ".sgame";
	}

	public static string GetPlayerDataSaveName() {
		return MAP_SAVE_FOLDER + "/" + PD_SAVE_NAME + CHOSEN_SAVE_SLOT + ".tdata";
	}

	public static string GetPlayerDataSaveNameForSlot(int slot) {
		return MAP_SAVE_FOLDER + "/" + PD_SAVE_NAME + slot + ".tdata";
	}


    public static string GetSettingsDataSaveName() {
        return MAP_SAVE_FOLDER + "/" + SD_SAVE_NAME + ".sdata";
    }

    public static string GetScreenShotFolderForSlot(int slot) {
        return MAP_SAVE_FOLDER + "/" + SCREENSHOTS + "/" + slot;
    }

    public static string GetScreenShotFolderForCurrentSlot() {
        return MAP_SAVE_FOLDER + "/" + SCREENSHOTS + "/" + CHOSEN_SAVE_SLOT + "/";
    }

    public static string GetScreenShotDataName() {
        return MAP_SAVE_FOLDER + "/" + SCREENSHOTS + "/" + CHOSEN_SAVE_SLOT + "/" + SCREENSHOTS + ".sdata";
    }

	public static string GetMapSaveFolder() {
		return MAP_SAVE_FOLDER;
	}
}
