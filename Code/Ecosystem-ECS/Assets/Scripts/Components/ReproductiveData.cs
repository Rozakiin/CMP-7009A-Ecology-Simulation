using System;
using Unity.Entities;
using UtilTools;

namespace Components
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct ReproductiveData : IComponentData
    {
        //Mate
        public float MatingDuration;
        public float MateStartTime;
        public float ReproductiveUrge;
        public float DefaultReproductiveIncrease;
        public float ReproductiveUrgeIncrease;
        public float MatingThreshold;

        //Pregnancy
        public float BirthDuration; //How long between babies being born
        public float BirthStartTime;
        public int BabiesBorn; //How many she has given birth to
        public int LitterSizeMin;
        public int LitterSizeMax;

        public int LitterSizeAve;

        //How many the female is carrying right now
        public int CurrentLitterSize;

        public int LitterSize =>
            ComponentTools.GaussianDistribution((LitterSizeMax - LitterSizeMin) / 2, LitterSizeAve, BirthStartTime);


        public float PregnancyStartTime;
        public float PregnancyLengthBase;
        public float PregnancyLengthModifier;

        public float PregnancyLength => PregnancyLengthBase * PregnancyLengthModifier;
    }
}