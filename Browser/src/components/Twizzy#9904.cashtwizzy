<template>
  <div id="clothesshop" class="window" v-if="visible">
    <div class="wrapper" id="clothesshop-wrapper">
      <div class="left">
        <div class="head">
          <h1 class="head-h1">Kategorien</h1>
        </div>

        <div class="body">
          <div class="item" v-for="item in categories" :key="item.Name">
            <h1 class="item-h1">{{ item.Name }}</h1>
            <div class="select" @click="SelectCategory(item.Id)">
              <div class="select-imgc">
                <img
                  src="../assets/img/ClothesShop/clothes.svg"
                  class="select-img"
                />
              </div>
              <h1 class="select-txt">Select</h1>
            </div>
          </div>
        </div>
      </div>

      <div class="right">
        <div class="head" style="height: 81px">
          <h1 class="head-h1">Kleidung</h1>
        </div>

        <div class="body">
          <div class="rc">
            <div
              :class="SelectedItem == item ? 'item item-active' : 'item'"
              v-for="item in GetItemsFromCategory"
              :key="item.Name"
            >
              <h1 class="item-h1">{{ item.Name }}</h1>
              <div
                :class="
                  SelectedItem == item
                    ? 'select-item select-item-active'
                    : 'select-item'
                "
                @click="SelectClothes(item)"
              >
                <div
                  class="buy-item-imgc"
                  v-if="SelectedItem == item"
                  @click="BuyClothes()"
                >
                  <img
                    src="../assets/img/ClothesShop/buy.svg"
                    class="buy-img"
                  />
                </div>
                <div class="select-item-imgc">
                  <img
                    src="../assets/img/ClothesShop/clothes.svg"
                    class="select-img"
                  />
                </div>
              </div>
              <input
                type="range"
                v-model="SelectedColor"
                min="0"
                :max="SelectedItem.MaxColor"
                :change="ChangeColor()"
                v-if="SelectedItem == item"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "ClothesShop",
  data() {
    return {
      visible: false,
      clothes: [],
      categories: [
        { Id: 1, Name: "Masken" },
        { Id: 11, Name: "Oberteil" },
        { Id: 8, Name: "Unterteil" },
        { Id: 3, Name: "Körper" },
        { Id: 4, Name: "Hosen" },
        { Id: 6, Name: "Schuhe" },
      ],
      SelectedItem: null,
      activeCategory: 1,
      SelectedColor: 0,
    };
  },
  methods: {
    Show(show, data) {
      this.visible = show;
      this.clothes = show ? JSON.parse(data) : [];
      this.SelectedItem = null;
      this.SelectedColor = 0;
      if (show) {
        setTimeout(() => {
          document
            .getElementById("clothesshop-wrapper")
            .classList.add("wrapper-anim");
        }, 10);
      } else {
        setTimeout(() => {
          document
            .getElementById("clothesshop-wrapper")
            .classList.remove("wrapper-anim");
        }, 10);
      }
    },
    SelectClothes(item) {
      this.SelectedItem = item;
      mp.trigger(
        "ChangeClothes",
        this.SelectedItem.Category,
        this.SelectedItem.Drawable,
        this.SelectedColor
      );
    },
    ChangeColor() {
      mp.trigger(
        "ChangeClothes",
        this.SelectedItem.Category,
        this.SelectedItem.Drawable,
        this.SelectedColor
      );
    },
    SelectCategory(cat) {
      this.activeCategory = cat;
      this.SelectedItem = null;
      this.SelectedColor = 0;
      this.SelectedMax = 0;
    },
    BuyClothes() {
      if (this.SelectedItem == null) return;

      mp.trigger(
        "BuyClothes",
        this.SelectedItem.Category,
        this.SelectedItem.Drawable,
        this.SelectedColor,
        this.SelectedItem.Price
      );
    },
  },
  computed: {
    GetItemsFromCategory() {
      return this.clothes.filter((x) => x.Category == this.activeCategory);
    },
  },
  mounted() {
    window.vm.ClothesShop = this;
  },
};
</script>

<style lang="scss" scoped>
.wrapper {
  position: absolute;
  width: 100%;
  height: 100%;
  top: 15%;
  left: 0%;
  opacity: 1;
  z-index: 2;
  border-radius: 15px;

  .left {
    position: absolute;
    width: 20%;
    height: 50%;
    top: 50%;
    transform: translateY(-50%);
    left: 1%;

    .head {
      width: 100%;
      height: 15%;
      background-image: url(../assets/img/Inventory/banner.png);
      background-position: center top;
      background-size: 100%;
      border-radius: 10px;
      display: flex;
      align-items: center;
    }

    .head-h1 {
      font-size: 23px;
      margin-left: 6%;
    }

    .body {
      width: 100%;
      height: 80%;
      margin-top: 5%;

      .item {
        position: relative;
        width: 100%;
        height: 13%;
        background: #16181ad9;
        border-radius: 10px;
        display: flex;
        align-items: center;
        margin-bottom: 3%;

        .item-h1 {
          margin: 0;
          margin-left: 5%;
          width: 60%;
        }

        .select {
          position: absolute;
          top: 50%;
          right: 4%;
          width: 28%;
          transform: translateY(-50%);
          height: 29.39px;
          background: #1a769277;
          border-radius: 5px 5px 5px 14px;

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

            .select-img {
              width: 70%;
              height: auto;
            }
          }

          .select-txt {
            margin-top: 6.5%;
            font-size: 16px;
          }
        }
      }
    }
  }

  .right {
    position: absolute;
    width: 20%;
    height: 90%;
    top: 5%;
    right: 1%;

    .head {
      width: 100%;
      height: 15%;
      background-image: url(../assets/img/Inventory/banner.png);
      background-position: center top;
      background-size: 100%;
      border-radius: 10px;
      display: flex;
      align-items: center;

      .head-h1 {
        font-size: 23px;
        margin-left: 6%;
      }
    }

    .rc {
      width: 100%;
      height: 90%;

      .item {
        position: relative;
        width: 100%;
        height: 7%;
        background: rgba(22, 24, 26, 0.85);
        border-radius: 10px;
        margin-bottom: 3%;
        transition: 200ms;

        .item-h1 {
          margin: 0;
          margin-left: 5%;
          margin-top: 3.5%;
          width: 60%;
        }

        .select-item {
          position: absolute;
          top: 50%;
          right: 0%;
          width: 28%;
          transform: translateY(-50%);
          height: 29.39px;
          background: transparent;
          border-radius: 5px 5px 5px 5px;
          margin-right: 3.3%;

          .buy-item-imgc {
            width: 30%;
            height: 100%;
            background: #4af10877;
            display: flex;
            align-items: center;
            justify-content: center;
            float: right;
            border-radius: 5px;
            margin-right: 5%;

            .buy-img {
              width: 65%;
              height: auto;
            }
          }

          .select-item-imgc {
            width: 30%;
            height: 100%;
            background: #239abe77;
            display: flex;
            align-items: center;
            justify-content: center;
            float: right;
            border-radius: 5px;
            margin-right: 5%;

            .select-img {
              width: 70%;
              height: auto;
            }
          }

          .select-item-active {
            top: 15%;
            transform: translateY(0%);
          }
        }
      }

      .item-active {
        height: 10%;
      }
    }
  }
}

.wrapper-anim {
  animation: slideup 300ms linear 1;
  animation-fill-mode: forwards;
}

@keyframes slideup {
  0% {
    opacity: 0;
    top: 15%;
  }

  100% {
    opacity: 1;
    top: 0%;
  }
}

input[type="range"] {
  position: absolute;
  bottom: 20%;
  left: 5%;
  width: 90%;
  height: 2%;
  -webkit-appearance: none;
  overflow: visible;
  background: transparent;
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
