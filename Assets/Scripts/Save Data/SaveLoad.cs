using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
 
public static class SaveLoad {
 
    public static List<SaveGame> savedGames = new List<SaveGame>();
    
             
    public static void Save() {
        SaveLoad.savedGames.Add(SaveGame.current);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create (Application.persistentDataPath + "/savedGames.ben");
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
    }   
     
    public static void Load() {
        if(File.Exists(Application.persistentDataPath + "/savedGames.ben")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.ben", FileMode.Open);
            SaveLoad.savedGames = (List<SaveGame>)bf.Deserialize(file);
            file.Close();
        } else {
            System.Console.WriteLine("Error: No Save Games Found");
        }
    }
}
