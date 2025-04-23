using UnityEngine;
using UnityEngine.UI;
using MNF;


public class Basic_1_Server : MonoBehaviour
{

	void Awake()
	{
		// LogManager is the log interface used by MNF.
		// The LogManager will log the logs through the UnityLogWriter class.
		LogManager.Instance.SetLogWriter(new UnityLogWriter());
		if (LogManager.Instance.Init() == false)
			Debug.Log("LogWriter init failed");

		if (TcpHelper.Instance.Start(isRunThread: false) == false)
		{
			LogManager.Instance.WriteError("TcpHelper.Instance.run() failed");
			return;
		}

		
		if (TcpHelper.Instance.StartAccept<Basic_1_ServerSession, Basic_1_ServerMessageDispatcher>(
			"127.0.0.1", "10000", 500) == false)
		{
			LogManager.Instance.WriteError("TcpHelper.Instance.StartAccept<Basic_1_ServerSession, Basic_1_ServerMessageDispatcher>() failed");
			return;
		}
	}


	void Update()
	{
		
		TcpHelper.Instance.DipatchNetworkInterMessage();
	}

	void OnDestroy()
	{
		Release();
	}

	void OnApplicationQuit()
	{
		Release();
	}


	void Release()
	{
		Debug.Log("Application ending after " + Time.time + " seconds");
		LookAround.Instance.Stop();
		TcpHelper.Instance.Stop();
		LogManager.Instance.Release();
	}
}
