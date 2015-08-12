// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace TextStyleDemonstration
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UITextView body { get; set; }

		[Outlet]
		UIKit.UILabel labelOne { get; set; }

		[Outlet]
		UIKit.UILabel labelThree { get; set; }

		[Outlet]
		UIKit.UILabel labelTwo { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (body != null) {
				body.Dispose ();
				body = null;
			}

			if (labelOne != null) {
				labelOne.Dispose ();
				labelOne = null;
			}

			if (labelThree != null) {
				labelThree.Dispose ();
				labelThree = null;
			}

			if (labelTwo != null) {
				labelTwo.Dispose ();
				labelTwo = null;
			}
		}
	}
}
