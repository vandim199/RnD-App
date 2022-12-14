using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Preset Option")]
public class PresetOptions : ScriptableObject
{
    public TextAsset startPreset;
    public List<TextAsset> options;
}
