using System.Collections.Generic;
using System.Linq;
using BaseX;

namespace FrooxEngine
{
	[Category("Utility")]
	public class BoundingBoxUserTracker : Component
	{
		public readonly Sync<UserRoot.UserNode> PositionSource;

		public readonly RawOutput<bool> IsLocalUserInside;

		public readonly RawOutput<bool> IsAnyUserInside;

		public readonly RawOutput<int> NumberOfUsersInside;

		protected readonly SyncBag<UserRef> _usersInside;

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

		protected override void OnAwake()
		{
			base.OnAwake();
			PositionSource.Value = UserRoot.UserNode.Root;
		}

		protected override void OnCommonUpdate()
		{
			base.OnCommonUpdate();
			BoundingBox b = this.Slot.ComputeBoundingBox();
			if (MathX.IsBetween(this.LocalUser.Root.GetGlobalPosition(PositionSource.Value), b.min, b.max))
			{
				if (!_usersInside.Any((KeyValuePair<RefID, UserRef> u) => u.Value.Target == this.LocalUser))
				{
					_usersInside.Add().Target = this.LocalUser;
				}
			}
			else
			{
				if (_usersInside.Any((KeyValuePair<RefID, UserRef> u) => u.Value.Target == this.LocalUser))
				{
					_usersInside.RemoveAll((UserRef u) => u.Target == this.LocalUser);
				}
			}
		}
	}
}