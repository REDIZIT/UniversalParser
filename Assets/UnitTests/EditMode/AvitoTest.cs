using InGame;
using InGame.Dynamics;
using InGame.Recognition;
using NSubstitute;
using NUnit.Framework;
using System.IO;
using System.Linq;
using UnityEngine;
using Zenject;

public class AvitoTest : ZenjectUnitTestFixture
{
    [SetUp]
    public void Install()
    {
        Container.Bind<AvitoDynamicParser>().AsSingle();

        Container.Bind<IStatus>().FromSubstitute();

        Container.Bind<IInputField>().To<FakeInputField>().AsSingle();
        var field = (FakeInputField)Container.Resolve<IInputField>();
        field.Setup("https://www.avito.ru/sankt-peterburg/kvartiry/prodam/do-2500000-rubley-ASgBAgECAUSSA8YQAUXGmgwXeyJmcm9tIjowLCJ0byI6MjUwMDAwMH0?f=ASgBAQECAUSSA8YQAUDKCBT~WAFFxpoMF3siZnJvbSI6MCwidG8iOjI1MDAwMDB9");

        Container.Bind<IPaging>().To<FakePaging>().AsSingle();
        Container.Bind<ISelectTable>().To<FakeSelectTable>().AsSingle();

        string folder = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", @"Assets\UnitTests\EditMode\AvitoTest"));
        var pathes = Directory.GetFiles(folder).Where(p => Path.GetExtension(p) == ".html");

        string[] htmls = pathes.Select(p => File.ReadAllText(p)).ToArray();

        Container.Bind<IBrowser>().To<FakeBrowser>()
            .FromMethod(() => new FakeBrowser(htmls));


        Container.Inject(this);


        RecognizerArea.Initialize();
        RecognizerStoreys.Initialize();
    }

    [Inject] private AvitoDynamicParser parser;

    [Test]
    public void Test1()
    {
        Pathes.Initialize();

        parser.Start(false);
    }

    public class SubstituteBase
    {

    }
}
