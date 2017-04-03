﻿using System;
using System.Data.SqlClient;
using bv.model.BLToolkit;
using eidss.model.Reports.AberrationAnalisys;
using EIDSS.Reports.AberrationAnalysis;

namespace EIDSS.Reports.Parameterized.AberrationAnalysis.Reports
{
    public partial class HumAberrationReport : AberrationReport
    {
        public HumAberrationReport()
        {
            InitializeComponent();
        }

        public void SetParameters(DbManagerProxy manager, HumAberrationModel model)
        {
            base.SetParameters(manager, model);

            cellTimeInterval.Text = model.GetInterval();
            cellLocation.Text = model.Location;
            cellGender.Text = model.GenderText;
            cellAge.Text = model.AgeText;
            cellDiagnosis.Text = model.MultipleDiagnosis.ToString();
            cellCaseClassification.Text = model.MultipleCaseClassification.ToString();

            Action<SqlConnection> action = (connection =>
            {
                m_aberrationDataTableAdapter1.Connection = connection;
                m_aberrationDataSet1.EnforceConstraints = false;

                m_aberrationDataTableAdapter1.FillHum(m_aberrationDataSet1.AberrationData, model.Language,
                    model.StartDate.ToString("s"), model.EndDate.ToString("s"),
                    model.TimeIntervalId,
                    model.RegionId, model.RayonId, model.SettlementId,
                    model.MultipleDiagnosis.ToXml(),
                    model.Gender, model.StartAge, model.EndAge,
                    model.MultipleCaseClassification.ToXml(),
                    model.DateFilter[0], model.DateFilter[1], model.DateFilter[2], model.DateFilter[3], model.DateFilter[4]);
            });

            FillDataTableWithArchive(action,
                (SqlConnection) manager.Connection,
                m_aberrationDataSet1.AberrationData,
                model.Mode,
                new[] {"date"});

            SetLabel();

            AberrationAlgorithm.Calculate(m_aberrationDataSet1.AberrationData,
                model.Baseline, model.Lag, model.Threshold);
        }
    }
}