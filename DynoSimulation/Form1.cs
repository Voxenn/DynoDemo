using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//This is a simulated graph. The numbers do not accurately reflect an actual dyno.
namespace DynoSimulation
{
    public partial class Form1 : Form
    {
        
        //Initialize variables to use for creating the graph
        Bitmap drawArea;
        double acceleration, force, distanceTime;
        double J, k1, k2;
        double revs, rpm, rpmPrevious;
        double time, timePrevious;
        double torque, torqueFinal, torqueNew;
        double velocity;
        Graphics graph;        
        Pen stockLine = new Pen(Color.Blue, 1);
        Pen stage1Line = new Pen(Color.Red, 1);
        Pen stage2Line = new Pen(Color.Green, 1);
        static Random random = new Random();
        string stageSelection;

        public void initVar()
        {
            acceleration = 0;
            force = 100;
            distanceTime = 0.05;
            J = 0.3;
            k1 = 0.1; 
            k2 = 0.001;
            revs = 0;
            rpm = 0;
            rpmPrevious = 0;
            time = 0;
            timePrevious = 0;
            torque = 0;
            torqueFinal = 0;
            torqueNew = 0;
            velocity = 0;
        }        

        private void dynoStart()
        {
            //If we've been on the dyno for more than 8 seconds, stop the dyno
            if (time > 5 && force > 0)
            {
                force -= 1;
            }        
            //Torque 
            torque = force * k1;
            torqueFinal = k2 * Math.Pow(velocity, 2);
            torqueNew = torque - torqueFinal;
            //Acceleration is the change in velocity divided by the change in time
            acceleration = torqueNew / J;
            velocity = velocity + acceleration * distanceTime;            
        }

        private void dynoUpdate(Pen pen, double RPM)
        {
            //Draw the next iteration of the dyno curve
            drawLine(pen, timePrevious * 40, 300 - 0.2 * rpmPrevious, time * 40, 300 - 0.2 * RPM);
            rpmPrevious = RPM;
            timePrevious = time;
            time = time + distanceTime;
        }

        public Form1()
        {
            InitializeComponent();
            drawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            graph = Graphics.FromImage(drawArea);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dynoStart();

            //Simulate the power drops for the curve
            if (time < 4)
            {
                revs = (random.NextDouble() *(28 - 26) + 26);
            }
            else if (time >= 4 && time < 9)
            {
                revs = (random.NextDouble() * (28 - 27) + 27);
            }
            else
            {
                revs = (random.NextDouble() * (30 - 28) + 28);
            }
            //Calculate the simulated RPM
            rpm = (revs* velocity) / Math.PI;
            dynoUpdate(stockLine, rpm);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            dynoStart();
            //Simulate the power drops for the curve
            if (time < 4)
            {
                revs = (random.NextDouble() * (38 - 36) + 36);
            }
            else if (time >= 4 && time < 9)
            {
                revs = (random.NextDouble() * (38 - 37) + 37);
            }
            else
            {
                revs = (random.NextDouble() * (40 - 38) + 38);
            }
            //Calculate the simulated RPM
            rpm = (revs * velocity) / Math.PI;
            dynoUpdate(stage1Line, rpm);
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            dynoStart();
            //Simulate the power drops for the curve
            if (time < 4)
            {
                revs = (random.NextDouble() * (44 - 43) + 43);
            }
            else if (time >= 4 && time < 9)
            {
                revs = (random.NextDouble() * (45 - 44) + 44);
            }
            else
            {
                revs = (random.NextDouble() * (46 - 45) + 45);
            }
            //Calculate the simulated RPM
            rpm = (revs * velocity) / Math.PI;
            dynoUpdate(stage2Line, rpm);
        }

        private void drawLine(Pen pen, double x1, double y1, double x2, double y2)
        {
            graph.DrawLine(pen, Convert.ToInt32(x1), Convert.ToInt32(y1), Convert.ToInt32(x2), Convert.ToInt32(y2));
            pictureBox1.Image = drawArea;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initVar();
            //Clear the graph
            graph.Clear(Color.WhiteSmoke);
            //Determines how fast to draw the graph
            timer1.Interval = (int)(distanceTime * 500);
            //Begin the timer
            timer1.Enabled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            stageSelection = comboBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Disable the first line
            timer1.Enabled = false;
            initVar();
            switch(stageSelection)
            {
                case "Stage 1":
                    if (timer3.Enabled == true)
                    {
                        timer3.Enabled = false;
                    }
                    label5.Text = "Stage 1";
                    timer2.Interval = (int)(distanceTime * 500);
                    timer2.Enabled = true;
                    break;
                case "Stage 2":
                    if (timer2.Enabled == true)
                    {
                        timer2.Enabled = false;
                    }
                    label6.Text = "Stage 2";
                    timer3.Interval = (int)(distanceTime * 500);
                    timer3.Enabled = true;
                    break;
                default:
                    break;
            }
        }
    }
}
