using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.RChat.Models
{
	public class ConfigurationModel : BaseNopModel
	{
		public int ActiveStoreScopeConfiguration { get; set; }


		[NopResourceDisplayName("Plugins.Widgets.RChat.NumberOfHistoryMessages")]
		public int numberOfHistoryMessages { get; set; }
		public bool numberOfHistoryMessages_OverrideForStore { get; set; }

		[NopResourceDisplayName("Plugins.Widgets.RChat.MessageCharacterLimit")]
		public int messageCharacterLimit { get; set; }
		public bool messageCharacterLimit_OverrideForStore { get; set; }

		[NopResourceDisplayName("Plugins.Widgets.RChat.NameCharacterLimit")]
		public int nameCharacterLimit { get; set; }
		public bool nameCharacterLimit_OverrideForStore { get; set; }

		[NopResourceDisplayName("Plugins.Widgets.RChat.ThrottleMilliseconds")]
		public int throttleMilliseconds { get; set; }
		public bool throttleMilliseconds_OverrideForStore { get; set; }

		[NopResourceDisplayName("Plugins.Widgets.RChat.BannerText")]
		public string bannerText { get; set; }
		public bool bannerText_OverrideForStore { get; set; }
	}
}