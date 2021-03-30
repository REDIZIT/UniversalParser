using InGame.Recognition;
using NUnit.Framework;

public class RecognizeTest
{
    [Test]
    public void Area()
    {
        RecognizerArea.Initialize();

        string str = "220 м²";
        Assert.AreEqual(str, RecognizerArea.TryExtractAreaString(str));

        str = "220м²";
        Assert.AreEqual(str, RecognizerArea.TryExtractAreaString(str));

        //
        // Comma separated way
        //
        str = "Торговое помещение, 10 м²";
        Assert.AreEqual("10 м²", RecognizerArea.TryExtractAreaString(str));


        str = "У м. Чернышевская, проходное, 20 квт";
        Assert.AreEqual("20 квт", RecognizerArea.TryExtractAreaString(str));

        str = "Склад контейнер, 30 м2";
        Assert.AreEqual("30 м2", RecognizerArea.TryExtractAreaString(str));



        //
        // Space separated way
        //
        str = "260м² активное торговое место";
        Assert.AreEqual("260м²", RecognizerArea.TryExtractAreaString(str));

        str = "Помещение под Офис или другое - 141 м²";
        Assert.AreEqual("141 м²", RecognizerArea.TryExtractAreaString(str));

        str = "Салон красоты, сфера услуг 74 м²";
        Assert.AreEqual("74 м²", RecognizerArea.TryExtractAreaString(str));

        str = "Торговая площадь, 133.4 м²";
        Assert.AreEqual("133.4 м²", RecognizerArea.TryExtractAreaString(str));

        str = "Коммерческое помещение 132,3 кв.м";
        Assert.AreEqual("132,3 кв.м", RecognizerArea.TryExtractAreaString(str));

        str = "Собственник продает Здание 2 821 кв м";
        Assert.AreEqual("2 821 кв м", RecognizerArea.TryExtractAreaString(str));



        Assert.IsTrue(RecognizerArea.IsAreaString("240м2"));
        Assert.IsFalse(RecognizerArea.IsAreaString("Я собака, ты собака"));
    }
}
