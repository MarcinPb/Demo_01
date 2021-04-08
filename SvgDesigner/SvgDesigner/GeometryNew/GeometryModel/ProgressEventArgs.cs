using System;

namespace GeometryModel
{
    public class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs(double progressRatio, string message)
        {
            this.ProgressRatio = progressRatio;
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets a message.
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Gets or sets the progress ratio.
        /// </summary>
        /// <remarks>
        /// The values should fall within 0 to 1, where 0 = 0% and 1 = 100%.
        /// </remarks>
        public double ProgressRatio { get; set; }
    }
}
