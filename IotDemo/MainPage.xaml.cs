using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;

namespace IotDemo {
  public sealed partial class MainPage : Page {
    private async void BlinkLed () {
      var controller = await GpioController.GetDefaultAsync();
      var gpio18 = controller.OpenPin(18);
      var gpio7 = controller.OpenPin(4);
      var gpio11 = controller.OpenPin(17);
      var gpio13 = controller.OpenPin(27);

      var gpioList = new List<GpioPin>() {
        gpio18, gpio7, gpio11, gpio13
      };

      gpioList.ForEach(item => item.SetDriveMode(GpioPinDriveMode.Output));

      var currentOn = 0;

      var timer = new DispatcherTimer {
        Interval = TimeSpan.FromMilliseconds(25)
      };

      timer.Tick += (sender, o) => {
        for (var i = 0; i < gpioList.Count; i++) {
          var gpio = gpioList[i];
          if (i == currentOn) {
            gpio.Write(GpioPinValue.High);
          } else {
            gpio.Write(GpioPinValue.Low);
          }
        }

        currentOn++;
        if (currentOn > gpioList.Count - 1) {
          currentOn = 0;
        }
      };

      timer.Start();
    }

    public MainPage () {
      this.InitializeComponent();
      this.BlinkLed();
    }
  }
}
