using UnityEngine;
using Zenject;

namespace InGame.Dynamics.UI
{
    public class AppInstaller : MonoInstaller
    {
        [SerializeField] private Themes themes;

        public override void InstallBindings()
        {
            Pathes.Initialize();

            Container.Bind<UIHelperPort>().AsSingle();
            Container.BindInstance(themes).AsSingle();
            Container.Bind<IBrowser>().To<Yandex>().AsSingle();
            Container.Bind<IWebClient>().To<WebClientWrapper>().AsTransient();
        }

        private void OnApplicationQuit()
        {
            Container.Resolve<IBrowser>().Close();
        }
    }
}