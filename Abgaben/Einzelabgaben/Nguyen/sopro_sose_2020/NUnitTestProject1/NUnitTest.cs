using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using sopro_sose_2020.Controllers;
using sopro_sose_2020.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace NUnitTestProject1
{
    [TestFixture]
    public class Test
    {
        // ( a, b, c) => a(partt of) b, c expected percentage
        [TestCase(2,5,40)]
        [TestCase(1,100,1)]
        [TestCase(1,1000,0.1)]
        public void testPercCalculation(int a,int b,double expected)
        {
            var controller = new EvaluationController();
            double result = controller.percCalc(a, b);
            Assert.AreEqual(result,expected);
        }
    }
}
