using System;
using DevExpress.XtraPivotGrid;

namespace EIDSS.RAM_DB.Common.EventHandlers
{
    public class PivotDataEventArgs : EventArgs
    {
        private readonly PivotGridControl m_PivotGrid;

        public PivotDataEventArgs(PivotGridControl pivotGrid)
        {
            m_PivotGrid = pivotGrid;
        }

        public PivotGridControl PivotGrid
        {
            get { return m_PivotGrid; }
        }
    }
}