using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace Metaboxer {
	public class CaptureImage : Form {
		private bool bHaveMouse;
		private Point ptOriginal = new Point();
		private Point ptLast = new Point();
		private Rectangle selection;
		private PictureBox pictureBox;

		public CaptureImage() {
			Cursor = Cursors.Cross;
			FormBorderStyle = FormBorderStyle.None;
			Icon = (Icon)new ResourceManager("Metaboxer", Assembly.GetExecutingAssembly()).GetObject("Metaboxer.ico");;
			Text = "Metaboxer - Capture Image";
			WindowState = System.Windows.Forms.FormWindowState.Maximized;
			KeyPress += new KeyPressEventHandler(CaptureImageKeyPress);

			pictureBox = new PictureBox();
			pictureBox.Image = CaptureScreen();
			pictureBox.Dock = DockStyle.Fill;
			pictureBox.MouseDown += new MouseEventHandler(PictureBoxMouseDown);
			pictureBox.MouseMove += new MouseEventHandler(PictureBoxMouseMove);
			pictureBox.MouseUp += new MouseEventHandler(PictureBoxMouseUp);
			Controls.Add(pictureBox);
				
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}
		
		private Bitmap CaptureScreen() {
			Thread.Sleep(100);
			Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
			return bitmap;
		}

		private void DrawRectangle(Point p1, Point p2) {
			p1 = PointToScreen(p1);
			p2 = PointToScreen(p2);
			if (p1.X < p2.X) {
				selection.X = p1.X;
				selection.Width = p2.X - p1.X;
			}
			else {
				selection.X = p2.X;
				selection.Width = p1.X - p2.X;
			}
			if (p1.Y < p2.Y) {
				selection.Y = p1.Y;
				selection.Height = p2.Y - p1.Y;
			}
			else {
				selection.Y = p2.Y;
				selection.Height = p1.Y - p2.Y;
			}
			ControlPaint.DrawReversibleFrame(selection, Color.Black, FrameStyle.Dashed);
		}
		
		private void PictureBoxMouseDown(object sender, MouseEventArgs e) {
			bHaveMouse = true;
			ptOriginal.X = e.X;
			ptOriginal.Y = e.Y;
			ptLast.X = -1;
			ptLast.Y = -1;
		}

		private void PictureBoxMouseMove(object sender, MouseEventArgs e) {
			Point ptCurrent = new Point(e.X, e.Y);
			if (bHaveMouse) {
				if (ptLast.X != -1)
					DrawRectangle(ptOriginal, ptLast);
				ptLast = ptCurrent;
				DrawRectangle(ptOriginal, ptCurrent);
			}
		}

		private void PictureBoxMouseUp(object sender, MouseEventArgs e) {
			if (selection.Width != 0 && selection.Height != 0) {
				Bitmap bitmap = pictureBox.Image as Bitmap;
				Close();
				new Uploader().Upload((Image)bitmap.Clone(selection, bitmap.PixelFormat));
			}
		}

		private void CaptureImageKeyPress(object sender, KeyPressEventArgs e) {
			if (e.KeyChar == (char)27) //Esc
				Close();
		}
	}
}
