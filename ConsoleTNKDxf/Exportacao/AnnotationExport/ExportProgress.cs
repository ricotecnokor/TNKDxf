using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTNKDxf.Exportacao.AnnotationExport
{
    public class ExportProgress : IProgressWrapper
    {
        public Func<bool> IsCancelled { get; set; }

        public event Action<double, bool> ReportEvent;

        public bool IsCancellationRequested()
        {
            return IsCancelled?.Invoke() ?? false;
        }

        public void Report(double value, bool cancellable)
        {
            this.ReportEvent?.Invoke(value, cancellable);
        }
    }

    public interface IProgressWrapper
    {
        Func<bool> IsCancelled { get; set; }
        void Report(double value, bool cancellable);
    }
}
