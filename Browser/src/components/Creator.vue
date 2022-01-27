<template>
  <div id="creator" class="window" v-if="visible">
    <div class="wrapper">
      <div class="head">
        <img src="../assets/img/Creator/logo.png" class="logo" />
        <h1>CHARACTER LOOK</h1>
        <h2>
          Erstelle deinen Charakter nach deinen<br />Vorstellungen. Du kannst
          dies später noch ändern.
        </h2>
      </div>
      <div class="item-wrapper">
        <div class="item">
          <h3>Geschlecht</h3>
          <div class="face-img-c">
            <img
              class="gender-svg"
              src="../assets/img/Creator/male.svg"
              @click="changeGender(1)"
            />
          </div>
          <div class="face-img-c">
            <img
              class="gender-svg"
              src="../assets/img/Creator/female.svg"
              @click="changeGender(0)"
            />
          </div>
        </div>
        <div class="item" v-if="!faces">
          <h3 style="float: left; margin-right: 3%; color: white">Vater</h3>
          <h3 @click="faces = !faces">Mutter</h3>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male01.webp"
              class="face-img"
              @click="changeParents(1, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male02.webp"
              class="face-img"
              @click="changeParents(2, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male03.webp"
              class="face-img"
              @click="changeParents(3, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male04.webp"
              class="face-img"
              @click="changeParents(4, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male05.webp"
              class="face-img"
              @click="changeParents(5, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male06.webp"
              class="face-img"
              @click="changeParents(6, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male07.webp"
              class="face-img"
              @click="changeParents(7, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male08.webp"
              class="face-img"
              @click="changeParents(8, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male09.webp"
              class="face-img"
              @click="changeParents(9, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male10.webp"
              class="face-img"
              @click="changeParents(10, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male11.webp"
              class="face-img"
              @click="changeParents(11, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male12.webp"
              class="face-img"
              @click="changeParents(12, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male13.webp"
              class="face-img"
              @click="changeParents(13, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male14.webp"
              class="face-img"
              @click="changeParents(14, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male15.webp"
              class="face-img"
              @click="changeParents(15, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male16.webp"
              class="face-img"
              @click="changeParents(16, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male17.webp"
              class="face-img"
              @click="changeParents(17, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male18.webp"
              class="face-img"
              @click="changeParents(18, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male19.webp"
              class="face-img"
              @click="changeParents(19, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male20.webp"
              class="face-img"
              @click="changeParents(20, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male21.webp"
              class="face-img"
              @click="changeParents(21, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male22.webp"
              class="face-img"
              @click="changeParents(22, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male23.webp"
              class="face-img"
              @click="changeParents(23, 'father')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/male24.webp"
              class="face-img"
              @click="changeParents(24, 'father')"
            />
          </div>
        </div>
        <div class="item" v-if="faces">
          <h3 style="float: left; margin-right: 3%" @click="faces = !faces">
            Vater
          </h3>
          <h3 style="color: white">Mutter</h3>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female01.webp"
              class="face-img female-img"
              @click="changeParents(1, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female02.webp"
              class="face-img female-img"
              @click="changeParents(2, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female03.webp"
              class="face-img female-img"
              @click="changeParents(3, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female04.webp"
              class="face-img female-img"
              @click="changeParents(4, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female05.webp"
              class="face-img female-img"
              @click="changeParents(5, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female06.webp"
              class="face-img female-img"
              @click="changeParents(6, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female07.webp"
              class="face-img female-img"
              @click="changeParents(7, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female08.webp"
              class="face-img female-img"
              @click="changeParents(8, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female09.webp"
              class="face-img female-img"
              @click="changeParents(9, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female10.webp"
              class="face-img female-img"
              @click="changeParents(10, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female11.webp"
              class="face-img female-img"
              @click="changeParents(11, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female12.webp"
              class="face-img female-img"
              @click="changeParents(12, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female13.webp"
              class="face-img female-img"
              @click="changeParents(13, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female14.webp"
              class="face-img female-img"
              @click="changeParents(14, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female15.webp"
              class="face-img female-img"
              @click="changeParents(15, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female16.webp"
              class="face-img female-img"
              @click="changeParents(16, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female17.webp"
              class="face-img female-img"
              @click="changeParents(17, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female18.webp"
              class="face-img female-img"
              @click="changeParents(18, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female19.webp"
              class="face-img female-img"
              @click="changeParents(19, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female20.webp"
              class="face-img female-img"
              @click="changeParents(20, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female21.webp"
              class="face-img female-img"
              @click="changeParents(21, 'mother')"
            />
          </div>
          <div class="face-img-c">
            <img
              src="../assets/img/Creator/female22.webp"
              class="face-img female-img"
              @click="changeParents(22, 'mother')"
            />
          </div>
        </div>
        <div class="item">
          <h3>Vererbung</h3>
          <h4>Ähnlichkeit</h4>
          <input
            type="range"
            min="0"
            step="0.1"
            max="1"
            v-model="character.Mix"
            @change="changeParentsMix()"
          />
        </div>
        <div class="item">
          <h3>Augenfarbe</h3>
          <div
            class="eyecolor"
            style="background: white"
            @click="changeEyeColor(0)"
          ></div>
          <div
            class="eyecolor"
            style="background: grey"
            @click="changeEyeColor(1)"
          ></div>
          <div
            class="eyecolor"
            style="background: green"
            @click="changeEyeColor(2)"
          ></div>
          <div
            class="eyecolor"
            style="background: blue"
            @click="changeEyeColor(3)"
          ></div>
          <div
            class="eyecolor"
            style="background: black"
            @click="changeEyeColor(4)"
          ></div>
          <div
            class="eyecolor"
            style="background: red"
            @click="changeEyeColor(5)"
          ></div>
        </div>
        <div class="item">
          <h3>Augenbraun</h3>
          <h4>Style</h4>
          <input
            type="range"
            min="0"
            step="1"
            max="30"
            v-model="character.Eyebrow"
            @change="changeEyebrows()"
          />
          <br /><br />
          <h4>Farbe</h4>
          <input
            type="range"
            min="0"
            step="1"
            max="30"
            v-model="character.EyebrowColor"
            @change="changeEyebrows()"
          />
        </div>
        <div class="item">
          <h3>Haare</h3>
          <h4>Style</h4>
          <input
            type="range"
            min="0"
            step="1"
            max="30"
            v-model="character.Style"
            @change="changeHairs()"
          />
          <br /><br />
          <h4>Farbe</h4>
          <input
            type="range"
            min="0"
            step="1"
            max="30"
            v-model="character.Color1"
            @change="changeHairs()"
          />
          <br /><br />
          <h4>Strähnen</h4>
          <input
            type="range"
            min="0"
            step="1"
            max="30"
            v-model="character.Color2"
            @change="changeHairs()"
          />
        </div>
        <div class="item">
          <h3>Bart</h3>
          <h4>Style</h4>
          <input
            type="range"
            min="0"
            step="1"
            max="30"
            v-model="character.Beard"
            @change="changeBeard()"
          />
          <br /><br />
          <h4>Farbe</h4>
          <input
            type="range"
            min="0"
            step="1"
            max="30"
            v-model="character.BeardColor"
            @change="changeBeard()"
          />
        </div>
      </div>
      <button @click="Submit()">Bestätigen</button>
    </div>
  </div>
</template>

<script>
export default {
  name: "Creator",
  data() {
    return {
      visible: false,
      character: {
        Gender: 1,
        Beard: 0,
        BeardColor: 0,
        EyeColor: 1,
        Eyebrow: 0,
        EyebrowColor: 0,
        Style: 0,
        Color1: 0,
        Color2: 0,
        Mother: 1,
        Father: 1,
        Mix: 0.5,
      },
      faces: false,
    };
  },
  methods: {
    Show(show, data) {
      this.visible = show;
      this.data = JSON.parse(data);
    },
    changeParents(newVal, type) {
      type == "mother"
        ? (this.character.Mother = newVal)
        : (this.character.Father = newVal);

      mp.trigger(
        "CreatorUpdate",
        "Parents",
        `{"mother":${this.character.Mother}, "father":${this.character.Father}, "mix":${this.character.Mix}}`
      );
    },
    changeParentsMix() {
      mp.trigger(
        "CreatorUpdate",
        "Parents",
        `{"mother":${this.character.Mother}, "father":${this.character.Father}, "mix":${this.character.Mix}}`
      );
    },
    changeGender(val) {
      this.character.Gender = val;
      mp.trigger("CreatorChangeGender", val);
    },
    changeEyeColor(newVal) {
      this.character.EyeColor = newVal;
      mp.trigger(
        "CreatorUpdate",
        "Eye",
        `{"eyeColor":${this.character.EyeColor}}`
      );
    },
    changeHairs() {
      mp.trigger(
        "CreatorUpdate",
        "Hair",
        `{"style":${this.character.Style}, "color":${this.character.Color1}, "color2":${this.character.Color2}}`
      );
    },
    changeEyebrows() {
      mp.trigger(
        "CreatorUpdate",
        "Eyebrow",
        `{"eyebrow":${this.character.Eyebrow}, "eyebrowColor":${this.character.EyebrowColor}}`
      );
    },
    changeBeard() {
      mp.trigger(
        "CreatorUpdate",
        "Beard",
        `{"beard":${this.character.Beard}, "beardColor":${this.character.BeardColor}}`
      );
    },
    Submit() {
      mp.trigger("CreatorSaveChanges", JSON.stringify(this.character));
    },
  },
  mounted() {
    window.vm.Creator = this;
  },
};
</script>

<style scoped>
.wrapper {
  position: absolute;
  top: 50%;
  left: 3%;
  transform: translateY(-50%);
  width: 20%;
  height: 80%;
  background: #07181a;
  background-image: url(../assets/img/Creator/roads.png);
  border-radius: 25px;
}
.head {
  position: relative;
  width: 100%;
  height: 22%;
  background-image: url(../assets/img/Creator/background.png);
  background-size: 100%;
  text-align: center;
}
.head::before {
  content: "";
  position: absolute;
  width: 200%;
  height: 200%;
  top: -50%;
  left: -50%;
  z-index: 1;
  background-image: radial-gradient(#3d3d3d 7%, transparent 7%),
    radial-gradient(#3d3d3d 7%, transparent 7%);
  background-position: 0 0, 20px 20px;
  background-size: 40px 40px;
  transform: rotate(45deg);
}
.logo {
  position: relative;
  width: auto;
  height: 45%;
  z-index: 2;
}
h1 {
  position: relative;
  z-index: 2;
  margin-top: -4%;
  font-size: 25px;
  font-weight: 600;
}
h2 {
  position: relative;
  z-index: 2;
  margin-top: -4%;
  font-size: 15px;
  font-weight: 400;
  color: rgb(184, 184, 184);
}
.item-wrapper {
  position: relative;
  width: 85%;
  height: 65%;
  margin-top: 5%;
  margin-left: 6%;
  overflow-y: scroll;
}
.item-wrapper::-webkit-scrollbar {
  display: none;
}
.item {
  position: relative;
  width: 100%;
  min-height: 15%;
  height: fit-content;
}
h3 {
  position: relative;
  font-size: 20px;
  font-weight: 400;
  color: grey;
  margin-left: 2%;
}
.face-img-c {
  position: relative;
  width: 75px;
  height: 75px;
  background: rgba(255, 255, 255, 0.1);
  border-radius: 7px;
  float: left;
  margin-left: 2%;
  margin-bottom: 2%;
  cursor: pointer;
}
.face-img {
  width: 150%;
  height: auto;
  margin-left: -10%;
}
.female-img {
  margin-left: -38%;
}
h4 {
  position: relative;
  font-size: 17px;
  font-weight: 300;
  color: grey;
  margin: 0;
  margin-left: 2%;
  margin-top: 0.3%;
  float: left;
}
.eyecolor {
  width: 25px;
  height: 25px;
  border-radius: 50%;
  margin-left: 3%;
  float: left;
  cursor: pointer;
}
.gender-svg {
  width: 100%;
  height: auto;
}
button {
  width: 60%;
  height: 5.5%;
  margin-top: 5%;
  margin-left: 20%;
  background: linear-gradient(35deg, #5dc5c1 0%, #1589a7 100%);
  border: none;
  border-radius: 7px;
  cursor: pointer;
  transition: 200ms;
}
button:active {
  transform: scale(80%);
}
input[type="range"] {
  width: 65%;
  height: 2%;
  -webkit-appearance: none;
  overflow: visible;
  margin-top: 2.3%;
  margin-left: 5%;
  background: transparent;
  float: right;
}
input[type="range"]:focus {
  outline: none;
}
input[type="range"]::-webkit-slider-runnable-track {
  width: 100%;
  height: 8px;
  cursor: pointer;
  animate: 0.2s;
  box-shadow: 0px 0px 0px #000000;
  background: #ffffff48;
  border-radius: 50px;
  border: 0px solid #000000;
}
input[type="range"]::-webkit-slider-thumb {
  box-shadow: 0px 0px 1px #000000;
  border: 0px solid #000000;
  height: 17px;
  width: 17px;
  border-radius: 43px;
  background: #ffffff;
  cursor: pointer;
  -webkit-appearance: none;
  margin-top: -4.5px;
}
input[type="range"]::-moz-range-track {
  width: 100%;
  height: 8px;
  cursor: pointer;
  animate: 0.2s;
  box-shadow: 0px 0px 0px #000000;
  background: #a9a9a9;
  border-radius: 50px;
  border: 0px solid #000000;
}
input[type="range"]::-moz-range-thumb {
  box-shadow: 0px 0px 1px #000000;
  border: 0px solid #000000;
  height: 17px;
  width: 17px;
  border-radius: 43px;
  background: #ffffff;
  cursor: pointer;
}
input[type="range"]::-ms-track {
  width: 100%;
  height: 8px;
  cursor: pointer;
  animate: 0.2s;
  background: transparent;
  border-color: transparent;
  color: transparent;
}
input[type="range"]::-ms-fill-lower {
  background: #a9a9a9;
  border: 0px solid #000000;
  border-radius: 100px;
  box-shadow: 0px 0px 0px #000000;
}
input[type="range"]::-ms-fill-upper {
  background: #a9a9a9;
  border: 0px solid #000000;
  border-radius: 100px;
  box-shadow: 0px 0px 0px #000000;
}
input[type="range"]::-ms-thumb {
  margin-top: 1px;
  box-shadow: 0px 0px 1px #000000;
  border: 0px solid #000000;
  height: 17px;
  width: 17px;
  border-radius: 43px;
  background: #ffffff;
  cursor: pointer;
}
</style>
