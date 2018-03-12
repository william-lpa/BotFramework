using System;
using System.Collections.Generic;


namespace Maratona_Bot.Model
{
    [Serializable]
    public class CustomVisionResult
    {
        public List<Prediction> Predictions { get; set; }
    }
}