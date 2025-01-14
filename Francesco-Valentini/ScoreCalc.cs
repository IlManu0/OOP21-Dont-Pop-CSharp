namespace Francesco_Valentini
{
    /// Manages final score, that will be displayed both during gameplay and after game over.
    /// Differs from ScoreManager, which manages GUI-related aspects of score displaying.
    public class ScoreCalc
    {
        /// Current player score.
        public int Score { get; private set; }

        /// Calculation status: true if score can be calculated, false otherwise.
        public bool CalcStatus { get; set; }

        /// Current multiplier value.
        /// Score increment will be multiplied by its value.
        public int Multiplier { get; private set; }

        private bool _hasMultiplier;
        private double _multiplierTime;
        private double _frameCounter;
        private const double MultiplierTime = 5;   //five seconds of multiplier
        private const int PointsPerSecond = 15;
        private const double SecondsPerPoint = 1 / (double) PointsPerSecond;
        private const int Multiplier2X = 2;

        /// Creates class and sets multiplier to 1 by default.
        public ScoreCalc()
        {
            this.Multiplier = 1;
            this.CalcStatus = false;
        }

        /// Increments score by an arbitrary amount, while still abiding by multiplier rules.
        /// <param name="deltaScore">amount by which current score will be increased</param>
        public void IncScore(int deltaScore) => this.Score += deltaScore * this.Multiplier;

        /// Manages multiplier time, decreasing it until it reaches 0.
        /// <param name="deltaTime">amount by which current score will be increased</param>
        private void ManageMultiplierTime(double deltaTime)
        {
            if (!this._hasMultiplier)
            {
                return;
            }
            if (this._multiplierTime > 0)
            {
                this.DecMultiplierTime(deltaTime);
            }
            else
            {
                this.ResetMultiplier();
            }
        }

        /// Increments score by 1 every 4 frames, giving >= 15 points per second.
        /// <param name="deltaTime">amount by which current score will be increased</param>
        public void CalculateScore(double deltaTime)
        {
            if (this.CalcStatus)
            {
                this._frameCounter += deltaTime;
                if (this._frameCounter >= SecondsPerPoint)
                {
                    this.IncScore(1);
                    this.ResetFrameCounter();
                }
                this.ManageMultiplierTime(deltaTime);
            }
        }

        /// Sets multiplier to any value.
        /// <param name="multiplier">amount by which current score increment should be multiplied</param>
        public void SetMultiplier(int multiplier)
        {
            this.Multiplier = multiplier;
            this._multiplierTime = MultiplierTime;
            this._hasMultiplier = true;
        }

        /// Sets multiplier to default value (2x).
        /// Useful when you don't want to specify the value of the multiplier.
        public void SetMultiplier() => this.SetMultiplier(Multiplier2X);

        /// Resets multiplier back to 1.
        public void ResetMultiplier()
        {
            this.Multiplier = 1;
            this._multiplierTime = 0;
            this._hasMultiplier = false;
        }

        /// Decreases multiplier time by an arbitrary amount.
        /// <param name="decrement">amount by which current multiplier remaining time should be decreased</param>
        private void DecMultiplierTime(double decrement) => this._multiplierTime -= decrement;

        /// Resets frame counter.
        private void ResetFrameCounter() => this._frameCounter -= SecondsPerPoint;
    }
}
