using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace InGame.Dynamics
{
    public abstract class DynamicParser
    {
        public bool IsWorking { get; private set; }

        public List<IDynamicElement> Elements { get; private set; } = new();

        private Thread thread;

        public void Start()
        {
            IsWorking = true;
            thread = new Thread(ThreadStart);
            thread.Start();
        }
        public void Stop()
        {
            IsWorking = false;
            OnStop();
            thread?.Abort();
        }
        protected void SwitchWorkState()
        {
            if (IsWorking) Stop();
            else Start();
        }

        protected abstract void OnStart();
        protected abstract void OnStop();

        private void ThreadStart()
        {
            StatusElement status = (StatusElement)Elements.First(e => e is StatusElement);

            try
            {
                OnStart();
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
    }
}