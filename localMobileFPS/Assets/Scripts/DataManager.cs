using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ColorMaterials
{
    public Material Material;
    public Teams Team;
}
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public ColorMaterials[] Materials;
    public Dictionary<Teams, Material> AllMaterials = new Dictionary<Teams, Material>();
    private void Awake()
    {
        if (DataManager.Instance == null)
        {
            DataManager.Instance = this;
            foreach (var item in Materials)
            {
                AllMaterials.Add(item.Team, item.Material);
            }
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
