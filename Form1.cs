using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV.Util;
using Emgu.CV.Features2D;
using Emgu.CV;
using Emgu.CV.GPU;
using Emgu.CV.VideoStab;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace deteksi_wajah
{
    public partial class Form1 : Form
    {
        Capture capture; // untuk koneksi ke webcam
        HaarCascade haar;



        public Form1()
        {

            InitializeComponent();


        }

        //method
        //Proses image aquisision bertipe rgb
        private void prosesFrame(object sender, EventArgs arg)
        {
            Image < Bgr, byte > image = capture.QueryFrame(); //hasil koneksi gambar didapat bertipe rbg
            imageBox1.Image = image; // citra yg didapat berada dalam box
               if( image != null)
               {
                    Image < Gray, byte > gray = image.Convert<Gray,byte>();
            var faces =  gray.DetectHaarCascade(haar, 1.1 , 1, 
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20,20))[0];
                   foreach (var face in faces)
                   {
                       Image<Gray,byte>hasil = image.Copy(face.rect).Convert<Gray,byte>().Resize(100, 100, INTER.CV_INTER_CUBIC);
                           image.Draw(face.rect, new Bgr(Color.Red),3);
                   }
               }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (capture == null)
            {
                try
                {
                    capture = new Capture();
                }
                catch
                {
                }
            }
            //jika camera tidak sama dengan null
            if (capture != null)
            {
                if (btn_start.Text == "Pause")
                {
                    btn_start.Text = "Resume";
                    Application.Idle -= prosesFrame; // mengaktifkan kamera
                }
                else
                {
                    btn_start.Text = "Pause";
                    Application.Idle += prosesFrame;
                }


            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            haar = new HaarCascade("haarcascade_frontalface_default.xml");
        }
    }
}
