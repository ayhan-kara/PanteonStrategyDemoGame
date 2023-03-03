using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationManager : MonoBehaviour
{
    public static InformationManager instance;

    public GameObject barrackInformationPanel;

    private void Awake()
    {
        instance = this;
    }

    public void OpenBarrackInformationPanel()
    {
        barrackInformationPanel.SetActive(true);
    }
}
