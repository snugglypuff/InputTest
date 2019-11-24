using UnityEngine;
using System.IO;

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    public static readonly string SAVE_FOLDER_SETTINGS = Application.dataPath + "/Saves/Settings/";

    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        if (!Directory.Exists(SAVE_FOLDER_SETTINGS))
        {
            Directory.CreateDirectory(SAVE_FOLDER_SETTINGS);
        }
    }
    
    private static void WriteControllerData(string saveString)
    {
        File.WriteAllText(SAVE_FOLDER_SETTINGS + "controller.txt", saveString);
    }

    public static void SaveControllerData()
    {
        ControllerData controllerData = new ControllerData
        {
            upPath = PersistentData.Instance.UpPath,
            leftPath = PersistentData.Instance.LeftPath,
            rightPath = PersistentData.Instance.RightPath,
            downPath = PersistentData.Instance.DownPath,
            dodgePath = PersistentData.Instance.DodgePath,
            usePath = PersistentData.Instance.UsePath,
            swordPath = PersistentData.Instance.SwordPath,
            gunPath = PersistentData.Instance.GunPath
        };

        string data = JsonUtility.ToJson(controllerData);
        WriteControllerData(data);
    }

    private static string ReadControllerData()
    {
        if (File.Exists(SAVE_FOLDER_SETTINGS + "controller.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER_SETTINGS + "controller.txt");
            if (saveString != null)
            {
                return saveString;
            }
        }
        return null;
    }

    public static void LoadControllerData()
    {
        string data = ReadControllerData();

        if(data != null)
        {
            ControllerData controllerData = JsonUtility.FromJson<ControllerData>(data);

            PersistentData.Instance.UpPath = controllerData.upPath;
            PersistentData.Instance.LeftPath = controllerData.leftPath;
            PersistentData.Instance.RightPath = controllerData.rightPath;
            PersistentData.Instance.DownPath = controllerData.downPath;
            PersistentData.Instance.DodgePath = controllerData.dodgePath;
            PersistentData.Instance.UsePath = controllerData.usePath;
            PersistentData.Instance.SwordPath = controllerData.swordPath;
            PersistentData.Instance.GunPath = controllerData.gunPath;
        }
        else
        {
            PersistentData.Instance.UpPath = "<Keyboard>/w";
            PersistentData.Instance.LeftPath = "<Keyboard>/a";
            PersistentData.Instance.RightPath = "<Keyboard>/d";
            PersistentData.Instance.DownPath = "<Keyboard>/s";
            PersistentData.Instance.DodgePath = "<Keyboard>/space";
            PersistentData.Instance.UsePath = "<Keyboard>/e";
            PersistentData.Instance.SwordPath = "<Mouse>/leftButton";
            PersistentData.Instance.GunPath = "<Mouse>/rightButton";
        }
    }


}

public class ControllerData
{
    public string upPath;
    public string leftPath;
    public string rightPath;
    public string downPath;
    public string dodgePath;
    public string usePath;
    public string swordPath;
    public string gunPath;
}