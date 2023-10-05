using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboDestroyer : MonoBehaviour
{
    public void Destroy()
    {
        gameObject.SetActive(false);
    }
}