// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace ImageSample
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextField filenameEdit { get; set; }

		[Outlet]
		AppKit.NSTextField filesList { get; set; }

		[Outlet]
		AppKit.NSPopUpButton TypePopupButton { get; set; }

		[Action ("BrowsePressed:")]
		partial void BrowsePressed (Foundation.NSObject sender);

		[Action ("ConvertPressed:")]
		partial void ConvertPressed (Foundation.NSObject sender);

		[Action ("TypeSelected:")]
		partial void TypeSelected (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (TypePopupButton != null) {
				TypePopupButton.Dispose ();
				TypePopupButton = null;
			}

			if (filenameEdit != null) {
				filenameEdit.Dispose ();
				filenameEdit = null;
			}

			if (filesList != null) {
				filesList.Dispose ();
				filesList = null;
			}
		}
	}
}
