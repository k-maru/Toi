using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Parser
{
    class LookAheadReader: IDisposable
    {

        private int lookAheadCount = 0;
        private TextReader reader = null;
        private int[] values = null;
        private bool isInitialized = false;

        public LookAheadReader(TextReader reader, int lookAheadCount = 1)
        {
            this.lookAheadCount = lookAheadCount;
            this.reader = reader;
            this.values = new int[lookAheadCount];
        }

        public int Read()
        {
            if (!isInitialized)
            {
                Init();
                isInitialized = true;
            }
            var result = values[0];
            for (var i = 1; i < lookAheadCount; i++)
            {
                values[i - 1] = values[i];
            }
            values[lookAheadCount - 1] = reader.Read();

            return result;
        }

        private void Init()
        {
            for (var i = 0; i < lookAheadCount; i++)
            {
                values[i] = reader.Read();
            }
            isInitialized = true;
        }

        public int Peek(int pos = 0)
        {
            if (pos < 0 || pos >= lookAheadCount)
            {
                throw new ArgumentOutOfRangeException("pos");
            }
            if (!isInitialized)
            {
                Init();
            }
            return values[pos];
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && reader != null)
                {
                    reader.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
