using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thorana
{
    public partial class Form : System.Windows.Forms.Form
    {

        double[,] x = new double[5, 4];
        double[,] y = new double[5, 4];
        int[] points = new int[5];
        Graphics g;

        public Form()
        {
            InitializeComponent();
        }
        private int Dpx(double x)
        {
            return (int)(x + 0.5);
        }

        private int Dpy(double y)
        {
            return panel.Height -((int)(y + 0.5));
        }

        private void DisplayLine(int x1, int y1, int x2, int y2)
        {
            try
            {
                g = panel.CreateGraphics();
            }
            catch (ObjectDisposedException e)
            { }
            g.DrawLine(Pens.Black, x1, y1, x2, y2);
        }

        private void HideLine(int x1, int y1, int x2, int y2)
        {
            try
            {
                g = panel.CreateGraphics();
            }
            catch (ObjectDisposedException e)
            { }
            g.DrawLine(Pens.White, x1, y1, x2, y2);
        }

        private void DrawPolygon(int o)
        {
            int j;
            for(int i=0;i<points[o];i++)
            {
                j = (i + 1) % points[o];
                DisplayLine(Dpx(x[o, i]), Dpy(y[o, i]), Dpx(x[o, j]), Dpy(y[o, j]));
            }
        }

        private void HidePolygon(int o)
        {
            int j;
            for (int i = 0; i < points[o]; i++)
            {
                j = (i + 1) % points[o];
                HideLine(Dpx(x[o, i]), Dpy(y[o, i]), Dpx(x[o, j]), Dpy(y[o, j]));
            }
        }

        private void Translate(int o,double tx,double ty)
        {
            for(int i=0;i<points[o];i++)
            {
                x[o, i] += tx;
                y[o, i] += ty;
            }
        }

        private void Rotate(int o,double t)
        {
            double x1, y1;
            for (int i = 0; i < points[o]; i++)
            {
                x1 = x[o, i];
                y1 = y[o, i];
                x[o, i] = x1 * Math.Cos(t) - y1 * Math.Sin(t);
                y[o, i] = x1 * Math.Sin(t) + y1 * Math.Cos(t);
            }
        }

        private void FixedRotate(int o,double t,double x,double y)
        {
            Translate(o, -x, -y);
            Rotate(o, t);
            Translate(o, x, y);
        }


        

        private async void buttonDraw_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < 5; i++)
                points[i] = 4;
            //big square
            x[0, 0] = -100; y[0, 0] = 100;
            x[0, 1] = 100; y[0, 1] = 100;
            x[0, 2] = 100; y[0, 2] = -100;
            x[0, 3] = -100; y[0, 3] = -100;

            //small square 
            x[1, 0] = -50; y[1, 0] = 50;
            x[1, 1] = 50; y[1, 1] = 50;
            x[1, 2] = 50; y[1, 2] = -50;
            x[1, 3] = -50; y[1, 3] = -50;

            for(int i=2;i<5;i++)
            {
                for(int j=0;j<4;j++)
                {
                    x[i, j] = x[1, j];
                    y[i, j] = y[1, j];
                }
            }

            Translate(0, 250, 250);
            Translate(1, 150, 350);
            Translate(2, 350, 350);
            Translate(3, 350, 150);
            Translate(4, 150, 150);


            for (int i=0;i<500;i++)
            {
                for (int j = 0; j < 5; j++)
                    DrawPolygon(j);

                await Task.Delay(100);

                for (int j = 0; j < 5; j++)
                    HidePolygon(j);

                FixedRotate(0, 0.1,250, 250);
                
                FixedRotate(1,0.1, 250, 250);
                FixedRotate(1, 0.1, x[0, 0], y[0, 0]);
                FixedRotate(2,0.1, 250, 250);
                FixedRotate(2, 0.1, x[0, 1], y[0, 1]);
                FixedRotate(3,0.1,250, 250);
                FixedRotate(3, 0.1, x[0, 2], y[0, 2]);
                FixedRotate(4,0.1, 250, 250);
                FixedRotate(4, 0.1, x[0, 3], y[0, 3]);

            }


        }
    }
}
