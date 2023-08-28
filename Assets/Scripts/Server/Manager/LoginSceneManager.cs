using UnityEngine;
using UnityEngine.UI;
using BackEnd;


public class LoginSceneManager : MonoBehaviour
{
    private static LoginSceneManager _instance;

    public static LoginSceneManager Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField] private Canvas _loginUICanvas;
    [SerializeField] private GameObject _loginButtonGroup;
    [SerializeField] private GameObject _touchStartButton;

    public Canvas GetLoginUICanvas()
    {
        return _loginUICanvas;
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }


    }
}
