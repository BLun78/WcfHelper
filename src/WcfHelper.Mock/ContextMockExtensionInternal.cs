namespace WcfHelper.Mock
{
    using System;
    using System.Fakes;
    using System.Runtime.InteropServices;

    using JetBrains.Annotations;

    using Microsoft.QualityTools.Testing.Fakes;

    public abstract class ContextMockExtensionInternal : IDisposable
    {
        [NotNull]
        private volatile object syncLock = new object();

        [CanBeNull]
        private volatile IDisposable shimsContext;

        protected static void ShimDateTimeNow([Optional, CanBeNull] DateTime now)
        {
            // Make DateTime.Now always return midnight Jan 1, 2015
            var dateTime = new DateTime(2015, 1, 1);

            if (now != default(DateTime))
            {
                dateTime = now;
            }
            ShimDateTime.NowGet = () => dateTime;
        }

        protected abstract void InitializeMock();

        public void StartShim()
        {
            lock (this.syncLock)
            {
                this.shimsContext = ShimsContext.Create();
                this.InitializeMock();
            }
        }

        public void StopShim()
        {
            this.Dispose();
        }

        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ContextMockExtensionInternal()
        {
            this.Dispose(false);
        }

        protected virtual void Dispose([NotNull]bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                this.DoDispose();
            }

        }

        protected void DoDispose()
        {
            lock (this.syncLock)
            {
                this.shimsContext?.Dispose();
            }
        }

        #endregion

    }
}
