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

    [SerializeField] MythSynergeInformation mythSynergeInformation = new MythSynergeInformation();
    [SerializeField] AnimalSynergeInformation animalSynergeInformation = new AnimalSynergeInformation();

    public void Test()
    {
        Debug.Log(25);
    }
}

[System.Serializable]
public class MythSynergeInformation
{
    public string[] mythSynergeName;
    public string[] mythSynergeExplanation;
    public int activeCount;
}

[System.Serializable]
public class AnimalSynergeInformation
{
    public string[] animalSynergeName;
    public string[] animalSynergeExplanation;
    public int activeCount;
}

//[System.Serializable]
//public class UnitedSynergeInformation
//{
//    public string[] mythSynergeName;
//    public string[] mythSynergeExplanation;
//}
