using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MNF;
using UnityEngine.SceneManagement;
using MNF_Common;
using MoreMountains.NiceVibrations;

public class Data_Control : MonoBehaviour {
	public GameObject Button_StartServer, Button_StartClient, BinaryChatServerScene, 
	BinaryChatClientScene,Button_SendMessage,server_cast;
	public bool Server;
	public GameObject Aircraft;


	
	void Update()
	{
        if (!!Server)
        {
			if(Input.GetMouseButtonDown (0) && RectTransformUtility.RectangleContainsScreenPoint(server_cast.GetComponent<Image>().rectTransform,Input.mousePosition) && server_cast.activeSelf == true)
            {
                MMVibrationManager.Haptic(HapticTypes.LightImpact);

                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    server_cast.transform.GetChild(0).GetComponent<Text>().text = "NO WIFI";
                    exit();
                    MMVibrationManager.Haptic(HapticTypes.Failure);
                    
                }

                else
                {
                    onServerButton_click();
                    server_cast.GetComponent<Image>().color = server_cast.GetComponent<Image>().color == new Color32(255, 255, 255, 128) ? new Color32(0, 255, 0, 128) : new Color32(255, 255, 255, 128);
                }
            }
                 
            if(server_cast.GetComponent<Image>().color == new Color32(0, 255, 0, 128) && !!Server)
            {
                OnBroadcastBinaryChatMessage_Server();
                server_cast.transform.GetChild(0).GetComponent<Text>().text = "ACTIVE";
            }

            else server_cast.transform.GetChild(0).GetComponent<Text>().text = "";

        }           
	}

    public void onServerButton_click()
    {
		Button_StartServer.SetActive(false);
		Button_StartClient.SetActive(false);
		Button_SendMessage.SetActive(false);
		BinaryChatServerScene.SetActive(true);
		InitServer();
		LogManager.Instance.Write(LookAround.Instance.MyIP + " " + LookAround.Instance.MyPort);
    }

	public void InitServer()
	{
		LogManager.Instance.Write("Start Binary Chat Server");

		if (MNF_ChatServer.Instance.init() == true)
					LogManager.Instance.Write("MNF_ChatServer init() success");
		else
					LogManager.Instance.Write("MNF_ChatServer init() failed");
	}

	public void InitClient()
	{
		LogManager.Instance.Write("Start Binary Chat Server");

		if (MNF_ChatClient.Instance.init() == true)
					LogManager.Instance.Write("MNF_ChatServer init() success");
				
		else
					LogManager.Instance.Write("MNF_ChatServer init() failed");

	}

	public void exit()
    {
		LookAround.Instance.Stop();
		TcpHelper.Instance.Stop();
	}

	public void onClientButton_click()
	{
		Button_StartClient.SetActive(false);
		BinaryChatClientScene.SetActive(true);
        Invoke("InitClient", 5);
		LogManager.Instance.Write(LookAround.Instance.MyIP + " " + LookAround.Instance.MyPort);
	}

	//Must call this method for message

	public void OnBroadcastBinaryChatMessage_Client()
	{
		if (Aircraft != null)
		{
			if (MNF_ChatClient.Instance.IsInit == false)
			{
				LogManager.Instance.Write("MNF_ChatClient.Instance.IsInit set false");
				return;
			}

			var InputText = Mathf.Round(Aircraft.transform.localRotation.eulerAngles.x * 1000) / 1000 + "a" +
			(int)Aircraft.GetComponent<AeroplaneController>().hdgreal + "b" +
			Mathf.Round(Aircraft.transform.localRotation.eulerAngles.z * 1000) / 1000 + "c" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().angle_pitch * 1000) / 1000 + "d" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().ForwardSpeed * 100) / 100 + "e" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().Altitude * 10000) / 10000 + "f" +
			(int)Aircraft.GetComponent<AeroplaneController>().hdgdeg + "h" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().StarterEnginePower * 100) / 100 + "g" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().EnginePower * 100) / 100 + "i" +
			Mathf.Round(Aircraft.transform.localRotation.x * 1000) / 1000 + "k" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().acceleration * 100) / 100 + "l" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().vertical_speed * 100) / 100 + "m" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().angle_roll * 100) / 100 + "n" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().Throttle * 100) / 100 + "o" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().maxEnginePower * 100) / 100 + "p";
			var message = new BinaryChatMessageDefine.PACK_CS_BROADCAST_CHAT_MESSAGE();
			message.stringField = InputText.ToString();
			//message.stringField = InputText.ToString().Replace(',', '.');
			var clientSession = TcpHelper.Instance.GetFirstClient<ChatClientSession>();

			if (clientSession != null)
				clientSession.AsyncSend((int)BinaryChatMessageDefine.ENUM_CS_.CS_BROADCAST_CHAT_MESSAGE, message);
		}
	}

	public void OnBroadcastBinaryChatMessage_Server()
	{
		if (Aircraft != null)
		{
			if (MNF_ChatServer.Instance.IsInit == false)
				return;

			if (TcpHelper.Instance.GetClientSessionCount() == 0)
				return;

			var InputText = Mathf.Round(Aircraft.transform.localRotation.eulerAngles.x * 1000) / 1000 + "a" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().hdgreal * 100) / 100 + "b" +
			Mathf.Round(Aircraft.transform.localRotation.eulerAngles.z * 1000) / 1000 + "c" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().angle_pitch * 1000) / 1000 + "d" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().ForwardSpeed * 100) / 100 + "e" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().Altitude * 10000) / 10000 + "f" +
			(int)Aircraft.GetComponent<AeroplaneController>().hdgdeg + "h" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().StarterEnginePower * 100) / 100 + "g" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().EnginePower * 100) / 100 + "i" +
			Mathf.Round(Aircraft.transform.localRotation.x * 1000) / 1000 + "k" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().acceleration * 100) / 100 + "l" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().vertical_speed * 100) / 100 + "m" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().angle_roll * 100) / 100 + "n" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().Throttle * 100) / 100 + "o" +
			Mathf.Round(Aircraft.GetComponent<AeroplaneController>().maxEnginePower * 100) / 100 + "p";

			var message = new BinaryChatMessageDefine.PACK_SC_BROADCAST_CHAT_MESSAGE();
			message.stringField = InputText.ToString();
			//message.stringField = InputText.ToString().Replace(',', '.');
			var clientSessionEnumerator = TcpHelper.Instance.GetClientSessionEnumerator();

			while (clientSessionEnumerator.MoveNext())
			{
				clientSessionEnumerator.Current.Value.AsyncSend(
				(int)BinaryChatMessageDefine.ENUM_SC_.SC_BROADCAST_CHAT_MESSAGE, message);
			}
		}
	}

}
