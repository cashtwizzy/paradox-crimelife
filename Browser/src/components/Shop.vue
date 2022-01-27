<template>
  <div id="shop" class="window" v-if="visible">
    <div class="wrapper" id="shop-wrapper">
      <div class="head1-c">
        <h1 class="head1">24/7 Shop</h1>
      </div>
      <div class="head2-c">
        <h1 class="head2">Warenkorb</h1>
      </div>
      <div class="wrapper2">
        <div class="items-c">
          <div class="item" v-for="item in items" :key="items.indexOf(item)">
            <img
              :src="
                require(`../assets/img/Items/${item.Name.toLowerCase()}.png`)
              "
              class="item-img"
            />
            <h1 class="item-h1">{{ item.Name }}</h1>
            <button class="btn1" @click="AddItemToCart(item)">
              Hinzufügen
            </button>
          </div>
        </div>
        <div class="cart-c">
          <div class="cart-c2">
            <div
              class="cart-item"
              v-for="item in cart"
              :key="cart.indexOf(item)"
            >
              <img
                :src="
                  require(`../assets/img/Items/${item.Name.toLowerCase()}.png`)
                "
                class="cart-img"
              />
              <h1 class="cart-h1">{{ item.Name }}</h1>
              <button class="cart-input-changer" @click="DecreaseAmount(item)">
                -
              </button>
              <input type="text" class="cart-input" v-model="item.Amount" />
              <button class="cart-input-changer" @click="IncreaseAmount(item)">
                +
              </button>
            </div>
          </div>
          <h1 class="cart-submit" @click="BuyItems()">Kaufen</h1>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Shop",
  data() {
    return {
      visible: false,
      items: [],
      cart: [],
    };
  },
  methods: {
    Show(show, data) {
      this.visible = show;
      this.items = show ? JSON.parse(data) : [];
      this.cart = [];
      if (show) {
        setTimeout(() => {
          document.getElementById("shop-wrapper").classList.add("wrapper-anim");
        }, 10);
      } else {
        setTimeout(() => {
          document
            .getElementById("shop-wrapper")
            .classList.remove("wrapper-anim");
        }, 10);
      }
    },
    AddItemToCart(item) {
      let existingItem = this.cart.find((x) => x.Name == item.Name);
      if (existingItem != null) {
        this.cart[this.cart.indexOf(existingItem)].Amount++;
      } else {
        this.cart.push({
          Id: item.Id,
          Name: item.Name,
          Price: item.Price,
          Amount: 1,
        });
      }
    },
    IncreaseAmount(item) {
      item.Amount++;
    },
    DecreaseAmount(item) {
      if (item.Amount <= 1) {
        this.cart.splice(this.cart.indexOf(item), 1);
      } else {
        item.Amount--;
      }
    },
    BuyItems() {
      if (this.cart.length < 1) return;

      mp.trigger("BuyItems", JSON.stringify(this.cart));
    },
  },
  mounted() {
    window.vm.Shop = this;
  },
};
</script>

<style scoped>
#shop {
  background-color: rgba(0, 85, 100, 0.5);
}
#shop::before {
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
  width: 55%;
  height: 60%;
  top: 65%;
  left: 50%;
  opacity: 0;
  transform: translate(-50%, -50%);
  z-index: 2;
  border-radius: 15px;
  overflow: visible;
}
.head1-c {
  width: 68.5%;
  height: 15%;
  background-image: url(../assets/img/Inventory/banner.png);
  background-position: center top;
  background-size: 100%;
  border-radius: 10px;
  display: flex;
  align-items: center;
  float: left;
  margin-bottom: 3%;
  margin-left: 2%;
}
.head2-c {
  width: 24%;
  height: 15%;
  background-image: url(../assets/img/Inventory/banner.png);
  background-position: center top;
  background-size: 100%;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  float: left;
  margin-bottom: 3%;
  margin-left: 3.5%;
}
.head1 {
  font-size: 25px;
  font-weight: 400;
  margin-left: 5%;
}
.head2 {
  font-size: 25px;
  font-weight: 400;
}
.wrapper2 {
  width: 96%;
  height: 75%;
  margin-left: 2%;
}
.items-c {
  width: 75%;
  height: 100%;
  float: left;
  overflow-y: scroll;
}
.items-c::-webkit-scrollbar {
  display: none;
}
.item {
  width: 20%;
  height: 35%;
  background: #131313;
  border-radius: 7px;
  float: left;
  margin-right: 5%;
  margin-bottom: 5%;
  text-align: center;
  background-image: url(../assets/img/Login/roads.png);
  background-size: 200%;
}
.item-img {
  width: 40%;
  height: min-content;
  margin-top: 12.5%;
  margin-bottom: -2%;
}
.item-h1 {
  font-size: 17px;
  font-weight: 400;
}
.btn1 {
  width: 50%;
  height: 14%;
  margin-top: -4%;
  background: linear-gradient(35deg, #5dc5c1 0%, #1589a7 100%);
  border: none;
  border-radius: 3px;
  cursor: pointer;
  transition: 200ms;
  font-size: 13px;
}
.btn1:active {
  transform: scale(80%);
}
.cart-c {
  width: 25%;
  height: 100%;
  background: #5dc5c1;
  border-radius: 11px;
}
.cart-c2 {
  width: 100%;
  height: 90%;
  border-radius: 11px;
  background: #131313;
  background-image: url(../assets/img/Login/roads.png);
  background-size: 200%;
}
.cart-item {
  width: 100%;
  height: 11%;
  margin-top: 2%;
  border-radius: 7px;
  display: flex;
  align-items: center;
}
.cart-img {
  width: min-content;
  height: 60%;
  margin-left: 5%;
  margin-right: 2%;
  float: left;
}
.cart-h1 {
  font-size: 15px;
  width: 45%;
}
.cart-input {
  width: 13%;
  height: 50%;
  float: right;
  text-align: center;
  background: transparent;
  border: none;
  border-bottom: 2px solid transparent;
  transition: 150ms;
}
.cart-input:focus {
  border-bottom: 2px solid #5dc5c1;
}
.cart-input-changer {
  width: 8%;
  height: 50%;
  background: transparent;
  border: none;
  cursor: pointer;
}
.cart-submit {
  width: 100%;
  height: 15%;
  margin-top: -2%;
  padding-top: 7%;
  z-index: 4;
  text-align: center;
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
