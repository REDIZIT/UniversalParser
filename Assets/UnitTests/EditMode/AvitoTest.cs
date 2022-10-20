using InGame.Dynamics;
using NUnit.Framework;
using Zenject;

public class AvitoTest : ZenjectUnitTestFixture
{
    [SetUp]
    public void Install()
    {
        Container.Bind<AvitoDynamicParser>().AsSingle();
        Container.Inject(this);
    }

    [Inject] private AvitoDynamicParser parser;

    [Test]
    public void Test1()
    {

    }
}
