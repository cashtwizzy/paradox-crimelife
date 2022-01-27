<template>
  <div id="garage" class="window" v-if="visible">
    <div class="wrapper" id="garage-wrapper">
      <div class="c1" v-if="currentCar != null && currentCar.Name != ''">
        <div class="c1-head"><h1>Current Car</h1></div>
        <div class="c1-body">
          <div class="c1-body-c">
            <h1>{{ currentCar.Name }}</h1>
            <h2>Information</h2>
            <h3>
              Nummernschild:
              <span class="vehicle-value">{{ currentCar.Plate }}</span>
            </h3>
            <h3>
              Kaufdatum:
              <span class="vehicle-value">{{ currentCar.RegDate }}</span>
            </h3>
            <h3>
              Tank:
              <span class="vehicle-value">100/100L</span>
            </h3>
            <h3>
              Kofferraum:
              <span class="vehicle-value"
                >{{ currentCar.TrunkWeight }}/{{
                  currentCar.MaxTrunkWeight
                }}
                KG</span
              >
            </h3>
            <h2 style="margin-top: 4%">Upgrades</h2>
            <div class="upgrade">
              <img src="../assets/img/Garage/engine.svg" class="upgrade-img" />
              <div class="upgrade-step-c">
                <div class="upgrade-step step-active"></div>
                <div class="upgrade-step step-active"></div>
                <div class="upgrade-step step-active"></div>
                <div class="upgrade-step"></div>
              </div>
            </div>
            <div class="upgrade">
              <img src="../assets/img/Garage/engine.svg" class="upgrade-img" />
              <div class="upgrade-step-c">
                <div class="upgrade-step step-active"></div>
                <div class="upgrade-step step-active"></div>
                <div class="upgrade-step"></div>
              </div>
            </div>
            <div class="upgrade">
              <img src="../assets/img/Garage/engine.svg" class="upgrade-img" />
              <div class="upgrade-step-c">
                <div
                  :class="
                    currentCar.Tuning.Transmission > 0
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
                <div
                  :class="
                    currentCar.Tuning.Transmission > 1
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
                <div
                  :class="
                    currentCar.Tuning.Transmission > 2
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
                <div
                  :class="
                    currentCar.Tuning.Transmission > 3
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
              </div>
            </div>
            <div class="upgrade">
              <img src="../assets/img/Garage/engine.svg" class="upgrade-img" />
              <div class="upgrade-step-c">
                <div
                  :class="
                    currentCar.Tuning.Suspension > 0
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
                <div
                  :class="
                    currentCar.Tuning.Suspension > 1
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
                <div
                  :class="
                    currentCar.Tuning.Suspension > 2
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
                <div
                  :class="
                    currentCar.Tuning.Suspension > 3
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
              </div>
            </div>
            <div class="upgrade" style="margin-right: 0">
              <img src="../assets/img/Garage/engine.svg" class="upgrade-img" />
              <div class="upgrade-step-c">
                <div
                  :class="
                    currentCar.Tuning.Turbo
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
              </div>
            </div>
          </div>
          <h1 class="park" @click="ParkVehicle()">Einparken</h1>
        </div>
      </div>
      <div class="c2">
        <div class="c2-head"><h1>Garage</h1></div>
        <div class="c2-body">
          <div class="item" v-for="item in vehicles" :key="item.Id">
            <div class="item-idc">
              <h1 class="item-id">{{ vehicles.indexOf(item) + 1 }}</h1>
            </div>
            <h1 class="item-name">{{ item.Id }} - {{ item.Name }}</h1>
            <div class="select" @click="SelectVehicle(item)">
              <div class="select-imgc">
                <img src="../assets/img/Garage/select.svg" class="select-img" />
              </div>
              <h1 class="select-txt">Select</h1>
            </div>
          </div>
        </div>
      </div>
      <div class="c1" style="margin-right: 0" v-if="selectedVehicle > -1">
        <div class="c1-head"><h1>Selected Car</h1></div>
        <div class="c1-body">
          <div class="c1-body-c">
            <h1>{{ vehicles[selectedVehicle].Name }}</h1>
            <h2>Information</h2>
            <h3>
              Nummernschild:
              <span class="vehicle-value">{{
                vehicles[selectedVehicle].Plate
              }}</span>
            </h3>
            <h3>
              Kaufdatum:
              <span class="vehicle-value">{{
                vehicles[selectedVehicle].RegDate
              }}</span>
            </h3>
            <h3>
              Tank:
              <span class="vehicle-value">100/100L</span>
            </h3>
            <h3>
              Kofferraum:
              <span class="vehicle-value"
                >{{ vehicles[selectedVehicle].TrunkWeight }}/{{
                  vehicles[selectedVehicle].MaxTrunkWeight
                }}
                KG</span
              >
            </h3>
            <h2 style="margin-top: 4%">Upgrades</h2>
            <div class="upgrade">
              <img src="../assets/img/Garage/engine.svg" class="upgrade-img" />
              <div class="upgrade-step-c">
                <div
                  :class="
                    vehicles[selectedVehicle].Tuning.Engine > 0
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
                <div
                  :class="
                    vehicles[selectedVehicle].Tuning.Engine > 1
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
                <div
                  :class="
                    vehicles[selectedVehicle].Tuning.Engine > 2
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
                <div
                  :class="
                    vehicles[selectedVehicle].Tuning.Engine > 3
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
              </div>
            </div>
            <div class="upgrade">
              <img src="../assets/img/Garage/engine.svg" class="upgrade-img" />
              <div class="upgrade-step-c">
                <div
                  :class="
                    vehicles[selectedVehicle].Tuning.Brakes > 0
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
                <div
                  :class="
                    vehicles[selectedVehicle].Tuning.Brakes > 1
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
                <div
                  :class="
                    vehicles[selectedVehicle].Tuning.Brakes > 2
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
              </div>
            </div>
            <div class="upgrade">
              <img src="../assets/img/Garage/engine.svg" class="upgrade-img" />
              <div class="upgrade-step-c">
                <div
                  :class="
                    vehicles[selectedVehicle].Tuning.Transmission > 0
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
                <div
                  :class="
                    vehicles[selectedVehicle].Tuning.Transmission > 1
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
                <div
                  :class="
                    vehicles[selectedVehicle].Tuning.Transmission > 2
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
                <div
                  :class="
                    vehicles[selectedVehicle].Tuning.Transmission > 3
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
              </div>
            </div>
            <div class="upgrade">
              <img src="../assets/img/Garage/engine.svg" class="upgrade-img" />
              <div class="upgrade-step-c">
                <div
                  :class="
                    vehicles[selectedVehicle].Tuning.Suspension > 0
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
                <div
                  :class="
                    vehicles[selectedVehicle].Tuning.Suspension > 1
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
                <div
                  :class="
                    vehicles[selectedVehicle].Tuning.Suspension > 2
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
                <div
                  :class="
                    vehicles[selectedVehicle].Tuning.Suspension > 3
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
              </div>
            </div>
            <div class="upgrade" style="margin-right: 0">
              <img src="../assets/img/Garage/engine.svg" class="upgrade-img" />
              <div class="upgrade-step-c">
                <div
                  :class="
                    vehicles[selectedVehicle].Tuning.Turbo
                      ? 'upgrade-step step-active'
                      : 'upgrade-step'
                  "
                ></div>
              </div>
            </div>
          </div>
          <h1 class="park" @click="TakeVehicle()">Ausparken</h1>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Garage",
  data() {
    return {
      visible: false,
      garageId: -1,
      vehicles: [],
      selectedVehicle: -1,
      currentCar: null,
    };
  },
  methods: {
    Show(show, _data) {
      this.visible = show;
      if (show) {
        let data = JSON.parse(_data);
        this.vehicles = data.Vehicles;
        this.currentCar = data.CurrentVehicle;
        this.garageId = data.GarageId;
        setTimeout(() => {
          document
            .getElementById("garage-wrapper")
            .classList.add("wrapper-anim");
        }, 10);
      } else {
        this.Vehicles = [];
        this.currentCar = null;
        this.selectedVehicle = -1;
        setTimeout(() => {
          document
            .getElementById("garage-wrapper")
            .classList.remove("wrapper-anim");
        }, 10);
      }
    },
    ParkVehicle() {
      if (this.currentCar == null || this.currentCar.Name == "") return;

      mp.trigger("ParkVehicle", this.currentCar.Id);
    },
    SelectVehicle(item) {
      this.selectedVehicle = this.vehicles.indexOf(item);
    },
    TakeVehicle() {
      if (
        this.selectedVehicle < 0 ||
        this.vehicles[this.selectedVehicle] == null
      )
        return;

      mp.trigger(
        "TakeVehicle",
        this.garageId,
        this.vehicles[this.selectedVehicle].Id
      );
    },
  },
  mounted() {
    window.vm.Garage = this;
  },
};
</script>

<style scoped>
#garage {
  background: linear-gradient(
    90deg,
    rgba(0, 164, 255, 1) 0%,
    rgba(0, 0, 0, 0.4) 71%
  );
  box-shadow: inset 0px 0px 150px 5px #000000;
}
#garage::before {
  content: "";
  position: absolute;
  width: 200%;
  height: 200%;
  top: -50%;
  left: -50%;
  z-index: 1;
  background-image: radial-gradient(#3d3d3d3a 7%, transparent 7%),
    radial-gradient(#3d3d3d3a 7%, transparent 7%);
  background-position: 0 0, 20px 20px;
  background-size: 40px 40px;
  transform: rotate(45deg);
}
.wrapper {
  position: absolute;
  width: 60%;
  height: 42%;
  top: 65%;
  left: 38%;
  opacity: 0;
  transform: translate(-50%, -50%);
  z-index: 1;
  animation-duration: 1s;
  animation-fill-mode: forwards;
  animation: none;
}
.c1 {
  width: 259.19px;
  height: 100%;
  border-radius: 10px;
  float: left;
  margin-right: 2.5%;
}
.c1-head {
  width: 100%;
  height: 20%;
  background-image: url(../assets/img/Garage/banner1.png);
  background-size: 100%;
  background-position: center;
  border-radius: 10px;
  display: flex;
  align-items: center;
}
h1 {
  font-size: 20px;
  font-weight: 400;
  margin-left: 10%;
}
.c1-body {
  width: 100%;
  height: 75%;
  margin-top: 10%;
  background: #24b1dd;
  background-size: 100%;
  border-radius: 10px;
  text-align: center;
  z-index: 1;
}
.c1-body > h1 {
  margin-left: 0;
}
.c1-body-c {
  position: relative;
  width: 82%;
  height: 75%;
  padding: 10%;
  padding-left: 8%;
  padding-bottom: 0;
  background: linear-gradient(300deg, #030a0c 0%, #131719 100%);
  background-size: 100%;
  border-radius: 10px;
  text-align: left;
}
.c1-body-c > h1 {
  margin-left: 0;
  font-size: 19px;
  font-weight: 500;
  margin-top: 0;
}
h2 {
  color: #84b6d2;
  font-size: 16px;
  font-weight: 400;
  margin-bottom: 3%;
}
h3 {
  font-size: 14px;
  font-weight: 400;
  margin: 0;
  margin-top: 1.5%;
}
.vehicle-value {
  color: #0384c8;
  font-size: 14px;
  font-weight: 500;
}
.upgrade {
  width: 18.3%;
  height: 26%;
  background: #0383c82f;
  border-radius: 5px;
  float: left;
  margin-right: 2%;
}
.upgrade-img {
  width: 55%;
  height: auto;
  margin-left: 22.5%;
  margin-top: 15%;
}
.upgrade-step-c {
  width: 30%;
  height: 45%;
  margin-left: 35%;
  margin-top: -2%;
  transform: rotate(180deg);
}
.upgrade-step {
  width: 100%;
  height: 3px;
  background: rgba(126, 126, 126, 0.8);
  margin-top: 31%;
  border-radius: 50px;
}
.step-active {
  background: #0384c8;
}
.park {
  width: 100%;
  height: 15%;
  margin-top: -2%;
  padding-top: 7%;
  z-index: 4;
}
.c2 {
  width: 388.8px; /* 45% */
  height: 100%;
  float: left;
  margin-right: 2.5%;
}
.c2-head {
  width: 100%;
  height: 20%;
  background-image: url(../assets/img/Garage/banner1.png);
  background-size: 100%;
  background-position: center;
  border-radius: 10px;
  display: flex;
  align-items: center;
}
.c2-body {
  width: 100%;
  height: 75%;
  margin-top: 6.2%;
  background-size: 100%;
  text-align: center;
  z-index: 1;
  overflow-y: scroll;
}
.c2-body::-webkit-scrollbar {
  display: none;
}
.item {
  position: relative;
  width: 100%;
  height: 16%;
  margin-bottom: 3%;
  background: rgb(19, 23, 25);
  background: linear-gradient(
    24deg,
    rgba(19, 23, 25, 0.9) 0%,
    rgba(3, 10, 12, 0.9) 100%
  );
  border-radius: 6px;
  text-align: left;
}
.item-idc {
  width: 9%;
  height: 60%;
  margin-top: 2.6%;
  margin-left: 3%;
  margin-right: 4%;
  background: #24b1dd;
  border-radius: 7px 7px 20px 7px;
  display: flex;
  align-items: center;
  float: left;
}
.item-id {
  margin-left: 30%;
}
.item-name {
  font-size: 18px;
  font-weight: 400;
  margin-left: 0;
  margin-top: 4%;
}
.select {
  position: absolute;
  top: 50%;
  right: 4%;
  width: 28%;
  height: 60%;
  transform: translateY(-50%);
  background: #1a769277;
  border-radius: 5px 5px 5px 14px;
}
.select-imgc {
  width: 30%;
  height: 100%;
  background: #239abe77;
  display: flex;
  align-items: center;
  justify-content: center;
  float: left;
  border-radius: 5px;
  margin-right: 14%;
}
.select-img {
  width: 70%;
  height: auto;
}
.select-txt {
  margin-top: 5%;
  font-size: 16px;
}
.wrapper-anim {
  animation: slideup 300ms linear 1;
  animation-fill-mode: forwards;
}

@keyframes slideup {
  0% {
    opacity: 0;
    top: 65%;
  }

  100% {
    opacity: 1;
    top: 48%;
  }
}
</style>
