using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BaseX;
using FrooxEngine;
using FrooxEngine.UIX;

namespace FrooxEngine
{
    [Category("Utility")]
    class GrabberSimulator : Component, ICustomInspector //, ILocomotionReference
    {
        public readonly SyncRef<Grabber> Grabber;
        public readonly SyncRef<User> SimulatingUser;
        public readonly Sync<bool> Grab;
        public readonly Sync<float> Radius;
        public readonly Sync<bool> SingleItem;

        // Sadly I wasn't able to get climbing to work, if anyone else can it would be cool.
        // All commented code here is related to that
        //public readonly Sync<bool> DoGripping; // aka climbing

        private readonly Digital _grab = new Digital();

        private Grabber _lastGrabber;

        /*
        public Chirality Side => Chirality.Left; // (Chirality)(-1);

        public Slot DirectionReference => Slot;

        public Slot GripReference => Slot;

        public Digital LocomotionGrip => _grab;
        */
        public void BuildInspectorUI(UIBuilder ui)
        {
            WorkerInspector.BuildInspectorUI(this, ui);
            ui.Button("Inspector.Collider.Visualize".AsLocaleKey(), VisualizeCollider);
        }

        [SyncMethod]
        private void VisualizeCollider(IButton button, ButtonEventData eventData)
        {
            button.Enabled = false;
            StartTask(new Func<IButton, Task>(RunColliderVisualization), button);
        }

        public async Task RunColliderVisualization(IButton source)
        {
            while (!source.IsRemoved && Grabber != null)
            {
                Debug.Sphere(Grabber.Slot.GlobalPosition, Grabber.Slot.LocalScaleToGlobal(Radius),
                    color.Green.SetA(0.25f), 2, 0f);
                await default(NextUpdate);
            }
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            Radius.Value = 0.07f;
            //DoGripping.Value = true;
            SimulatingUser.Target = LocalUser;
            Grabber.Target = Slot.AddSlot("Grabber").AttachComponent<Grabber>();
        }

        protected override void OnCommonUpdate()
        {
            if (SimulatingUser.Target == LocalUser)
            {
                /*
                if (DoGripping.Value)
                {
                    if (LocalUserRoot.GetRegisteredComponent<LocomotionController>()?.ActiveModule is PhysicalLocomotion loco)
                    {
                        if (_grab.Pressed)
                        {
                            loco.GetType().GetMethod("CheckAquireGrip", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(loco, new object[] { this });
                            //UniLog.Log("grip qire");
                        }
                    }
                }
                */
                if (_lastGrabber != Grabber.Target)
                {
                    if (_lastGrabber != null && _grab.Held)
                    {
                        _lastGrabber.Release();
                    }

                    _lastGrabber = Grabber.Target;
                    _grab.UpdateState(false);
                }

                if (_lastGrabber != null)
                {
                    _grab.UpdateState(this.Grab.Value);
                    if (_grab.Pressed)
                    {
                        List<ICollider> list = Pool.BorrowList<ICollider>();

                        World.Physics.SphereOverlap(_lastGrabber.Slot.GlobalPosition, Radius, list);

                        Slot holderSlot = _lastGrabber.HolderSlot;
                        holderSlot.LocalPosition = float3.Zero;
                        holderSlot.LocalRotation = floatQ.Identity;
                        holderSlot.GlobalScale = float3.One;

                        var success = _lastGrabber.Grab(list, null, true, SingleItem);

                        Pool.Return(ref list);
                    }
                    else if (_grab.Held)
                    {
                        _lastGrabber.Update();
                    }
                    else if (_grab.Released)
                    {
                        _lastGrabber.Release();
                    }
                }
            }
        }
    }
}