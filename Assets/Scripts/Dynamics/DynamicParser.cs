using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;
using Zenject;

namespace InGame.Dynamics
{
    public abstract class DynamicParser
    {
        public bool IsWorking { get; private set; }

        protected IStatus status;

        private Thread thread;
        private IDynamicElement[] elements = new IDynamicElement[0];

        [Inject]
        private void Construct(IStatus status)
        {
            this.status = status;
            status.Setup(new(this)
            {
                onSwitchWorkStatus = SwitchWorkState
            });
        }
        public void Start(bool useThreading)
        {
            IsWorking = true;

            if (useThreading)
            {
                thread = new Thread(ThreadStart);
                thread.Start();
            }
            else
            {
                ThreadStart();
            }
        }
        public void Stop()
        {
            IsWorking = false;
            OnStop();
            thread?.Abort();
        }
        public bool IsReadyToStart()
        {
            return elements.Count() > 0 && elements.All(e => e.IsValid);
        }
        protected void SwitchWorkState()
        {
            if (IsWorking) Stop();
            else Start(true);
        }

        protected abstract void OnStart();
        protected abstract void OnStop();

        private void ThreadStart()
        {
            try
            {
                OnStart();
                Stop();
            }
            catch (ThreadAbortException)
            {
                status.Status = "";
                status.Progress = "";
            }
            catch(Exception err)
            {
                status.Status = "<color=F28>Произлошла ошибка</color>";
                status.Progress = "<size=8>" + err.Message + "</size>";
                Debug.LogError(err);
                Stop();
            }
        }
        protected void BakeElements()
        {
            Type type = typeof(IDynamicElement);

            var props = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            var where = props.Where(f => f.GetValue(this) is IDynamicElement).ToArray();
            elements = where.Select(f => f.GetValue(this)).Cast<IDynamicElement>().ToArray();

            Debug.Log("Baked elements: " + String.Join(", ", elements.Select(e => e.GetType().Name)));
        }
    }
}