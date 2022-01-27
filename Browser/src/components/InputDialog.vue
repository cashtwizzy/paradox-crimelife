<template>
  <div id="inputdialog" class="window" v-if="visible">
    <div id="inputdialog-wrapper" class="plopped-wrapper" v-if="visible">
      <div class="plopup">
        <img src="../assets/img/Inventory/popup.png" class="popup-img" />
        <img
          class="popup-exit"
          src="../assets/img/Inventory/exit.svg"
          @click="Exit()"
        />
        <div class="plop-wrapper">
          <div class="plopped-item-c">
            <img
              :src="require(`../assets/img/${itemImage}`)"
              class="item-img"
            />
          </div>
          <div class="plopped-amount">
            <input type="text" v-model="input" placeholder="Eingabe..." />
          </div>
        </div>
        <h5 @click="Submit()">Continue</h5>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "InputDialog",
  data() {
    return {
      visible: false,
      input: "",
      itemImage: "",
      callback: "",
    };
  },
  methods: {
    Show(show, _data) {
      this.visible = show;
      if (show) {
        let data = JSON.parse(_data);
        this.callback = data.CallBack;
        this.itemImage = data.ItemImage;
        setTimeout(() => {
          document
            .getElementById("inputdialog-wrapper")
            .classList.add("wrapper-anim");
        }, 10);
      } else {
        this.input = "";
        this.callback = "";
        this.itemImage = "";
        setTimeout(() => {
          document
            .getElementById("inputdialog-wrapper")
            .classList.remove("wrapper-anim");
        }, 10);
      }
    },
    Submit() {
      if (this.input.length < 1) return;

      mp.trigger(this.callback, this.input);
    },
    Exit() {
      mp.trigger("ShowComponent", "InputDialog", false, "");
    },
  },
  mounted() {
    window.vm.InputDialog = this;
  },
};
</script>

<style scoped>
#inputdialog {
  background-color: rgba(0, 85, 100, 0.5);
}
#inputdialog::before {
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
.plopped-wrapper {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  z-index: 2;
}
.plopup {
  position: absolute;
  top: 45%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 10.8%;
  height: 25%;
  background: #24b1dd;
  border-radius: 10px;
  text-align: center;
}
.popup-img {
  width: 100%;
  height: auto;
  border-radius: 10px;
}
.popup-exit {
  position: absolute;
  top: 3%;
  right: 3%;
  width: 12%;
  height: auto;
  z-index: 2;
}
.plop-wrapper {
  position: absolute;
  top: 0;
  right: 0;
  width: 100%;
  height: 84%;
  text-align: center;
}
.plopped-item-c {
  position: relative;
  display: flex;
  width: 45%;
  height: 40%;
  margin-top: 20%;
  margin-left: 27.5%;
  background: #2d5157bb;
  border-radius: 5px;
  align-items: center;
  justify-content: center;
}
.item-img {
  width: 60%;
  height: auto;
}
.plopped-amount {
  display: flex;
  width: 80%;
  height: 17.5%;
  text-align: center;
  background: #2d5157bb;
  margin-top: 10%;
  margin-left: 10%;
  border: none;
  border-radius: 10px;
  align-items: center;
}
h4 {
  width: 45%;
  height: 50%;
  text-align: center;
}
.arrow-img {
  width: auto;
  height: 60%;
  margin-left: 5%;
}
h5 {
  height: 20%;
  width: 100%;
  margin-top: -3%;
  padding-top: 6%;
  z-index: 4;
}
input {
  width: 100%;
  height: 90%;
  background: transparent;
  border: none;
  text-align: center;
}
</style>
