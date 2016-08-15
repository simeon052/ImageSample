using System;
using AppKit;
using CoreGraphics;
using PCLStorage;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using ImageIO;
using MobileCoreServices;

namespace Lib.Mac
{
	public class ImageConvert
	{
		public ImageConvert()
		{
		}

		public static async Task<bool> Convert(string src, string dist)
		{
			IFolder folder = await FileSystem.Current.GetFolderFromPathAsync(Path.GetDirectoryName(src));
			IFile file = await folder.GetFileAsync(Path.GetFileName(src));
			NSImage srcImg = NSImage.FromStream(await file.OpenAsync(PCLStorage.FileAccess.Read));
			var rect = new CGRect(0f, 0f, srcImg.Size.Width, srcImg.Size.Height);
			CGImage cgImage = srcImg.AsCGImage(ref rect, null, null);

			using (var url = NSUrl.FromFilename(dist))
			using (var dstImg = CGImageDestination.Create(url, UTType.PNG, 1))
			{
				dstImg.AddImage(cgImage);
				return dstImg.Close();
			}
		}
	
	}
	
}
