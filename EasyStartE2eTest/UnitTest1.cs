using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Threading;

namespace EasyStartE2eTest
{
  [TestClass]
  public class ScenarioE2e : EasyStartSession
  {

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
      // TODO: Try and catch here. If the WinAppDriver is not started, this will throw, very common
      Setup(context);
    }

    [TestMethod]
    public void HappyPathInstall()
    {
      // Just Launched...

      // "Welcome to Easy Start"  <Manage Options> or <Accept All Options>  ---------------------------------

      FindAndClickByAccessibilityId("consent-accept-all-button");

      // Check before you start <Continue>                                  ---------------------------------
      FindAndClickByAccessibilityId("Continue");

      // "Searching for printers..." -- Action not required, but must wait

      // "Choose a printer" -- list of found printers                       ---------------------------------
      var uiList = SmartFindElementByAccessibilityId("PrinterList");
      Assert.IsNotNull(uiList);

      var list = uiList.FindElementsByClassName("ListBoxItem");
      if (list.Count > 0)
      {
        WindowsElement firstItem = (WindowsElement)list[0];
        firstItem.Click();
      }

      // TODO
      // "Checking your software needs..." -- Action not required, but must wait

      // "Find your printer PIN" -- Enter PIN -- Happens many times
      // If the PIN dialog shows up, fill it in
      HandlePinDialog(true);

      // TODO
      // "Checking your software needs..." -- Action not required, but must wait

      // TODO
      // "Please wait..." -- Action not required, but must wait

      // "Connected printing services" -- <Accept All>                      ---------------------------------
      FindAndClickByAccessibilityId("consent-accept-all-button");

      // "Create an account" -- <Skip account activation>                   ---------------------------------
      FindAndClickByName("Skip account activation");

      // "Dont miss out" -- <Skip account activation>                       ---------------------------------
      FindAndClickByName("Skip account activation");

      // TODO
      // "Find your printer PIN" -- Enter PIN -- Happens many times
      Thread.Sleep(TimeSpan.FromMilliseconds(msWaitForDisplayed));
      var handledPinDialog = HandlePinDialog(true);

      // "Printer updates" -- <Notify>  >>  <Apply>                                       ---------------------------------
      var notifyElement = session.FindElementByXPath("//Pane[@Name = 'HP Product Setup']//child::Text[@Name = 'Notify']//preceding-sibling::Text[1]");
      Assert.IsNotNull(notifyElement);

      if (notifyElement != null)
      {
        notifyElement.Click();
      }

      FindAndClickByName("Apply");

      // "Choose your software and driver" -- <Basic Drivers>  >>  <Continue>                    ---------------------------------
      FindAndClickByXPath("//Button[@Name = 'Continue']//child::Text[@Name = 'Continue']//parent::Button");

      // BBB: End of Happy Path Install
      int j = 11;
    }


    [ClassCleanup]
    public static void ClassCleanup()
    {
      TearDown();
    }
  }
}
