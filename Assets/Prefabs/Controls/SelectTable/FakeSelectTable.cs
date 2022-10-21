using InGame.Parse;
using System.Linq;
using UnityEngine;

namespace InGame.Dynamics
{
    public class FakeSelectTable : ISelectTable
    {
        public GameObject gameObject => null;
        public bool IsValid => true;

        public IParseResult resultToSave;

        public void SaveResult(IParseResult result)
        {
            Debug.Log("Save results: " + result.EnumerateLots().Count());
            resultToSave = result;
        }

        public void Setup(ISelectTable.Model model) { }
    }
}