namespace Nop.Plugin.Widgets.RChat
{
	public class RChatHistory
	{
		static RChatMessage[] messages;
		static int head = -1;
		static int index;
		public static void AddMessage(int id, string username, string message)
		{
			lock (messages)
			{
				head++;
				head %= messages.Length;
				messages[head] = new RChatMessage{
					id       = id,
					username = username,
					message  = message,
					index    = index++
				};
			}
		}

		public static int GetMessages(RChatMessage[] result)
		{
			lock (messages)
			{
				int len = messages.Length;
				for (int i = 0; i < System.Math.Min(result.Length, len); i++)
				{
					RChatMessage message = messages[((head - i) + len) % len];
					if (message.message == null) return i;
					result[i] = message;
				}
			}
			return result.Length;
		}

		public static void Resize(int size)
		{
			if (messages == null || messages.Length != size) messages = new RChatMessage[size];
		}
	}
}