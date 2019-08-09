using System.Collections.Generic;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Security;
using Nop.Services.Security;

namespace Nop.Plugin.Widgets.RChat
{
	public class RChatPermissions : IPermissionProvider
	{
		public static readonly PermissionRecord View         = new PermissionRecord { Name = "View chat messages", SystemName = "RChatView", Category = "RChat" };
		public static readonly PermissionRecord Send         = new PermissionRecord { Name = "Send chat messages", SystemName = "RChatSend", Category = "RChat" };
		public static readonly PermissionRecord Edit         = new PermissionRecord { Name = "Edit chat messages", SystemName = "RChatEdit", Category = "RChat" };
		public static readonly PermissionRecord Flag         = new PermissionRecord { Name = "Flag chat messages", SystemName = "RChatFlag", Category = "RChat" };
		public static readonly PermissionRecord Link         = new PermissionRecord { Name = "Send links in chat", SystemName = "RChatLink", Category = "RChat" };
		public static readonly PermissionRecord Embed        = new PermissionRecord { Name = "Embed media in chat", SystemName = "RChatEmbed", Category = "RChat" };
		public static readonly PermissionRecord DeleteOwn    = new PermissionRecord { Name = "Delete your own chat messages", SystemName = "RChatDeleteOwn", Category = "RChat" };
		public static readonly PermissionRecord DeleteOthers = new PermissionRecord { Name = "Delete other's chat messages", SystemName = "RChatDeleteOthers", Category = "RChat" };

		public IEnumerable<PermissionRecord> GetPermissions()
		{
			return new []
			{
				View,
				Send,
				Edit,
				Flag,
				Link,
				Embed,
				DeleteOwn,
				DeleteOthers
			};
		}

		public virtual IEnumerable<DefaultPermissionRecord> GetDefaultPermissions()
		{
			return new []
			{
				new DefaultPermissionRecord
				{
					CustomerRoleSystemName = NopCustomerDefaults.AdministratorsRoleName,
					PermissionRecords = new []
					{
						View,
						Send,
						Edit,
						Flag,
						Link,
						Embed,
						DeleteOwn,
						DeleteOthers
					}
				},
				new DefaultPermissionRecord
				{
					CustomerRoleSystemName = NopCustomerDefaults.ForumModeratorsRoleName,
					PermissionRecords = new []
					{
						View,
						Send,
						Edit,
						Flag,
						Link
					}
				},
				new DefaultPermissionRecord
				{
					CustomerRoleSystemName = NopCustomerDefaults.GuestsRoleName,
					PermissionRecords = new []
					{
						View
					}
				},
				new DefaultPermissionRecord
				{
					CustomerRoleSystemName = NopCustomerDefaults.RegisteredRoleName,
					PermissionRecords = new []
					{
						View,
						Send,
						Edit,
						Flag
					}
				},
				new DefaultPermissionRecord
				{
					CustomerRoleSystemName = NopCustomerDefaults.VendorsRoleName,
					PermissionRecords = new []
					{
						View,
						Send,
						Edit,
						Flag
					}
				}
			};
		}
	}
}