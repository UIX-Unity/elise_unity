using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEffectColor", menuName = "Create New GameEffectColor")]
public class GameEffectColor : ScriptableObject
{
    [SerializeField] public Color perfectColorUpGradient;
    [SerializeField] public Color perfectColorDownGradient;
    
    [SerializeField] public Color fineColorUpGradient;
    [SerializeField] public Color fineColorDownGradient;

    [SerializeField] public Color badColorUpGradient;
    [SerializeField] public Color badColorDownGradient;

    [SerializeField] public Color comboColorUpGradient;
    [SerializeField] public Color comboColorDownGradient;
}