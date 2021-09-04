using Items;
using UnityEngine;

public class LegendaryManager : MonoBehaviour
{
    public static LegendaryManager LegendaryManagerInstance;
    
    [SerializeField]private GameObject[] legendary;
    [SerializeField] public byte groudonIndi = 11;
    [SerializeField] public byte kyogreIndi = 12;
    [SerializeField] public byte rayIndi = 13;

    private void Awake()
    {
        LegendaryManagerInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ItemManager.ItemManagerInstance.OnLegendaryLoad += LoadLegis;
    }
    
    private void LoadLegis()
    {
        if (ItemManager.ItemManagerInstance.wasTaken[groudonIndi])
        {
            legendary[0].SetActive(false);
        }
        if (ItemManager.ItemManagerInstance.wasTaken[kyogreIndi])
        {
            legendary[1].SetActive(false);
        }
        if (ItemManager.ItemManagerInstance.wasTaken[rayIndi])
        {
            legendary[2].SetActive(false);
        }
    }
}
