using UnityEngine;
using System;
using System.IO;
using System.Security;
using System.Linq.Expressions;

public class FileDataHandler
{
    private string dataDirPath = "";            // directory path of where we want to save our data
    private string dataFileName = "";           // name of the file we want to save to

    // public constructor
    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        { 
            try
            {
                // load serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // deserialize the data from Json back into the c# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        //string fullPath = dataDirPath + "/" + dataFileName;           // NO - diff system have different file seperators
        string fullPath = Path.Combine(dataDirPath, dataFileName);      // accounts for different OS's

        try
        {
            // create dir the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize the c# game data object into Json
            string dataToStroe = JsonUtility.ToJson(data, true);        // true for formatting | JsonUtility does not support more complex dataTypes like Dictionaries

            // write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))           // using bc this ensures connection to file is closed when we're done reading / writing
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStroe);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }
}
