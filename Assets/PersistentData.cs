using UnityEngine;

public class PersistentData : MonoBehaviour
{
    public static PersistentData Instance { get; private set; }

    public string UpPath;
    public string LeftPath;
    public string RightPath;
    public string DownPath;
    public string DodgePath;
    public string UsePath;
    public string SwordPath;
    public string GunPath;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("I'm being destroyed for some reason");
    }
}