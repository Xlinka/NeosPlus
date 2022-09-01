using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.CommonAvatar;
using FrooxEngine.FinalIK;
using FrooxEngine.UIX;

namespace FrooxEngine
{
	[Category("Transform")]
	public class SlotProtection : Component, IItemPermissions, IComponent, IComponentBase, IDestroyable, IWorker, IWorldElement, IUpdatable, IChangeable, IAudioUpdatable, IInitializable, ILinkable, ICustomInspector
	{
		public readonly CloudUserRef User;

		private string _registeredUserId;

		private bool _allowRemoval;

		public bool CanSave => CanUse(base.LocalUser);

		protected override void OnAwake()
		{
			base.OnAwake();
			User.User.MarkDirectAccessOnly();
			User._userId.MarkDirectAccessOnly();
			persistent.OnValueChange += Persistent_OnValueChange;
			User.User.OnTargetChange += User_OnTargetChange;
			User._userId.OnValueChange += _userId_OnValueChange;
		}

		private void Persistent_OnValueChange(SyncField<bool> syncField)
		{
			if (!syncField.Value)
			{
				ScheduleDelete();
			}
		}

		private void User_OnTargetChange(SyncRef<User> reference)
		{
			if (_registeredUserId != null && reference.Target?.UserID != _registeredUserId)
			{
				CheckChange();
			}
		}

		private void _userId_OnValueChange(SyncField<string> syncField)
		{
			CheckChange();
			_registeredUserId = syncField.Value;
		}

		private void CheckChange()
		{
			if (_registeredUserId != null && base.World.CanMakeSynchronousChanges && !CanUse(base.LocalUser))
			{
				ScheduleDelete();
			}
		}

		public bool CanUse(User user)
		{
			if (base.World == Userspace.UserspaceWorld)
			{
				return true;
			}
			if (_registeredUserId == null)
			{
				return true;
			}
			if (user == null)
			{
				return false;
			}
			return _registeredUserId == user.UserID;
		}

		protected override void OnDestroying()
		{
			if (!CanUse(base.LocalUser) || (!_allowRemoval && !base.Slot.IsDestroying))
			{
				ScheduleDelete();
			}
		}

		public override void OnUserLeft(User user)
		{
			if (_registeredUserId != null && user.UserID == _registeredUserId)
			{
				ScheduleDelete();
			}
		}

		private void ScheduleDelete()
		{
			base.World.RunSynchronously(delegate
			{
				Slot slot = base.Slot.ActiveUserRoot?.Slot;
				if (slot == null)
				{
					slot = base.Slot.GetComponentInParents<AvatarGroup>()?.Slot;
				}
				if (slot == null)
				{
					slot = base.Slot.GetComponentInParents<VRIKAvatar>()?.Slot;
				}
				if (slot == null)
				{
					slot = base.Slot.GetObjectRoot();
				}
				slot.Destroy();
			});
		}

		protected override void OnAttach()
		{
			User.Target = base.LocalUser;
		}

		public void BuildInspectorUI(UIBuilder ui)
		{
			ui.PushStyle();
			ui.Style.MinHeight = 100f;
			LocaleString text = "Inspector.SimpleAvatarProtection.Warning".AsLocaleKey();
			ui.Text(in text, bestFit: true, Alignment.TopLeft);
			ui.PopStyle();
			WorkerInspector.BuildInspectorUI(this, ui);
			text = "Inspector.SimpleAvatarProtection.RemoveAll".AsLocaleKey();
			ui.Button(in text, OnRemoveAllInstances);
			text = "Inspector.SimpleAvatarProtection.RemoveSingle".AsLocaleKey();
			ui.Button(in text, OnRemoveSingleInstance);
		}

		[SyncMethod]
		private void OnRemoveSingleInstance(IButton button, ButtonEventData eventData)
		{
			Remove(eventData, allInstances: false);
		}

		[SyncMethod]
		private void OnRemoveAllInstances(IButton button, ButtonEventData eventData)
		{
			Remove(eventData, allInstances: true);
		}

		private void Remove(ButtonEventData eventData, bool allInstances)
		{
			if (!CanUse(base.LocalUser))
			{
				return;
			}
			Userspace.OpenContextMenu(eventData.source.Slot, new ContextMenuOptions
			{
				disableFlick = true
			}, async delegate (ContextMenu menu)
			{
				ContextMenu contextMenu = menu;
				LocaleString label = "Inspector.SimpleAvatarProtection.ConfirmRemoveAll".AsLocaleKey();
				color color = color.Red;
				contextMenu.AddItem(in label, (Uri)null, in color).Button.LocalPressed += delegate
				{
					StartTask(async delegate
					{
						await default(ToWorld);
						if (allInstances)
						{
							base.Slot.GetObjectRoot().GetComponentsInChildren<SlotProtection>().ForEach(delegate (SlotProtection p)
							{
								if (p.CanUse(base.LocalUser))
								{
									p._allowRemoval = true;
									p.Destroy();
								}
							});
						}
						else if (CanUse(base.LocalUser))
						{
							_allowRemoval = true;
							Destroy();
						}
					});
					menu.Close();
				};
				for (int i = 0; i < 4; i++)
				{
					ContextMenu contextMenu2 = menu;
					label = "General.Cancel".AsLocaleKey();
					color = color.Gray;
					contextMenu2.AddItem(in label, (Uri)null, in color, menu.CloseMenu);
				}
			});
		}
	}

}