using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private bool gamePaused = false;

    // congif manager
    private XmlManager xmlManager;

    // save / new game / new game with code
    private bool newGameWithCode;

    private bool continueGame;

    private bool postCredits;
    private bool canSkipCredits;

    private bool firstStart = true;

    private string finalChoice;

    protected override void Awake()
    {
        base.Awake();

        // access to xml
        xmlManager = new XmlManager();

        // load config
        LoadFile("config");
    }

    // PUBLIC METHODS
    public bool getPauseState()
    {
        return gamePaused;
    }

    public void setPauseState(bool state)
    {
        gamePaused = state;
    }

    public void RemovePause()
    {
        gamePaused = false;
        Time.timeScale = 1;
    }

    public bool GetContinueGame()
    {
        return continueGame;
    }

    public void SetContinueGame(bool state)
    {
        continueGame = state;
    }

    public bool GetNewGameWithCode()
    {
        return newGameWithCode;
    }

    public void SetNewGameWithCode(bool state)
    {
        newGameWithCode = state;
    }

    public bool GetPostCredits()
    {
        return postCredits;
    }

    public void SetPostCredits(bool state)
    {
        postCredits = state;
    }

    public bool GetCanSkipCredits()
    {
        return canSkipCredits;
    }

    public void SetCanSkipCredits(bool state)
    {
        canSkipCredits = state;
    }

    public bool GetFirstStart()
    {
        return firstStart;
    }

    public void SetFirstStart(bool state)
    {
        firstStart = state;
    }

    public string GetFinalChoice()
    {
        return finalChoice;
    }

    public void SetFinalChoice(string choice)
    {
        finalChoice = choice;
    }

    // XML PUBLIC METHODS
    public void LoadFile(string fileType)
    {
        xmlManager.LoadFile(fileType);
    }

    public void SaveFile(string fileType)
    {
        xmlManager.SaveFile(fileType);
    }

    public void NewSave()
    {
        xmlManager.NewSaveFile();
    }

    public void NewSaveLevel2()
    {
        xmlManager.NewSaveFileLevel2();
    }

    public void NewSaveLevel3()
    {
        xmlManager.NewSaveFileLevel3();
    }

    public void NewSaveLevel4()
    {
        xmlManager.NewSaveFileLevel4();
    }

    public void NewSaveLevel5()
    {
        xmlManager.NewSaveFileLevel5();
    }

    public void NewSaveLevel6()
    {
        xmlManager.NewSaveFileLevel6();
    }

    public void NewMap()
    {
        xmlManager.NewMapFile();
    }

    public void NewMapCode()
    {
        xmlManager.NewMapFileCode();
    }

    // CONFIG XML METHODS
    public int GetMusicVolume()
    {
        return xmlManager.GetMusicVolume();
    }

    public void SetMusicVolume(int volume)
    {
        xmlManager.SetMusicVolume(volume);
        xmlManager.SaveFile("config");
    }

    public int GetVoicesVolume()
    {
        return xmlManager.GetVoicesVolume();
    }

    public void SetVoicesVolume(int volume)
    {
        xmlManager.SetVoicesVolume(volume);
        xmlManager.SaveFile("config");
    }

    public int GetSfxVolume()
    {
        return xmlManager.GetSfxVolume();
    }

    public void SetSfxVolume(int volume)
    {
        xmlManager.SetSfxVolume(volume);
        xmlManager.SaveFile("config");
    }

    public bool GetRotationStick()
    {
        return xmlManager.GetRotationStick();
    }

    public void SetRotationStick(bool state)
    {
        xmlManager.SetRotationStick(state);
        xmlManager.SaveFile("config");
    }

    // SAVE XML METHODS
    public void SetGamePhase(int phase)
    {
        xmlManager.SetGamePhase(phase);
    }

    public int GetGamePhase()
    {
        return xmlManager.GetGamePhase();
    }

    public void SetMovementsMode(string mode)
    {
        xmlManager.SetMovementsMode(mode);
    }

    public string GetMovementsMode()
    {
        return xmlManager.GetMovementsMode();
    }

    public void SetDimensionssMode(string mode)
    {
        xmlManager.SetDimensionssMode(mode);
    }

    public string GetDimensionsMode()
    {
        return xmlManager.GetDimensionsMode();
    }

    public void SetColorssMode(string mode)
    {
        xmlManager.SetColorssMode(mode);
    }

    public string GetColorsMode()
    {
        return xmlManager.GetColorsMode();
    }

    public void SetAudioMode(string mode)
    {
        xmlManager.SetAudioMode(mode);
    }

    public string GetAudioMode()
    {
        return xmlManager.GetAudioMode();
    }

    public void SetPlayerPosition(Vector3 playerPos)
    {
        xmlManager.SetPlayerPosition(playerPos.x, playerPos.y, playerPos.z);
    }

    public Vector3 GetPlayerPosition()
    {
        float[] pos = xmlManager.GetPlayerPosition();

        Vector3 playerPos = new Vector3(pos[0], pos[1], pos[2]);

        return playerPos;
    }

    public void SetPlayerRotation(Vector3 playerRot)
    {
        xmlManager.SetPlayerRotation(playerRot.x, playerRot.y, playerRot.z);
    }

    public Vector3 GetPlayerRotation()
    {
        float[] rot = xmlManager.GetPlayerRotation();

        Vector3 playerRot = new Vector3(rot[0], rot[1], rot[2]);

        return playerRot;
    }

    public int GetSaveType()
    {
        return xmlManager.GetSaveType();
    }

    public void SetMapCaseType(int caseID, int type)
    {
        xmlManager.SetMapCaseType(caseID, type);
    }

    public int[] GetMapCasesType()
    {
        return xmlManager.GetMapCasesType();
    }

    public void UpdateInventory(int itemID, int state)
    {
        xmlManager.UpdateInventory(itemID, state);
    }

    public int[] GetInventory()
    {
        return xmlManager.GetInventory();
    }

    public void UpdateInventoryDiscovered(int itemID, int state)
    {
        xmlManager.UpdateInventoryDiscovered(itemID, state);
    }

    public int[] GetInventoryDiscovered()
    {
        return xmlManager.GetInventoryDiscovered();
    }

    public void SetQuestPhase(int questID, int questPhase)
    {
        xmlManager.SetQuestPhase(questID, questPhase);
    }

    public int GetQuestPhase(int questID)
    {
        return xmlManager.GetQuestPhase(questID);
    }

    public void AddToLogText(string log)
    {
        xmlManager.SetLogText(log);

        //string[] currentLogs = GetLogText();

        //// check if log already exists
        //int currentLogID = System.Array.IndexOf(currentLogs, log);

        //if (currentLogID != -1)
        //{
        //    // move all logs after the one to move
        //    for (int i = currentLogID; i < currentLogs.Length - 1; i++)
        //    {
        //        currentLogs[i] = currentLogs[i + 1];
        //    }
        //}

        //// get first empty log
        //int firstEmpty = System.Array.IndexOf(currentLogs, "");

        //currentLogs[firstEmpty] = log;

        //xmlManager.SetLogText(currentLogs);

        //// BUG ON OCULUS APP !!!!!!!!!!!!!!!!!
        //// check if log already exists
        //string[] currentLogs = GetLogText();

        //int currentLogID = System.Array.IndexOf(currentLogs, log);

        //if (currentLogID != -1)
        //{
        //    // move all logs after the one to move
        //    for (int i = currentLogID; i < currentLogs.Length - 1; i++)
        //    {
        //        currentLogs[i] = currentLogs[i + 1];
        //    }

        //    currentLogs[currentLogs.Length - 1] = log;
        //}
        //else if (currentLogs[0] == "")
        //{
        //    currentLogs[0] = log;
        //}
        //else
        //{
        //    string[] tmpArray = new string[currentLogs.Length + 1];
        //    currentLogs.CopyTo(tmpArray, 0);
        //    tmpArray[currentLogs.Length] = log;
        //    currentLogs = tmpArray;
        //}

        //xmlManager.SetLogText(currentLogs);
    }

    //public string[] GetLogText()
    //{
    //    return xmlManager.GetLogText();
    //}

    public string GetLogText()
    {
        return xmlManager.GetLogText();
    }

    public void SetSaveType(int saveType)
    {
        xmlManager.SetSaveType(saveType);
    }
}