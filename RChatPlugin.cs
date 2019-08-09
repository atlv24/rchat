using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.RChat
{
	public class RChatPlugin : BasePlugin, IWidgetPlugin
	{
		readonly ILocalizationService _localizationService;
		readonly ISettingService      _settingService;
		readonly IWebHelper           _webHelper;
		readonly IPermissionService   _permissionService;

		public RChatPlugin
		( ILocalizationService localizationService
		, ISettingService      settingService
		, IStoreContext        storeContext
		, IWebHelper           webHelper
		, IPermissionService   permissionService
		)
		{
			_localizationService = localizationService;
			_settingService      = settingService;
			_webHelper           = webHelper;
			_permissionService   = permissionService;

			RChatSettings rChatSettings = _settingService.LoadSetting<RChatSettings>(storeContext.CurrentStore.Id);
			RChatHistory.Resize(rChatSettings.numberOfHistoryMessages);
		}

		IList<string> IWidgetPlugin.GetWidgetZones()
		{
			return new List<string> { PublicWidgetZones.BoardsForumAfterHeader, PublicWidgetZones.BoardsForumGroupAfterHeader };
		}

		string IWidgetPlugin.GetWidgetViewComponentName(string widgetZone)
		{
			return "WidgetsRChat";
		}

		public override string GetConfigurationPageUrl()
		{
			return _webHelper.GetStoreLocation() + "Admin/WidgetsRChat/Configure";
		}

		public override void Install()
		{
			// settings
			_settingService.SaveSetting(new RChatSettings
			{
				numberOfHistoryMessages = 100,
				messageCharacterLimit   = 1000,
				nameCharacterLimit      = 10,
				throttleMilliseconds    = 500,
				bannerText              = "Welcome to RChat"
			});

			_permissionService.InstallPermissions(new RChatPermissions());

			RChatHistory.Resize(100);

			// locales
			_localizationService.AddOrUpdatePluginLocaleResource("Plugins.Widgets.RChat.NumberOfHistoryMessages", "Number of History Messages");
			_localizationService.AddOrUpdatePluginLocaleResource("Plugins.Widgets.RChat.MessageCharacterLimit"  , "Message Character Limit"   );
			_localizationService.AddOrUpdatePluginLocaleResource("Plugins.Widgets.RChat.NameCharacterLimit"     , "Name Character Limit"      );
			_localizationService.AddOrUpdatePluginLocaleResource("Plugins.Widgets.RChat.ThrottleMilliseconds"   , "Throttle Milliseconds"     );
			_localizationService.AddOrUpdatePluginLocaleResource("Plugins.Widgets.RChat.BannerText"             , "Banner Text"               );

			base.Install();
		}

		public override void Uninstall()
		{
			// settings
			_settingService.DeleteSetting<RChatSettings>();

			_permissionService.UninstallPermissions(new RChatPermissions());

			// locales
			_localizationService.DeletePluginLocaleResource("Plugins.Widgets.RChat.NumberOfHistoryMessages");
			_localizationService.DeletePluginLocaleResource("Plugins.Widgets.RChat.MessageCharacterLimit"  );
			_localizationService.DeletePluginLocaleResource("Plugins.Widgets.RChat.NameCharacterLimit"     );
			_localizationService.DeletePluginLocaleResource("Plugins.Widgets.RChat.ThrottleMilliseconds"   );
			_localizationService.DeletePluginLocaleResource("Plugins.Widgets.RChat.BannerText"             );

			base.Uninstall();
		}
	}
}