using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using BaseX;
using FrooxEngine.UIX;
using FrooxEngine.Logix.Collections.Delegates;

namespace FrooxEngine.Logix.Collections.Objs
{
    public class ArrayXEditor<T> : Component
    {
        protected readonly SyncRef<ArrayX<T>> _targetArray;

        private bool setup;

        protected readonly SyncRef<Button> _addNewButton;

        protected virtual int MinLabelWidth => 24;
        public virtual void Setup(ArrayX<T> target, Button button)
        {
            this._targetArray.Target = target;
            this._addNewButton.Target = button;
            this._addNewButton.Target.Pressed.Target = AddNewPressed;
        }
        protected override void OnChanges()
        {
            if (!this.setup && this._targetArray.Target != null)
            {
                if (base.World.IsAuthority)
                {
                    this.setup = true;
                    base.Slot.DestroyChildren();
                    this._targetArray.Target.DataWritten += ArrayLengthChange;
                    this._targetArray.Target.DataInsert += DataInsert;
                    this._targetArray.Target.DataShortened += ArrayXDataChange;
                    this.Target_ElementsAdded(this._targetArray.Target, 0, this._targetArray.Target.Count);
                }
                else
                {
                    this.setup = true;
                    this._targetArray.Target.DataWritten += ArrayLengthChange;
                    this._targetArray.Target.DataInsert += DataInsert;
                    this._targetArray.Target.DataShortened += ArrayXDataChange;
                }
            }
        }


        protected virtual void Reindex(int i = 0)
        {
            for (; i < base.Slot.ChildrenCount; i++)
            {
                base.Slot[i][0].GetComponent<Text>().Content.Value = i + ":";
            }
        }
        private void Target_ElementsRemoved(ArrayX<T> array, int startIndex, int count)
        {
            base.World.RunSynchronously(delegate
            {
                List<Slot> list2;
                list2 = Pool.BorrowList<Slot>();
                for (int i = startIndex; i < startIndex + count; i++)
                {
                    list2.Add(base.Slot[i]);
                }
                foreach (Slot item in list2)
                {
                    item.Destroy();
                }
                base.MarkChangeDirty();
                Pool.Return(ref list2);
                Reindex();
            });
        }

        private void Target_ElementsAdded(ArrayX<T> array, int startIndex, int count,bool Reindex = false)
        {
            base.World.RunSynchronously(delegate
            {
                for (int i = startIndex; i < startIndex + count; i++)
                {
                    Slot root;
                    root = base.Slot.InsertSlot(i, "Element");
                    this.BuildListItem(array, i, root);

                }
                if (Reindex)
                {
                    this.Reindex(startIndex);
                }
            });
        }

        protected virtual void BuildListItem(ArrayX<T> array, int index, Slot root)
        {
            root.AttachComponent<HorizontalLayout>().Spacing.Value = 4f;
            UIBuilder ui;
            ui = new UIBuilder(root);
            ui.Style.MinWidth = this.MinLabelWidth;
            this.BuildLabel(array, index, ui);
            ui.Style.MinWidth = -1f;
            ui.Style.FlexibleWidth = 100f;
            this.BuildListElement(array, index, ui);
            ui.Style.FlexibleWidth = 0f;
            ui.Style.MinWidth = 24f;
            LocaleString text;
            text = "X";
            ui.Button(in text).Pressed.Target = RemovePressed;
        }
        private void RemovePressed(IButton button, ButtonEventData eventData)
        {
            for (int i = 0; i < base.Slot.ChildrenCount; i++)
            {
                if (base.Slot[i].GetComponentsInChildren<Button>().Any((Button b) => b == button))
                {
                    this._targetArray.Target?.RemoveAt(i);
                    break;
                }
            }
        }
        protected virtual string GetElementName(ArrayX<T> array, int index)
        {
            return index + ":";
        }
        protected virtual void BuildLabel(ArrayX<T> array, int index, UIBuilder ui)
        {
            LocaleString text2;
            text2 = this.GetElementName(array, index);
            Text text;
            text = ui.Text(in text2, bestFit: true, null, parseRTF: false);
            InteractionElement.ColorDriver colorDriver;
            colorDriver = text.Slot.AttachComponent<Button>().ColorDrivers.Add();
            colorDriver.ColorDrive.Target = text.Color;
            colorDriver.NormalColor.Value = color.Black;
            colorDriver.HighlightColor.Value = color.Blue;
            colorDriver.PressColor.Value = color.Blue;
            ReferenceProxySource refProxy;
            refProxy = text.Slot.AttachComponent<ReferenceProxySource>();
            if (index >= 0 && index < array.Count)
            {
                refProxy.Reference.Target = (IWorldElement)array.GetObj(index);
            }
        }

        protected virtual void BuildListElement(ArrayX<T> array, int index, UIBuilder ui)
        {
            if (index >= 0 && index < array.Count)
            {
                SyncMemberEditorBuilder.Build((SyncObject)array.GetObj(index), null, null, ui);
            }
        }

        private void DataInsert(ArrayX<T> sender, int index, int Amount)
        {
            Target_ElementsAdded(sender, index, 1);
        }
        private void ArrayLengthChange(ArrayX<T> sender, int index, int Amount)
        {
            Target_ElementsAdded(sender, index, Amount);
        }
        private void ArrayXDataChange(ArrayX<T> sender, int index, int Amount)
        {
            Target_ElementsRemoved(sender, index, Amount);
        }
        private void AddNewPressed(IButton button, ButtonEventData eventData)
        {
            this._targetArray.Target.Add(default(T));
        }
    }
}
