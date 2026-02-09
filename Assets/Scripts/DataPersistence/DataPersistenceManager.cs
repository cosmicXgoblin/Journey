using UnityEngine;
using System.Linq;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string _fileName;
    private FileDataHandler _dataHandler;

    private GameData _gameData;
    private List<IDataPersistence> dataPersistenceObjects;

    public static DataPersistenceManager instance {  get; private set; }

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one DataPersistenceManager in this scene found.");
        instance = this;
    }

    private void Start()
    {
        this._dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();

        LoadGame();                                                   // TODO - change this later, we don't want to automatically load a game
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


    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
