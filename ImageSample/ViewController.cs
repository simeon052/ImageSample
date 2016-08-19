using System;

using AppKit;
using Foundation;
//using static Lib.Mac.ImageConvert;
using Lib.Mac;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ImageSample
{
	public partial class ViewController : NSViewController
	{
		public ViewController(IntPtr handle) : base(handle)
		{
		}

		private List<string> srcList = new List<string>();
		private ImageConvert.ImageType imgType;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Do any additional setup after loading the view.
			filesList.StringValue = string.Empty;

			foreach (ImageConvert.ImageType t in Enum.GetValues(typeof(ImageConvert.ImageType)))
			{
				TypePopupButton.InsertItem(t.ToString(), (nint)(int)t);
				System.Diagnostics.Debug.WriteLine($"{t}. {(int)t}");
			}
		}

		public override NSObject RepresentedObject
		{
			get
			{
				return base.RepresentedObject;
			}
			set
			{
				base.RepresentedObject = value;
				// Update the view, if already loaded.
			}
		}

		async partial void ConvertPressed(NSObject sender)
		{
			//			List<string> srcList = new List<string> { @"/Users/matsussa/Documents/cat1.png", @"/Users/matsussa/Documents/cat2.jpg" };
			if (srcList.Count != 0)
			{
				var ic = new ImageConvert(this);
				var result = await ic.Convert(srcList, imgType).ConfigureAwait(false);

				InvokeOnMainThread(() =>
				{
					if (result)
					{
						filenameEdit.StringValue = "Done!";
					}
					else {
						filenameEdit.StringValue = "Fail!";
					}
				});
				filesList.StringValue = string.Empty;
			}
		}

		partial void TypeSelected(NSObject sender)
		{
			imgType = (Lib.Mac.ImageConvert.ImageType)(int)TypePopupButton.IndexOfSelectedItem;
		}

		partial void BrowsePressed(NSObject sender)
		{
			using (var panel = new NSOpenPanel())
			{
				panel.AllowsMultipleSelection = true;
				panel.AllowedFileTypes = new string[] { "jpg", "png", "bmp", "tif", "pdf" };
				//panel.AllowedFileTypes = new string[] { "tif" };

				panel.RunModal();

				srcList.AddRange(panel.Filenames);
				filesList.StringValue = string.Empty;
				foreach (var s in srcList)
				{
					filesList.StringValue += s + System.Environment.NewLine;
				}
			}
		}
	}
}
