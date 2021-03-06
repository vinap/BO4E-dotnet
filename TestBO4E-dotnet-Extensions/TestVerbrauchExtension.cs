﻿using System;
using System.Collections.Generic;
using System.Linq;
using BO4E.COM;
using BO4E.ENUM;
using BO4E.Extensions.COM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static BO4E.Extensions.COM.VerbrauchExtension;

namespace TestBO4EExtensions
{
    [TestClass]
    public class TestVerbrauchExtension
    {

        [TestMethod]
        public void TestMergeNoOverlap()
        {
            Verbrauch v1 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KWH,
                Wert = 5,
                Startdatum = new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc)
            };
            Verbrauch v2 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KWH,
                Wert = 3,
                Startdatum = new DateTime(2018, 2, 28, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 3, 31, 23, 0, 0, DateTimeKind.Utc)
            };
            HashSet<Verbrauch> result = v1.Merge(v2);
            Assert.AreEqual(2, result.Count);

            Assert.IsTrue(result.SetEquals(new HashSet<Verbrauch> { v1, v2 }));
        }

        [TestMethod]
        public void TestMergeAdjacentExtensive()
        {
            Verbrauch v1 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KWH,
                Wert = 5,
                Startdatum = new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc)
            };
            Verbrauch v2 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KWH,
                Wert = 3,
                Startdatum = new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 2, 28, 23, 0, 0, DateTimeKind.Utc)
            };
            HashSet<Verbrauch> result = v1.Merge(v2);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(v1.Startdatum, result.First().Startdatum);
            Assert.AreEqual(v2.Enddatum, result.First().Enddatum);
            Assert.AreEqual(8, result.First().Wert);

            Assert.IsTrue(result.SetEquals(v2.Merge(v1)));
        }

        [TestMethod]
        public void TestMergeAdjacentIntensive()
        {
            Verbrauch v1 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KW,
                Wert = 5,
                Startdatum = new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc)
            };
            Verbrauch v2 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KW,
                Wert = 3,
                Startdatum = new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 2, 28, 23, 0, 0, DateTimeKind.Utc)
            };
            HashSet<Verbrauch> result12 = v1.Merge(v2);
            Assert.AreEqual(2, result12.Count);

            Assert.IsTrue(result12.SetEquals(v2.Merge(v1)));

            Verbrauch v3 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KW,
                Wert = 5,
                Startdatum = new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc)
            };
            Verbrauch v4 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KW,
                Wert = 5,
                Startdatum = new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 2, 28, 23, 0, 0, DateTimeKind.Utc)
            };
            HashSet<Verbrauch> result34 = v3.Merge(v4);
            //Assert.AreEqual(1, result34.Count);

            Assert.IsTrue(result34.SetEquals(v4.Merge(v3)));
        }


        [TestMethod]
        public void TestMergeOverlappingExtensive()
        {
            Verbrauch v1 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KWH,
                Wert = 5,
                Startdatum = new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc)
            };
            Verbrauch v2 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KWH,
                Wert = 3,
                Startdatum = new DateTime(2018, 1, 15, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 2, 28, 23, 0, 0, DateTimeKind.Utc)
            };
            HashSet<Verbrauch> result = v1.Merge(v2);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(v1.Startdatum, result.First().Startdatum);
            Assert.AreEqual(v2.Enddatum, result.First().Enddatum);
            Assert.AreEqual(8, result.First().Wert);

            Assert.IsTrue(result.SetEquals(v2.Merge(v1)));
        }


        [TestMethod]
        public void TestMergeOverlappingIntensive()
        {
            Verbrauch v1 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KW,
                Wert = 5,
                Startdatum = new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc)
            };
            Verbrauch v2 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KW,
                Wert = 3,
                Startdatum = new DateTime(2018, 1, 15, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 2, 28, 23, 0, 0, DateTimeKind.Utc)
            };
            var rawResult = v1.Merge(v2);
            List<Verbrauch> result = new List<Verbrauch>(rawResult);
            Assert.AreEqual(3, result.Count);
            result.Sort(new VerbrauchDateTimeComparer());
            Assert.AreEqual(v1.Startdatum, result.First().Startdatum);
            Assert.AreEqual(5, result.First().Wert);
            Assert.AreEqual(v2.Startdatum, result[1].Startdatum);
            Assert.AreEqual(v1.Enddatum, result[1].Enddatum);
            Assert.AreEqual(8, result[1].Wert);
            Assert.AreEqual(v2.Enddatum, result.Last().Enddatum);
            Assert.AreEqual(3, result.Last().Wert);

            Assert.IsTrue(rawResult.SetEquals(v2.Merge(v1)));
        }

        [TestMethod]
        public void TestMergeRedundantIntensiveSameTime()
        {
            Verbrauch v1 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KW,
                Wert = 5,
                Startdatum = new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc)
            };
            Verbrauch v2 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KW,
                Wert = 5,
                Startdatum = new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc)
            };
            var rawResult = v1.MergeRedundant(v2, true);
            List<Verbrauch> result = new List<Verbrauch>(rawResult);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(v1, v2);
            Assert.AreEqual(v1, result.First());
            Assert.AreEqual(5, result.First().Wert);
        }

        [TestMethod]
        public void TestMergeRedundantExtensiveSameTime()
        {
            Verbrauch v1 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KWH,
                Wert = 5,
                Startdatum = new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc)
            };
            Verbrauch v2 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KWH,
                Wert = 5,
                Startdatum = new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc)
            };
            var rawResult = v1.MergeRedundant(v2, true);
            List<Verbrauch> result = new List<Verbrauch>(rawResult);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(5, result.First().Wert);
        }


        [TestMethod]
        public void TestMergeRedundantExtensiveLeftJustifiedOverlap()
        {
            Verbrauch v1 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KWH,
                Wert = 5,
                Startdatum = new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 2, 28, 23, 0, 0, DateTimeKind.Utc)
            };
            Verbrauch v2 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.KWH,
                Wert = 3,
                Startdatum = new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc)
            };
            var rawResult = v1.MergeRedundant(v2, true);

            List<Verbrauch> result = new List<Verbrauch>(rawResult);
            result.Sort(new VerbrauchDateTimeComparer());
            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(2, result.First().Wert);
            Assert.AreEqual(new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc), result.First().Startdatum);
            //Assert.AreEqual(new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc), result.First().enddatum);

            //Assert.AreEqual(5, result.Last().wert);
            //Assert.AreEqual(new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc), result.Last().startdatum);
            Assert.AreEqual(new DateTime(2018, 2, 28, 23, 0, 0, DateTimeKind.Utc), result.Last().Enddatum);

        }

        [TestMethod]
        public void TestMergeRedundantRightJustifiedOverlap()
        {
            Verbrauch v1 = JsonConvert.DeserializeObject<Verbrauch>("{\"startdatum\":\"2018-12-25T16:22:00Z\",\"enddatum\":\"2019-12-25T08:20:00Z\",\"wertermittlungsverfahren\":0,\"obiskennzahl\":\"1-1:1.8.0\",\"wert\":1539,\"einheit\":2,\"zaehlernummer\":\"10000548\"}");
            Verbrauch v2 = JsonConvert.DeserializeObject<Verbrauch>("{\"startdatum\":\"2018-09-01T00:00:00Z\",\"enddatum\":\"2018-12-25T16:22:00Z\",\"wertermittlungsverfahren\":0,\"obiskennzahl\":\"1-1:1.8.0\",\"wert\":911,\"einheit\":2,\"zaehlernummer\":\"10000548\"}");
            var rawResult = v1.MergeRedundant(v2, true);
            List<Verbrauch> result = new List<Verbrauch>(rawResult);
            result.Sort(new VerbrauchDateTimeComparer());
            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(2450.0M, result.First().Wert);
            Assert.AreEqual(new DateTime(2018, 9, 1, 0, 0, 0, DateTimeKind.Utc), result.First().Startdatum);
            Assert.AreEqual(new DateTime(2019, 12, 25, 08, 20, 0, DateTimeKind.Utc), result.First().Enddatum);
        }

        private static readonly Verbrauch dtV1 = new Verbrauch()
        {
            Obiskennzahl = "123",
            Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
            Einheit = Mengeneinheit.KWH,
            Wert = 31 + 2 * 28,
            Startdatum = new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc),
            Enddatum = new DateTime(2018, 2, 28, 23, 0, 0, DateTimeKind.Utc)
        };

        private static readonly Verbrauch dtV2 = new Verbrauch()
        {
            Obiskennzahl = "123",
            Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
            Einheit = Mengeneinheit.KWH,
            Wert = 31,
            Startdatum = new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc),
            Enddatum = new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc)
        };

        private static readonly Verbrauch dtV3 = new Verbrauch()
        {
            Obiskennzahl = "123",
            Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
            Einheit = Mengeneinheit.KWH,
            Wert = 31 + 2 * 28 + 3 * 31,
            Startdatum = new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc),
            Enddatum = new DateTime(2018, 3, 31, 23, 0, 0, DateTimeKind.Utc)
        };

        [TestMethod]
        public void TestDetangleTwofold()
        {
            var result = Detangle(new List<Verbrauch> { dtV1, dtV2 });
            result.Sort(new VerbrauchDateTimeComparer());
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc), result.First().Startdatum);
            Assert.AreEqual(new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc), result.First().Enddatum);
            Assert.AreEqual(31, result.First().Wert);
            Assert.AreEqual(new DateTime(2018, 1, 31, 23, 0, 0, DateTimeKind.Utc), result.Last().Startdatum);
            Assert.AreEqual(new DateTime(2018, 2, 28, 23, 0, 0, DateTimeKind.Utc), result.Last().Enddatum);
            Assert.AreEqual(2 * 28, result.Last().Wert);
        }

        [TestMethod]
        public void TestDetangleThreefold()
        {
            var result = Detangle(new List<Verbrauch> { dtV1, dtV2, dtV3 });
            result.Sort(new VerbrauchDateTimeComparer());
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(new DateTime(2017, 12, 31, 23, 0, 0, 0, DateTimeKind.Utc), result[0].Startdatum);
            Assert.AreEqual(new DateTime(2018, 1, 31, 23, 0, 0, 0, DateTimeKind.Utc), result[0].Enddatum);
            Assert.AreEqual(31, result[0].Wert);
            Assert.AreEqual(new DateTime(2018, 1, 31, 23, 0, 0, 0, DateTimeKind.Utc), result[1].Startdatum);
            Assert.AreEqual(new DateTime(2018, 2, 28, 23, 0, 0, 0, DateTimeKind.Utc), result[1].Enddatum);
            Assert.AreEqual(2 * 28, result[1].Wert);
            Assert.AreEqual(new DateTime(2018, 2, 28, 23, 0, 0, 0, DateTimeKind.Utc), result[2].Startdatum);
            Assert.AreEqual(new DateTime(2018, 3, 31, 23, 0, 0, 0, DateTimeKind.Utc), result[2].Enddatum);
            Assert.AreEqual(3 * 31, result[2].Wert);
        }

        [TestMethod]
        public void TestHfSapDataDetangle()
        {
            List<Verbrauch> testList = JsonConvert.DeserializeObject<List<Verbrauch>>("[{\"startdatum\":\"2000-01-01T00:00:00Z\",\"enddatum\":\"2018-09-01T00:00:00Z\",\"wertermittlungsverfahren\":1,\"obiskennzahl\":\"1-1:1.8.0\",\"wert\":50,\"einheit\":2,\"zaehlernummer\":\"10000548\"},{\"startdatum\":\"2000-01-01T00:00:00Z\",\"enddatum\":\"2018-12-25T16:22:00Z\",\"wertermittlungsverfahren\":0,\"obiskennzahl\":\"1-1:1.8.0\",\"wert\":961,\"einheit\":2,\"zaehlernummer\":\"10000548\"},{\"startdatum\":\"2000-01-01T00:00:00Z\",\"enddatum\":\"2019-12-25T08:20:00Z\",\"wertermittlungsverfahren\":1,\"obiskennzahl\":\"1-1:1.8.0\",\"wert\":2500,\"einheit\":2,\"zaehlernummer\":\"10000548\"},{\"startdatum\":\"2018-09-01T00:00:00Z\",\"enddatum\":\"2018-12-25T16:22:00Z\",\"wertermittlungsverfahren\":0,\"obiskennzahl\":\"1-1:1.8.0\",\"wert\":911,\"einheit\":2,\"zaehlernummer\":\"10000548\"},{\"startdatum\":\"2018-09-01T00:00:00Z\",\"enddatum\":\"2019-12-25T08:20:00Z\",\"wertermittlungsverfahren\":1,\"obiskennzahl\":\"1-1:1.8.0\",\"wert\":2450,\"einheit\":2,\"zaehlernummer\":\"10000548\"},{\"startdatum\":\"2018-12-25T16:22:00Z\",\"enddatum\":\"2019-12-25T08:20:00Z\",\"wertermittlungsverfahren\":0,\"obiskennzahl\":\"1-1:1.8.0\",\"wert\":1539,\"einheit\":2,\"zaehlernummer\":\"10000548\"}]");
            Assert.AreEqual(3, testList.Where(v => v.Wertermittlungsverfahren == Wertermittlungsverfahren.MESSUNG).Count());
            Assert.AreEqual(3, testList.Where(v => v.Wertermittlungsverfahren == Wertermittlungsverfahren.PROGNOSE).Count());
            var result = Detangle(testList);
            result.Sort(new VerbrauchDateTimeComparer());
            //Assert.AreEqual(5, result.Count);

            var subResultMessung = result.Where(v => v.Wertermittlungsverfahren == Wertermittlungsverfahren.MESSUNG).ToList();
            Assert.AreEqual(2, subResultMessung.Count);

            Assert.AreEqual(new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc), subResultMessung[0].Startdatum);
            Assert.AreEqual(new DateTime(2018, 9, 1, 0, 0, 0, DateTimeKind.Utc), subResultMessung[0].Enddatum);
            Assert.AreEqual(50, subResultMessung[0].Wert);
            Assert.AreEqual(new DateTime(2018, 9, 1, 0, 0, 0, DateTimeKind.Utc), subResultMessung[1].Startdatum);
            Assert.AreEqual(new DateTime(2019, 12, 25, 8, 20, 0, DateTimeKind.Utc), subResultMessung[1].Enddatum);
            Assert.AreEqual(2450, subResultMessung[1].Wert);

            var subResultPrognose = result.Where(v => v.Wertermittlungsverfahren == Wertermittlungsverfahren.PROGNOSE).ToList();
            Assert.AreEqual(3, subResultPrognose.Count);
            Assert.AreEqual(new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc), subResultPrognose[0].Startdatum);
            Assert.AreEqual(new DateTime(2018, 9, 1, 0, 0, 0, DateTimeKind.Utc), subResultPrognose[0].Enddatum);
            Assert.AreEqual(50, subResultPrognose[0].Wert);
            Assert.AreEqual(new DateTime(2018, 9, 1, 0, 0, 0, DateTimeKind.Utc), subResultPrognose[1].Startdatum);
            Assert.AreEqual(new DateTime(2018, 12, 25, 16, 22, 0, DateTimeKind.Utc), subResultPrognose[1].Enddatum);
            Assert.AreEqual(911, subResultPrognose[1].Wert);
            Assert.AreEqual(new DateTime(2018, 12, 25, 16, 22, 0, DateTimeKind.Utc), subResultPrognose[2].Startdatum);
            Assert.AreEqual(new DateTime(2019, 12, 25, 8, 20, 0, DateTimeKind.Utc), subResultPrognose[2].Enddatum);
            Assert.AreEqual(1539, subResultPrognose[2].Wert);
        }

        [TestMethod]
        public void TestUnitConversion()
        {
            Verbrauch v1 = new Verbrauch()
            {
                Obiskennzahl = "123",
                Wertermittlungsverfahren = Wertermittlungsverfahren.MESSUNG,
                Einheit = Mengeneinheit.MW,
                Wert = 17,
                Startdatum = new DateTime(2017, 12, 31, 23, 0, 0, DateTimeKind.Utc),
                Enddatum = new DateTime(2018, 3, 31, 23, 0, 0, DateTimeKind.Utc)
            };
            v1.ConvertToUnit(Mengeneinheit.KW);
            Assert.AreEqual(Mengeneinheit.KW, v1.Einheit);
            Assert.AreEqual(17000.0M, v1.Wert);

            Assert.ThrowsException<InvalidOperationException>(() => v1.ConvertToUnit(Mengeneinheit.KWH));
        }
    }
}