using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Broken Color", menuName = "Data/BrokenColor")]
public class BrokenColor : ScriptableObject
{
    public Color[] brokenColor;
    public int MaxLevel => (brokenColor == null) ? 0 : brokenColor.Length - 1;
}
