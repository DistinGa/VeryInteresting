using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EditZones : MonoBehaviour
{
    [ContextMenu("Numerate zones")]
    void Numerate()
    {
        foreach (var item in GetComponentsInChildren<TMPro.TextMeshPro>())
        {
            item.text = (item.transform.parent.GetSiblingIndex() + 1).ToString();
            EditorUtility.SetDirty(item);
            PrefabUtility.RecordPrefabInstancePropertyModifications(item);
        }
    }
}
