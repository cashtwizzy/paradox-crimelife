<template>
  <div class="wrapper" v-if="visible">
    <div class="input">
      <input id="chatinput" type="text" placeholder="/help" v-model="input" />
      <div class="wrapper2">
        <button @click="Submit()">
          <img src="../assets/img/Creator/play.svg" />
        </button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Chat",
  data() {
    return {
      visible: false,
      input: "",
      lastInputs: [],
      index: -1,
    };
  },
  methods: {
    Show(show, data) {
      this.visible = show;
      console.log(data);
      setTimeout(() => {
        if (show) document.getElementById("chatinput").focus();
        else this.input = "";
      }, 100);
    },
    Submit() {
      let userInput = this.input.substr(1);
      if (this.input == "") {
        mp.trigger("ShowComponent", "Chat", false, "");
        this.lastInputs.unshift(this.input);
        this.index = -1;
        return;
      }

      if (this.input.charAt(0) == "/") {
        this.lastInputs.unshift(this.input);
        mp.trigger("PlayerChat", userInput);
        this.index = -1;
      }
    },
    onKeyUp(e) {
      if (e.keyCode === 38) {
        this.index++;
        if (this.index >= this.lastInputs.length) {
          this.index = this.lastInputs.length - 1;
        }
      } else if (e.keyCode === 40) {
        this.index--;
        if (this.index <= 0) {
          this.index = 0;
        }
      } else if (e.keyCode === 27) {
        mp.trigger("ShowComponent", "Chat", false, "");
      } else if (e.keyCode === 13 && this.visible) {
        this.Submit();
      }
    },
  },
  mounted() {
    window.vm.Chat = this;
    window.addEventListener("keyup", this.onKeyUp);
  },
  watch: {
    index(index) {
      if (index > -1) this.input = this.lastInputs[index];
    },
  },
  unmounted() {
    window.removeEventListener("keyup", this.onKeyUp);
  },
};
</script>

<style lang="scss" scoped>
.wrapper {
  position: absolute;
  top: 35%;
  left: 1%;
  width: 26%;
  height: 5%;
  border-radius: 10px;

  .input {
    position: relative;
    width: 90%;
    height: 100%;
    transform: translateY(-50%);
    background: #00181de6;
    color: white;
    border: none;
    border-radius: 15px;
    transform: SkewX(-20deg);
    overflow: visible;
    margin-left: -10%;

    input {
      position: relative;
      width: 72%;
      height: 95%;
      background: transparent;
      border: none;
      transform: skewX(20deg);
      margin-left: 15%;
      float: left;
    }

    .wrapper2 {
      position: relative;
      display: inline-block;
      width: 12%;
      height: 68%;
      margin-top: 1.8%;
      transform: skewX(20deg);
      border-radius: 12px;

      button {
        position: relative;
        width: 90%;
        height: 100%;
        margin-top: 2%;
        background: #3c87a3;
        border: none;
        transform: skewX(-20deg);
        margin-left: -15%;
        border-radius: 12px;
        cursor: pointer;

        img {
          transform: skewX(20deg);
          height: 80%;
          width: auto;
          margin-top: 1%;
          margin-left: 15%;
        }
      }
    }
  }
}
</style>
