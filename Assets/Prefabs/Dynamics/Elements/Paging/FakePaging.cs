using UnityEngine;

namespace InGame.Dynamics
{
    public class FakePaging : IPaging
    {
        public int Start { get; private set; }
        public int End
        {
            get
            {
                if (_end == -1) return RequestPagesCount;
                else return Mathf.Clamp(_end, 1, RequestPagesCount);
            }
        }

        /// <summary>Shows how many pages selected in range. Will be equal to <see cref="int.MaxValue"/> if start or end is not defined.</summary>
        public int Count => End - Start + 1;

        public int RequestPagesCount { get; set; }

        public GameObject gameObject => null;
        public bool IsValid => true;

        private int _end;
        private IPaging.Model model;

        public string GetPagedUrl(string url, int page)
        {
            return model.GetPagedUrl(url, page);
        }

        public void Setup(IPaging.Model model)
        {
            this.model = model;
        }
    }
}