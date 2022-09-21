using UnityEngine;
using Zenject;

namespace InGame.Dynamics.UI
{
    public class AppInstaller : MonoInstaller
    {
        [SerializeField] private Themes themes;

        public override void InstallBindings()
        {
            Container.Bind<UIHelperPort>().AsSingle();
            Container.BindInstance(themes).AsSingle();
        }
    }
}