


using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace EasyStartE2eTest
{
  public class EasyStartSession
  {
    protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

    //private const string AppId = @"D:\Downloads\installers\karnak\Basic_Webpack_x64-51.7.5471-SJ0001_Basicx64_Webpack.exe";
    //private const string AppId = @"D:\Downloads\installers\hp-work-installers\from-hp-com\7855\HPEasyStart_14_4_7.exe";
    private const string AppId = @"C:\Users\sparksb\AppData\Local\Temp\7zS657B\HP.EasyStart.exe";

    protected static int msWaitForDisplayed;
    protected static int countWaitForDisplayed;

    protected static WindowsDriver<WindowsElement> session;

    // ==================================================================================================
    // --------------------------------------------------------------------------------------------------

    public static void Setup(TestContext context)
    {
      // Default 1.5 sec
      msWaitForDisplayed = 50;
      countWaitForDisplayed = 30;

      if (session == null)
      {
        // Launch Notepad
        DesiredCapabilities appCapabilities = new DesiredCapabilities();
        appCapabilities.SetCapability("app", AppId);
        //appCapabilities.SetCapability("appArguments", @"MyTestFile.txt");
        //appCapabilities.SetCapability("appWorkingDir", @"C:\MyTestFolder\");
        session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);

        //// Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
        //session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);

        // Use the session to control the app
        //session.FindElementByClassName("Edit").SendKeys("This is some text");

        //// Appium.WebDriver.4.1.1
        //// Launch Notepad
        //var appiumOptions = new OpenQA.Selenium.Appium.AppiumOptions();
        //appiumOptions.AddAdditionalCapability("app", @"C:\Windows\System32\notepad.exe");
        //appiumOptions.AddAdditionalCapability("appArguments", @"MyTestFile.txt");
        //appiumOptions.AddAdditionalCapability("appWorkingDir", @"C:\MyTestFolder\");
        //var session = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appiumOptions);

        //// Use the session to control the app
        //session.FindElementByClassName("Edit").SendKeys("This is some text");

      }
    }

    // --------------------------------------------------------------------------------------------------

    [TestInitialize]
    public void TestInitialize()
    {
      //// Select all text and delete to clear the edit box
      //editBox.SendKeys(Keys.Control + "a" + Keys.Control);
      //editBox.SendKeys(Keys.Delete);
      //Assert.AreEqual(string.Empty, editBox.Text);
    }

    // ==================================================================================================
    // ==================================================================================================
    // --------------------------------------------------------------------------------------------------

    public static void FindAndClickByXPath(string selector, bool expectPinDialog = false)
    {
      FindAndClickBy("xpath", selector, expectPinDialog);
    }

    // --------------------------------------------------------------------------------------------------

    public static void FindAndClickByAccessibilityId(string selector, bool expectPinDialog = false)
    {
      FindAndClickBy("accessibility id", selector, expectPinDialog);
    }

    // --------------------------------------------------------------------------------------------------

    public static void FindAndClickByName(string selector, bool expectPinDialog = false)
    {
      FindAndClickBy("name", selector, expectPinDialog);
    }

    // --------------------------------------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------

    public static WindowsElement SmartFindElementByXPath(string selector, bool expectPinDialog = false)
    {
      return SmartFindBy("xpath", selector, expectPinDialog);
    }

    // --------------------------------------------------------------------------------------------------

    public static WindowsElement SmartFindElementByAccessibilityId(string selector, bool expectPinDialog = false)
    {
      return SmartFindBy("accessibility id", selector, expectPinDialog);
    }

    // --------------------------------------------------------------------------------------------------

    public static WindowsElement SmartFindElementByName(string selector, bool expectPinDialog = false)
    {
      return SmartFindBy("name", selector, expectPinDialog);
    }

    // --------------------------------------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------

    public static void FindAndClickBy(string by, string id, bool expectPinDialog = false)
    {
      var element = SmartFindBy(by, id, expectPinDialog);
      if (element != null)
      {
        element.Click();
      }

      //var spinnerDisplayed = IsSpinnerDisplayed();
      //var element = FindElementBy(by, id);

      //bool somethingHappened = false;
      //for (int i = 0; i < 5; i++)
      //{
      //  somethingHappened = false;
      //  if (element != null)
      //  {
      //    element.Click();
      //    return;
      //  }

      //  // Wait for any spinner
      //  if (IsSpinnerDisplayed())
      //  {
      //    somethingHappened = true;
      //    WaitForSpinnerDone();
      //  }

      //  // Wait for and handle PIN
      //  if (HandlePinDialog(expectPinDialog))
      //  {
      //    somethingHappened = true;
      //  }

      //  if (!somethingHappened)
      //  {
      //    Thread.Sleep(TimeSpan.FromMilliseconds(msWaitForDisplayed));
      //  }
      //}

      //ShouldNotHappen();
    }

    // --------------------------------------------------------------------------------------------------

    public static WindowsElement SmartFindBy(string by, string id, bool expectPinDialog = false)
    {
      var spinnerDisplayed = IsSpinnerDisplayed();
      WindowsElement element;

      bool somethingHappened = false;
      for (int i = 0; i < 5; i++)
      {
        element = FindElementBy(by, id);
        somethingHappened = false;
        if (element != null)
        {
          return element;
        }

        // Wait for and handle PIN
        if (HandlePinDialog(expectPinDialog))
        {
          somethingHappened = true;
        }

        // Wait for any spinner
        if (!somethingHappened && IsSpinnerDisplayed())
        {
          WaitForSpinnerDone();
          somethingHappened = true;
        }

        if (!somethingHappened)
        {
          Thread.Sleep(TimeSpan.FromMilliseconds(msWaitForDisplayed));
        }
      }

      ShouldNotHappen();
      return null;
    }

    // ==================================================================================================
    // ==================================================================================================
    // --------------------------------------------------------------------------------------------------

    public static void TearDown()
    {
      // Close the application and delete the session
      if (session != null)
      {
        try
        {
          session.Close();

          session.Quit();
        }
        catch(Exception ex)
        {
          SeeException(ex);
        }

        session = null;
      }
    }

    // ==================================================================================================
    // ==================================================================================================
    // --------------------------------------------------------------------------------------------------

    public static bool IsSpinnerDisplayed()
    {
      var element = FindElementByAccessibilityId("tb_Finding");
      if (element != null && element.Displayed)
      {
        return true;
      }

      element = FindElementByAccessibilityId("tb_Solution_Search");
      if (element != null && element.Displayed)
      {
        return true;
      }

      return false;
    }

    // --------------------------------------------------------------------------------------------------

    public static bool WaitForSpinnerDone()
    {
      for (int i = 0; i < 1000; i++)
      {
        if (!IsSpinnerDisplayed())
        {
          return true;
        }
        Thread.Sleep(TimeSpan.FromMilliseconds(msWaitForDisplayed));
      }
      return false;
    }

    // --------------------------------------------------------------------------------------------------

    public static bool HandlePinDialog(bool isExpected = false)
    {
      int count = 1;
      bool isFast = false;
      if (isExpected)
      {
        // Try and find it quick, but try again after a few delays if not immediately seen
        count = 3;
        isFast = true;
      }

      for (int i = 0; i < count; i++)
      {
        // If the PIN dialog shows up, fill it in
        var pinEdit = WaitFindElementByAccessibilityId("printerPin", isFast);
        if (pinEdit != null)
        {
          Assert.IsNotNull(pinEdit);
          pinEdit.SendKeys("28385606");

          //FindAndClickByName("Submit", false);
          var element = FindElementByName("Submit");
          if (element != null)
          {
            element.Click();
            return true;
          }

          ShouldNotHappen();
          return true;
        }
        Thread.Sleep(TimeSpan.FromMilliseconds(msWaitForDisplayed));
      }

      return false;
    }

    // ==================================================================================================
    // --------------------------------------------------------------------------------------------------

    public static WindowsElement FindElementByXPath(string selector)
    {
      return FindElementBy("xpath", selector);
    }

    // --------------------------------------------------------------------------------------------------

    public static WindowsElement FindElementByAccessibilityId(string selector)
    {
      return FindElementBy("accessibility id", selector);
    }

    // --------------------------------------------------------------------------------------------------

    public static WindowsElement FindElementByName(string selector)
    {
      return FindElementBy("name", selector);
    }

    // --------------------------------------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------

    public static ReadOnlyCollection<WindowsElement> FindElementsByXPath(string selector)
    {
      return FindElementsBy("xpath", selector);
    }

    // --------------------------------------------------------------------------------------------------

    public static ReadOnlyCollection<WindowsElement> FindElementsByAccessibilityId(string selector)
    {
      return FindElementsBy("accessibility id", selector);
    }

    // --------------------------------------------------------------------------------------------------

    public static ReadOnlyCollection<WindowsElement> FindElementsByName(string selector)
    {
      return FindElementsBy("name", selector);
    }

    // --------------------------------------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------

    public static WindowsElement WaitFindElementByXPath(string selector, bool fast)
    {
      return WaitFindElementBy("xpath", selector, fast);
    }

    // --------------------------------------------------------------------------------------------------

    public static WindowsElement WaitFindElementByAccessibilityId(string selector, bool fast)
    {
      return WaitFindElementBy("accessibility id", selector, fast);
    }

    // --------------------------------------------------------------------------------------------------

    public static WindowsElement WaitFindElementByName(string selector, bool fast)
    {
      return WaitFindElementBy("name", selector, fast);
    }

    // --------------------------------------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------

    public static ReadOnlyCollection<WindowsElement> WaitFindElementsByXPath(string selector, bool fast)
    {
      return WaitFindElementsBy("xpath", selector, fast);
    }

    // --------------------------------------------------------------------------------------------------

    public static ReadOnlyCollection<WindowsElement> WaitFindElementsByAccessibilityId(string selector, bool fast)
    {
      return WaitFindElementsBy("accessibility id", selector, fast);
    }

    // --------------------------------------------------------------------------------------------------

    public static ReadOnlyCollection<WindowsElement> WaitFindElementsByName(string selector, bool fast)
    {
      return WaitFindElementsBy("name", selector, fast);
    }

    // ==================================================================================================
    // --------------------------------------------------------------------------------------------------

    public static WindowsElement FindElementBy(string by, string selector)
    {
      try
      {
        var elt = session.FindElement(by, selector);
        if (elt.Displayed)
        {
          return elt;
        }
      }
      catch (Exception ex)
      {
        // Print it?
        SeeException(ex);
      }

      return null;
    }

    // --------------------------------------------------------------------------------------------------

    public static ReadOnlyCollection<WindowsElement> FindElementsBy(string by, string selector)
    {
      try
      {
        var elts = session.FindElements(by, selector);
        //if (elts.Displayed)
        //{
        //  return elts;
        //}
        return elts;
      }
      catch (Exception ex)
      {
        // Print it?
        SeeException(ex);
      }

      return null;
    }

    // ==================================================================================================
    // --------------------------------------------------------------------------------------------------

    public static WindowsElement WaitFindElementBy(string by, string selector, bool fast)
    {
      var count = countWaitForDisplayed;
      if (fast)
      {
        count = 2;    // Only sleep once
      }

      for (int i = 0; i < count; i++)
      {
        try
        {
          var elt = session.FindElement(by, selector);
          if (elt.Displayed)
          {
            return elt;
          }
        }
        catch (Exception ex)
        {
          // Print it?
          SeeException(ex);
        }

        Thread.Sleep(TimeSpan.FromMilliseconds(msWaitForDisplayed));
      }

      return null;
    }

    // --------------------------------------------------------------------------------------------------

    public static ReadOnlyCollection<WindowsElement> WaitFindElementsBy(string by, string selector, bool fast)
    {
      for (int i = 0; i < countWaitForDisplayed; i++)
      {
        try
        {
          var elts = session.FindElements(by, selector);
          //if (elts.Displayed)
          //{
          //  return elts;
          //}
          return elts;
        }
        catch (Exception ex)
        {
          // Print it?
          SeeException(ex);
        }

        // Try only once?
        if (fast)
        {
          return null;
        }

        Thread.Sleep(TimeSpan.FromMilliseconds(msWaitForDisplayed));
      }

      return null;
    }

    // ==================================================================================================
    // --------------------------------------------------------------------------------------------------

    public static void SeeException(Exception ex)
    {
      // BBB: See the exception
      int i = 10;
    }

    // --------------------------------------------------------------------------------------------------

    public static void ShouldNotHappen()
    {
      // BBB: Should not happen
      int i = 10;
    }
  }
}
