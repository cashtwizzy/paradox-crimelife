<template>
  <div id="loading" class="window" v-if="visible">
    <div class="wrapper">
      <div class="circle"></div>
      <h1>{{ msg }}</h1>
    </div>
  </div>
</template>

<script>
export default {
  name: "Loading",
  data() {
    return {
      visible: false,
      msg: "",
    };
  },
  methods: {
    Show(show, data) {
      this.visible = show;
      this.msg = data;
    },
    ChangeData(msg) {
      this.msg = msg;
    },
  },
  mounted() {
    window.vm.Loading = this;
  },
};
</script>

<style scoped>
#loading {
  background-color: rgba(0, 0, 0, 0.7);
  background: radial-gradient(
    circle,
    rgba(0, 0, 0, 0.5) 0%,
    rgba(0, 0, 0, 1) 100%
  );
  z-index: 4;
}
.wrapper {
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 10%;
  height: 20%;
  text-align: center;
}
.circle {
  display: inline-block;
  width: 64px;
  height: 64px;
  border-radius: 100%;
  border: 7px solid transparent;
  border-top: 7px solid white;
  border-bottom: 7px solid white;
  animation: spin 1000ms linear infinite;
  transform-origin: center;
  margin-bottom: 2%;
}
h1 {
  font-size: 15px;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>
