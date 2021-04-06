using InGame.Recognition;
using NUnit.Framework;
using RestSharp.Contrib;
using UnityEngine;

public class RecognizeTest
{
    [Test]
    public void Area()
    {
        RecognizerArea.Initialize();
        RecognizerStoreys.Initialize();

        string str = "220 м²";
        Recognizer.TryRecognize(str, out Recognizer.Result result);
        Assert.AreEqual("220 м²", result.area);

        str = "220м²";
        Recognizer.TryRecognize(str, out result);
        Assert.AreEqual("220м²", result.area);

        //
        // Comma separated way
        //
        str = "Торговое помещение, 10 м²";
        Recognizer.TryRecognize(str, out result);
        Assert.AreEqual("Торговое помещение", result.name);
        Assert.AreEqual("10 м²", result.area);


        str = "У м. Чернышевская, проходное, 20 квт";
        Recognizer.TryRecognize(str, out result);
        Assert.AreEqual("У м. Чернышевская, проходное", result.name);
        Assert.AreEqual("20 квт", result.area);

        str = "Склад контейнер, 30 м2";
        Recognizer.TryRecognize(str, out result);
        Assert.AreEqual("Склад контейнер", result.name);
        Assert.AreEqual("30 м2", result.area);



        //
        // Space separated way
        //
        str = "260м² активное торговое место";
        Recognizer.TryRecognize(str, out result);
        Assert.AreEqual("активное торговое место", result.name);
        Assert.AreEqual("260м²", result.area);

        str = "Помещение под Офис или другое - 141 м²";
        Recognizer.TryRecognize(str, out result);
        Assert.AreEqual("Помещение под Офис или другое -", result.name);
        Assert.AreEqual("141 м²", result.area);

        str = "Салон красоты, сфера услуг 74 м²";
        Recognizer.TryRecognize(str, out result);
        Assert.AreEqual("Салон красоты, сфера услуг", result.name);
        Assert.AreEqual("74 м²", result.area);

        str = "Торговая площадь, 133.4 м²";
        Recognizer.TryRecognize(str, out result);
        Assert.AreEqual("Торговая площадь", result.name);
        Assert.AreEqual("133.4 м²", result.area);

        str = "Коммерческое помещение 132,3 кв.м";
        Recognizer.TryRecognize(str, out result);
        Assert.AreEqual("Коммерческое помещение", result.name);
        Assert.AreEqual("132,3 кв.м", result.area);

        str = "Собственник продает Здание 2 821 кв м";
        Recognizer.TryRecognize(str, out result);
        Assert.AreEqual("Собственник продает Здание", result.name);
        Assert.AreEqual("2 821 кв м", result.area);




        #region Flats, storeys

        str = "3-к. квартира, 60 м², 4/5 эт.";
        Recognizer.TryRecognize(str, out result);
        Assert.AreEqual("3-к. квартира", result.name);
        Assert.AreEqual("60 м²", result.area);
        Assert.AreEqual("4/5 эт.", result.storeys);


        str = "2-к. квартира, 75 м², 11/14 эт.";
        Recognizer.TryRecognize(str, out result);
        Assert.AreEqual("2-к. квартира", result.name);
        Assert.AreEqual("75 м²", result.area);
        Assert.AreEqual("11/14 эт.", result.storeys);

        Debug.Log(result.name + " | " + result.area + " | " + result.storeys);

        #endregion


        Assert.IsTrue(RecognizerArea.IsAreaString("240м2"));
        Assert.IsFalse(RecognizerArea.IsAreaString("Я собака, ты собака"));
    }
}
