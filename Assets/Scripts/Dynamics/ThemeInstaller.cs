using UnityEngine;
using Zenject;

namespace InGame.Dynamics.UI
{
    public class ThemeInstaller : MonoInstaller
    {
        [SerializeField] private Themes themes;

        public override void InstallBindings()
        {
            Container.BindInstance(themes).AsSingle();
        }
    }
}