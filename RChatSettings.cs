using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.RChat
{
	public class RChatSettings : ISettings
	{
		public int    numberOfHistoryMessages { get; set; }
		public int    messageCharacterLimit   { get; set; }
		public int    nameCharacterLimit      { get; set; }
		public int    throttleMilliseconds    { get; set; }
		public string bannerText              { get; set; }
	}
}