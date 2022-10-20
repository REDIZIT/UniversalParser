using InGame.Dynamics;
using NUnit.Framework;
using Zenject;

public class AvitoTest : ZenjectUnitTestFixture
{
    [SetUp]
    public void Install()
    {
        Container.Bind<AvitoDynamicParser>().AsSingle();

        Container.Bind<IInputField>().FromSubstitute();
        Container.Bind<IPaging>().FromSubstitute();
        Container.Bind<ISelectTable>().FromSubstitute();
        Container.Bind<IStatus>().FromSubstitute();
        Container.Bind<IBrowser>().FromSubstitute();

        
        Container.Inject(this);
    }

    [Inject] private AvitoDynamicParser parser;

    [Test]
    public void Test1()
    {
        parser.Start();
    }

    public class SubstituteBase
    {

    }
}
