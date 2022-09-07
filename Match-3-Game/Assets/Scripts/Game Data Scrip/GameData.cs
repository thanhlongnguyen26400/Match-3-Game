using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


[Serializable]
public class SaveData
{
    public bool[] isActive;
    public int[] highScores;
    public int[] stars;

}

public class GameData : MonoBehaviour
{
   
    public static GameData gameData;
    public SaveData saveData;


    // Start is called before the first frame update
    void Awake()
    {
        if (gameData == null)
        {
            DontDestroyOnLoad(gameObject);
            gameData = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Load();
    }
    private void Start()
    {
    }

    public void Save()
    {
        // Create a binary formatter which can read binary files
        BinaryFormatter formatter = new BinaryFormatter();
        // Create a route from the program to the file
        FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Create);

        // Create a blank save Data
        SaveData data = new SaveData();
        data = saveData;

        // Actually save the date in the file
        formatter.Serialize(file, data);
        // close the data stream
        file.Close();

    }
    public void Load()
    {
        // CHECK if the save game file exists
        if (File.Exists(Application.persistentDataPath + "/player.dat"))
        {
            // Create a Binary Formatter
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Open);
            saveData = formatter.Deserialize(file) as SaveData;
            file.Close();
            Debug.Log("file + " + file);
        }
    }
    private void OnApplicationQuit()
    {
        Save();        
    }

    private void OnDisable()
    {
        Save();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
