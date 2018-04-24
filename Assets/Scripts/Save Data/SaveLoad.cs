using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
 
public static class SaveLoad {

    private static SaveGame savedGame = new SaveGame();

    public static SaveGame SavedGame {
        get {
            return savedGame;
        }

        set {
            savedGame = value;
        }
    }

    public static void Save() {
        SavedGame.UpdateSave();
        Debug.Log("Gold when game was saved: " + SavedGame.gold);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create (Application.persistentDataPath + "/savedGames.ben");
        bf.Serialize(file, SavedGame);
        file.Close();
    }   
     
    public static void Load() {
        if(File.Exists(Application.persistentDataPath + "/savedGames.ben")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.ben", FileMode.Open);
            SavedGame = (SaveGame)bf.Deserialize(file);
            file.Close();
        } else {
            System.Console.Write("Error: No Save Game Found");
        }
    }
}
