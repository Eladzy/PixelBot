using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;//user 32 &dll import

namespace PixelBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const UInt32 MOUSEEVENT_LEFTDOWN = 0x0002;
        const UInt32 MOUSEEVENT_LEFTUP = 0x0004;
        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlag, uint dx, uint dy, uint dwData, uint dwExtraInf);
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int posX, int posY);
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Click()
        {
            mouse_event(MOUSEEVENT_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENT_LEFTUP, 0, 0, 0, 0);
        }
        private void DoubleClickPosition(int posX,int posY)
        {
            SetCursorPos(posX, posY);
            Click();
            System.Threading.Thread.Sleep(250);
            Click();       
        }
        private void OnSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            string input = Txt1.Text;
            SearchPixel(input);
        }
        private bool SearchPixel(string hexCode)
        {
            Bitmap bitmap = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            Graphics graphics = Graphics.FromImage(bitmap as System.Drawing.Image);
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            System.Drawing.Color DesiredColor = ColorTranslator.FromHtml(hexCode);//translate hex codecolor to a color obj
            for (int i = 0; i < SystemInformation.VirtualScreen.Width; i++)
            {
                for (int j = 0; j < SystemInformation.VirtualScreen.Height; j++)
                {
                    //getting current pixel
                    System.Drawing.Color currentPixelColor = bitmap.GetPixel(i, j);
                    //compare current pixel to desired color
                    if (DesiredColor == currentPixelColor)
                    {
                        System.Windows.MessageBox.Show(string.Format($"Found at pixel({i},{j})-Now set mouse cursor"));
                        //set mouse cursor+double click
                        DoubleClickPosition(i, j);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
