using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboPooling : MonoBehaviour
{
    [SerializeField] UILabel comboLabel;
    [SerializeField] UILabel resultLabel;
    [SerializeField] GameEffectColor effect;

    [SerializeField] UnityEngine.GameObject instance;

    [SerializeField] List<UnityEngine.GameObject> pooling = new List<UnityEngine.GameObject>();
    [SerializeField] List<UnityEngine.GameObject> activePooling = new List<UnityEngine.GameObject>();

    private void CreateInstance(int type, int Combo, string result)
    {
        switch (type)
        {
            case 0:
                // Perfect
                resultLabel.gradientTop = effect.perfectColorUpGradient;
                resultLabel.gradientBottom = effect.perfectColorDownGradient;
                break;

            case 1:
                // Fine
                resultLabel.gradientTop = effect.fineColorUpGradient;
                resultLabel.gradientBottom = effect.fineColorDownGradient;
                break;

            case 2:
                // bad
                resultLabel.gradientTop = effect.badColorUpGradient;
                resultLabel.gradientBottom = effect.badColorDownGradient;
                break;

            case 3:
                // miss
                resultLabel.gradientTop = effect.comboColorUpGradient;
                resultLabel.gradientBottom = effect.comboColorDownGradient;
                break;
        }

        if (pooling.Count == 0)
        {
            UnityEngine.GameObject obj = Instantiate(instance, transform) as UnityEngine.GameObject;
            activePooling.Add(obj);
            obj.SetActive(true);
        }
        else
        {
            UnityEngine.GameObject obj = pooling[0];
            pooling.Remove(obj);
            activePooling.Add(obj);
            obj.SetActive(true);
        }
    }

    public void AddPooling(UnityEngine.GameObject obj)
    {
        activePooling.Remove(obj);
        pooling.Add(obj);
    }

    public void PlayCombo(int type, int Combo, string result)
    {
        switch (type)
        {
            case 0:
                // Perfect
                resultLabel.gradientTop = effect.perfectColorUpGradient;
                resultLabel.gradientBottom = effect.perfectColorDownGradient;
                CreateInstance(type, Combo, result);
                break;

            case 1:
                // Fine
                resultLabel.gradientTop = effect.fineColorUpGradient;
                resultLabel.gradientBottom = effect.fineColorDownGradient;
                CreateInstance(type, Combo, result);
                break;

            case 2:
                // bad

                break;

            case 3:
                // miss

                break;
        }
    }
}