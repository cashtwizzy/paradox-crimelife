<template>
  <div id="login" class="window" v-if="visible">
    <div class="wrapper" id="login-wrapper">
      <img src="../assets/img/Login/logo.png" class="logo" />
      <img src="../assets/img/Login/franklin.png" class="franklin" />
      <div class="right">
        <h1>SIGN IN</h1>
        <h2>
          Bitte logge dich mit deinen Nutzerdaten ein.<br />
          Neu hier?<br />
          Unter paradoxcl.de kannst du dich ganz einfach<br />
          registrieren. Wir sehen uns in Los Santos!
        </h2>
        <input
          type="text"
          class="name"
          placeholder="Username"
          v-model="name"
        /><br />
        <input
          type="password"
          class="pass"
          placeholder="Password"
          v-model="pass"
        /><br />
        <div class="error-wrapper">
          <h3 id="error"></h3>
        </div>
        <button @click="Submit()">Sign In</button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Login",
  data() {
    return {
      visible: false,
      name: "",
      pass: "",
    };
  },
  methods: {
    Show(show, name) {
      this.name = name;
      this.visible = show;
      if (show)
        setTimeout(() => {
          document
            .getElementById("login-wrapper")
            .classList.add("wrapper-anim");
        }, 10);
      else
        setTimeout(() => {
          document
            .getElementById("login-wrapper")
            .classList.remove("wrapper-anim");
        }, 10);
    },
    Submit() {
      if (!this.name.match(/([a-zA-Z]+)_([a-zA-Z]+)/)) {
        this.DisplayError(
          "Der angegebene Name muss im Format Vor_Nachname sein!"
        );
        return;
      }
      if (this.name.length < 4) {
        this.DisplayError(
          "Der angegebene Name muss mind. 4 Zeichen lang sein!"
        );
        return;
      }
      if (this.name.length > 20) {
        this.DisplayError("Der angegebene Name ist zu lang!(max. 20 Zeichen)");
        return;
      }
      if (this.pass.length < 6) {
        this.DisplayError(
          "Das angegebene Passwort muss mind. 6 Zeichen lang sein!"
        );
        return;
      }

      mp.trigger("PlayerLogin", this.name, this.pass);
    },
    DisplayError(data) {
      document.getElementById("error").innerText = data;
    },
  },
  mounted() {
    window.vm.Login = this;
  },
};
</script>

<style scoped>
#login {
  background-color: rgba(0, 85, 100, 0.5);
}
#login::before {
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
  width: 65%;
  height: 50%;
  top: 67%;
  opacity: 0;
  left: 50%;
  transform: translate(-50%, -50%);
  background-image: url(../assets/img/Login/roads.png);
  z-index: 2;
  border-radius: 25px;
  box-shadow: 0px 0px 50px 20px #003e4d;
  overflow: visible;
}
.logo {
  width: auto;
  height: 100%;
  border-radius: 25px;
}
.franklin {
  width: auto;
  height: 115%;
  margin-top: -15%;
  margin-left: -10%;
  margin-right: -10%;
}
.right {
  width: 37%;
  height: 100%;
  float: right;
}
h1 {
  margin-top: 15%;
  margin-bottom: 1%;
  font-size: 40px;
  font-weight: 600;
}
h2 {
  margin-top: 0%;
  margin-bottom: 9%;
  font-size: 15px;
  font-weight: 300;
  color: #b6b6b6;
}
input {
  width: 70%;
  height: 9%;
  background: rgba(0, 24, 27, 0.6);
  border: none;
  border-radius: 7px;
  margin-top: 3.5%;
  background-repeat: no-repeat;
  transition: 300ms;
  text-indent: 15%;
  background-position: 4% center;
  background-size: 8%;
  color: grey;
}
.name {
  background-image: url(../assets/img/Login/user.svg);
}
.name:focus {
  background-image: url(../assets/img/Login/user-active.svg);
  background-position: 6% center;
}
.pass {
  background-image: url(../assets/img/Login/pass.svg);
}
.pass:focus {
  background-image: url(../assets/img/Login/pass-active.svg);
  background-position: 6% center;
}
.error-wrapper {
  width: 70%;
  height: fit-content;
  margin-bottom: 0;
  text-align: center;
}
h3 {
  color: red;
  margin-top: 3%;
}
button {
  width: 40%;
  height: 9%;
  margin-top: 2%;
  margin-left: 15%;
  background: linear-gradient(35deg, #5dc5c1 0%, #1589a7 100%);
  border: none;
  border-radius: 7px;
  cursor: pointer;
  transition: 200ms;
}
button:active {
  transform: scale(80%);
}
.wrapper-anim {
  animation: slideup 300ms linear 1;
  animation-fill-mode: forwards;
}

@keyframes slideup {
  0% {
    opacity: 0;
    top: 67%;
  }

  100% {
    opacity: 1;
    top: 50%;
  }
}
</style>
