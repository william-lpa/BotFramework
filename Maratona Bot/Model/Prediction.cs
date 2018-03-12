using System;

namespace Maratona_Bot.Model
{
    [Serializable]
    public class Prediction
    {
        public string TagId { get; set; }
        public string Tag { get; set; }
        public double Probability { get; set; }
    }
}