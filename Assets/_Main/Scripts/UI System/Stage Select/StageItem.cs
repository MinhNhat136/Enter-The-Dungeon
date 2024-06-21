using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageItem : MonoBehaviour
{
    private Button button;
    [SerializeField] private string scene; 

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(LoadScene);
    }

    private void LoadScene()
    {
        if (!string.IsNullOrEmpty(scene))
        {
            SceneManager.LoadScene(scene);
        }
        else
        {
            Debug.LogWarning("Scene name is not set.");
        }
    }
}