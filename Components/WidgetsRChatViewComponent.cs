using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.RChat.Models;
using Nop.Services.Configuration;
using Nop.Web.Framework.Components;
using Nop.Services.Security;

namespace Nop.Plugin.Widgets.RChat.Components
{
	[ViewComponent(Name = "WidgetsRChat")]
	public class WidgetsRChatViewComponent : NopViewComponent
	{
		readonly IStoreContext      _storeContext;
		readonly ISettingService    _settingService;
		readonly IPermissionService _permissions;

		public WidgetsRChatViewComponent
		( IStoreContext      storeContext
		, ISettingService    settingService
		, IPermissionService permissions
		)
		{
			_storeContext   = storeContext;
			_settingService = settingService;
			_permissions    = permissions;
		}

		public IViewComponentResult Invoke(string widgetZone, object additionalData)
		{
			var rChatSettings = _settingService.LoadSetting<RChatSettings>(_storeContext.CurrentStore.Id);

			var model = new RChatModel
			{
				numberOfHistoryMessages = rChatSettings.numberOfHistoryMessages,
				messageCharacterLimit   = rChatSettings.messageCharacterLimit,
				nameCharacterLimit      = rChatSettings.nameCharacterLimit,
				throttleMilliseconds    = rChatSettings.throttleMilliseconds,
				bannerText              = rChatSettings.bannerText,
				canView         = _permissions.Authorize(RChatPermissions.View),
				canSend         = _permissions.Authorize(RChatPermissions.Send),
				canEdit         = _permissions.Authorize(RChatPermissions.Edit),
				canFlag         = _permissions.Authorize(RChatPermissions.Flag),
				canDeleteOwn    = _permissions.Authorize(RChatPermissions.DeleteOwn),
				canDeleteOthers = _permissions.Authorize(RChatPermissions.DeleteOthers)
			};

			return View("~/Plugins/Widgets.RChat/Views/RChat.cshtml", model);
		}
	}
}
