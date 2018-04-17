using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
 
public static class SaveLoad {
 
    public static SaveGame savedGame = new SaveGame();
    
             
    public static void Save() {
        savedGame.UpdateSave();
        Debug.Log("Gold when game was saved: " + savedGame.gold);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create (Application.persistentDataPath + "/savedGames.ben");
        bf.Serialize(file, savedGame);
        file.Close();
    }   
     
    public static void Load() {
        if(File.Exists(Application.persistentDataPath + "/savedGames.ben")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.ben", FileMode.Open);
            savedGame = (SaveGame)bf.Deserialize(file);
            file.Close();
        } else {
            System.Console.Write("Error: No Save Game Found");
        }
    }
}
