using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Contains assertion types that are not provided with the standard MSTest assertions.
/// </summary>
public static class ExceptionAssert
{
    #region Public Static Methods

    /// <summary>
    /// Checks to make sure that the input delegate throws an exception of type TException.
    /// </summary>
    /// <typeparam name="TException">The type of exception expected.</typeparam>
    /// <param name="blockToExecute">The block of code to execute to generate the exception.</param>
    public static void Throws<TException>(Action blockToExecute) where TException : Exception
    {
        try
        {
            blockToExecute();
        }
        catch (Exception ex)
        {
            Assert.IsTrue(ex.GetType() == typeof(TException), $"Expected exception of type {typeof(TException)} but type of {ex.GetType()} was thrown instead.");
            return;
        }

        Assert.Fail($"Expected exception of type {typeof(TException)} but no exception was thrown.");
    }

    /// <summary>
    /// Checks to make sure that the input delegate throws an exception of type TException.
    /// </summary>
    /// <typeparam name="TException">The type of exception expected.</typeparam>
    /// <param name="blockToExecute">The block of code to execute to generate the exception.</param>
    /// <param name="expectedMessage">The expected exception message.</param>
    public static void Throws<TException>(string expectedMessage, Action blockToExecute) where TException : Exception
    {
        try
        {
            blockToExecute();
        }
        catch (Exception ex)
        {
            Assert.IsTrue(ex.GetType() == typeof(TException), $"Expected exception of type {typeof(TException)} but type of {ex.GetType()} was thrown instead.");
            Assert.AreEqual(expectedMessage, ex.Message, $"Expected exception with a message of '{expectedMessage}' but exception with message of '{ex.Message}' was thrown instead.");
            return;
        }

        Assert.Fail($"Expected exception of type {typeof(TException)} but no exception was thrown.");
    }

    /// <summary>
    /// Checks to make sure that the input delegate throws an exception of type TException.
    /// </summary>
    /// <typeparam name="TException">The type of exception expected.</typeparam>
    /// <param name="action">The block of code to execute to generate the exception.</param>
    /// <param name="message">The expected exception message.</param>
    public static void Throws<TException>(Action action, string message) where TException : Exception
    {
        try
        {
            action();

            Assert.Fail($"Exception of type {typeof(TException).Name} expected; got no exception");
        }
        catch (TException ex)
        {
            Assert.AreEqual(message, ex.Message);
        }
        catch (Exception ex)
        {
            Assert.Fail($"Exception of type {typeof(TException).Name} expected; got exception of type {ex.GetType().Name}");
        }
    }

    #endregion
}