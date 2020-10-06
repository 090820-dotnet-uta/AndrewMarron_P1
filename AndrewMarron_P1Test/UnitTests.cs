using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RevatureP1;
using RevatureP1.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace RevatureP1Test
{
    public class UnitTests
    {
        [Fact]
        public void InitializeCacheCheck()
        {
            // Act
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            // Arrange
            IMemoryCache _cache = RevatureP1.UtilMethods.InitializeCacheIfNeeded(memoryCache);
            var resultVal = (string)_cache.Get("doLogout");
            var expectedVal = "false";
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        //[Fact]
        //public void ClearRunSessionClearsState()
        //{
        //    // Act
        //    RunSession runSess = new RunSession();
        //    runSess.CurrentState = "incorrectState";
        //    // Arrange
        //    string expectedResult = "LoginLoopChoice"; // The default state
        //    runSess = RevatureP0.UtilMethods.ResetSession(runSess);
        //    //Assert
        //    Assert.Equal(expectedResult, runSess.CurrentState);
        //}

        //[Fact]
        //public void CheckEmptyCartHandling()
        //{
        //    // Act
        //    RunSession runSess = new RunSession();
        //    // Arrange
        //    string expectedResult = "Cart is empty.";
        //    string testResult = RevatureP0.UtilMethods.OutputItemsInCart(runSess);
        //    //Assert
        //    Assert.Equal(expectedResult, testResult);
        //}

        //[Fact]
        //public void StringValidationAcceptsValidAnswers()
        //{
        //    // Act
        //    int maxLength = 10;
        //    List<string> testInputList = new List<string>(new string[] { "1", "y" });
        //    foreach (string testInput in testInputList)
        //    {
        //        // Arrange
        //        bool expectedResult = true;
        //        // checkStringInput(string inStr, int maxLength)
        //        bool testResult = RevatureP0.UtilMethods.checkStringInput(testInput, maxLength);
        //        // Assert
        //        Assert.Equal(expectedResult, testResult);
        //    }
        //}

        //[Fact]
        //public void StringValidationRejectsInvalidAnswers()
        //{
        //    // Act
        //    int maxLength = 10;
        //    List<string> testInputList = new List<string>(new string[] { "", "aaaaaaaaaaaaaaaaaaaaaaaa" });
        //    foreach (string testInput in testInputList)
        //    {
        //        // Arrange
        //        bool expectedResult = false;
        //        // checkStringInput(string inStr, int maxLength)
        //        bool testResult = RevatureP0.UtilMethods.checkStringInput(testInput, maxLength);
        //        // Assert
        //        Assert.Equal(expectedResult, testResult);
        //    }

        //}

        //[Fact]
        //public void IntValidationAcceptsValidAnswers_MinOnly()
        //{
        //    // Act
        //    int maxLength = 10;
        //    int minIn = 0;
        //    List<string> testInputList = new List<string>(new string[] { "0", "1" });
        //    foreach (string testInput in testInputList)
        //    {
        //        // Arrange
        //        bool expectedResult = true;
        //        //checkIntInput(string inStr, int maxLength, int minIn = -1, int maxIn = -1)
        //        bool testResult = RevatureP0.UtilMethods.checkIntInput(testInput, maxLength, minIn);
        //        // Assert
        //        Assert.Equal(expectedResult, testResult);
        //    }
        //}

        //[Fact]
        //public void IntValidationRejectsInvalidAnswers_MinOnly()
        //{
        //    // Act
        //    int maxLength = 10;
        //    int minIn = 0;
        //    List<string> testInputList = new List<string>(new string[] { "-1", "a", "" });
        //    foreach (string testInput in testInputList)
        //    {
        //        // Arrange
        //        bool expectedResult = false;
        //        //checkIntInput(string inStr, int maxLength, int minIn = -1, int maxIn = -1)
        //        bool testResult = RevatureP0.UtilMethods.checkIntInput(testInput, maxLength, minIn);
        //        // Assert
        //        Assert.Equal(expectedResult, testResult);
        //    }
        //}

        //[Fact]
        //public void IntValidationAcceptsValidAnswers_MinAndMax()
        //{
        //    // Act
        //    int maxLength = 10;
        //    int minIn = 1;
        //    int maxIn = 4;
        //    List<string> testInputList = new List<string>(new string[] { "1", "3" });
        //    foreach (string testInput in testInputList)
        //    {
        //        // Arrange
        //        bool expectedResult = true;
        //        //checkIntInput(string inStr, int maxLength, int minIn = -1, int maxIn = -1)
        //        bool testResult = RevatureP0.UtilMethods.checkIntInput(testInput, maxLength, minIn, maxIn);
        //        // Assert
        //        Assert.Equal(expectedResult, testResult);
        //    }
        //}

        //[Fact]
        //public void IntValidationRejectsInvalidAnswers_MinAndMax()
        //{
        //    // Act
        //    int maxLength = 10;
        //    int minIn = 1;
        //    int maxIn = 4;
        //    List<string> testInputList = new List<string>(new string[] { "0", "5", "a", "" });
        //    foreach (string testInput in testInputList)
        //    {
        //        // Arrange
        //        bool expectedResult = false;
        //        //checkIntInput(string inStr, int maxLength, int minIn = -1, int maxIn = -1)
        //        bool testResult = RevatureP0.UtilMethods.checkIntInput(testInput, maxLength, minIn, maxIn);
        //        // Assert
        //        Assert.Equal(expectedResult, testResult);
        //    }
        //}

        //[Fact]
        //public void ConfirmValidationAcceptsValidAnswers()
        //{
        //    // Act
        //    int maxLength = 10;
        //    List<string> testInputList = new List<string>(new string[] { "y", "n" });
        //    foreach (string testInput in testInputList)
        //    {
        //        // Arrange
        //        bool expectedResult = true;
        //        // checkConfirmationInput(string inStr, int maxLength)
        //        bool testResult = RevatureP0.UtilMethods.checkConfirmationInput(testInput, maxLength);
        //        // Assert
        //        Assert.Equal(expectedResult, testResult);
        //    }
        //}

        //[Fact]
        //public void ConfirmValidationRejectsInvalidAnswers()
        //{
        //    // Act
        //    int maxLength = 10;
        //    List<string> testInputList = new List<string>(new string[] { "a", "1", "" });
        //    foreach (string testInput in testInputList)
        //    {
        //        // Arrange
        //        bool expectedResult = false;
        //        // checkConfirmationInput(string inStr, int maxLength)
        //        bool testResult = RevatureP0.UtilMethods.checkConfirmationInput(testInput, maxLength);
        //        // Assert
        //        Assert.Equal(expectedResult, testResult);
        //    }
        //}
    }
}
