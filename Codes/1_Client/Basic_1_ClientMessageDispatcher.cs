using UnityEngine.UI;
using UnityEngine;
using MNF;


public class Basic_1_ClientMessageDispatcher
    : DefaultDispatchHelper<Basic_1_ClientSession, BasicMessageDefine, BasicMessageDefine.SC>
{
	int onHi_Client(Basic_1_ClientSession session, object message)
	{
		var hiClient = (BasicMessageDefine.PACK_Hi_Client)message;

        var outputField = GameObject.FindWithTag("Output").GetComponent<InputField>();
        outputField.text = hiClient.msg;
		Debug.Log(hiClient.msg);
		
        return 0;
	}

	int onHello_Client(Basic_1_ClientSession session, object message)
	{
        var helloClient = (BasicMessageDefine.PACK_Hello_Client)message;

		var outputField = GameObject.FindWithTag("Output").GetComponent<InputField>();
		outputField.text = helloClient.msg;
        Debug.Log(helloClient.msg);

		return 0;
	}
}