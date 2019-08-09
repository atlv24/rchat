using System.Text.RegularExpressions;

namespace Nop.Plugin.Widgets.RChat
{
	public class RChatUtility
	{
		static Regex links = new Regex(@"(?<=^|\s)https?:\/\/[\w@#%&()+=.?:\/-]+(?=$|\s|[!.,?])", RegexOptions.Compiled);
		static Regex media = new Regex(@"(?<=^|\s)https?:\/\/[\w@#%&()+=.?:\/-]+\.(?:jpe?g|png|gif)(?=$|\s|[!.,?])", RegexOptions.Compiled);
		static Regex video = new Regex(@"(?<=^|\s)https?:\/\/(?:www\.)?youtu(?:be\.com\/watch\?v=|\.be\/)([\w-]*)(&(?:amp;)?‌​[\w\?‌​=]*)?(?=$|\s|[!.,?])", RegexOptions.Compiled);

		public static string EmbedMedia(string message) => media.Replace(message, SubMedia);
		public static string EmbedVideo(string message) => video.Replace(message, SubVideo);
		public static string EmbedLinks(string message) => links.Replace(message, SubLinks);

		static string SubMedia(Match match) => $"<img src='{match.Value}'/>";
		static string SubVideo(Match match) => $"<iframe src='https://www.youtube-nocookie.com/embed/{match.Groups[1].Value}' allowfullscreen frameborder=0></iframe>";
		static string SubLinks(Match match) => $"<a href='{match.Value}'>{match.Value}</a>";
	}
}
