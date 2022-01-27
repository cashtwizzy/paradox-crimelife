<template>
  <div class="hud" v-if="visible">
    <div class="topright">
      <img src="../assets/img/Hud/logo-big.png" class="tr-logo" />
      <h1 class="tr-players">
        <span style="color: #078dbe; font-weight: 500; font-size: 12px"
          >ONLINE</span
        >
        {{ players }}
        <span style="color: #078dbe; font-weight: 600; font-size: 12px">•</span>
        400
      </h1>
    </div>

    <div class="bottomleft">
      <div class="bl-item">
        <img src="../assets/img/Hud/location.svg" class="bl-item-img" />
        <h1>{{ street }}</h1>
        <h2>{{ zone }}</h2>
      </div>
      <br />
      <div class="bl-item" id="bl-i-2">
        <img src="../assets/img/Hud/purse.svg" class="bl-item-img" />
        <h3>
          {{ ConvertMoneyString }}
          <span style="color: #0786b4; font-weight: 600">€</span>
        </h3>
      </div>
      <br />
      <div class="bl-item">
        <img src="../assets/img/Hud/user.svg" class="bl-item-img" />
        <h1>
          <span style="color: white; font-weight: 500">{{ data.name }}</span>
        </h1>
        <h2 style="font-size: 12px">
          ID:
          <span style="color: #0786b4; font-weight: 500; font-size: 12px">{{
            data.id
          }}</span>
        </h2>
      </div>
      <br />
      <div class="bl-item">
        <img src="../assets/img/Hud/clock.svg" class="bl-item-img" />
        <h1>
          <span style="color: white; font-weight: 500">{{ time }}</span>
        </h1>
        <h2 style="font-size: 12px">{{ date }}</h2>
        <div class="vline" v-if="aduty"></div>
        <img
          style="margin-top: -6%; margin-left: 4%; margin-right: 4%"
          src="../assets/img/Hud/admin.svg"
          class="bl-item-img"
          v-if="aduty"
        />
        <h1 style="color: white; margin-top: -5.7%" v-if="aduty">Admin Mode</h1>
        <h2
          style="color: #0786b4; margin-top: 0%; font-size: 13px"
          v-if="aduty"
        >
          ACTIVE
        </h2>
      </div>
    </div>

    <div class="bottomcenter" id="interaction">
      <img src="../assets/img/Hud/key_e.svg" class="bc-img-e" />
      <h4 id="interactiontext">
        Drücke <span style="color: #078dbe">E</span> um mit
        <span style="color: #078dbe">Workstation</span> zu Interargieren.
      </h4>
    </div>

    <div class="bottomright">
      <div class="speedo-wrapper" v-if="speedoData.Visible">
        <div class="speedo-upper">
          <img src="../assets/img/Hud/speedo.png" class="speedo-img" />
          <h1 class="speed">
            {{ speedoData.Speed }}<span style="color: #03b4f5">KM/H</span>
          </h1>
        </div>
        <div class="speedo-lower">
          <div class="svline vline1"></div>
          <div class="svline vline2"></div>
          <div class="svline vline3"></div>
          <img src="../assets/img/Hud/gas_station.svg" class="speedo-l-img" />
          <img
            :src="
              speedoData.Engine
                ? require('../assets/img/Hud/engine-active.png')
                : require('../assets/img/Hud/engine.png')
            "
            class="speedo-l-img"
          />
          <img
            :src="
              speedoData.Locked
                ? require('../assets/img/Hud/lock-active.png')
                : require('../assets/img/Hud/lock.png')
            "
            class="speedo-l-img s-key"
          />
          <img
            src="../assets/img/Hud/lights.png"
            class="speedo-l-img s-lights"
          />
        </div>
      </div>
      <div class="voice-wrapper">
        <div class="voice-item" id="voice-c">
          <img src="../assets/img/Hud/voice.svg" class="voice-item-img" />
          <div class="dots">
            <div :class="voiceData.Voice > 0 ? 'dot dot-active' : 'dot'"></div>
            <div :class="voiceData.Voice > 1 ? 'dot dot-active' : 'dot'"></div>
            <div :class="voiceData.Voice > 2 ? 'dot dot-active' : 'dot'"></div>
          </div>
        </div>
        <div class="voice-item" id="phone-c" v-if="voiceData.Phone > 0">
          <img src="../assets/img/Hud/phone.svg" class="voice-item-img" />
          <div class="dots">
            <div :class="voiceData.Phone > 0 ? 'dot dot-active' : 'dot'"></div>
            <div :class="voiceData.Phone > 1 ? 'dot dot-active' : 'dot'"></div>
            <div :class="voiceData.Phone > 2 ? 'dot dot-active' : 'dot'"></div>
          </div>
        </div>
        <div class="voice-item" id="radio-c" v-if="voiceData.Radio > 0">
          <img src="../assets/img/Hud/radio.svg" class="voice-item-img" />
          <div class="dots">
            <div :class="voiceData.Radio > 0 ? 'dot dot-active' : 'dot'"></div>
            <div :class="voiceData.Radio > 1 ? 'dot dot-active' : 'dot'"></div>
            <div :class="voiceData.Radio > 2 ? 'dot dot-active' : 'dot'"></div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Hud",
  data() {
    return {
      visible: false,
      data: {
        money: "-1",
        name: "",
        id: -1,
      },
      aduty: false,
      players: -1,
      zone: "undefined",
      street: "undefined",
      time: "00:00",
      date: "01.01.0001",
      voiceData: {
        Voice: 2,
        Phone: 1,
        Radio: 3,
      },
      speedoData: {
        Visible: false,
        Speed: -1,
        Engine: false,
        Locked: true,
      },
    };
  },
  methods: {
    Show(show, data) {
      mp.trigger("Log", "Show:" + data);
      this.visible = show;
      if (data != "") this.data = JSON.parse(data);
    },
    SetRatio(ratio) {
      switch (ratio) {
        case 1.78: // 16:9
          document.getElementsByClassName("bottomleft")[0].style.left =
            "16.25%";
          document.getElementsByClassName("bottomleft")[0].style.bottom = "1%";
          break;
        case 1.6: // 16:10
          document.getElementsByClassName("bottomleft")[0].style.left = "17.5%";
          document.getElementsByClassName("bottomleft")[0].style.bottom =
            "1.5%";
          break;
        case 1.25: // 5:4
          document.getElementsByClassName("bottomleft")[0].style.left = "22%";
          document.getElementsByClassName("bottomleft")[0].style.bottom =
            "0.8%";
          break;
        case 1.33: // 4:3
          document.getElementsByClassName("bottomleft")[0].style.left = "21%";
          document.getElementsByClassName("bottomleft")[0].style.bottom =
            "0.8%";
          break;
      }
    },
    SetData(data) {
      this.data = JSON.parse(data);
    },
    SetLocation(zone, street) {
      this.zone = zone;
      this.street = street;
    },
    SetMoney(money) {
      this.data.money = money;
    },
    SetPlayers(players) {
      this.players = players;
    },
    SetAduty(state) {
      this.aduty = state;
    },
    ShowInteraction(name) {
      document.getElementById("interactiontext").innerHTML = name;
      let inter = document.getElementById("interaction");
      inter.style.bottom = "-5%";

      let progress = -5;
      let i = setInterval(() => {
        inter.style.bottom = progress + "%";
        progress += 0.7;
        if (progress > 10) {
          clearInterval(i);
        }
      }, 10);
    },
    RemoveInteraction() {
      let inter = document.getElementById("interaction");
      let progress = 10;
      let i = setInterval(() => {
        inter.style.bottom = progress + "%";
        progress -= 0.7;
        if (progress < -5) {
          clearInterval(i);
          document.getElementById("interactiontext").innerText = "";
        }
      }, 10);
    },
    UpdateTime() {
      let date = new Date();
      let d = date.getDate();

      d = d < 10 ? "0" + d : d;
      let mo = date.getMonth() + 1;

      mo = mo < 10 ? "0" + mo : mo;

      let y = date.getFullYear();

      this.date = `${d}.${mo}.${y}`;

      let h = date.getHours();
      h = h < 10 ? "0" + h : h;

      let m = date.getMinutes();
      m = m < 10 ? "0" + m : m;

      this.time = `${h}:${m}`;
    },
    ShowSpeedo(state) {
      this.speedoData.Visible = state;
    },
    SetVehicleEngineState(state) {
      this.speedoData.Engine = state;
    },
    SetVehicleLockState(state) {
      this.speedoData.Locked = state;
    },
    SetVehicleSpeed(speed) {
      this.speedoData.Speed = speed;
    },
  },
  mounted() {
    window.vm.Hud = this;
    this.UpdateTime();
    setInterval(() => {
      this.UpdateTime();
    }, 20000);
  },
  computed: {
    ConvertMoneyString() {
      return this.data.money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    },
  },
};
</script>

<style scoped>
.bottomright {
  position: absolute;
  bottom: 3%;
  right: 3%;
  width: 20%;
  height: 7.5%;
}
.voice-wrapper {
  width: 45%;
  height: 100%;
  float: right;
  margin-right: 3%;
}
.speedo-wrapper {
  width: 50%;
  height: 100%;
  float: right;
}
.speedo-upper {
  width: 100%;
  height: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
}
.speedo-img {
  height: 70%;
  width: auto;
  margin-right: 5%;
}
.speed {
  width: 60%;
  font-size: 24px;
  font-weight: 500;
  color: white;
  margin: 0;
}
.speed > span {
  margin-left: 5%;
}
.speedo-lower {
  width: 100%;
  height: 50%;
  display: flex;
  justify-content: center;
}
.svline {
  width: 2px;
  height: 27%;
  background: white;
  margin-top: 1%;
  margin-right: 2.1%;
  border-radius: 50px;
}
.speedo-l-img {
  height: 60%;
  width: auto;
  margin-top: -2%;
  margin-right: 3%;
}
.s-key {
  height: 40%;
  margin-top: 0;
}
.s-lights {
  height: 55%;
  margin-right: 10%;
}
.voice-item {
  position: relative;
  width: 51.8334px;
  height: 81px;
  background: #0b0b0b;
  float: right;
  margin-left: 2.45%;
  border-radius: 7px;
  -webkit-box-shadow: inset 0px 0px 20px -3px #000000;
  box-shadow: inset 0px 0px 20px -3px #000000;
  display: flex;
  align-items: center;
  justify-content: center;
}
.voice-item-img {
  width: 60%;
  height: auto;
}
.dots {
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 25%;
  display: flex;
  justify-content: center;
  align-items: center;
}
.dot {
  width: 6px;
  height: 6px;
  background: rgba(31, 31, 31, 0.8);
  border-radius: 50%;
  float: left;
  margin: 5% 5%;
}
.dot-active {
  background: rgba(255, 255, 255, 0.6);
}
.topright {
  position: absolute;
  top: 2%;
  right: 1%;
  width: 12%;
  height: 6%;
}
.tr-logo {
  width: auto;
  height: 100%;
  float: right;
}
.tr-players {
  position: absolute;
  bottom: 15%;
  right: 29.7%;
  color: white;
  font-size: 12px;
  font-weight: 500;
}
.bottomcenter {
  position: absolute;
  display: flex;
  bottom: -5%;
  left: 50%;
  transform: translateX(-50%);
  width: 35%;
  height: 3%;
  justify-content: center;
}
.bc-img-e {
  width: auto;
  height: 100%;
  float: left;
}
h4 {
  height: 100%;
  margin-top: 0.8%;
  margin-left: 1%;
  float: left;
}
.bottomleft {
  position: absolute;
  bottom: 1.5%;
  left: 20%;
  width: 15%;
  height: 18%;
}
.bl-item {
  position: relative;
  width: 90%;
  height: 15%;
  align-items: center;
  margin-top: -4%;
  margin-bottom: 0;
  overflow: visible;
}
.bl-item-img {
  position: relative;
  height: 100%;
  width: auto;
  float: left;
  margin-right: 2%;
  margin-top: 1%;
}
h1 {
  position: relative;
  color: #078dbe;
  font-size: 15px;
  font-weight: 500;
  margin-bottom: 0;
}
h2 {
  position: relative;
  color: white;
  font-size: 15px;
  font-weight: 500;
  margin-top: 0;
  float: left;
}
#bl-i-2 {
  display: flex;
  align-items: center;
}
h3 {
  position: relative;
  color: white;
  font-size: 15px;
  font-weight: 500;
}
.vline {
  position: relative;
  width: 0.1%;
  height: 100%;
  background: #078dbe;
  margin-top: -6%;
  margin-left: 10%;
  float: left;
}
</style>
