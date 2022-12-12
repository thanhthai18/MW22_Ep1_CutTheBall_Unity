using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField] private Button btnStart;
    [SerializeField] private TextMeshProUGUI txtScore;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance.gameObject);
    }
    private void Start()
    {
       // btnStart.onClick.AddListener(GameController.instance.Set)
    }
}
