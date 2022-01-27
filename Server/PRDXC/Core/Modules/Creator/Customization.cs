using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRDXC.Core.Modules.Creator
{
    public class Customization
    {
        public int Gender { get; set; }
        public (byte Index, byte Color) Beard { get; set; }
        public byte EyeColor { get; set; }
        public (byte Index, byte Color) EyeBrows { get; set; }
        public (int Style, byte Color, byte Color2) Hair { get; set; }
        public (byte Mother, byte Father, float Mix) Parents { get; set; }
        public bool FinishedCreation { get; set; }

        public Customization()
        {
            this.Gender = 1;
            this.Beard = (1, 1);
            this.EyeColor = 1;
            this.EyeBrows = (1, 1);
            this.Hair = (1, 1, 1);
            this.Parents = (1, 1, 0.5f);
            this.FinishedCreation = false;
        }
        public Customization(int gender, byte beard, byte beardColor, byte eyeColor, byte eyeBrows, byte eyeBrowsColor, int hairStyle, byte hairColor, byte hairColor2, byte mother, byte father, float mix, bool finished)
        {
            this.Gender = gender;
            this.Beard = (beard, beardColor);
            this.EyeColor = eyeColor;
            this.EyeBrows = (eyeBrows, eyeBrowsColor);
            this.Hair = (hairStyle, hairColor, hairColor2);
            this.Parents = (mother, father, mix);
            this.FinishedCreation = finished;
        }

        public void ApplyCustomization(Player player)
        {
            // GENDER
            player.SetSkin((this.Gender == 1) ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01);
            // PARENTS
            player.HeadBlend = new HeadBlend
            {
                ShapeFirst = this.Parents.Mother,
                ShapeSecond = this.Parents.Father,
                ShapeThird = 0,
                SkinFirst = this.Parents.Mother,
                SkinSecond = this.Parents.Father,
                SkinThird = 0,
                SkinMix = this.Parents.Mix
            };
            // HAIR
            player.SetClothes(2, this.Hair.Style, 0);
            NAPI.Player.SetPlayerHairColor(player, this.Hair.Color, this.Hair.Color2);
            // BEARD
            player.SetHeadOverlay(1, new HeadOverlay { Index = this.Beard.Index, Color = this.Beard.Color, SecondaryColor = this.Beard.Color, Opacity = 1 });
            // EYEBROWS
            player.SetHeadOverlay(2, new HeadOverlay { Index = this.EyeBrows.Index, Color = this.EyeBrows.Color, SecondaryColor = this.EyeBrows.Color, Opacity = 1 });
            // EYES
            NAPI.Player.SetPlayerEyeColor(player, this.EyeColor);
        }
    }
}
