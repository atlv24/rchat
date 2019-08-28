using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.RChat.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework;

namespace Nop.Plugin.Widgets.RChat.Controllers
{
	[Area(AreaNames.Admin)]
	public class WidgetsRChatController : BasePluginController
	{
		readonly IStoreContext        _storeContext;
		readonly INotificationService _notificationService;
		readonly IPermissionService   _permissionService;
		readonly ISettingService      _settingService;
		readonly ILocalizationService _localizationService;

		public WidgetsRChatController
		( IStoreContext        storeContext
		, INotificationService notificationService
		, IPermissionService   permissionService
		, ISettingService      settingService
		, ILocalizationService localizationService
		)
		{
			_storeContext        = storeContext;
			_notificationService = notificationService;
			_permissionService   = permissionService;
			_settingService      = settingService;
			_localizationService = localizationService;
		}

		public IActionResult Configure()
		{
			if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
				return AccessDeniedView();

			// load settings for a chosen store scope
			var storeScope = _storeContext.ActiveStoreScopeConfiguration;
			var rChatSettings = _settingService.LoadSetting<RChatSettings>(storeScope);
			var model = new ConfigurationModel
			{
				numberOfHistoryMessages = rChatSettings.numberOfHistoryMessages,
				messageCharacterLimit   = rChatSettings.messageCharacterLimit,
				nameCharacterLimit      = rChatSettings.nameCharacterLimit,
				throttleMilliseconds    = rChatSettings.throttleMilliseconds,
				bannerText              = rChatSettings.bannerText,
				ActiveStoreScopeConfiguration = storeScope
			};

			if (storeScope > 0)
			{
				model.numberOfHistoryMessages_OverrideForStore = _settingService.SettingExists(rChatSettings, x => x.numberOfHistoryMessages, storeScope);
				model.messageCharacterLimit_OverrideForStore   = _settingService.SettingExists(rChatSettings, x => x.messageCharacterLimit  , storeScope);
				model.nameCharacterLimit_OverrideForStore      = _settingService.SettingExists(rChatSettings, x => x.nameCharacterLimit     , storeScope);
				model.throttleMilliseconds_OverrideForStore    = _settingService.SettingExists(rChatSettings, x => x.throttleMilliseconds   , storeScope);
				model.bannerText_OverrideForStore              = _settingService.SettingExists(rChatSettings, x => x.bannerText             , storeScope);
			}

			return View("~/Plugins/Widgets.RChat/Views/Configure.cshtml", model);
		}

		[HttpPost]
		public IActionResult Configure(ConfigurationModel model)
		{
			if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
				return AccessDeniedView();

			// load settings for a chosen store scope
			var storeScope = _storeContext.ActiveStoreScopeConfiguration;
			var rChatSettings = _settingService.LoadSetting<RChatSettings>(storeScope);

			if (rChatSettings.numberOfHistoryMessages != model.numberOfHistoryMessages)
			{
				RChatHistory.Resize(model.numberOfHistoryMessages);
			}

			rChatSettings.numberOfHistoryMessages = model.numberOfHistoryMessages;
			rChatSettings.messageCharacterLimit   = model.messageCharacterLimit  ;
			rChatSettings.nameCharacterLimit      = model.nameCharacterLimit     ;
			rChatSettings.throttleMilliseconds    = model.throttleMilliseconds   ;
			rChatSettings.bannerText              = model.bannerText             ;

			// We do not clear cache after each setting update.
			// This behavior can increase performance because cached settings
			// will not be cleared and loaded from database after each update.
			_settingService.SaveSettingOverridablePerStore(rChatSettings, x => x.numberOfHistoryMessages, model.numberOfHistoryMessages_OverrideForStore, storeScope, false);
			_settingService.SaveSettingOverridablePerStore(rChatSettings, x => x.messageCharacterLimit  , model.messageCharacterLimit_OverrideForStore  , storeScope, false);
			_settingService.SaveSettingOverridablePerStore(rChatSettings, x => x.nameCharacterLimit     , model.nameCharacterLimit_OverrideForStore     , storeScope, false);
			_settingService.SaveSettingOverridablePerStore(rChatSettings, x => x.throttleMilliseconds   , model.throttleMilliseconds_OverrideForStore   , storeScope, false);
			_settingService.SaveSettingOverridablePerStore(rChatSettings, x => x.bannerText             , model.bannerText_OverrideForStore             , storeScope, false);

			// now clear settings cache
			_settingService.ClearCache();

			_notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
			return Configure();
		}
	}
}