using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MiniLogger;

namespace LoggerTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //This line does the logger initialisation as well as UI Binding !!
            logUCtrl1.Bind(Logger.GetInstance());
        }

        private void LogBT_Click(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            if (index != -1)
            {
                Logger.Log(textBox1.Text, Logger.Severities[index]);
            }
        }
    }
}
