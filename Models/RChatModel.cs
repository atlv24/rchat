using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.RChat.Models
{
	public class RChatModel : BaseNopModel
	{
		public int numberOfHistoryMessages { get; set; }
		public int messageCharacterLimit   { get; set; }
		public int nameCharacterLimit      { get; set; }
		public int throttleMilliseconds    { get; set; }
		public string bannerText           { get; set; }
		public bool canView                { get; set; }
		public bool canSend                { get; set; }
		public bool canEdit                { get; set; }
		public bool canFlag                { get; set; }
		public bool canDeleteOwn           { get; set; }
		public bool canDeleteOthers        { get; set; }
	}
}