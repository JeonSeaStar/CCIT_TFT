using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SynergeInformation : MonoBehaviour
{
    [SerializeField] GameObject synergeInformationBoard;

    [SerializeField] TextMeshProUGUI synergeName;
    [SerializeField] TextMeshProUGUI synergeExplanation;
    [SerializeField] GameObject synergeActiveParent;

    [SerializeField] List<MythSynergeInformation> mythSynergeInformation = new List<MythSynergeInformation>();
    [SerializeField] List<AnimalSynergeInformation> animalSynergeInformation = new List<AnimalSynergeInformation>();

}

[System.Serializable]
public class MythSynergeInformation
{
    public string mythSynergeName;
    public string mythSynergeExplanation;
    public int activeCount;

    public List<string> synergeActiveExplanations = new List<string>();
}

[System.Serializable]
public class AnimalSynergeInformation
{
    public string animalSynergeName;
    public string animalSynergeExplanation;
    public int activeCount;

    public List<string> synergeActiveExplanations = new List<string>();
}

[System.Serializable]
public class UnitedSynergeInformation
{
    public string[] mythSynergeName;
    public string[] mythSynergeExplanation;
}
