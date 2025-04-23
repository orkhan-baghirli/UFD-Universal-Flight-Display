using MNF;

public class Basic_1_ClientSession : JsonSession
{

	public override int OnConnectSuccess()
	{
		LogManager.Instance.Write("onConnectSuccess : {0}:{1}", ToString(), GetType());

		// Add connected client.
		TcpHelper.Instance.AddClientSession(GetHashCode(), this);
		return 0;
	}


	public override int OnConnectFail()
	{
		LogManager.Instance.Write("onConnectFail : {0}:{1}", ToString(), GetType());
		return 0;
	}

	
	public override int OnDisconnect()
	{
		LogManager.Instance.Write("onDisconnect : {0}:{1}", ToString(), GetType());

		// Remove disconnected client.
		TcpHelper.Instance.RemoveClientSession(GetHashCode());
		return 0;
	}
}