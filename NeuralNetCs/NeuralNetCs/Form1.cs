using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using NeuralNetCs;

namespace JA_project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.Text = "repeat";
            int[] tmparray = new int[10000/5];
            for (int i = 5; i <= 10000; i+=5)
               comboBox1.Items.Add(i);

            comboBox3.Text = "gate type";
            comboBox3.Items.Add("AND");
            comboBox3.Items.Add("OR");
            comboBox3.Items.Add("XOR");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[] schematic = new int[] { 3,  1 }; //static
            double[][] data = new double[4][];
            double[] expected_values;
            if (comboBox3.SelectedIndex == 2)
            {
                data[0] = new double[] { 1, 0, 0 };
                data[1] = new double[] { 1, 0, 1 };
                data[2] = new double[] { 1, 0, 1 };
                data[3] = new double[] { 1, 1, 1 };
                expected_values = new double[4] { 1, 0, 0, 1 };
            }
            else if (comboBox3.SelectedIndex == 1)
            {
                data[0] = new double[] { 1, 0, 0 };
                data[1] = new double[] { 1, 0, 1 };
                data[2] = new double[] { 1, 0, 1 };
                data[3] = new double[] { 1, 1, 1 };
                expected_values = new double[4] { 0, 1, 1, 1 };
            }
            else //AND 
            {
                data[0] = new double[] { 1, 0, 0 };
                data[1] = new double[] { 1, 0, 1 };
                data[2] = new double[] { 1, 0, 1 };
                data[3] = new double[] { 1, 1, 1 };
                expected_values = new double[4] { 0, 0, 0, 1 };
            }
            int attempts = comboBox1.SelectedIndex+1;

            Net myNet = new Net(schematic, 0.005);

            myNet.learn(data, expected_values, attempts*5);

            myNet._propagate_the_signal(data[0]);
            List<List<Neuron>> _neuron_net = myNet._neuron_net;
            textBox1.Text = "output: " + Math.Round(_neuron_net[_neuron_net.Count - 1][0]._last_output, 3) + " expected: " + expected_values[0]+ "\r\n";

            myNet._propagate_the_signal(data[1]);
            _neuron_net = myNet._neuron_net;
            textBox1.Text += "output: " + Math.Round(_neuron_net[_neuron_net.Count - 1][0]._last_output,3) + " expected: " + expected_values[1] + "\r\n";

            myNet._propagate_the_signal(data[2]);
            _neuron_net = myNet._neuron_net;
            textBox1.Text += "output: " + Math.Round(_neuron_net[_neuron_net.Count - 1][0]._last_output, 3) + " expected: " + expected_values[2] + "\r\n";

            myNet._propagate_the_signal(data[3]);
            _neuron_net = myNet._neuron_net;
            textBox1.Text += "output: " + Math.Round(_neuron_net[_neuron_net.Count - 1][0]._last_output, 3) + " expected: " + expected_values[3] + "\r\n";



        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.ResetText();
            comboBox3.ResetText();
            comboBox3.Text = "gate type";
            comboBox1.Text = "repeat";
            textBox1.Clear();
        }
    }
}
