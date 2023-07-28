using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactManager : MonoBehaviour
{
    [SerializeField] ArtifactList _artifactList;
    Pedestal[] _pedestals;

    // Start is called before the first frame update
    void Start()
    {
        _pedestals = FindObjectsOfType<Pedestal>();

        foreach(Pedestal pedestal in _pedestals)
        {
            for (int i = 0; i < _artifactList._artifacts.Count; i++)
            {
                if (pedestal._artifact == _artifactList._artifacts[i])
                {
                    pedestal.UpdatePedestal(_artifactList._foundArtifacts[i]);
                }
            }
        }
    }

    public void CheckCompletion()
    {
        int total = _artifactList._artifacts.Count;
        int numFound = 0;

        foreach (Pedestal pedestal in _pedestals)
        {
            if (pedestal._artifactFound) numFound++;
        }
        
        float compRatio = numFound / total;
    }
}
