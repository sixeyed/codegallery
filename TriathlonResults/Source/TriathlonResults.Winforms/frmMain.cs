using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TriathlonResults.Winforms.TriathlonResults.Central.Services;
using TriathlonResults.Winforms.Properties;
using TriathlonResults.Central.ServiceRequests;
using entities = TriathlonResults.Central.ServiceRequests.Entities;
using ESBSimpleSamples.ServiceClient.Helpers;

namespace TriathlonResults.Winforms
{
    /// <summary>
    /// Form collecting sector time & submitting to WS or ESB
    /// </summary>
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            //default to today's date:
            this.dtpRaceDate.Value = DateTime.Today;
            this.dtpStartTime.Format = dtpEndTime.Format = DateTimePickerFormat.Time;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (this.IsValid())
                {
                    SectorTime time = new SectorTime();
                    time.RaceId = int.Parse(this.txtRaceId.Text);
                    time.SectorId = 2;
                    time.AthleteId = int.Parse(this.txtAthleteId.Text);
                    time.StartTime = this.CombineDateAndTime(dtpRaceDate.Value, dtpStartTime.Value);
                    time.EndTime = this.CombineDateAndTime(dtpRaceDate.Value, dtpEndTime.Value);
                    if (this.SubmitRequest(time))
                    {
                        MessageBox.Show("Athlete results submitted.");
                    }
                    else
                    {
                        MessageBox.Show("Result submission failed.");
                    }
                    //clear the athlete settings:
                    this.txtAthleteId.Text = string.Empty;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private bool SubmitRequest(SectorTime time)
        {
            bool submitted = false;
            //use ESB or WS as configured:
            if (Settings.Default.UseESB)
            {
                entities.SectorTime result = new entities.SectorTime();
                result.AthleteId = time.AthleteId;
                result.RaceId = time.RaceId;
                result.SectorId = time.SectorId;
                result.StartTime = time.StartTime;
                result.EndTime = time.EndTime;

                RecordResult serviceRequest = new RecordResult();
                serviceRequest.Request.result = result;
                submitted = serviceRequest.Response.RecordResultResult;
            }
            else
            {
                ResultService service = new ResultService();
                submitted = service.RecordResult(time);
            }
            return submitted;
        }

        private DateTime CombineDateAndTime(DateTime date, DateTime time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        }

        private bool IsValid()
        {
            bool valid = false;
            int temp;
            valid = int.TryParse(this.txtRaceId.Text, out temp);
            valid = valid && (int.TryParse(this.txtAthleteId.Text, out temp));
            valid = valid && (dtpRaceDate.Value.Date <= DateTime.Today.Date);
            valid = valid && (dtpStartTime.Value.TimeOfDay.Ticks < dtpEndTime.Value.TimeOfDay.Ticks);
            return valid;
        }
    }
}