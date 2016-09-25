using EyeTracking.Core.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EyeTracking.PreprocessingBuilder
{
    public partial class FilterSettings : Form
    {
        public IFilter Filter { get; private set; }

        public FilterSettings(IFilter filter)
        {
            InitializeComponent();
            Filter = filter;
            InitGridView();
        }

        private void InitGridView()
        {
            gridview.ColumnCount = 2;
            gridview.ColumnHeadersVisible = true;

            gridview.Columns[0].Name = "Name";
            gridview.Columns[1].Name = "Value";
            
            var rows = Filter.GetType()
                .GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(FilterConfigSettingAttribute), false).Any())
                .Select(x => 
                    {
                        var row = new DataGridViewRow();
                        row.Cells.AddRange(new DataGridViewTextBoxCell() { Value = x.Name });
                        row.Cells.AddRange(new DataGridViewTextBoxCell() { Value = Filter.GetType().GetProperty(x.Name).GetValue(Filter) });
                        return row;
                    });

            gridview.Rows.AddRange(rows.ToArray());
        }

        private void ApplySettings()
        {
            foreach (DataGridViewRow r in gridview.Rows)
            {
                var name = r.Cells[0].Value;
                var value = r.Cells[1].Value;
                if (value == null)
                    continue;
                var convertedvalue = Convert.ChangeType(value, Filter.GetType().GetProperty(name.ToString()).PropertyType);
                Filter.GetType().GetProperty(name.ToString()).SetValue(Filter, convertedvalue);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ApplySettings();
        }
    }
}
