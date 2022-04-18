using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XmlManager
{
    private ConfigData configData = new ConfigData();
    private readonly string configPath = "config";
    private readonly string configFilename = "config.xml";

    private SaveData saveData = new SaveData();
    private readonly string savePath = "save";
    private readonly string saveFilename = "save.xml";

    private MapData mapData = new MapData();
    private readonly string mapPath = "map";
    private readonly string mapFilename = "map.xml";

    private NewSaveData newSaveData = new NewSaveData();
    private NewSaveDataLevel2 newSaveDataLevel2 = new NewSaveDataLevel2();
    private NewSaveDataLevel3 newSaveDataLevel3 = new NewSaveDataLevel3();
    private NewSaveDataLevel4 newSaveDataLevel4 = new NewSaveDataLevel4();
    private NewSaveDataLevel5 newSaveDataLevel5 = new NewSaveDataLevel5();
    private NewSaveDataLevel6 newSaveDataLevel6 = new NewSaveDataLevel6();

    private NewMapData newMapData = new NewMapData();
    private NewMapDataCode newMapDataCode = new NewMapDataCode();

    private string filePath;
    private string fileName;

    public bool DoSaveExists()
    {
        string directory = Path.Combine(Application.persistentDataPath, savePath);

        string path = Path.Combine(directory, saveFilename);

        // if file doesn't exist –> create it
        if (File.Exists(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // set type of file needed
    private void SetFileType(string fileType)
    {
        if (fileType == "config")
        {
            filePath = configPath;
            fileName = configFilename;
        }
        else if (fileType == "map")
        {
            filePath = mapPath;
            fileName = mapFilename;
        }
        else
        {
            filePath = savePath;
            fileName = saveFilename;
        }
    }

    // xml file write method
    public void SaveFile(string fileType)
    {
        SetFileType(fileType);

        string directory;

        directory = Path.Combine(Application.persistentDataPath, filePath);

        // if directory does not exist –> create it
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Combine(directory, fileName);

        if (fileType == "config")
        {
            // write through xml file
            XmlSerializer serializer = new XmlSerializer(typeof(ConfigData));
            FileStream stream = new FileStream(path, FileMode.Create);
            serializer.Serialize(stream, configData);
            stream.Close();
        }
        else if (fileType == "map")
        {
            // write through xml file
            XmlSerializer serializer = new XmlSerializer(typeof(MapData));
            FileStream stream = new FileStream(path, FileMode.Create);
            serializer.Serialize(stream, mapData);
            stream.Close();
        }
        else
        {
            // write through xml file
            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            FileStream stream = new FileStream(path, FileMode.Create);
            serializer.Serialize(stream, saveData);
            stream.Close();
        }
    }

    // if new save –> overwrite exsting with defaut values
    public void NewSaveFile()
    {
        string directory = Path.Combine(Application.persistentDataPath, savePath);

        // if directory does not exist –> create it
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Combine(directory, saveFilename);

        // write through xml file
        XmlSerializer serializer = new XmlSerializer(typeof(NewSaveData));
        FileStream stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, newSaveData);

        stream.Close();
    }

    public void NewSaveFileLevel2()
    {
        string directory = Path.Combine(Application.persistentDataPath, savePath);

        // if directory does not exist –> create it
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Combine(directory, saveFilename);

        // write through xml file
        XmlSerializer serializer = new XmlSerializer(typeof(NewSaveDataLevel2));
        FileStream stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, newSaveDataLevel2);

        stream.Close();
    }

    public void NewSaveFileLevel3()
    {
        string directory = Path.Combine(Application.persistentDataPath, savePath);

        // if directory does not exist –> create it
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Combine(directory, saveFilename);

        // write through xml file
        XmlSerializer serializer = new XmlSerializer(typeof(NewSaveDataLevel3));
        FileStream stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, newSaveDataLevel3);

        stream.Close();
    }

    public void NewSaveFileLevel4()
    {
        string directory = Path.Combine(Application.persistentDataPath, savePath);

        // if directory does not exist –> create it
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Combine(directory, saveFilename);

        // write through xml file
        XmlSerializer serializer = new XmlSerializer(typeof(NewSaveDataLevel4));
        FileStream stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, newSaveDataLevel4);

        stream.Close();
    }

    public void NewSaveFileLevel5()
    {
        string directory = Path.Combine(Application.persistentDataPath, savePath);

        // if directory does not exist –> create it
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Combine(directory, saveFilename);

        // write through xml file
        XmlSerializer serializer = new XmlSerializer(typeof(NewSaveDataLevel5));
        FileStream stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, newSaveDataLevel5);

        stream.Close();
    }

    public void NewSaveFileLevel6()
    {
        string directory = Path.Combine(Application.persistentDataPath, savePath);

        // if directory does not exist –> create it
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Combine(directory, saveFilename);

        // write through xml file
        XmlSerializer serializer = new XmlSerializer(typeof(NewSaveDataLevel6));
        FileStream stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, newSaveDataLevel6);

        stream.Close();
    }

    public void NewMapFile()
    {
        string directory = Path.Combine(Application.persistentDataPath, mapPath);

        // if directory does not exist –> create it
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Combine(directory, mapFilename);

        // write through xml file
        XmlSerializer serializer = new XmlSerializer(typeof(NewMapData));
        FileStream stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, newMapData);

        stream.Close();
    }

    public void NewMapFileCode()
    {
        string directory = Path.Combine(Application.persistentDataPath, mapPath);

        // if directory does not exist –> create it
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Combine(directory, mapFilename);

        // write through xml file
        XmlSerializer serializer = new XmlSerializer(typeof(NewMapDataCode));
        FileStream stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, newMapDataCode);

        stream.Close();
    }

    // XML FILE PUBLIC READ AND WRITE METHODS
    public void LoadFile(string fileType)
    {
        SetFileType(fileType);

        string directory;

        directory = Path.Combine(Application.persistentDataPath, filePath);

        // if directory does not exist –> create it
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Combine(directory, fileName);

        // if file doesn't exist –> create it
        if (!File.Exists(path))
        {
            SaveFile(fileType);
        }

        if (fileType == "config")
        {
            // read through xml file
            XmlSerializer serializer = new XmlSerializer(typeof(ConfigData));
            StreamReader fileStream = new StreamReader(path);
            configData = serializer.Deserialize(fileStream) as ConfigData;
            fileStream.Close();
        }
        else if (fileType == "map")
        {
            // read through xml file
            XmlSerializer serializer = new XmlSerializer(typeof(MapData));
            StreamReader fileStream = new StreamReader(path);
            mapData = serializer.Deserialize(fileStream) as MapData;
            fileStream.Close();
        }
        else
        {
            // read through xml file
            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            StreamReader fileStream = new StreamReader(path);
            saveData = serializer.Deserialize(fileStream) as SaveData;
            fileStream.Close();
        }
    }

    public void DeleteFile(string fileType)
    {
        SetFileType(fileType);
        string directory = Path.Combine(Application.persistentDataPath, filePath);
        string path = Path.Combine(directory, fileName);

        File.Delete(path);
    }

    // CONFIG PUBLIC METHODS
    public int GetMusicVolume()
    {
        return configData.configOptionsData.musicVolume;
    }

    public void SetMusicVolume(int volume)
    {
        configData.configOptionsData.musicVolume = volume;
    }

    public int GetVoicesVolume()
    {
        return configData.configOptionsData.voicesVolume;
    }

    public void SetVoicesVolume(int volume)
    {
        configData.configOptionsData.voicesVolume = volume;
    }

    public int GetSfxVolume()
    {
        return configData.configOptionsData.sfxVolume;
    }

    public void SetSfxVolume(int volume)
    {
        configData.configOptionsData.sfxVolume = volume;
    }
    public bool GetRotationStick()
    {
        return configData.configOptionsData.rotationStick;
    }

    public void SetRotationStick(bool state)
    {
        configData.configOptionsData.rotationStick = state;
    }


    // SAVE PUBLIC METHODS
    public void SetMovementsMode(string mode)
    {
        saveData.saveOptionsData.movementsMode = mode;
    }

    public string GetMovementsMode()
    {
        return saveData.saveOptionsData.movementsMode;
    }

    public void SetDimensionssMode(string mode)
    {
        saveData.saveOptionsData.dimensionsMode = mode;
    }

    public string GetDimensionsMode()
    {
        return saveData.saveOptionsData.dimensionsMode;
    }

    public void SetColorssMode(string mode)
    {
        saveData.saveOptionsData.colorsMode = mode;
    }

    public string GetColorsMode()
    {
        return saveData.saveOptionsData.colorsMode;
    }

    public void SetAudioMode(string mode)
    {
        saveData.saveOptionsData.audioMode = mode; 
    }

    public string GetAudioMode()
    {
        return saveData.saveOptionsData.audioMode;
    }

    public void SetPlayerPosition(float posX, float posY, float posZ)
    {
        saveData.saveOptionsData.playerPosition[0] = posX;
        saveData.saveOptionsData.playerPosition[1] = posY;
        saveData.saveOptionsData.playerPosition[2] = posZ;
    }

    public float[] GetPlayerPosition()
    {
        return saveData.saveOptionsData.playerPosition;
    }

    public void SetPlayerRotation(float rotX, float rotY, float rotZ)
    {
        saveData.saveOptionsData.playerRotation[0] = rotX;
        saveData.saveOptionsData.playerRotation[1] = rotY;
        saveData.saveOptionsData.playerRotation[2] = rotZ;
    }

    public float[] GetPlayerRotation()
    {
        return saveData.saveOptionsData.playerRotation;
    }


    public void SetGamePhase(int phase)
    {
        saveData.saveOptionsData.gamePhase = phase;
    }

    public int GetGamePhase()
    {
        return saveData.saveOptionsData.gamePhase;
    }

    public void SetSaveType(int saveType)
    {
        saveData.saveOptionsData.saveType = saveType;
    }

    public int GetSaveType()
    {
        return saveData.saveOptionsData.saveType;
    }

    public void SetMapCaseType(int caseID, int type)
    {
        mapData.mapOptionsData.mapCasesType[caseID] = type;
    }

    public int[] GetMapCasesType()
    {
        return mapData.mapOptionsData.mapCasesType;
    }

    public void UpdateInventory(int itemID, int state)
    {
        if (state == 0)
        {
            saveData.saveOptionsData.inventory[itemID] = state;
        }
        else
        {
            saveData.saveOptionsData.inventory[itemID]++;
        }
    }

    public int[] GetInventory()
    {
        return saveData.saveOptionsData.inventory;
    }

    public void UpdateInventoryDiscovered(int itemID, int state)
    {
        saveData.saveOptionsData.inventoryDiscovered[itemID] = state;
    }

    public int[] GetInventoryDiscovered()
    {
        return saveData.saveOptionsData.inventoryDiscovered;
    }

    public void SetQuestPhase(int questID, int questPhase)
    {
        saveData.saveOptionsData.questsPhase[questID] = questPhase;
    }

    public int GetQuestPhase(int questID)
    {
        return saveData.saveOptionsData.questsPhase[questID];
    }

    //public void SetLogText(string[] logs)
    //{
    //    saveData.saveOptionsData.logText = logs;
    //}

    //public string[] GetLogText()
    //{
    //    return saveData.saveOptionsData.logText;
    //}
    public void SetLogText(string logs)
    {
        saveData.saveOptionsData.logText += logs;
    }

    public string GetLogText()
    {
        return saveData.saveOptionsData.logText;
    }
}