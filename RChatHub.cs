using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System;
using Microsoft.AspNetCore.SignalR;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Configuration;
using Nop.Services.Security;
using System.Text.RegularExpressions;

namespace Nop.Plugin.Widgets.RChat
{
	public class RChatHub : Hub
	{
		readonly IWorkContext       _workContext;
		readonly IStoreContext      _storeContext;
		readonly IPermissionService _permissions;
		readonly ISettingService    _settingService;

		readonly static Dictionary<int, long> _throttleTracker = new Dictionary<int, long>();

		public RChatHub
		( IWorkContext       workContext
		, IStoreContext      storeContext
		, IPermissionService permissions
		, ISettingService    settingService
		)
		{
			_workContext    = workContext;
			_storeContext   = storeContext;
			_permissions    = permissions;
			_settingService = settingService;
		}

		public async Task Send(string message)
		{
			if (!_permissions.Authorize(RChatPermissions.Send)) return;

			message = message.Trim();
			if (message == "") return;
			RChatSettings rChatSettings = _settingService.LoadSetting<RChatSettings>(_storeContext.CurrentStore.Id);

			Customer customer = _workContext.CurrentCustomer;
			string name = customer.Username;
			int    id   = customer.Id;

			bool throttle = _throttleTracker.TryGetValue(id, out long millis) && millis > DateTime.Now.Ticks;
			_throttleTracker[id] = TimeSpan.TicksPerMillisecond * rChatSettings.throttleMilliseconds + DateTime.Now.Ticks;
			if (throttle) return;

			if (message.Length > rChatSettings.messageCharacterLimit) message = message.Remove(rChatSettings.messageCharacterLimit);
			if (name.Length    > rChatSettings.   nameCharacterLimit) name    = name   .Remove(rChatSettings.   nameCharacterLimit);
			message = Sanitize(message);

			RChatHistory.AddMessage(id, name, message);
			await Clients.All.SendAsync("Send", id, name, message);
		}

		string Sanitize(string message)
		{
			message = HttpUtility.HtmlEncode(message);
			if (_permissions.Authorize(RChatPermissions.Embed))
			{
				message = EmbedMedia(message);
				message = EmbedVideo(message);
			}
			if (_permissions.Authorize(RChatPermissions.Link))
			{
				message = EmbedLinks(message);
			}
			return message;
		}

		static Regex links = new Regex(@"(?<=^|\s)https?:\/\/[\w@#%&()+=.?:\/-]+(?=$|\s|[!.,?])", RegexOptions.Compiled);
		static Regex media = new Regex(@"(?<=^|\s)https?:\/\/[\w@#%&()+=.?:\/-]+\.(?:jpe?g|png|gif)(?=$|\s|[!.,?])", RegexOptions.Compiled);
		static Regex video = new Regex(@"(?<=^|\s)https?:\/\/(?:www\.)?youtu(?:be\.com\/watch\?v=|\.be\/)([\w-]*)(&(?:amp;)?‌​[\w\?‌​=]*)?(?=$|\s|[!.,?])", RegexOptions.Compiled);

		static string EmbedMedia(string message) => media.Replace(message, SubMedia);
		static string EmbedVideo(string message) => video.Replace(message, SubVideo);
		static string EmbedLinks(string message) => links.Replace(message, SubLinks);

		static string SubMedia(Match match) => $"<img src='{match.Value}'/>";
		static string SubVideo(Match match) => $"<iframe src='https://www.youtube-nocookie.com/embed/{match.Groups[1].Value}' allowfullscreen frameborder=0></iframe>";
		static string SubLinks(Match match) => $"<a href='{match.Value}'>{match.Value}</a>";
	}
}
