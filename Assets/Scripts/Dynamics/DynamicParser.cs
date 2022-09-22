using System.Collections.Generic;
using System.Threading;

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
            thread = new Thread(OnStart);
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
    }
}