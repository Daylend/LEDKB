using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.ServiceProcess;
using System.Diagnostics;
using Microsoft.Win32;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        private ServiceController sc = new ServiceController();
        private EventLog el = new EventLog();
        private Thread thread;
        int speed = 100;

        public Form1()
        {
            InitializeComponent();
            el.Source = "PowerBiosServerSource";
            el.Log = "PowerBiosServerLog";
            sc.ServiceName = "PowerBiosServer";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int num = 0xa000;
            int left = random.Next(1, 7);
            int mid = random.Next(1, 7) << 4;
            int right = random.Next(1, 7) << 8;
            int color = (right | mid) | left;
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\hotkey\LEDKB", "LEDKBColor", color);
            int effect = 0x10000000;
            int num2 = (color | num) | effect;
            this.el.WriteEntry(Convert.ToString(num2));
            Thread.Sleep(50);
            this.sc.ExecuteCommand(0xe7);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (thread != null)
                thread.Abort();
            Effects effect = new Effects();
            thread = new Thread(new ParameterizedThreadStart(effect.Effect1));
            thread.IsBackground = true;
            thread.Start(speed);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (thread != null)
                thread.Abort();
            Effects effect = new Effects();
            thread = new Thread(new ParameterizedThreadStart(effect.Effect2));
            thread.IsBackground = true;
            thread.Start(speed);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (thread != null)
                thread.Abort();
            Effects effect = new Effects();
            thread = new Thread(new ParameterizedThreadStart(effect.Effect3));
            thread.IsBackground = true;
            thread.Start(speed);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread != null)
            {
                thread.Abort();
                Thread.Sleep(100);
                Random random = new Random();
                int num = 0xa000;
                int left = random.Next(1, 7);
                int mid = random.Next(1, 7) << 4;
                int right = random.Next(1, 7) << 8;
                int color = (right | mid) | left;
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\hotkey\LEDKB", "LEDKBColor", color);
                int effect = 0x10000000;
                int num2 = (color | num) | effect;
                this.el.WriteEntry(Convert.ToString(num2));
                Thread.Sleep(50);
                this.sc.ExecuteCommand(0xe7);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            speed += 25;
            label1.Text = "Speed: " + speed;
            if (speed <= vScrollBar1.Maximum)
            {
                vScrollBar1.Value = speed;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (speed > 0)
            {
                speed -= 25;
                label1.Text = "Speed: " + speed;
                if (speed >= vScrollBar1.Minimum)
                {
                    vScrollBar1.Value = speed;
                }
            }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            speed = vScrollBar1.Value;
            label1.Text = "Speed: " + speed;
        }
    }

    public class Effects
    {
        private ServiceController sc = new ServiceController();
        private EventLog el = new EventLog();

        public Effects()
        {
            el.Source = "PowerBiosServerSource";
            el.Log = "PowerBiosServerLog";
            sc.ServiceName = "PowerBiosServer";
        }

        enum Color
        {
            Black, Blue, Red, Purple, Green, Cyan, Yellow, White
        }

        public void SetColor(int left, int mid, int right)
        {
            right = right << 8;
            mid = mid << 4;
            int color = (right | mid) | left;
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\hotkey\LEDKB", "LEDKBColor", color);
            int num2 = (color | 0xa000) | 0x10000000;
            el.WriteEntry(Convert.ToString(num2));
            Thread.Sleep(50);
            sc.ExecuteCommand(0xe7);
        }

        public void Effect1(object parameter)
        {
            while (true)
            {
                for (int i = 1; i < 8; i++)
                {
                    SetColor(i, i, i);
                    Thread.Sleep((int)parameter);
                }
            }
        }

        public void Effect2(object parameter)
        {
            while (true)
            {
                Random random = new Random();
                SetColor(random.Next(1, 7), random.Next(1, 7), random.Next(1, 7));
                Thread.Sleep((int)parameter);
            }
        }

        public void Effect3(object parameter)
        {
            while (true)
            {
                Random random = new Random();
                int rnd = random.Next(1, 7);
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                    {
                        SetColor(rnd, 0, 0);
                    }

                    if (i == 1 || i == 3)
                    {
                        SetColor(0, rnd, 0);
                    }

                    if (i == 2)
                    {
                        SetColor(0, 0, rnd);
                    }
                    Thread.Sleep((int)parameter);
                }
            }
        }
    }
}
