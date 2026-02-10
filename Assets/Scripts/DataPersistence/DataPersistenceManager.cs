using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Linq.Expressions;
using System.Security;
using Unity.VisualScripting;



public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string _fileName;
    private FileDataHandler _dataHandler;

    private GameData _gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    [SerializeField] private SavefileData _loadedData;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one DataPersistenceManager in this scene found.");
        instance = this;
    }

    private void Start()
    {
        //this._dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName);
        //this.dataPersistenceObjects = FindAllDataPersistenceObjects();

        //LoadGame();                                                   // TODO - change this later, we don't want to automatically load a game
    }

    public void NewGame()
    {
        // initialize our _gameData to a new GameData
        this._gameData = new GameData();
    }

    public void LoadGame()
    {
        // load saved data from file using the DataHandler
        this._gameData = _dataHandler.Load();

        // if no data can be loaded, initialize to a new game
        if (this._gameData == null)
        {
            Debug.Log("No saved data found. Starting a new game");      // TODO - change this: start a new game ONLY from Button_NewGame
            NewGame();
        }
        // TODO - push loaded data to the other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        // TODO - pass data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref _gameData);
        }


        // save that data to a file using DataHandler
        _dataHandler.Save(_gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void CallSelectFilename(string inputFilename)
    {
        _fileName = inputFilename;
        this._dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName);

        this.dataPersistenceObjects = FindAllDataPersistenceObjects();

        LoadGame();
        AddToSavefiles(inputFilename);
    }

    private void AddToSavefiles(string inputFilename)
    {
        // TODO
        // i want to have a simple list that can hold three references (strings) to savefiles

        // at first, i try to load the list (deserialize)

        AddToSaveFilesLoad(inputFilename);

        if (_loadedData == null)
        {
            Debug.Log("No savefiles found. Starting a new game");
            _loadedData = new SavefileData();
        }
   
        Debug.Log("_loadedData: " + _loadedData);

        // after loading the list, i will add this new save file
        if (_loadedData.savefile1 == null)
        {
            _loadedData.savefile1 = _fileName; 
            AddToSaveFilesSave(inputFilename);
            return;
        }
        else
        {
            if (_loadedData.savefile2 == null)
            {
                _loadedData.savefile2 = _fileName;
                AddToSaveFilesSave(inputFilename);
                return;
            }
            else
            {
                if (_loadedData.savefile3 == null)
                {
                    _loadedData.savefile3 = _fileName;
                    AddToSaveFilesSave(inputFilename);
                    return;
                }
                else Debug.LogError("You already have 3 savefiles. Delete one and try again.");
            }
        }
        AddToSaveFilesSave(inputFilename);
    }

    public void AddToSaveFilesLoad(string inputFilename)
    {
        string _savefileName = "savefiles.list.journey";
        string _fullPath = Path.Combine(Application.persistentDataPath, _savefileName);
        Debug.Log("fullPath: " + _fullPath);

        _loadedData = null;

        if (File.Exists(_fullPath))
        {
            Debug.Log("File already exists.");
            try
            {
                string dataToLoad = _savefileName;
                using (FileStream stream = new FileStream(_fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                _loadedData = JsonUtility.FromJson<SavefileData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + _fullPath + "\n" + e);
            }
        }
    }

    public void AddToSaveFilesSave(string inputFilename)
    {
        Debug.Log("File will be created.");

        string _savefileName = "savefiles.list.journey";
        string _fullPath = Path.Combine(Application.persistentDataPath, _savefileName);

        // ... if the list ist not existent, create one
        try
        {
            // create dir the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(_fullPath));

            // serialize the c# game data object into Json
            string dataToStroe = JsonUtility.ToJson(_loadedData, true);        // true for formatting | JsonUtility does not support more complex dataTypes like Dictionaries

            // write the serialized data to the file
            using (FileStream stream = new FileStream(_fullPath, FileMode.Create))           // using bc this ensures connection to file is closed when we're done reading / writing
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStroe);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + _fullPath + "\n" + e);
        }
    }


    // after adding to the list, i will save the list to the disc (serialize)




// deserialize the data from Json back into the c# object


private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
