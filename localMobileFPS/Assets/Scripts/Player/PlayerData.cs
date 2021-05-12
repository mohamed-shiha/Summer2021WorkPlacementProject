using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public Teams team { set 
        {
            GetComponentInChildren<MeshRenderer>().material = DataManager.Instance.AllMaterials[value];
            _team = value;
        }
        get { return _team; }
    }

    private Teams _team;

}
