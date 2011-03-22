using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Metaboxer {
	public class Uploader {
		private string path = Path.GetTempPath() + "metaboxer.png";

		public void Upload(Image image) {
			FileStream fileStream = File.Create(path);
			fileStream.Flush();
			fileStream.Close();
			image.Save(path);
			new Thread(new ThreadStart(Metabox)).Start();
		}
		
		private void Metabox() {
			try {
				Process.Start(Encoding.ASCII.GetString(new WebClient().UploadFile("http://metabox.it/", "POST", path)));
			}
			catch (WebException ex) {
				MessageBox.Show(ex.Message, "Metaboxer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			catch (Win32Exception ex) {
				MessageBox.Show(ex.Message, "Metaboxer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			finally {
				File.Delete(path);	
			}
		}
	}
}
