using UnityEngine;
using UnityEngine.UI;
using Battlehub.Dispatcher;
using TMPro;
public class LoginUI : MonoBehaviour
{
    private static LoginUI instance;    // �ν��Ͻ�

    public GameObject mainTitle;
    public GameObject subTitle;
    public GameObject touchStart;
    public GameObject loginObject;
    public GameObject customLoginObject;
    public GameObject signUpObject;
    public GameObject errorObject;
    public GameObject nicknameObject;
    public GameObject updateObject;

    private TMP_InputField[] loginField;
    private TMP_InputField[] signUpField;
    private TMP_InputField nicknameField;
    private TMP_Text errorText;
    private GameObject loadingObject;
    private FadeAnimation fadeObject;

    private const byte ID_INDEX = 0;
    private const byte PW_INDEX = 1;
    private const string VERSION_STR = "Ver {0}";

    const string PlayStoreLink = "";
    const string AppStoreLink = "";
    void Awake()
    {
        instance = this;
    }

    public static LoginUI GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("LoginUI �ν��Ͻ��� �������� �ʽ��ϴ�.");
            return null;
        }
        return instance;
    }

    void Start()
    {
        mainTitle.SetActive(true);
        touchStart.SetActive(true);
        subTitle.SetActive(false);
        loginObject.SetActive(false);
        customLoginObject.SetActive(false);
        signUpObject.SetActive(false);
        errorObject.SetActive(false);
        nicknameObject.SetActive(false);
        updateObject.SetActive(false);

        loginField = customLoginObject.GetComponentsInChildren<TMP_InputField>();
        signUpField = signUpObject.GetComponentsInChildren<TMP_InputField>();
        nicknameField = nicknameObject.GetComponentInChildren<TMP_InputField>();
        errorText = errorObject.GetComponentInChildren<TMP_Text>();

        loadingObject = GameObject.FindGameObjectWithTag("Loading");
        loadingObject.SetActive(false);

        var fade = GameObject.FindGameObjectWithTag("Fade");
        if (fade != null)
        {
            fadeObject = fade.GetComponent<FadeAnimation>();
        }

        mainTitle.GetComponentInChildren<TMP_Text>().text = string.Format(VERSION_STR, Application.version); //���� ����

        var google = loginObject.transform.GetChild(2).gameObject;
#if UNITY_ANDROID
        google.SetActive(true);
#endif
    }

    public void OpenUpdatePopup()
    {
        updateObject.SetActive(true);
    }

    public void OpenStoreLink()
    {
#if UNITY_ANDROID
        Application.OpenURL(PlayStoreLink);
#elif UNITY_IOS
        Application.OpenURL(AppStoreLink);
#else
        Debug.LogError("�������� �ʴ� �÷��� �Դϴ�.");
#endif
        Debug.Log("����� ��ũ ���� ��ư Ŭ��");
    }

    public void TouchStart()
    {
        // ������Ʈ �˾��� �������� ���� X
        if (updateObject.activeSelf == true)
        {
            return;
        }

        loadingObject.SetActive(true);
        BackEndServerManager.GetInstance().BackendTokenLogin((bool result, string error) =>
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                if (result)
                {
                    ChangeLobbyScene();
                    return;
                }
                loadingObject.SetActive(false);
                if (!error.Equals(string.Empty))
                {
                    errorText.text = "���� ���� �ҷ����� ����\n\n" + error;
                    errorObject.SetActive(true);
                    return;
                }
                mainTitle.SetActive(false);
                touchStart.SetActive(false);
                subTitle.SetActive(true);
                //customLoginObject.SetActive(true);
                loginObject.SetActive(true);
            });
        });
    }

    public void Login()
    {
        if (errorObject.activeSelf)
        {
            return;
        }
        string id = loginField[ID_INDEX].text;
        string pw = loginField[PW_INDEX].text;

        if (id.Equals(string.Empty) || pw.Equals(string.Empty))
        {
            errorText.text = "ID Ȥ�� PW �� ���� �Է����ּ���.";
            errorObject.SetActive(true);
            return;
        }

        loadingObject.SetActive(true);
        BackEndServerManager.GetInstance().CustomLogin(id, pw, (bool result, string error) =>
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                if (!result)
                {
                    loadingObject.SetActive(false);
                    errorText.text = "�α��� ����\n\n" + error;
                    errorObject.SetActive(true);
                    return;
                }
                ChangeLobbyScene();
            });
        });
    }

    public void SignUp()
    {
        if (errorObject.activeSelf)
        {
            return;
        }
        string id = signUpField[ID_INDEX].text;
        string pw = signUpField[PW_INDEX].text;

        if (id.Equals(string.Empty) || pw.Equals(string.Empty))
        {
            errorText.text = "ID Ȥ�� PW �� ���� �Է����ּ���.";
            errorObject.SetActive(true);
            return;
        }

        loadingObject.SetActive(true);
        BackEndServerManager.GetInstance().CustomSignIn(id, pw, (bool result, string error) =>
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                if (!result)
                {
                    loadingObject.SetActive(false);
                    errorText.text = "ȸ������ ����\n\n" + error;
                    errorObject.SetActive(true);
                    return;
                }
                ChangeLobbyScene();
            });
        });
    }

    public void ActiveNickNameObject()
    {
        Dispatcher.Current.BeginInvoke(() =>
        {
            mainTitle.SetActive(false);
            touchStart.SetActive(false);
            subTitle.SetActive(true);
            loginObject.SetActive(false);
            customLoginObject.SetActive(false);
            signUpObject.SetActive(false);
            errorObject.SetActive(false);
            loadingObject.SetActive(false);
            nicknameObject.SetActive(true);
        });
    }

    public void UpdateNickName()
    {
        if (errorObject.activeSelf)
        {
            return;
        }
        string nickname = nicknameField.text;
        if (nickname.Equals(string.Empty))
        {
            errorText.text = "�г����� ���� �Է����ּ���";
            errorObject.SetActive(true);
            return;
        }
        loadingObject.SetActive(true);
        BackEndServerManager.GetInstance().UpdateNickname(nickname, (bool result, string error) =>
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                if (!result)
                {
                    loadingObject.SetActive(false);
                    errorText.text = "�г��� ���� ����\n\n" + error;
                    errorObject.SetActive(true);
                    return;
                }
                ChangeLobbyScene();
            });
        });
    }

    public void GuestLogin()
    {
        if (errorObject.activeSelf)
        {
            return;
        }

        loadingObject.SetActive(true);
        BackEndServerManager.GetInstance().GuestLogin((bool result, string error) =>
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                if (!result)
                {
                    loadingObject.SetActive(false);
                    errorText.text = "�α��� ����\n\n" + error;
                    errorObject.SetActive(true);
                    return;
                }
                ChangeLobbyScene();
            });
        });
    }

    void ChangeLobbyScene()
    {
        if (fadeObject != null)
        {
            GameManager_Server.GetInstance().ChangeState(GameManager_Server.GameState.MatchLobby, (bool isDone) =>
            {
                Dispatcher.Current.BeginInvoke(() => loadingObject.transform.Rotate(0, 0, -10));
                if (isDone)
                {
                    fadeObject.ProcessFadeOut();
                }
            });
        }
        else
        {
            GameManager_Server.GetInstance().ChangeState(GameManager_Server.GameState.MatchLobby);
        }
    }
}
