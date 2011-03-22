using System;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

[assembly: AssemblyTitle("Metaboxer")]
[assembly: AssemblyDescription("Capture Utility")]
[assembly: AssemblyCompany("Simonxz")]
[assembly: AssemblyProduct("Metaboxer")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

namespace Metaboxer {
	class Program {
		[STAThread]
		static void Main() {
			ToolStripMenuItem toolStripMenuItemSendClipboard = new ToolStripMenuItem();
			toolStripMenuItemSendClipboard.Text = "Send Clipboard";
			toolStripMenuItemSendClipboard.Click += new EventHandler(ToolStripMenuItemSendClipboard);
			
			ToolStripMenuItem toolStripMenuItemCaptureImage = new ToolStripMenuItem();
			toolStripMenuItemCaptureImage.Text = "Capture Image";
			toolStripMenuItemCaptureImage.Click += new EventHandler(ToolStripMenuItemCaptureImageClick);

			ToolStripSeparator toolStripSeparator = new ToolStripSeparator();

			ToolStripMenuItem toolStripMenuItemExit = new ToolStripMenuItem();
			toolStripMenuItemExit.Text = "Exit";
			toolStripMenuItemExit.Click += new EventHandler(ToolStripMenuItemExitClick);

			ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
			contextMenuStrip.Items.AddRange(new ToolStripItem[] { 
				toolStripMenuItemSendClipboard,
				toolStripMenuItemCaptureImage,
				toolStripSeparator, 
				toolStripMenuItemExit
			});

			NotifyIcon notifyIcon = new NotifyIcon();
			notifyIcon.Icon = (Icon)new ResourceManager("Metaboxer", Assembly.GetExecutingAssembly()).GetObject("Metaboxer.ico");;
			notifyIcon.Text = "Metaboxer";
			notifyIcon.ContextMenuStrip = contextMenuStrip;
			notifyIcon.Visible = true;
			Application.Run();
			notifyIcon.Visible = false;
		}
		
		private static void ToolStripMenuItemSendClipboard(object sender, EventArgs e) {
			if (Clipboard.ContainsImage())
				new Uploader().Upload(Clipboard.GetImage());
			else
				MessageBox.Show("The clipboard does not contain any image", "Metaboxer",  MessageBoxButtons.OK);
		}
		
		private static void ToolStripMenuItemCaptureImageClick(object sender, EventArgs e) {
			new CaptureImage().Show();
		}

		private static void ToolStripMenuItemExitClick(object sender, EventArgs e) {
			Application.Exit();
		}
	}
}
