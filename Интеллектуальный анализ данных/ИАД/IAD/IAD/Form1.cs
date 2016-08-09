using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.IO;

namespace IAD
{
    public partial class IAD : Form
    {
        GraphBuilder graphBuilder;
        AR AutoR = new AR();
        MA MovA = new MA();
        RW RWwD = new RW();
        int Model = 0;
        int lenghtData = 32;
        int steps;
        int graphFat = 3, forecastFat = 3, modelFat = 2, borderFat = 1;
        List<PointF> data = new List<PointF>();
        List<PointF> result = new List<PointF>();
        List<PointF> upBorder;
        List<PointF> bottomBorder;
        Pen pen = new Pen(Color.Black);
        float[] Student = { 12.706f, 4.3027f, 3.1824f, 2.7764f, 2.5706f, 2.4469f, 2.3646f, 2.3060f, 2.2622f, 2.2281f, 
                            2.2010f, 2.1788f, 2.1604f, 2.1448f, 2.1314f, 2.1199f, 2.1098f, 2.1009f, 2.0930f, 2.0860f, 
                            2.0796f, 2.0739f, 2.0687f, 2.0639f, 2.0595f, 2.0555f, 2.0518f, 2.0484f, 2.0452f, 2.0423f,
                            2.0395f, 2.0369f, 2.0345f, 2.0322f, 2.0301f, 2.0281f, 2.0262f, 2.0244f, 2.0227f, 2.0211f, 
                            2.0195f, 2.0181f, 2.0167f, 2.0154f, 2.0141f, 2.0129f, 2.0117f, 2.0106f, 2.0096f, 2.0086f };
        void GenerateData()
        {
            data.Clear();
            /*Random rand = new Random();
            for (int i = 0; i < lenghtData; i++)
            {
                data.Add(new PointF(i + 1, rand.Next(-5 + i, 15 + i)));
            }*/
            data.Add(new PointF(1,15.465f));
            data.Add(new PointF(2,13.334f));
            data.Add(new PointF(3,15.844f));
            data.Add(new PointF(4,16.450f));
            data.Add(new PointF(5,11.009f));
            data.Add(new PointF(6,13.078f));
            data.Add(new PointF(7,18.911f));
            data.Add(new PointF(8,23.522f));
            data.Add(new PointF(9 ,21.600f));
            data.Add(new PointF(10,22.061f)); 
            data.Add(new PointF(11,14.657f)); 
            data.Add(new PointF(12,27.738f)); 
            data.Add(new PointF(13,12.334f)); 
            data.Add(new PointF(14,11.957f)); 
            data.Add(new PointF(15,19.651f)); 
            data.Add(new PointF(16,20.038f)); 
            data.Add(new PointF(17,8.834f));
            data.Add(new PointF(18,19.716f));
            data.Add(new PointF(19,21.467f));
            data.Add(new PointF(20,20.501f));
            data.Add(new PointF(21,22.210f));
            data.Add(new PointF(22,18.591f));
            data.Add(new PointF(23,30.516f));
            data.Add(new PointF(24,13.264f));
            data.Add(new PointF(25,14.636f));
            data.Add(new PointF(26,15.340f));
            data.Add(new PointF(27,18.886f));
            data.Add(new PointF(28,13.743f));
            data.Add(new PointF(29,20.858f));
            data.Add(new PointF(30,22.713f));
            data.Add(new PointF(31,35.458f));
            data.Add(new PointF(32,24.220f));
        }

        void CreateConfidenceLimits(int length)
        {
            upBorder = new List<PointF>();
            bottomBorder = new List<PointF>();
            upBorder.Clear();
            bottomBorder.Clear();
            upBorder.Add(new PointF(data[data.Count - 1].X, data[data.Count - 1].Y));
            bottomBorder.Add(new PointF(data[data.Count - 1].X, data[data.Count - 1].Y));
            for (int k = 0; k < length; k++)
            {
                float Sy, meanY;
                Sy = meanY = 0;
                for (int i = 0; i < data.Count; i++)
                {
                    meanY += data[i].Y;
                }
                for (int i = 0; i < k; i++)
                {
                    meanY += result[i + 1].Y;
                }
                meanY /= data.Count + k + 1;

                for (int i = 0; i < data.Count; i++)
                {
                    Sy += (data[i].Y - meanY) * (data[i].Y - meanY);
                }
                for (int i = 0; i < k; i++)
                {
                    Sy += (result[i + 1].Y - meanY) * (result[i + 1].Y - meanY);
                }
                Sy /= data.Count + k + 1;

                Sy = (float)Math.Sqrt(Sy);
                upBorder.Add(new PointF(result[k + 1].X, result[k + 1].Y + Sy * Student[lenghtData - 1]));
                bottomBorder.Add(new PointF(result[k + 1].X, result[k + 1].Y - Sy * Student[lenghtData - 1]));
            }
            pen = new Pen(Color.Blue, borderFat);
            pen.DashStyle = DashStyle.Dash;
            graphBuilder.DrawPlot(pen, upBorder);
            graphBuilder.DrawPlot(pen, bottomBorder);
        }

        void CreateGraphicAR()
        {
            //Исходный график
            pen = new Pen(Color.Red, graphFat);
            graphBuilder.DrawPlot(pen, data);
            
            //Тренд
            dataDGV.Rows.Clear();
            for (int i = 0; i < data.Count; i++)
            {
                dataDGV.Rows.Add(data[i].X, data[i].Y);
            }
            //прогноз по AR
            steps = Convert.ToInt32(stepsART.Text);
            int exponent = (int)modelARNmr.Value;
            if (steps > 0) 
            { 
                List<PointF> forecast = AutoR.Forecast(data, steps,exponent);
                result = forecast;
                CreateConfidenceLimits(steps);
                pen = new Pen(Color.Green, forecastFat);
                graphBuilder.DrawPlot(pen, forecast);
                List<PointF> model = AutoR.Model(data,exponent);
                pen = new Pen(Color.Black, modelFat);
                pen.DashStyle = DashStyle.Dash;
                graphBuilder.DrawPlot(pen, model);
                for (int i = 1; i < steps + 1; i++)
                {
                    dataDGV.Rows.Add(forecast[i].X, forecast[i].Y);
                }
            }
        }

        void CreateGraphicMA()
        {
            //Исходный график
            pen = new Pen(Color.Red, graphFat);
            graphBuilder.DrawPlot(pen, data);
            dataDGV.Rows.Clear();
            for (int i = 0; i < data.Count; i++)
            {
                dataDGV.Rows.Add(data[i].X, data[i].Y);
            }
            //прогноз по MA
            steps = Convert.ToInt32(stepsMAT.Text);
            int q = Convert.ToInt32(modelMANmr.Value);
            if (steps > 0)
            {
                List<PointF> forecast = MovA.Solve(data, q, steps);
                result = forecast;
                CreateConfidenceLimits(steps);
                pen = new Pen(Color.Green, forecastFat);
                graphBuilder.DrawPlot(pen, forecast);
                
                for (int i = 1; i < steps + 1; i++)
                {
                    dataDGV.Rows.Add(forecast[i].X, forecast[i].Y);
                }
            }
        }

        void CreateGraphicRW()
        {
            //Исходный график
            pen = new Pen(Color.Red, graphFat);
            graphBuilder.DrawPlot(pen, data);
            dataDGV.Rows.Clear();
            for (int i = 0; i < data.Count; i++)
            {
                dataDGV.Rows.Add(data[i].X, data[i].Y);
            }
            //прогноз по MA
            steps = Convert.ToInt32(stepsRWT.Text);
            float alfa = (float)Convert.ToDouble(alfaT.Text);
            if (steps > 0)
            {
                List<PointF> forecast = RWwD.forecast(data, steps, alfa);            

                result = forecast;
                CreateConfidenceLimits(steps);
                pen = new Pen(Color.Green, forecastFat);
                graphBuilder.DrawPlot(pen, forecast);

                for (int i = 1; i < steps + 1; i++)
                {
                    dataDGV.Rows.Add(forecast[i].X, forecast[i].Y);
                }
            }
        }

        void GraphicsInitial(int flag)
        {
            if (flag == 0)
            {
                graphBuilder.SetGraphics(scene, graphBuilder.center);
            }
            else
            {
                graphBuilder.SetGraphics(scene, new Point(
                -XScrBr.Value * scaleXTrcBr.Value + scene.Width / 2,
                -YScrBr.Value * scaleYTrcBr.Value + scene.Height / 2));
            }
            graphBuilder.DrawGrid(new Pen(Color.LightGray));
        }
        
        public IAD()
        {
            InitializeComponent();
            graphBuilder = new GraphBuilder(scene, new Point(scene.Width / 2, scene.Height / 2), new Point(5, 5));
            GraphicsInitial(0);
            scaleXTrcBr.Value = 5;
            scaleYTrcBr.Value = 5;
            modelMANmr.Maximum = lenghtData - 1;
            scaleValX.Text = scaleXTrcBr.Value.ToString();
            scaleValY.Text = scaleYTrcBr.Value.ToString();
        }

        private void dataLoadBtn_Click(object sender, EventArgs e)
        {
            GenerateData();
            GraphicsInitial(0);
            if (Model != 0)
            {
                if (Model == 1) CreateGraphicAR();
                if (Model == 2) CreateGraphicMA();
                if (Model == 3) CreateGraphicRW();
            }
            modelMANmr.Maximum = lenghtData - 1;
        }

        private void scalexTrcBr_Scroll(object sender, EventArgs e)
        {
            GraphicsInitial(0);
            graphBuilder.scale.X = scaleXTrcBr.Value;
            scaleYTrcBr.Value = scaleXTrcBr.Value;
            graphBuilder.scale.Y = scaleYTrcBr.Value;
            scaleValX.Text = scaleXTrcBr.Value.ToString();
            scaleValY.Text = scaleYTrcBr.Value.ToString();
            if (Model != 0)
            {
                if (Model == 1) CreateGraphicAR();
                if (Model == 2) CreateGraphicMA();
                if (Model == 3) CreateGraphicRW();
            }
        }

        private void scaleyTrcBr_Scroll(object sender, EventArgs e)
        {
            GraphicsInitial(0);
            graphBuilder.scale.Y = scaleYTrcBr.Value;
            scaleXTrcBr.Value = scaleYTrcBr.Value;
            graphBuilder.scale.X = scaleXTrcBr.Value;
            scaleValX.Text = scaleXTrcBr.Value.ToString();
            scaleValY.Text = scaleYTrcBr.Value.ToString();
            if (Model != 0)
            {
                if (Model == 1) CreateGraphicAR();
                if (Model == 2) CreateGraphicMA();
                if (Model == 3) CreateGraphicRW();
            }
        }

        private void gridBtn_Click(object sender, EventArgs e)
        {
            GraphicsInitial(0);
        }

        private void модельАвторегрессииToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dataLoadBtn.Enabled = true;
            graphBuildBtn.Enabled = true;
            dataDGV.Enabled = true;
            dataLbl.Enabled = true;
            readBtn.Enabled = true;
            writeBtn.Enabled = true;
            dataLoadBtn.Visible = true;
            graphBuildBtn.Visible = true;
            dataDGV.Visible = true;
            dataLbl.Visible = true;
            readBtn.Visible = true;
            writeBtn.Visible = true;
            
            modelARLbl.Enabled = true;
            modelARNmr.Enabled = true;
            stepsARLbl.Enabled = true;
            stepsART.Enabled = true;
            modelARLbl.Visible = true;
            modelARNmr.Visible = true;
            stepsARLbl.Visible = true;
            stepsART.Visible = true;


            modelMANmr.Enabled = false;
            modelMALbl.Enabled = false;
            stepsMALbl.Enabled = false;
            stepsMAT.Enabled = false;
            modelMANmr.Visible = false;
            modelMALbl.Visible = false;
            stepsMALbl.Visible = false;
            stepsMAT.Visible = false;

            stepsRWLbl.Enabled = false;
            stepsRWT.Enabled = false;
            RWalfaLbl.Enabled = false;
            alfaT.Enabled = false;
            stepsRWLbl.Visible = false;
            stepsRWT.Visible = false;
            RWalfaLbl.Visible = false;
            alfaT.Visible = false;

            Model = 1;
            modelNameLbl.Text = "Модель:Авторегрессия";
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            GraphicsInitial(1);
            if (Model == 1)
                CreateGraphicAR();
            if (Model == 2)
                CreateGraphicMA();
            if (Model == 3)
                CreateGraphicRW();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            GraphicsInitial(1);
            if (Model == 1)
                CreateGraphicAR();
            if (Model == 2)
                CreateGraphicMA();
            if (Model == 3)
                CreateGraphicRW();
        }

        private void модельСкользящегоСреднегоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataLoadBtn.Enabled = true;
            graphBuildBtn.Enabled = true;
            dataDGV.Enabled = true;
            dataLbl.Enabled = true;
            readBtn.Enabled = true;
            writeBtn.Enabled = true;
            dataLoadBtn.Visible = true;
            graphBuildBtn.Visible = true;
            dataDGV.Visible = true;
            dataLbl.Visible = true;
            readBtn.Visible = true;
            writeBtn.Visible = true;

            modelARLbl.Enabled = false;
            modelARNmr.Enabled = false;
            stepsARLbl.Enabled = false;
            stepsART.Enabled = false;
            modelARLbl.Visible = false;
            modelARNmr.Visible = false;
            stepsARLbl.Visible = false;
            stepsART.Visible = false;
            
            
            modelMANmr.Enabled = true;
            modelMALbl.Enabled = true;
            stepsMALbl.Enabled = true;
            stepsMAT.Enabled = true;
            modelMANmr.Visible = true;
            modelMALbl.Visible = true;
            stepsMALbl.Visible = true;
            stepsMAT.Visible = true;

            stepsRWLbl.Enabled = false;
            stepsRWT.Enabled = false;
            RWalfaLbl.Enabled = false;
            alfaT.Enabled = false;
            stepsRWLbl.Visible = false;
            stepsRWT.Visible = false;
            RWalfaLbl.Visible = false;
            alfaT.Visible = false;

            Model = 2;
            modelNameLbl.Text = "Модель:Скользящее среднее";
        }

        private void graphBuildBtn_Click(object sender, EventArgs e)
        {
            GraphicsInitial(0);
            if (Model == 1)
                CreateGraphicAR();
            if (Model == 2)
                CreateGraphicMA();
            if (Model == 3)
                CreateGraphicRW();
            
        }

        private void stepsT_TextChanged(object sender, EventArgs e)
        {
            string text = stepsART.Text;
            if (text != "")
            {
                try
                {
                    Convert.ToInt32(stepsART.Text);
                }
                catch
                {
                    MessageBox.Show("Введена не цифра");
                    stepsART.Text = "";
                }
            }
        }

        private void модельСлучайногоБлужданияСоСдвигомToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataLoadBtn.Enabled = true;
            graphBuildBtn.Enabled = true;
            dataDGV.Enabled = true;
            dataLbl.Enabled = true;
            readBtn.Enabled = true;
            writeBtn.Enabled = true;
            dataLoadBtn.Visible = true;
            graphBuildBtn.Visible = true;
            dataDGV.Visible = true;
            dataLbl.Visible = true;
            readBtn.Visible = true;
            writeBtn.Visible = true;

            modelARLbl.Enabled = false;
            modelARNmr.Enabled = false;
            stepsARLbl.Enabled = false;
            stepsART.Enabled = false;
            modelARLbl.Visible = false;
            modelARNmr.Visible = false;
            stepsARLbl.Visible = false;
            stepsART.Visible = false;


            modelMANmr.Enabled = false;
            modelMALbl.Enabled = false;
            stepsMALbl.Enabled = false;
            stepsMAT.Enabled = false;
            modelMANmr.Visible = false;
            modelMALbl.Visible = false;
            stepsMALbl.Visible = false;
            stepsMAT.Visible = false;

            stepsRWLbl.Enabled = true;
            stepsRWT.Enabled = true;
            RWalfaLbl.Enabled = true;
            alfaT.Enabled = true;
            stepsRWLbl.Visible = true;
            stepsRWT.Visible = true;
            RWalfaLbl.Visible = true;
            alfaT.Visible = true;

            Model = 3;
            modelNameLbl.Text = "Модель:Случайное блуждание со сдвигом";
        }

        private void stepsMAT_TextChanged(object sender, EventArgs e)
        {
            string text = stepsMAT.Text;
            if (text != "")
            {
                try
                {
                    Convert.ToInt32(stepsMAT.Text);
                }
                catch
                {
                    MessageBox.Show("Введена не цифра");
                    stepsMAT.Text = "";
                }
            }
        }

        private void stepsRWT_TextChanged(object sender, EventArgs e)
        {
            string text = stepsRWT.Text;
            if (text != "")
            {
                try
                {
                    Convert.ToInt32(stepsRWT.Text);
                }
                catch
                {
                    MessageBox.Show("Введена не цифра");
                    stepsRWT.Text = "";
                }
            }
        }

        private void alfaT_TextChanged(object sender, EventArgs e)
        {
            string text = alfaT.Text;
            if(text != "-")
            try
            {
                Convert.ToDouble(alfaT.Text);
            }
            catch
            {
                MessageBox.Show("Введена не цифра с плавающей точкой");
                alfaT.Text = "";
            }
        }

        private void readBtn_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            try
            {
                string fileName = openFileDialog1.FileName;
                StreamReader stream = new StreamReader(fileName, Encoding.ASCII);
                char[] separator = new char[3];
                separator[0] = ' ';
                separator[1] = ';';
                data.Clear();
                while (!stream.EndOfStream)
                {
                    string str = stream.ReadLine();
                    string[] values = str.Split(separator);
                    data.Add(new PointF((float)Convert.ToDouble(values[0]), (float)Convert.ToDouble(values[1])));
                }
                stream.Close();
                lenghtData = data.Count;
                modelMANmr.Maximum = lenghtData - 1;
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при открытии файла.\nПожалуйста повторите попытку.");
            }
        }
    }
}
