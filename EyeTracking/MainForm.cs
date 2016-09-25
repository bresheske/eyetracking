using AForge.FrameProcessing.FrameProviders;
using EyeTracking.Core.Filters;
using EyeTracking.Core.FrameProcessors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EyeTracking.PreprocessingBuilder
{
    public partial class MainForm : Form
    {
        FilterFrameProcessor _processor;

        public MainForm()
        {
            InitializeComponent();
            InitFrameProviders();
            InitFilters();
        }

        private void InitFrameProviders()
        {
            comboboxframeprovider.Items.AddRange(FrameProviderFactory.GetAllFrameProviders().ToArray());
            comboboxframeprovider.SelectedIndex = 1;
        }

        private void InitFilters()
        {
            listboxallfilters.Items.AddRange(IFilterFactory.GetAllFilters().ToArray());
        }

        private void btntoggleframeprovider_Click(object sender, EventArgs e)
        {
            if (_processor == null || !_processor.IsRunning)
            {
                _processor.Start();
            }
            else
            {
                _processor.Stop();
            }
        }

        public void processor_OnNextFrame(object sender, FrameEventArgs e)
        {
            var old = imagebox.Image;
            imagebox.Image = e.Frame;
            if (old != null)
                old.Dispose();
        }

        private void listboxallfilters_DoubleClick(object sender, EventArgs e)
        {
            var filter = (IFilter)((IFilter)listboxallfilters.SelectedItem)
                .Clone();

            listboxactivefilters.Items.Add(filter);
            lock(_processor.Filters)
            {
                _processor.Filters.Add(filter);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_processor != null && _processor.IsRunning)
                _processor.Stop();
        }

        private void listboxactivefilters_DoubleClick(object sender, EventArgs e)
        {
            var filter = (IFilter)listboxactivefilters.SelectedItem;
            listboxactivefilters.Items.Remove(filter);
            lock(_processor.Filters)
            {
                _processor.Filters.Remove(filter);
            }
        }

        private void listboxactivefilters_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                var filter = (IFilter)listboxactivefilters.SelectedItem;
                new FilterSettings(filter).Show();
            }
        }

        private void comboboxframeprovider_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_processor == null || !_processor.IsRunning)
            {
                var provider = (IFrameProvider)comboboxframeprovider.SelectedItem;
                _processor = new FilterFrameProcessor(provider);
                _processor.OnNextFrame += processor_OnNextFrame;
            }
            else
            {
                _processor.OnNextFrame -= processor_OnNextFrame;
            }
        }
    }
}