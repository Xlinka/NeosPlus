using System.Collections.Generic;
using System.Linq;
using BaseX;

namespace FrooxEngine
{
	[Category("Utility")]
	public class BoundingBoxUserTracker : Component
	{
		public readonly RawOutput<bool> IsLocalUserInside;

		public readonly RawOutput<bool> IsAnyUserInside;

		public readonly RawOutput<int> NumberOfUsersInside;

		protected readonly SyncBag<UserRef> _usersInside;

		private List<User> usersInWorld = new List<User>();

		protected override void OnChanges()
		{
			base.OnChanges();
			_usersInside.RemoveAll((UserRef u) => u.Target == null);
			IsAnyUserInside.Value = _usersInside.Count > 0;
			NumberOfUsersInside.Value = _usersInside.Count;
			IsLocalUserInside.Value = _usersInside.Any((KeyValuePair<RefID, UserRef> u) => u.Value.Target == this.LocalUser);
		}

		public override void OnUserLeft(User user)
		{
			_usersInside.RemoveAll((UserRef u) => u.Target == user);
		}

		protected override void OnCommonUpdate()
		{
			base.OnCommonUpdate();
			usersInWorld.Clear();
			this.World.GetUsers(usersInWorld);
			BoundingBox b = this.Slot.ComputeBoundingBox();
			foreach (User user in usersInWorld)
			{
				if (MathX.IsBetween(user.Root.Slot.GlobalPosition, b.min, b.max))
				{
					if (!_usersInside.Any((KeyValuePair<RefID, UserRef> u) => u.Value.Target == user))
					{
						_usersInside.Add().Target = user;
					}
				}
				else
				{
					if (_usersInside.Any((KeyValuePair<RefID, UserRef> u) => u.Value.Target == user))
					{
						_usersInside.RemoveAll((UserRef u) => u.Target == user);
					}
				}
			}
		}
	}
}