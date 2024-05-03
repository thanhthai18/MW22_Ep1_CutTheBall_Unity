using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DGPlay : MonoBehaviour
{
    private DGUtilis[] dGUtilises;

    public void Awake()
    {
        dGUtilises=GetComponents<DGUtilis>();    
    }

    public void Play()
    {
        foreach (var dG in dGUtilises)
        {
            dG.Play();
        }
    }

}

