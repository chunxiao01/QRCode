using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;


namespace QRCode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("黑色");
            comboBox1.Items.Add("绿色");
            comboBox1.Items.Add("黄色");
            comboBox1.Items.Add("红色");
            comboBox1.SelectedIndex = 0;
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            int width = 320;
            int height = 320;
            try
            {
                if (text.Length == 0 || String.IsNullOrEmpty(text))
                {
                    MessageBox.Show("二维码内容不能为空!", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string frcolor = "#000000";
                    switch (comboBox1.Text)
                    {
                        case "黑色":
                            frcolor = "#000000";
                            break;
                        case "绿色":
                            frcolor = "#3AB36C";
                            break;
                        case "黄色":
                            frcolor = "#FA9623";
                            break;
                        case "红色":
                            frcolor = "#FE3636";
                            break;
                        default:
                            frcolor = "#000000";
                            break;

                    }
                    if (pictureBox2.Image == null)
                    {
                        pictureBox1.Image = Generate_Color(text, frcolor,width, height);
                    }
                    else {
                        Bitmap bitmaplogo = new Bitmap(pictureBox2.Image);
                        pictureBox1.Image = Generate_Logo_Color(text, bitmaplogo, frcolor, width, height); 
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }


        /// <summary>
        /// 保存二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null && String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("二维码图片为空!", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                SaveFileDialog saveImgD = new SaveFileDialog();
                saveImgD.Title = "二维码保存";
                saveImgD.Filter = @"jpeg|*.jpg|png|*.png";
                saveImgD.FileName = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                if (saveImgD.ShowDialog() == DialogResult.OK)
                {
                    string filename = saveImgD.FileName.ToString();
                    if (filename != "" && filename != null)
                    {
                        string fileextname = filename.Substring(filename.LastIndexOf(".") + 1).ToString();
                        ImageFormat imgformat = null;
                        if (fileextname != "")
                        {
                            switch (fileextname)
                            {
                                case "jpg":
                                    imgformat = ImageFormat.Jpeg;
                                    break;
                                case "png":
                                    imgformat = ImageFormat.Png;
                                    break;
                                default:
                                    imgformat = ImageFormat.Jpeg;
                                    break;
                            }
                            try
                            {
                                Bitmap bitmap = new Bitmap(pictureBox1.Image);
                                MessageBox.Show("保存路径：" + filename, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                pictureBox1.Image.Save(filename, imgformat);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 从剪切板粘贴内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();
            if (iData.GetDataPresent(DataFormats.Text))
            {
                textBox1.Text = (String)iData.GetData(DataFormats.Text);
            }
        }

        /// <summary>
        /// 添加二维码Logo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "图片|*.jpg;*.png;*.jpeg;&.ico";
            openfile.ShowDialog();
            if (openfile.FileName != String.Empty) {
                try
                {
                    Image img = Image.FromFile(openfile.FileName);
                    img = ImageSizeFormat(img, pictureBox2.Width, pictureBox2.Height);
                    pictureBox2.Image = img;
                }
                catch (Exception ex) {
                    Console.WriteLine(ex);
                }
            }
            openfile.Dispose();
        }

        /// <summary>
        /// 打开二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "图片|*.jpg;*.png;*.jpeg;&.ico";
            openfile.ShowDialog();
            if (openfile.FileName != String.Empty)
            {
                try
                {
                    Image img = Image.FromFile(openfile.FileName);
                    img = ImageSizeFormat(img, pictureBox1.Width, pictureBox1.Height);
                    pictureBox1.Image = img;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            openfile.Dispose();
        }

        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("二维码图片不能为空!", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                BarcodeReader reader = new BarcodeReader();
                //reader.Options = options;
                reader.AutoRotate = true;
                reader.Options.CharacterSet = "UTF-8";
                Bitmap map = new Bitmap(pictureBox1.Image);
                Result result = reader.Decode(map);
                textBox2.Text= result != null ? result.Text : String.Empty;
            }
        }


        /// <summary>
        /// 复制到剪切板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {

            if (textBox2.Text != "")
            {
                Clipboard.SetDataObject(textBox2.Text);
            }

        }

        /// <summary>
        /// 生成特定颜色二维码
        /// </summary>
        /// <returns>图片</returns
        public static Bitmap Generate_Color(string text, string frcolor, int width, int height)
        {
            BarcodeWriter writer = new BarcodeWriter();
            Color bgcolor = ColorTranslator.FromHtml("#ffffff");
            Color forecolor = ColorTranslator.FromHtml(frcolor);
            writer.Renderer = new ZXing.Rendering.BitmapRenderer { Background = bgcolor, Foreground = forecolor };
            writer.Format = BarcodeFormat.QR_CODE;
            QrCodeEncodingOptions options = new QrCodeEncodingOptions()
            {
                DisableECI = true,//设置内容编码
                CharacterSet = "UTF-8",  //设置二维码的宽度和高度
                Width = width,
                Height = height,
                Margin = 1//设置二维码的边距,单位不是固定像素
            };

            writer.Options = options;
            Bitmap map = writer.Write(text);
            return map;
        }


        /// <summary>
        /// 生成特定颜色二维码
        /// </summary>
        /// <returns>图片</returns
        public static Bitmap Generate_Logo_Color(string text, Bitmap bitmaplogo, string frcolor ,int width, int height)
        {

            //Logo 图片
            Bitmap logo = bitmaplogo;
            //构造二维码写码器
            MultiFormatWriter writer = new MultiFormatWriter();
            Dictionary<EncodeHintType, object> hint = new Dictionary<EncodeHintType, object>();
            hint.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            hint.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);

            //生成二维码 
            BitMatrix bm = writer.encode(text, BarcodeFormat.QR_CODE, width, height, hint);
            //BarcodeWriter barcodeWriter = new BarcodeWriter();
            Color bgcolor = ColorTranslator.FromHtml("#ffffff");
            Color forecolor = ColorTranslator.FromHtml(frcolor);
            BarcodeWriter barcodeWriter = new BarcodeWriter { 
            Renderer =new ZXing.Rendering.BitmapRenderer {
                Foreground = forecolor
            }
            };
            Bitmap map = barcodeWriter.Write(bm);


            //获取二维码实际尺寸（去掉二维码两边空白后的实际尺寸）
            int[] rectangle = bm.getEnclosingRectangle();

            //计算插入图片的大小和位置
            int middleW = Math.Min((int)(rectangle[2] / 4), logo.Width);
            int middleH = Math.Min((int)(rectangle[3] / 4), logo.Height);
            int middleL = (map.Width - middleW) / 2;
            int middleT = (map.Height - middleH) / 2;

            //将img转换成bmp格式，否则后面无法创建Graphics对象
            Bitmap bmpimg = new Bitmap(map.Width, map.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmpimg))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(map, 0, 0);
            }
            //将二维码插入图片
            Graphics myGraphic = Graphics.FromImage(bmpimg);
            //白底
            myGraphic.FillRectangle(Brushes.Red, middleL, middleT, middleW, middleH);
            myGraphic.DrawImage(logo, middleL, middleT, middleW, middleH);
            return bmpimg;
        }


        public static Image ImageSizeFormat(Image img, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            Point[] destPoints = new Point[] {
            new Point(0,0),
            new Point(width,0),
            new Point(0,height)
            };
            Rectangle rect = RectImg(img.Width, img.Height);
            graphics.DrawImage(img, destPoints, rect, GraphicsUnit.Pixel);
            Image image = Image.FromHbitmap(bitmap.GetHbitmap());
            bitmap.Dispose();
            graphics.Dispose();
            return image;
        }

        public static Rectangle RectImg(int width, int height) {
            int x = 0;
            int y = 0;
            if (height > width)
            {
                height = width;
                y = (height - width) / 2;
            }
            else {
                width = height;
                x = (width - height) / 2;
            }

            return new Rectangle(x, y, width, height);
        }
    }
}
