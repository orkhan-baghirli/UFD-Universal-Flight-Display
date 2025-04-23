using UnityEngine.UI;
using UnityEngine;
using MNF;

public class Basic_1_Client : MonoBehaviour
{
  
	void Awake()
    {
		
		LogManager.Instance.SetLogWriter(new UnityLogWriter());
		if (LogManager.Instance.Init() == false)
			Debug.Log("LogWriter init failed");

		
		if (TcpHelper.Instance.Start(isRunThread: false) == false)
		{
			LogManager.Instance.WriteError("TcpHelper.Instance.run() failed");
			return;
		}

		
		if (TcpHelper.Instance.AsyncConnect<Basic_1_ClientSession, Basic_1_ClientMessageDispatcher>("127.0.0.1", "10000") == null)
		{
			LogManager.Instance.WriteError("TcpHelper.Instance.AsyncConnect() failed");
			return;
		}
	}

	
	void Update()
	{
		// The message received by Basic_1_ClientSession is managed as a queue inside the MNF,
		TcpHelper.Instance.DipatchNetworkInterMessage();
	}

	public void OnHiServerSend()
	{
        var hiServer = new BasicMessageDefine.PACK_Hi_Server();
        hiServer.msg = "Hi, Server. I'm Client.";
        var session = TcpHelper.Instance.GetFirstClient<Basic_1_ClientSession>();
        session.AsyncSend((int)BasicMessageDefine.CS.Hi_Server, hiServer);
	}

	public void OnHelloServerSend()
	{
		var helloServer = new BasicMessageDefine.PACK_Hello_Server();
		helloServer.msg = "Hello, Server. I'm Client.";
		var session = TcpHelper.Instance.GetFirstClient<Basic_1_ClientSession>();
		session.AsyncSend((int)BasicMessageDefine.CS.Hello_Server, helloServer);
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
