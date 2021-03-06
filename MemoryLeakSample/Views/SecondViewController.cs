﻿using System;
using Foundation;
using UIKit;

namespace MemoryLeakSample.Views
{
    [Register("SecondViewController")]
    public partial class SecondViewController : UIViewController
    {
        int count = Counter.Default.Count;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            InitializeUI();

            disiplayAlertButton.TouchUpInside += (s, e) =>
            {
                PresentAlert("Alert");
            };

            dismissViewButton.TouchUpInside += (s, e) =>
            {
                DismissViewController(true, () =>
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    System.Diagnostics.Debug.WriteLine("---Close SecondView------------------------------");
                });
            };
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            System.Diagnostics.Debug.WriteLine($"Disposed SecondViewController {count}");

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        ~SecondViewController()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            System.Diagnostics.Debug.WriteLine("Finalized SecondViewController");
        }

        void PresentAlert(string message)
        {
            var alertController = UIAlertController.Create(string.Empty, message, UIAlertControllerStyle.Alert);
            alertController.AddAction(UIAlertAction.Create("Close", UIAlertActionStyle.Cancel, null));
            PresentViewController(alertController, true, null);
        }

    }
}

