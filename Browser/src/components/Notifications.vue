<template>
  <div class="window" id="notification">
    <div class="announce" id="annc">
      <div class="announce2">
        <div class="an-img-c">
          <img class="an-svg" src="../assets/img/Hud/megaphone.svg" />
        </div>
        <h4>{{ announce.title }}</h4>
        <h5>{{ announce.message }}</h5>
      </div>
    </div>
    <div id="notify">
      <div class="item" v-for="item in notifications" :key="item.Id">
        <div class="item-img-c">
          <img src="../assets/img/Hud/bell.svg" class="item-img" />
        </div>
        <h1>{{ item.Title }}</h1>
        <h2>{{ item.Message }}</h2>
      </div>
      <div
        class="progress"
        v-for="item in progressbars"
        :key="item.Id"
        :id="'ProgId-' + item.Id"
      >
        <div class="prog-img-c">
          <img
            :src="
              item.Image == ''
                ? require(`../assets/img/Items/${item.Title.toLowerCase()}.png`)
                : require('../assets/img/' + item.Image)
            "
            class="prog-img"
          />
        </div>
        <h1 class="h12">{{ item.Title }}</h1>
        <h2 class="h22">{{ item.Desc }}</h2>
        <h3>{{ item.Progress }}<span style="color: #00c3ff">%</span></h3>
        <div class="prog">
          <div :style="`width:${item.Progress}%;`" class="prog2"></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Notifications",
  data() {
    return {
      progressbars: [],
      notifications: [],
      notificationCount: 0,
      announce: {
        title: "FIB Nachricht",
        message:
          "Die Staatsbank(+200m) ist hiermit eine Sperrzone. Unbefugtes betreten wird mit einer Festnahme geahndet.",
      },
    };
  },
  methods: {
    Show(show, _data) {
      let data = JSON.pare(_data);
      console.log(data);
    },
    ShowAnnounce(_data) {
      let data = JSON.parse(_data);
      this.announce.title = data.Title;
      this.announce.message = data.Message;
      let el = document.getElementById("annc");
      el.style.transform = "translateX(-70%)";
      setTimeout(() => {
        el.style.opacity = "1";
        el.style.transform = "translateX(-50%)";
      }, 1000);
      setTimeout(() => {
        el.style.opacity = "0";
        el.style.transform = "translateX(-30%)";
      }, 7000);
    },
    ShowNotify(_data) {
      let data = JSON.parse(_data);
      data.Id = ++this.notificationCount;
      this.notifications.push(data);

      setTimeout(() => {
        this.notifications.splice(this.notifications.indexOf(data), 1);
      }, data.Time);
    },
    ShowProgress(_data) {
      try {
        let data = JSON.parse(_data);
        data.Id = ++this.notificationCount;
        data.Progress = 0;
        this.progressbars.push(data);
        let item = this.progressbars.find((e) => e.Id == data.Id);
        let i = setInterval(() => {
          if (item.Progress >= 99) {
            clearInterval(i);
            let el = document.getElementById("ProgId-" + data.Id);
            let progr = 1;
            let i2 = setInterval(() => {
              progr -= 0.05;
              el.setAttribute("style", `opacity:${progr} !important;`);
              if (progr <= 0) clearInterval(i2);
            }, 10);
            setTimeout(() => {
              this.progressbars.splice(this.progressbars.indexOf(item), 1);
            }, 500);
          }
          item.Progress++;
        }, data.Time / 100);
      } catch (ex) {
        mp.trigger("Log", ex);
        mp.trigger("Log", JSON.stringify(ex));
      }
    },
  },
  mounted() {
    window.vm.Notifications = this;
  },
};
</script>

<style scoped>
#notification {
  pointer-events: none;
}
.announce {
  position: absolute;
  top: 10%;
  left: 50%;
  transform: translateX(-30%);
  opacity: 0;
  width: 25%;
  border-radius: 7px;
  height: 9%;
  transition: 1s;
}
.announce2 {
  position: relative;
  height: 100%;
  width: 100%;
  margin-left: -5%;
  transform: skewX(-20deg);
  border-radius: 7px;
  background: rgba(10, 168, 189, 0.9);
}
.an-img-c {
  position: relative;
  display: flex;
  width: 50px;
  height: 50px;
  background: rgba(0, 0, 0, 0.01);
  transform: skewX(20deg);
  margin-top: 4.7%;
  margin-left: 9%;
  border-radius: 5px;
  align-items: center;
  justify-content: center;
  box-shadow: inset 0px 0px 25px -3px #00000062;
  float: left;
  margin-right: 2%;
}
.an-svg {
  width: 80%;
  height: auto;
}
h4,
h5 {
  transform: skewX(20deg);
}
h4 {
  font-size: 15px;
  font-weight: 500;
  margin-bottom: 0;
}
h5 {
  font-size: 12px;
  font-weight: 400;
  margin-left: 23.5%;
  margin-top: 1%;
}

#notify {
  position: absolute;
  bottom: 22%;
  left: 2.3%;
  width: 14.4%;
  height: 50%;
  transform: rotate(180deg);
}
.progress {
  position: relative;
  display: block;
  width: 100%;
  height: 18%;
  background: rgba(0, 57, 83, 0.8);
  border-radius: 7px;
  margin-top: 2%;
  transform: rotate(-180deg);
  opacity: 0;
  animation: fadein 200ms linear;
  animation-fill-mode: forwards;
}
.prog-img-c {
  width: 50px;
  height: 50px;
  background: rgba(0, 57, 83, 1);
  box-shadow: inset 0px 0px 20px -3px #000000;
  border-radius: 7px;
  float: left;
  margin-top: 8.5%;
  margin-left: 7%;
  margin-right: 3%;
  display: flex;
  justify-content: center;
  align-items: center;
}
.prog-img {
  width: 60%;
  height: auto;
}
.h12 {
  margin-top: 8%;
}
.h22 {
  width: 55%;
  height: 30%;
}
.prog {
  position: absolute;
  bottom: 10%;
  left: 6.5%;
  width: 87%;
  height: 8%;
  background: #465f68;
  border-radius: 50px;
}
.prog2 {
  width: 40%;
  height: 100%;
  background: #00c3ff;
  border-radius: 50px;
}
h3,
span {
  font-size: 11px;
  font-weight: 300;
}
h3 {
  margin-top: -7%;
  margin-right: 8%;
  float: right;
}
.item {
  position: relative;
  display: block;
  width: 100%;
  height: 15%;
  background: rgba(64, 164, 194, 0.7);
  border-radius: 7px;
  margin-top: 2%;
  transform: rotate(-180deg);
  opacity: 0;
  animation: fadein 200ms linear, fadeout 200ms linear 4850ms;
  animation-fill-mode: forwards;
  box-shadow: inset 0px 0px 30px -5px rgba(0, 0, 0, 0.5);
}
.item-img-c {
  width: 27%;
  height: 100%;
  background: rgba(80, 206, 245, 0.5);
  border-radius: 7px;
  display: flex;
  align-items: center;
  justify-content: center;
  float: left;
  margin-right: 3%;
  box-shadow: inset 0px 0px 30px -5px rgba(0, 0, 0, 0.5);
}
.item-img {
  width: 60%;
  height: auto;
}
h1 {
  font-size: 15px;
  font-weight: 500;
  margin-top: 4%;
  margin-bottom: 0;
}
h2 {
  font-size: 12px;
  font-weight: 400;
  margin-top: 1%;
}

@keyframes fadein {
  100% {
    opacity: 1;
  }
}
@keyframes fadeout {
  100% {
    opacity: 0;
  }
}
</style>
