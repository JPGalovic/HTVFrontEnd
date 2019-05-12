﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTVFrontEnd
{
    public partial class FormVehicles : Form
    {
        private IODatabase _database;
        private int _row;
        private bool _updateFlag;
        private string _primaryKey;

        public FormVehicles()
        {
            InitializeComponent();

            _database = new IODatabase(".\\SQLEXPRESS", "HTVDatabase");
            _row = 0;

            // Load first row
            List<List<string>> formData = _database.FetchData("vehicles");
            if(formData.Count >= _row)
            {
                text_serialNumber.Text = formData[_row][0];
                text_name.Text = formData[_row][1];
                text_model.Text = formData[_row][2];
                text_year.Text = formData[_row][3];
                text_make.Text = formData[_row][4];
                text_baseCost.Text = formData[_row][5];
                _primaryKey = formData[_row][0];
            }

            // Disable Previous & Next if only one row
            btn_prev.Enabled = false;
            if (formData.Count == _row + 1)
                btn_next.Enabled = false;

            _updateFlag = true;
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            // Clear text fields
            text_serialNumber.Text = "";
            text_name.Text = "";
            text_model.Text = "";
            text_year.Text = "";
            text_make.Text = "";
            text_baseCost.Text = "";

            // Change update button to add button.
            btn_updateInsert.Text = "Add";
            _updateFlag = false;

            // Disable new button
            btn_new.Enabled = false;

            // Make row state invalid, disables previous, enables next.
            _row = -1;
            btn_prev.Enabled = false;
            btn_next.Enabled = true;
        }

        private void btn_updateInsert_Click(object sender, EventArgs e)
        {
            // Collate data
            List<string> row = new List<string>();

            row.Add(text_serialNumber.Text);
            row.Add(text_name.Text);
            row.Add(text_model.Text);
            row.Add(text_year.Text);
            row.Add(text_make.Text);
            row.Add(text_baseCost.Text);

            List<string> primaryKey = new List<string>();
            primaryKey.Add(_primaryKey);

            if(_updateFlag) // If update mode
            {
                // TODO UPDATE
            }
            else // Add mode
            {
                // Add data to database
                if(_database.WriteData("vehicles", row))
                {
                    List<List<string>> formData = _database.FetchData("vehicles");
                    _row = formData.Count - 1;

                    text_serialNumber.Text = formData[_row][0];
                    text_name.Text = formData[_row][1];
                    text_model.Text = formData[_row][2];
                    text_year.Text = formData[_row][3];
                    text_make.Text = formData[_row][4];
                    text_baseCost.Text = formData[_row][5];
                    _primaryKey = formData[_row][0];

                    if (_row > 0) // check if there is previous
                        btn_prev.Enabled = true;
                    btn_next.Enabled = false;

                    // Re-enable new button & change to update button.
                    btn_new.Enabled = true;
                    btn_updateInsert.Text = "Update";
                    _updateFlag = true;

                    // Show success
                    MessageBox.Show("New Vehicle sucessfully added.");
                }
                else
                {
                    // TODO: On write error
                }
            }
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            // Decrement row and fetch previous
            _row--;

            List<List<string>> formData = _database.FetchData("vehicles");
            text_serialNumber.Text = formData[_row][0];
            text_name.Text = formData[_row][1];
            text_model.Text = formData[_row][2];
            text_year.Text = formData[_row][3];
            text_make.Text = formData[_row][4];
            text_baseCost.Text = formData[_row][5];
            _primaryKey = formData[_row][0];

            if (_row == 0) // check if there is previous
                btn_prev.Enabled = false;

            if (formData.Count > _row + 1) //check if remaining next's
                btn_next.Enabled = true;
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            // Decrement row and fetch previous
            _row++;

            List<List<string>> formData = _database.FetchData("vehicles");
            text_serialNumber.Text = formData[_row][0];
            text_name.Text = formData[_row][1];
            text_model.Text = formData[_row][2];
            text_year.Text = formData[_row][3];
            text_make.Text = formData[_row][4];
            text_baseCost.Text = formData[_row][5];
            _primaryKey = formData[_row][0];

            if (_row > 0) // check if there is previous
                btn_prev.Enabled = true;

            if (formData.Count == _row + 1) //check if remaining next's
                btn_next.Enabled = false;

            if (_row == 0) // on re-entering data set reset update button.
            {
                btn_updateInsert.Text = "Update";
                btn_new.Enabled = true;
                _updateFlag = true;
            }
        }
    }
}
