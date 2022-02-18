using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Aztec;
using System.IO;

namespace QR_Scanner
{
    public partial class Form1 : Form
    {
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice captureDevice;
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in filterInfoCollection)
                Device1.Items.Add(Device.Name);
            Device1.SelectedIndex = 0;
        }

        private void btnScan(object sender, EventArgs e)
        {
            captureDevice = new VideoCaptureDevice(filterInfoCollection[Device1.SelectedIndex].MonikerString);
            captureDevice.NewFrame += CaptureDevice_NewFrame;
            captureDevice.Start();
            timer1.Start();
           
        }

        private void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void btnExit(object sender, EventArgs e)
        {
            try
            {
                DialogResult iExit;
                iExit = MessageBox.Show("Confirm if you want to exit", "Scan QRCode",
                  MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (iExit == DialogResult.OK)
                {
                    Application.Exit();
                }

                else if (iExit == DialogResult.Cancel)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "QRCODE SCANNER", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (captureDevice.IsRunning)
                captureDevice.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(pictureBox1.Image !=null)
            {
                BarcodeReader reader = new BarcodeReader();
                Result result = reader.Decode((Bitmap)pictureBox1.Image);

                if (result != null)
                {
                    StreamWriter Decode;
                    Decode = File.AppendText("QRCODE FILE.txt");
                    Decode.WriteLine("QRCODE INFORMATION");
                    Decode.WriteLine(" ");
                    Decode.WriteLine("Date:" + " " + DateTime.Now.ToString("MM/dd/yyyy"));
                    Decode.WriteLine("Time:" + " " + DateTime.Now.ToString("h:mm:ss tt"));
                    Decode.WriteLine(" ");
                    Decode.WriteLine(result.Text);
                    Decode.WriteLine(" ");
                    Decode.WriteLine(" ");
                    Decode.Close();
                    MessageBox.Show("Saved!");
                }
            }
        }
    }
}
