namespace SkinAssistance.Core.Refrecter
{
    public class AverageCounter
    {
        private int _circIndex = -1;
        private double _current = double.NaN;
        private readonly int _length;
        private bool _filled;
        private readonly double _oneOverLength;
        private readonly double[] _circularBuffer;
        private double _total;

        public int Length
        {
            get
            {
                return this._length;
            }
        }

        public double Current
        {
            get
            {
                return this._current;
            }
        }

        public AverageCounter(int length)
        {
            this._length = length;
            this._oneOverLength = 1.0 / (double)length;
            this._circularBuffer = new double[length];
        }

       

        public void Push(double value)
        {
            if (++this._circIndex == this._length)
                this._circIndex = 0;
            double num = this._circularBuffer[this._circIndex];
            this._circularBuffer[this._circIndex] = value;
            this._total += double.IsNaN(value) ? 0.0 : value;
            this._total -= num;
            if (!this._filled && this._circIndex != this._length - 1)
            {
                this._current = double.NaN;
                return;
            }
            this._filled = true;
            this._current = this._total * this._oneOverLength;
        }
    }
}