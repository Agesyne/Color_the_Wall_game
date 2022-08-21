using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System.Net.Sockets;
using System.Timers;
using System;

public class MenuSceneControls : MonoBehaviour
{
    private GameObject mainMenu;
    private GameObject infoMenu;
    private GameObject gameMenu;
    private GameObject inthernetSubmenu;

    private Dropdown sizeDropdown;
    private Dropdown typeDropdown;
    private Dropdown enemyDropdown;

    private GameObject connectToServerSubmenu;
    private TMPro.TMP_InputField remoteIPLabel;
    private Text connectToServerButtonLabel;

    private GameObject createServerSubmenu;
    private Text localIPLabel;
    private Text createServerButtonLabel;

    private GameObject connectionErrorSubmenu;
    private Text connectionErrorLabel;

    private GameObject creatingServerErrorSubmenu;
    private Text creatingServerLabel;



    void Start()
    {
        mainMenu = GameObject.Find("MainMenu");
        gameMenu = GameObject.Find("GameMenu");
        infoMenu = GameObject.Find("InfoMenu");
        inthernetSubmenu = GameObject.Find("InthernetMenu");

        sizeDropdown = GameObject.Find("Size_Dropdown").GetComponent<Dropdown>();
        typeDropdown = GameObject.Find("Type_Dropdown").GetComponent<Dropdown>();
        enemyDropdown = GameObject.Find("Enemy_Dropdown").GetComponent<Dropdown>();

        connectToServerSubmenu = GameObject.Find("ConnectToServer_Submenu");
        remoteIPLabel = GameObject.Find("RemoteIP_Label").GetComponent<TMPro.TMP_InputField>();
        connectToServerButtonLabel = GameObject.Find("ConnectToServer_Button").GetComponent<Text>();

        createServerSubmenu = GameObject.Find("CreateServer_Submenu");
        localIPLabel = GameObject.Find("LocalIP_Label").GetComponent<Text>();
        createServerButtonLabel = GameObject.Find("CreateServer_Button").GetComponent<Text>();

        connectionErrorSubmenu = GameObject.Find("ConnectionError_Submenu");
        connectionErrorLabel = GameObject.Find("ConnectionError_Label").GetComponent<Text>();

        creatingServerErrorSubmenu = GameObject.Find("CreatingServerError_Submenu");
        creatingServerLabel = GameObject.Find("CreatingServerError_Label").GetComponent<Text>();

        mainMenu.SetActive(true);
        gameMenu.SetActive(false);
        infoMenu.SetActive(false);
        inthernetSubmenu.SetActive(false);
        connectionErrorSubmenu.SetActive(false);
        creatingServerErrorSubmenu.SetActive(false);


        if (SceneSwitcher.IsPlayMenuLoading)
        {
            PlayButtonPressed();
        }
    }


    public void PlayButtonPressed()
    {
        sizeDropdown.value = (int) SceneSwitcher.GameFieldSize;
        typeDropdown.value = (int) SceneSwitcher.GameFieldType;
        enemyDropdown.value = (int) SceneSwitcher.GameType;

        gameMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void InfoButtonPressed()
    {
        infoMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
    }

    public void OpenInthernetMenu()
    {
        inthernetSubmenu.SetActive(true);
        gameMenu.SetActive(false);
    }

    public void RemoteIPLabelSelected()
    {
        if (remoteIPLabel.text == "XXX.XXX.XXX.XXX")
            remoteIPLabel.text = "";
    }

    public void RemoteIPLabelUnselected()
    {
        if (remoteIPLabel.text == "")
            remoteIPLabel.text = "XXX.XXX.XXX.XXX";
    }

    public void OnConnectionError(bool toMakeActive)
    {
        connectToServerSubmenu.SetActive(true);
        creatingServerErrorSubmenu.SetActive(false);

        connectionErrorSubmenu.SetActive(toMakeActive);
        createServerSubmenu.SetActive(!toMakeActive);
    }

    public void OnCreatingServerError(bool toMakeActive)
    {
        connectionErrorSubmenu.SetActive(false);
        createServerSubmenu.SetActive(true);

        connectToServerSubmenu.SetActive(!toMakeActive);
        creatingServerErrorSubmenu.SetActive(toMakeActive);
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        // The client identifier to be authenticated
        var clientId = request.ClientNetworkId;

        // Additional connection data defined by user code
        var connectionData = request.Payload;

        // Your approval logic determines the following values
        response.Approved = true;
        response.CreatePlayerObject = true;

        // If additional approval steps are needed, set this to true until the additional steps are complete
        // once it transitions from true to false the connection approval response will be processed.
        response.Pending = false;
    }

    private bool isAlreadyLaunched = false;
    public void LaunchServer()
    {
        if (isAlreadyLaunched)
        {
            isAlreadyLaunched = !isAlreadyLaunched;

            createServerButtonLabel.text = "Создать сервер";
            creatingServerLabel.text = "";
            OnCreatingServerError(false);
            return;
        }

        try
        {
            isAlreadyLaunched = !isAlreadyLaunched;
            createServerButtonLabel.text = "Отключить сервер";
            throw new SocketException();


            int port = 15352;

            NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
            NetworkManager.Singleton.StartHost();
            localIPLabel.text = "";
        }
        catch (SocketException ex)
        {
            OnCreatingServerError(true);
            creatingServerLabel.text = $"В процессе создания сервера произошла ошибка: {ex.Message}";
        }
    }


    private bool isAlreadyTriedToConnect = false;
    public void TryToConnectServer()
    {
        if (isAlreadyTriedToConnect)
        {
            isAlreadyTriedToConnect = !isAlreadyTriedToConnect;
            connectionErrorLabel.text = "";
            OnConnectionError(false);
            return;
        }

        try
        {
            throw new SocketException();

            int port = 15352;
            string remoteIP = remoteIPLabel.text;

            NetworkManager.Singleton.StartClient();
        }
        catch (SocketException ex)
        {
            isAlreadyTriedToConnect = !isAlreadyTriedToConnect;

            connectionErrorLabel.text = $"В процессе подключения к серверу произошла ошибка: {ex.Message}";
            OnConnectionError(true);
        }
    }

    public void StartGameButtonPressed()
    {
        SceneSwitcher.SetGameSettings((GameProvider.GameFieldSize) sizeDropdown.value, (GameProvider.GameFieldType) typeDropdown.value, (GameProvider.GameType) enemyDropdown.value);
        
        if (enemyDropdown.value == (int) GameProvider.GameType.VS_PLAYER_BY_INTHERNET)
            OpenInthernetMenu();
        else
            SceneSwitcher.LoadGameScene();
    }


    public void BackButtonPressed()
    {
        mainMenu.SetActive(true);
        gameMenu.SetActive(false);
        infoMenu.SetActive(false);
    }
}
