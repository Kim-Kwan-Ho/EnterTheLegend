using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildInfoCleaner : IPreprocessBuildWithReport
{
#if UNITY_EDITOR
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report)
    {
        PlayerPrefs.DeleteAll();

        var equipmentInfos = Utilities.ResourcesLoader<EquipmentInfoSO>("ItemInfos");

        for (int i = 0; i < equipmentInfos.Count; i++)
            equipmentInfos[i].IsEquipped = false;

        Debug.Log("Info Cleared");
    }

#endif
}
