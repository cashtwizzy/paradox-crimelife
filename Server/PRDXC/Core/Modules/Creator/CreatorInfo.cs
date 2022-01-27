using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Creator
{
    public class CreatorInfo
    {
        public int Gender { get; set; }
        public byte Beard { get; set; }
        public byte BeardColor { get; set; }
        public byte EyeColor { get; set; }
        public byte Eyebrow { get; set; }
        public byte EyebrowColor { get; set; }
        public byte Style { get; set; }
        public byte Color1 { get; set; }
        public byte Color2 { get; set; }
        public byte Mother { get; set; }
        public byte Father { get; set; }
        public float Mix { get; set; }

        public CreatorInfo()
        {
            Gender = 0;
            Beard = 1;
            BeardColor = 1;
            EyeColor = 1;
            Eyebrow = 1;
            EyebrowColor = 1;
            Style = 1;
            Color1 = 1;
            Color2 = 1;
            Mother = 1;
            Father = 1;
            Mix = 0.5f;
        }

        public CreatorInfo(int gender, byte beard, byte beardColor, byte eyeColor, byte eyebrow, byte eyebrowColor, byte Style, byte Color1, byte Color2, byte mother, byte father, float mix)
        {
            Gender = gender;
            Beard = beard;
            BeardColor = beardColor;
            EyeColor = eyeColor;
            Eyebrow = eyebrow;
            EyebrowColor = eyebrowColor;
            this.Style = Style;
            this.Color1 = Color1;
            this.Color2 = Color2;
            Mother = mother;
            Father = father;
            Mix = mix;
        }

        public Customization ToServerModel()
        {
            return new Customization(
                Gender,
                Beard,
                BeardColor,
                EyeColor,
                Eyebrow,
                EyebrowColor,
                Style,
                Color1,
                Color2,
                Mother,
                Father,
                Mix,
                true
                );
        }
    }
}
