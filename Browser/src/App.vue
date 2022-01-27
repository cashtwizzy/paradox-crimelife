<template>
  <div id="background"></div>
  <Hud />
  <Login />
  <Creator />
  <Chat />
  <Notifications />
  <Inventory />
  <Garage />
  <Shop />
  <ClothesShop />
  <XMenu />
  <InputDialog />
  <Loading />
</template>

<script>
import Hud from "./components/Hud.vue";
import Login from "./components/Login.vue";
import Creator from "./components/Creator.vue";
import Chat from "./components/Chat.vue";
import Inventory from "./components/Inventory.vue";
import Garage from "./components/Garage.vue";
import Shop from "./components/Shop.vue";
import ClothesShop from "./components/ClothesShop.vue";
import XMenu from "./components/XMenu.vue";
import InputDialog from "./components/InputDialog.vue";
import Notifications from "./components/Notifications.vue";
import Loading from "./components/Loading.vue";

export default {
  name: "App",
  components: {
    Hud,
    Login,
    Creator,
    Chat,
    Inventory,
    Garage,
    Shop,
    ClothesShop,
    XMenu,
    InputDialog,
    Notifications,
    Loading,
  },
  methods: {},
  mounted() {
    window.vm.App = this;
  },
};
</script>

<style lang="scss">
@import url("https://fonts.googleapis.com/css2?family=Roboto+Condensed:wght@300;400;700&family=Roboto:wght@100;300;400;500&display=swap");

* {
  font-family: "Roboto", sans-serif;
  font-weight: 400;
  font-size: 17px;
  color: rgb(230, 230, 230);
  overflow: hidden;
  outline: none;
  user-select: none;
}

#background {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-size: 100%;
}

.window {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
}

.np {
  pointer-events: none;
}
</style>
