using UnityEngine;

public class LoadData : MonoBehaviour
{
    public static LoadData LoadDataInstance;
    
    public delegate void LoadGame();
    public LoadGame OnLoad;
    
    private void Awake()
    {
        if (LoadDataInstance != null)
        {
            Destroy(this);
        }
        else
        {
            LoadDataInstance = this;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.continueGame)
        {
            OnLoad?.Invoke();
            GameManager.Instance.continueGame = false;
        }
    }
}
